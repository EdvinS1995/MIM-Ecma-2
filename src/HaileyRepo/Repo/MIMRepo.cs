using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using HaileyRepo.Entity;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Pattern B (reflection over a typed POCO) — see references/dynamic-schema-pattern.md.
// The API (Swagger at /docs/index.html) is well documented and mostly flat, with one
// dynamic sub-object (customFieldsData) handled separately from the reflected schema.

namespace HaileyRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string BaseUrl { get; set; }
        public SecureString AccessToken { get; set; }

        public IList<CSEntryChange> CSEntries { get; set; }
        public SchemaType SchemaDef { get; set; }
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        private HttpClient _hClient;
        private const string Anchor = "EmployeeId"; // must match the MIMPerson property name used as anchor
        private const string CustomFieldPrefix = "CustomField_";

        #endregion

        #region Logging

        private static readonly string LogFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "HaileyRepo", "HaileyRepo.log");

        private void Log(string level, string message)
        {
            try
            {
                string dir = Path.GetDirectoryName(LogFile);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.AppendAllText(LogFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}");
            }
            catch { /* logging must never crash the MA */ }
        }
        private void LogInfo(string m) => Log("INFO ", m);
        private void LogWarn(string m) => Log("WARN ", m);
        private void LogError(string m) => Log("ERROR", m);

        #endregion

        #region Auth

        // HaileyHR uses a personal access token (a JWT the user creates in the Hailey UI),
        // sent as a plain Bearer header — there is no token endpoint/exchange to perform.
        public void Authenticate()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string baseUrl = BaseUrl ?? "";
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            _hClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _hClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStringToString(AccessToken));
        }

        private string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try { return Marshal.PtrToStringBSTR(bstr); }
            finally { Marshal.FreeBSTR(bstr); }
        }

        #endregion

        #region Handlers

        public void Get()
        {
            LogInfo("Get: starting import run");
            Authenticate();

            string listJson = FetchJson("Employees");
            List<EmployeeListItemDto> employees = JsonConvert.DeserializeObject<List<EmployeeListItemDto>>(listJson)
                ?? new List<EmployeeListItemDto>();
            LogInfo($"Get: fetched {employees.Count} employees from GET Employees");

            var entries = new List<CSEntryChange>();

            foreach (var employee in employees)
            {
                var person = new MIMPerson(employee);
                MergeEmployeeDetail(person);

                var csentry = ToCSEntryChange(person);
                AddCustomFieldAttributes(csentry, employee.CustomFieldsData);
                entries.Add(csentry);
            }

            // Safety-net dedup by anchor value, in case the API ever returns the same
            // employeeId twice (e.g. a paging edge case if HaileyHR adds pagination later).
            var deduped = new List<CSEntryChange>();
            var seenAnchors = new HashSet<string>();
            foreach (var csentry in entries)
            {
                string anchorValue = csentry.AnchorAttributes.FirstOrDefault(a => a.Name == Anchor)?.Value?.ToString();
                if (anchorValue == null || seenAnchors.Add(anchorValue))
                {
                    deduped.Add(csentry);
                }
                else
                {
                    LogWarn($"Get: suppressing duplicate CSEntryChange for anchor {Anchor}={anchorValue}");
                }
            }

            CSEntries = deduped;
            LogInfo($"Get: import complete, total entries={CSEntries.Count}");
        }

        private void MergeEmployeeDetail(MIMPerson person)
        {
            string detailJson;
            if (!TryFetchJson($"Employees/{person.EmployeeId}", out detailJson))
            {
                LogWarn($"Get: could not fetch Employees/{person.EmployeeId} detail, importing base fields only");
                return;
            }

            try
            {
                JObject detail = JObject.Parse(detailJson);

                var employmentsToken = detail.SelectToken("jobData.employment.employments");
                var employments = employmentsToken?.ToObject<List<EmploymentVmDto>>();
                string pickedEmploymentId = person.ApplyEffectualEmployment(employments);
                if (pickedEmploymentId != null)
                    LogInfo($"Get: employee {person.EmployeeId} effectual employment={pickedEmploymentId} (of {employments.Count})");

                var salariesToken = detail.SelectToken("salaries");
                var salaries = salariesToken?.ToObject<List<SalaryDto>>();
                bool ambiguousSalary;
                string pickedSalaryId = person.ApplyCurrentSalary(salaries, out ambiguousSalary);
                if (pickedSalaryId != null)
                    LogInfo($"Get: employee {person.EmployeeId} current salary={pickedSalaryId} (of {salaries.Count})");
                if (ambiguousSalary)
                    LogWarn($"Get: employee {person.EmployeeId} has more than one ongoing salary (no EndDate); picked the first one returned by the API");
            }
            catch (Exception ex)
            {
                LogWarn($"Get: failed to parse Employees/{person.EmployeeId} detail, importing base fields only. {ex.Message}");
            }
        }

        public void Put()
        {
            LogInfo($"Put: starting export run, entry count={CSEntries?.Count}");
            Authenticate();
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (var csentryChange in CSEntries)
            {
                var attributeChanges = new List<AttributeChange>();
                MAExportError status = MAExportError.ExportErrorCustomStopRun;
                string errorName = "";
                string errorMessage = "";

                try
                {
                    switch (csentryChange.ObjectModificationType)
                    {
                        case ObjectModificationType.Add:
                            HandleAdd(csentryChange, attributeChanges, out status, out errorMessage);
                            break;

                        case ObjectModificationType.Update:
                        case ObjectModificationType.Replace:
                            HandleUpdate(csentryChange, out status, out errorMessage);
                            break;

                        case ObjectModificationType.Delete:
                            status = MAExportError.ExportErrorCustomStopRun;
                            errorName = "HaileyHR error";
                            errorMessage = "HaileyHR's API does not expose a delete-employee endpoint; offboard/disable the employee manually in HaileyHR.";
                            break;

                        default:
                            LogWarn($"Put: unhandled ModificationType={csentryChange.ObjectModificationType} for {csentryChange.Identifier}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Put: unhandled exception for {csentryChange.Identifier}: {ex}");
                    status = MAExportError.ExportErrorCustomStopRun;
                    errorName = "HaileyRepo error";
                    errorMessage = ex.Message;
                }

                ExportEntriesResults.CSEntryChangeResults.Add(
                    CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
            }

            LogInfo($"Put: export run complete, results count={ExportEntriesResults.CSEntryChangeResults.Count}");
        }

        private void HandleAdd(CSEntryChange csentryChange, List<AttributeChange> attributeChanges, out MAExportError status, out string errorMessage)
        {
            var dto = new AddEmployeeRequestDto
            {
                FirstName = GetAttributeChangeValue(csentryChange, "FirstName"),
                LastName = GetAttributeChangeValue(csentryChange, "LastName"),
                CompanyEmail = GetAttributeChangeValue(csentryChange, "CompanyEmail"),
                StartDate = GetAttributeChangeValue(csentryChange, "DateOfJoining"),
                DepartmentId = GetAttributeChangeValue(csentryChange, "DepartmentId"),
                LegalEntityId = GetAttributeChangeValue(csentryChange, "LegalEntityId"),
                CostCenterId = GetAttributeChangeValue(csentryChange, "CostCenterId"),
                BusinessAreaId = GetAttributeChangeValue(csentryChange, "BusinessAreaId"),
                LocationId = GetAttributeChangeValue(csentryChange, "LocationId"),
                ReportsToEmployeeId = GetAttributeChangeValue(csentryChange, "ManagerEmployeeId"),
                TeamIds = GetAttributeChangeValues(csentryChange, "TeamIds"),
                TitleIds = GetAttributeChangeValues(csentryChange, "TitleIds"),
            };

            var missing = new List<string>();
            if (string.IsNullOrEmpty(dto.FirstName)) missing.Add("FirstName");
            if (string.IsNullOrEmpty(dto.LastName)) missing.Add("LastName");
            if (string.IsNullOrEmpty(dto.CompanyEmail)) missing.Add("CompanyEmail");
            if (string.IsNullOrEmpty(dto.StartDate)) missing.Add("DateOfJoining");

            if (missing.Count > 0)
            {
                status = MAExportError.ExportErrorInvalidAttributeValue;
                errorMessage = $"HaileyHR quick-add requires: {string.Join(", ", missing)}";
                LogWarn($"Put/Add: {csentryChange.Identifier} missing required field(s): {string.Join(", ", missing)}");
                return;
            }

            string payload = JsonConvert.SerializeObject(dto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            string httpStatus = PostJson("Employee", payload, "POST", out string responseBody);

            if (httpStatus != "Created")
            {
                status = Util.ErrValidate(httpStatus);
                errorMessage = $"HTTP {httpStatus}: {responseBody}";
                LogWarn($"Put/Add: {csentryChange.Identifier} failed, {errorMessage}");
                return;
            }

            // POST /Employee returns 201 with no body and no documented Location header, so the
            // only way to learn the new employeeId is to look the employee back up by the
            // (required, presumably unique) companyEmail we just created it with.
            string newEmployeeId = FindEmployeeIdByCompanyEmail(dto.CompanyEmail);
            if (newEmployeeId == null)
            {
                status = MAExportError.ExportErrorCustomContinueRun;
                errorMessage = $"Employee was created in HaileyHR but could not be located afterwards by companyEmail '{dto.CompanyEmail}' to learn its employeeId; verify manually in HaileyHR and re-run import to pick it up.";
                LogWarn($"Put/Add: {csentryChange.Identifier} - {errorMessage}");
                return;
            }

            attributeChanges.Add(AttributeChange.CreateAttributeAdd(Anchor, newEmployeeId));
            status = MAExportError.Success;
            errorMessage = "";
            LogInfo($"Put/Add: {csentryChange.Identifier} created as employeeId={newEmployeeId}");
        }

        private void HandleUpdate(CSEntryChange csentryChange, out MAExportError status, out string errorMessage)
        {
            string employeeId = csentryChange.AnchorAttributes.FirstOrDefault(a => a.Name == Anchor)?.Value?.ToString();
            if (string.IsNullOrEmpty(employeeId))
            {
                status = MAExportError.ExportErrorInvalidDN;
                errorMessage = $"CSEntryChange {csentryChange.Identifier} has no {Anchor} anchor value";
                return;
            }

            var errors = new List<string>();
            bool anyWritableChange = false;

            string newCompanyEmail = GetAttributeChangeValue(csentryChange, "CompanyEmail");
            if (newCompanyEmail != null)
            {
                anyWritableChange = true;
                var dto = new UpdateCompanyEmailRequestDto { CompanyEmail = newCompanyEmail };
                string payload = JsonConvert.SerializeObject(dto);
                string httpStatus = PostJson($"Employee/{employeeId}/CompanyEmail", payload, "PUT", out string responseBody);
                if (Util.ErrValidate(httpStatus) != MAExportError.Success)
                {
                    errors.Add($"CompanyEmail update failed, HTTP {httpStatus}: {responseBody}");
                    LogWarn($"Put/Update: {employeeId} CompanyEmail update failed, HTTP {httpStatus}: {responseBody}");
                }
                else
                {
                    LogInfo($"Put/Update: {employeeId} CompanyEmail updated");
                }
            }

            var customFieldChanges = csentryChange.AttributeChanges.Where(ac => ac.Name.StartsWith(CustomFieldPrefix)).ToList();
            if (customFieldChanges.Count > 0)
            {
                anyWritableChange = true;
                var customFieldsData = new Dictionary<string, object>();
                foreach (var ac in customFieldChanges)
                {
                    string key = ac.Name.Substring(CustomFieldPrefix.Length).Replace("_", " ");
                    var addValues = ac.ValueChanges.Where(v => v.ModificationType == ValueModificationType.Add).Select(v => v.Value?.ToString()).ToList();
                    if (addValues.Count == 0) continue;
                    customFieldsData[key] = addValues.Count == 1 ? (object)addValues[0] : addValues;
                }

                if (customFieldsData.Count > 0)
                {
                    var dto = new UpdateCustomEmployeeFieldsRequestDto { CustomFieldsData = customFieldsData };
                    string payload = JsonConvert.SerializeObject(dto);
                    string httpStatus = PostJson($"Employee/{employeeId}/CustomEmployeeField", payload, "PUT", out string responseBody);
                    if (Util.ErrValidate(httpStatus) != MAExportError.Success)
                    {
                        errors.Add($"CustomEmployeeField update failed, HTTP {httpStatus}: {responseBody}");
                        LogWarn($"Put/Update: {employeeId} CustomEmployeeField update failed, HTTP {httpStatus}: {responseBody}");
                    }
                    else
                    {
                        LogInfo($"Put/Update: {employeeId} custom fields updated ({string.Join(", ", customFieldsData.Keys)})");
                    }
                }
            }

            var ignoredAttributes = csentryChange.AttributeChanges
                .Where(ac => ac.Name != "CompanyEmail" && !ac.Name.StartsWith(CustomFieldPrefix))
                .Select(ac => ac.Name)
                .ToList();
            if (ignoredAttributes.Count > 0)
                LogWarn($"Put/Update: {employeeId} ignored non-writable attribute(s), HaileyHR only supports updating CompanyEmail and CustomField_*: {string.Join(", ", ignoredAttributes)}");

            if (errors.Count > 0)
            {
                status = MAExportError.ExportErrorCustomStopRun;
                errorMessage = string.Join(" | ", errors);
            }
            else
            {
                status = MAExportError.Success;
                errorMessage = "";
                if (!anyWritableChange)
                    LogInfo($"Put/Update: {employeeId} had no writable attribute changes (no-op)");
            }
        }

        private string FindEmployeeIdByCompanyEmail(string companyEmail)
        {
            try
            {
                string listJson = FetchJson("Employees");
                var employees = JsonConvert.DeserializeObject<List<EmployeeListItemDto>>(listJson) ?? new List<EmployeeListItemDto>();
                return employees.FirstOrDefault(e => string.Equals(e.CompanyEmail, companyEmail, StringComparison.OrdinalIgnoreCase))?.EmployeeId;
            }
            catch (Exception ex)
            {
                LogWarn($"FindEmployeeIdByCompanyEmail: lookup for '{companyEmail}' failed: {ex.Message}");
                return null;
            }
        }

        public bool MakeSchema()
        {
            LogInfo("MakeSchema: building schema via reflection over MIMPerson, plus discovered CustomField_* attributes");
            Authenticate();

            var schemaDef = SchemaType.Create("Person", false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(Anchor, AttributeType.String));

            foreach (PropertyInfo p in typeof(MIMPerson).GetProperties())
            {
                if (p.Name == Anchor) continue;

                bool isCollection = typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string);
                if (isCollection)
                    schemaDef.Attributes.Add(SchemaAttribute.CreateMultiValuedAttribute(p.Name, AttributeType.String));
                else
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(p.Name, Util.GetType(p.PropertyType.Name)));
            }

            AddCustomFieldSchemaAttributes(schemaDef);

            SchemaDef = schemaDef;
            return true;
        }

        private void AddCustomFieldSchemaAttributes(SchemaType schemaDef)
        {
            List<EmployeeListItemDto> employees;
            try
            {
                string listJson = FetchJson("Employees");
                employees = JsonConvert.DeserializeObject<List<EmployeeListItemDto>>(listJson) ?? new List<EmployeeListItemDto>();
            }
            catch (Exception ex)
            {
                LogWarn($"MakeSchema: could not sample Employees to discover custom fields, schema will have no CustomField_* attributes. {ex.Message}");
                return;
            }

            // Walk every sample record (not just the first) since different employees may
            // have different optional custom fields populated.
            var multiValued = new HashSet<string>();
            var seenAttributeNames = new Dictionary<string, string>(); // attribute name -> original key, to detect sanitization collisions

            foreach (var employee in employees)
            {
                if (employee.CustomFieldsData == null) continue;
                foreach (var kv in employee.CustomFieldsData)
                {
                    string attrName = CustomFieldPrefix + kv.Key.Replace(" ", "_");

                    if (seenAttributeNames.TryGetValue(attrName, out string existingKey) && existingKey != kv.Key)
                    {
                        LogWarn($"MakeSchema: custom field keys '{existingKey}' and '{kv.Key}' both sanitize to attribute '{attrName}'; skipping '{kv.Key}' to avoid a collision");
                        continue;
                    }
                    seenAttributeNames[attrName] = kv.Key;

                    if (kv.Value is JArray) multiValued.Add(attrName);
                }
            }

            foreach (var attrName in seenAttributeNames.Keys)
            {
                schemaDef.Attributes.Add(multiValued.Contains(attrName)
                    ? SchemaAttribute.CreateMultiValuedAttribute(attrName, AttributeType.String)
                    : SchemaAttribute.CreateSingleValuedAttribute(attrName, AttributeType.String));
            }

            LogInfo($"MakeSchema: discovered {seenAttributeNames.Count} custom field attribute(s) across {employees.Count} sample employees");
        }

        #endregion

        #region Converters

        private CSEntryChange ToCSEntryChange(MIMPerson person)
        {
            var csentry = CSEntryChange.Create();
            csentry.ObjectModificationType = ObjectModificationType.Add;
            csentry.ObjectType = "Person";

            foreach (PropertyInfo p in typeof(MIMPerson).GetProperties())
            {
                object value = p.GetValue(person);

                if (p.Name == Anchor)
                {
                    csentry.AnchorAttributes.Add(AnchorAttribute.Create(Anchor, value?.ToString()));
                    continue;
                }
                if (value == null) continue;

                if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    var values = enumerable.Cast<object>().Select(v => v?.ToString()).Where(v => !string.IsNullOrEmpty(v)).ToList();
                    if (values.Count > 0)
                        csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, values));
                }
                else if (!string.IsNullOrEmpty(value.ToString()))
                {
                    csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, value.ToString()));
                }
            }

            return csentry;
        }

        private void AddCustomFieldAttributes(CSEntryChange csentry, Dictionary<string, object> customFieldsData)
        {
            if (customFieldsData == null) return;

            foreach (var kv in customFieldsData)
            {
                string attrName = CustomFieldPrefix + kv.Key.Replace(" ", "_");

                if (kv.Value is JArray array)
                {
                    var values = array.Select(v => v.ToString()).Where(v => !string.IsNullOrEmpty(v)).ToList();
                    if (values.Count > 0)
                        csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(attrName, values));
                }
                else if (kv.Value != null && !string.IsNullOrEmpty(kv.Value.ToString()))
                {
                    csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(attrName, kv.Value.ToString()));
                }
            }
        }

        private string GetAttributeChangeValue(CSEntryChange csentryChange, string attributeName)
        {
            var ac = csentryChange.AttributeChanges.FirstOrDefault(a => a.Name == attributeName);
            return ac?.ValueChanges.FirstOrDefault(v => v.ModificationType == ValueModificationType.Add)?.Value?.ToString();
        }

        private List<string> GetAttributeChangeValues(CSEntryChange csentryChange, string attributeName)
        {
            var ac = csentryChange.AttributeChanges.FirstOrDefault(a => a.Name == attributeName);
            if (ac == null) return null;
            var values = ac.ValueChanges.Where(v => v.ModificationType == ValueModificationType.Add).Select(v => v.Value?.ToString()).Where(v => !string.IsNullOrEmpty(v)).ToList();
            return values.Count > 0 ? values : null;
        }

        #endregion

        #region HTTP

        private string FetchJson(string relativePath)
        {
            var response = _hClient.GetAsync(relativePath).ConfigureAwait(false).GetAwaiter().GetResult();
            string body = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"GET {relativePath} failed, HTTP {(int)response.StatusCode} {response.StatusCode}: {body}");
            return body;
        }

        // Non-throwing variant used for per-employee detail calls: one failed employee
        // shouldn't abort the entire import run, just fall back to base-fields-only for that one.
        private bool TryFetchJson(string relativePath, out string json)
        {
            json = null;
            try
            {
                var response = _hClient.GetAsync(relativePath).ConfigureAwait(false).GetAwaiter().GetResult();
                json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    LogWarn($"GET {relativePath} failed, HTTP {(int)response.StatusCode} {response.StatusCode}: {json}");
                    json = null;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogWarn($"GET {relativePath} threw: {ex.Message}");
                return false;
            }
        }

        private string PostJson(string relativePath, string data, string httpMethod, out string responseBody)
        {
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), relativePath) { Content = content };
            var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return response.StatusCode.ToString();
        }

        #endregion
    }
}
