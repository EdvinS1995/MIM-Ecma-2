using FourHumanRepo.Entity;
using FourHumanRepo.Repo;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StaticLib;
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
using System.Threading;        // SemaphoreSlim, Interlocked
using System.Threading.Tasks;  // Task, Task<T>
using System.Xml;
using System.Xml.Serialization;

namespace FourHumanRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string CompanyCode { get; set; }
        public string LoginEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int DaysBeforeStart { get; set; }

        public bool GetEmployees { get; set; } = true;
        public IList<CSEntryChange> CSEntries { get; set; }
        public string JsonRoot { get; set; }
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }
        public PutExportEntriesResults ExportEntriesResults { get; set; }
        public List<Dictionary<string, string>> EmpDic { get; set; }

        private HttpClient _hClient;
        private string Token;
        private string _anchor = "UserId";

        #endregion

        #region Auth

        public MIMRepo() { }

        public bool DecideAuth(string companyCode)
        {
            switch (companyCode)
            {
                case "KHGET":
                case "KHPUT":
                    ApiEndpoint = "https://api.hrm-4human.com/";
                    ClientId = "21GKI5ETR4XP6BADULS0";
                    ClientSecret = SecretRef.KHGet_ClientSecret;
                    DaysBeforeStart = 35;
                    AuthPost();
                    break;
            }
            return true;
        }

        public void AuthPost()
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Token = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var form = new Dictionary<string, string>
                    {
                        { "grant_type", "client_credentials" },
                        { "client_id", ClientId },
                        { "client_secret", ClientSecret },
                    };

                    HttpContent content = new FormUrlEncodedContent(form);
                    string tokenUrl = ApiEndpoint + "token";

                    HttpResponseMessage tokenResponse = client.PostAsync(tokenUrl, content)
                        .ConfigureAwait(false).GetAwaiter().GetResult();

                    string raw = tokenResponse.Content.ReadAsStringAsync()
                        .ConfigureAwait(false).GetAwaiter().GetResult();

                    if (!tokenResponse.IsSuccessStatusCode)
                        throw new Exception($"Token request failed. Url={tokenUrl}, Status={tokenResponse.StatusCode}, Body={raw}");

                    JObject obj = JObject.Parse(raw);
                    Token = (string)obj["access_token"];

                    if (string.IsNullOrEmpty(Token))
                        throw new Exception($"Token response did not contain access_token. Body={raw}");

                    _hClient = new HttpClient();
                    _hClient.BaseAddress = new Uri(ApiEndpoint);
                    _hClient.DefaultRequestHeaders.Accept.Clear();
                    _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                    _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _hClient.DefaultRequestHeaders.Add("Api-Version", "v3");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"AuthPost failed. ApiEndpoint='{ApiEndpoint}', ClientId='{ClientId}'. Inner: {ex}");
            }
        }

        public void AuthHClient()
        {
            var aut = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            string base6 = Convert.ToBase64String(aut);
            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.Timeout = new TimeSpan(0, 5, 0);
        }

        #endregion

        #region Handlers

        public void Get()
        {
            DecideAuth(CompanyCode + "GET");
            CSEntries = new List<CSEntryChange>();

            if (!GetEmployees) return;

            // Hent brukere og company-employees parallelt – uavhengige kall
            var taskUsers = Task.Run(() => FetchAllUsers());
            var taskEmployees = Task.Run(() => FetchAllCompanyEmployees());

            Task.WhenAll(taskUsers, taskEmployees)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            var allUsers = taskUsers.Result;
            var allCompanyEmployees = taskEmployees.Result;

            System.Diagnostics.Trace.WriteLine(
                $"4human: Hentet {allUsers.Count} brukere og {allCompanyEmployees.Count} company-employees.");

            var mimEmployees = MakeMimObjects(allUsers, allCompanyEmployees);

            foreach (var emp in mimEmployees)
                CSEntries.Add(FillCSEntry(emp, _anchor, "Person"));

            System.Diagnostics.Trace.WriteLine(
                $"4human: Ferdig – {CSEntries.Count} CSEntries klare for MIM.");
        }

        public void Put()
        {
            DecideAuth(CompanyCode + "PUT");
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                var status = Util.ErrValidate("BadRequest");
                string errorName = "";
                string errorMessage = "";

                switch (csentryChange.ObjectModificationType)
                {
                    case ObjectModificationType.Add:
                    case ObjectModificationType.Delete:
                        errorMessage = "Not implemented";
                        errorName = "IFS error";
                        status = Util.ErrValidate("BadRequest");
                        ExportEntriesResults.CSEntryChangeResults.Add(
                            CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                        break;

                    case ObjectModificationType.Replace:
                    case ObjectModificationType.Update:
                        {
                            var payload = FillRootObject(csentryChange, out var anchor);

                            if (!string.IsNullOrEmpty(anchor) && !string.IsNullOrEmpty(payload))
                            {
                                bool patchOk = UpdateContactDetailsWorkEmail(payload, anchor, out var wcError);
                                if (patchOk)
                                    status = MAExportError.Success;
                                else
                                {
                                    errorName = "4human contact-details error";
                                    errorMessage = wcError;
                                }
                            }

                            string companyEmployeeId = GetAddValueFromChanges(csentryChange, "CompanyEmployeeId");
                            string newKhValue = GetAddValueFromChanges(csentryChange, "KhValue");
                            string khFieldId = GetAddValueFromChanges(csentryChange, "KhFieldId");

                            if (!string.IsNullOrWhiteSpace(companyEmployeeId) &&
                                !string.IsNullOrWhiteSpace(khFieldId) &&
                                !string.IsNullOrWhiteSpace(newKhValue))
                            {
                                bool khOk = UpdateCompanyEmployeeKhNumber(companyEmployeeId, khFieldId, newKhValue, out var khError);
                                if (!khOk)
                                {
                                    errorName = string.IsNullOrEmpty(errorName) ? "4human KH-nummer error" : errorName;
                                    errorMessage = string.IsNullOrEmpty(errorMessage)
                                        ? khError
                                        : $"{errorMessage} | KH-nummer: {khError}";
                                    status = Util.ErrValidate("BadRequest");
                                }
                            }

                            ExportEntriesResults.CSEntryChangeResults.Add(
                                CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }
                }
            }
        }

        // Hjelpemetode – henter første Add-verdi for et navngitt attributt fra CSEntryChange
        private static string GetAddValueFromChanges(CSEntryChange cse, string attrName)
        {
            foreach (var ac in cse.AttributeChanges)
            {
                if (!ac.Name.Equals(attrName, StringComparison.OrdinalIgnoreCase)) continue;
                if (ac.ValueChanges == null || ac.ValueChanges.Count == 0) continue;
                var vc = ac.ValueChanges[0];
                if (vc != null && vc.ModificationType == ValueModificationType.Add)
                    return vc.Value as string;
            }
            return null;
        }

        private string FillRootObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";
            var anchorAttr = cse.AnchorAttributes.FirstOrDefault(a => a.Name == _anchor);
            if (anchorAttr?.Value == null) return null;

            anchor = anchorAttr.Value.ToString();
            string emailWork = GetAddValueFromChanges(cse, "EmailWork");

            if (string.IsNullOrWhiteSpace(emailWork)) return null;

            return JsonConvert.SerializeObject(new { workEmail = emailWork });
        }

        #endregion

        #region Convert

        // ---------------------------------------------------------------
        // MakeMimObjects – parallell henting, O(1) lookup, retry
        // ---------------------------------------------------------------
        private List<MIMObject> MakeMimObjects(
            List<UserItem> users,
            List<CompanyEmployeesItem> employeesItems)
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-DaysBeforeStart);
            DateTime endDate = DateTime.Now.Date.AddDays(DaysBeforeStart);

            var mimObjects = users.Select(p => new MIMObject(p)).ToList();

            // O(1) lookup istedenfor O(n) Where per bruker
            var employeeLookup = employeesItems
                .Where(x =>
                    x.startDate.Date <= endDate &&
                    (x.endDate == null || x.endDate.Value.Date > startDate))
                .GroupBy(x => x.userId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.startDate).First());

            var mimWithEmployee = mimObjects
                .Where(m => employeeLookup.ContainsKey(m.UserId))
                .ToList();

            System.Diagnostics.Trace.WriteLine(
                $"4human: {mimWithEmployee.Count} av {mimObjects.Count} brukere har aktiv employment.");

            // 25 samtidige brukere = maks 75 HTTP-kall samtidig
            const int maxDegree = 25;
            var semaphore = new SemaphoreSlim(maxDegree);
            int processed = 0;

            var tasks = mimWithEmployee.Select(mim => Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var emp = employeeLookup[mim.UserId];

                    mim.CompanyEmployeeId = emp.id.ToString();
                    mim.EmployeeId = emp.employeeId;
                    mim.StartDate = emp.startDate.ToString("yyyy-MM-dd");
                    mim.EndDate = emp.endDate?.ToString("yyyy-MM-dd");
                    mim.CompanyId = emp.companyId;
                    mim.SeniorityDate = emp.seniorityDate;
                    mim.EmploymentNumber = emp.primaryEmployment.id.ToString();

                    // Alle 3 kall per bruker kjøres samtidig
                    var tCe = GetAPIWithRetryAsync($"personnel/company-employees/{mim.CompanyEmployeeId}?external=false");
                    var tProf = GetAPIWithRetryAsync($"personnel/profiles/{mim.UserId}");
                    var tEmp = GetAPIWithRetryAsync($"personnel/employments/{mim.EmploymentNumber}");

                    await Task.WhenAll(tCe, tProf, tEmp);

                    ApplyCeDetails(mim, await tCe);
                    ApplyProfile(mim, await tProf);
                    ApplyEmployment(mim, await tEmp);

                    int done = Interlocked.Increment(ref processed);
                    if (done % 100 == 0)
                        System.Diagnostics.Trace.WriteLine(
                            $"4human: Fremgang {done}/{mimWithEmployee.Count}");
                }
                finally
                {
                    semaphore.Release();
                }
            })).ToList();

            Task.WhenAll(tasks).ConfigureAwait(false).GetAwaiter().GetResult();

            System.Diagnostics.Trace.WriteLine($"4human: MakeMimObjects fullført – {processed} brukere hentet.");

            return mimObjects;
        }

        private void ApplyCeDetails(MIMObject mim, string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            try
            {
                var customFields = JObject.Parse(json)["customFields"] as JArray;
                if (customFields == null) return;

                foreach (var cf in customFields)
                {
                    if (string.Equals((string)cf["field"], "KH-nummer", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals((string)cf["fieldExternalName"], "KH-nummer", StringComparison.OrdinalIgnoreCase))
                    {
                        mim.KhFieldId = (string)cf["fieldId"];
                        mim.KhValue = (string)cf["value"];
                        break;
                    }
                }
            }
            catch (Exception) { }
        }

        private void ApplyProfile(MIMObject mim, string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            try
            {
                var p = JsonConvert.DeserializeObject<UserProfileResponse>(json);
                if (p == null) return;

                mim.FirstName = p.firstName;
                mim.LastName = p.surname;
                mim.BirthDate = p.dateOfBirth?.ToString("yyyy-MM-dd") ?? "";
                mim.Gender = p.gender;

                mim.UserCustomField1 = p.customFields?
                    .FirstOrDefault(x => x.fieldExternalId == "1")?.valueExternalName ?? "";

                var workEmail = p.workContactDetails?.contactInfo?.email;
                if (!string.IsNullOrWhiteSpace(workEmail))
                    mim.EmailWork = workEmail;

                var mobile =
                    p.workContactDetails?.contactInfo?.mobilePhone?.phoneNumber
                    ?? p.privateContactDetails?.contactInfo?.mobilePhone?.phoneNumber;

                if (!string.IsNullOrWhiteSpace(mobile) && string.IsNullOrWhiteSpace(mim.CellPhone))
                {
                    mim.CellPhone = mobile;
                    mim.mobilePhone = mobile;
                }
            }
            catch (Exception) { }
        }

        private void ApplyEmployment(MIMObject mim, string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            try
            {
                var e = JsonConvert.DeserializeObject<PersonEmploymentResponse>(json);
                if (e == null) return;

                mim.PositionTitle = e.positionTitle;
                mim.EmployeeType = e.employeeType?.employeeType;
                mim.EmployeeTypeId = e.employeeType?.typeId;
                mim.EmploymentType = e.employeeType?.employmentType;
                mim.EmploymentStartDate = e.startDate.ToString("yyyy-MM-dd");
                mim.EmploymentEndDate = e.endDate?.ToString("yyyy-MM-dd");
                mim.Manager = e.manager?.employeeId;
                mim.CompanyLocationName = e.companyLocation?.name;
                mim.Job = e.job?.title;
                mim.OccupationalCode = e.job?.occupationalCodes?.FirstOrDefault()?.code;
                mim.OrgUnitExternalId = e.orgUnit?.externalId;
                mim.OrgUnitName = e.orgUnit?.name;
                mim.IsPrimaryEmployment = e.isPrimaryEmployment;

                var cf = e.customFields;
                mim.EmploymentCustomField1 = GetEmploymentCustomFieldValueByName(cf, "Avtalemal Quinyx");
                mim.EmploymentCustomField2 = GetEmploymentCustomFieldValueByName(cf, "Pensjonsordning");
                mim.EmploymentCustomField3 = GetEmploymentCustomFieldValueByName(cf, "Arbeidsordre");
                mim.EmploymentCustomField4 = GetEmploymentCustomFieldValueByName(cf, "Produktområde");
                mim.EmploymentCustomField5 = GetEmploymentCustomFieldValueByName(cf, "Team");
                mim.EmploymentCustomField6 = GetEmploymentCustomFieldValueByName(cf, "Lederkategori");
                mim.EmploymentCustomField7 = GetEmploymentCustomFieldValueByName(cf, "Er stillingen ledende eller særlig uavhengig?");
                mim.EmploymentCustomField8 = GetEmploymentCustomFieldValueByName(cf, "Jobbfamilie");
                mim.EmploymentCustomField9 = GetEmploymentCustomFieldValueByName(cf, "Stillingsnivå");
                mim.EmploymentCustomField10 = GetEmploymentCustomFieldValueByName(cf, "Karrierebånd");
                mim.EmploymentCustomField15 = GetEmploymentCustomFieldValueByName(cf, "Bredbånd dekket av arbeidsgiver");
                mim.EmploymentCustomField16 = GetEmploymentCustomFieldValueByName(cf, "Mobil dekket av arbeidsgiver");
            }
            catch (Exception) { }
        }

        #endregion

        #region API

        private List<UserItem> FetchAllUsers()
        {
            var allUsers = new List<UserItem>();
            var filters = new[] { "active", "inactive", "invited", "uninvited", "offboarding", "expired" };

            var firstPageTasks = filters.Select(filter => Task.Run(async () =>
            {
                var json = await GetAPIWithRetryAsync($"users?limit=100&page=1&filter={filter}");
                if (json == null) return (Filter: filter, Data: (UsersResponse)null);
                return (Filter: filter, Data: JsonConvert.DeserializeObject<UsersResponse>(json));
            })).ToList();

            var firstPages = Task.WhenAll(firstPageTasks)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            var remainingTasks = new List<Task<List<UserItem>>>();

            foreach (var result in firstPages)
            {
                if (result.Data?.items == null) continue;
                allUsers.AddRange(result.Data.items);

                for (int page = 2; page <= result.Data.pages; page++)
                {
                    var p = page; var f = result.Filter;
                    remainingTasks.Add(Task.Run(async () =>
                    {
                        var json = await GetAPIWithRetryAsync($"users?limit=100&page={p}&filter={f}");
                        if (json == null) return new List<UserItem>();
                        return JsonConvert.DeserializeObject<UsersResponse>(json)?.items?.ToList()
                               ?? new List<UserItem>();
                    }));
                }
            }

            if (remainingTasks.Any())
            {
                foreach (var batch in Task.WhenAll(remainingTasks).ConfigureAwait(false).GetAwaiter().GetResult())
                    allUsers.AddRange(batch);
            }

            return allUsers.GroupBy(u => u.id).Select(g => g.First()).ToList();
        }

        private List<CompanyEmployeesItem> FetchAllCompanyEmployees()
        {
            var all = new List<CompanyEmployeesItem>();

            var firstJson = GetAPIWithRetryAsync("personnel/company-employees?limit=100&page=1")
                .ConfigureAwait(false).GetAwaiter().GetResult();

            if (firstJson == null) return all;

            var firstPage = JsonConvert.DeserializeObject<CompanyEmployeesResponse>(firstJson);
            if (firstPage?.items == null) return all;

            all.AddRange(firstPage.items);
            if (firstPage.pages <= 1) return all;

            var pageTasks = Enumerable.Range(2, firstPage.pages - 1).Select(page => Task.Run(async () =>
            {
                var json = await GetAPIWithRetryAsync($"personnel/company-employees?limit=100&page={page}");
                if (json == null) return new List<CompanyEmployeesItem>();
                return JsonConvert.DeserializeObject<CompanyEmployeesResponse>(json)?.items?.ToList()
                       ?? new List<CompanyEmployeesItem>();
            })).ToList();

            foreach (var batch in Task.WhenAll(pageTasks).ConfigureAwait(false).GetAwaiter().GetResult())
                all.AddRange(batch);

            return all;
        }

        private async Task<string> GetAPIWithRetryAsync(string getType, int maxRetries = 3, int baseDelayMs = 500)
        {
            string url = getType.StartsWith("http") ? getType : $"{ApiEndpoint}{getType}";

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var response = await _hClient.GetAsync(url);

                    if (response.StatusCode == (HttpStatusCode)429)
                    {
                        int delay = baseDelayMs * (int)Math.Pow(2, attempt);
                        if (response.Headers.TryGetValues("Retry-After", out var vals) &&
                            int.TryParse(vals.FirstOrDefault(), out int retryAfter))
                            delay = retryAfter * 1000;

                        await Task.Delay(delay);
                        continue;
                    }

                    if (!response.IsSuccessStatusCode) return null;

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException) when (attempt < maxRetries)
                {
                    await Task.Delay(baseDelayMs * (int)Math.Pow(2, attempt));
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        // Beholdt for bakoverkompatibilitet (brukes av GetKhFieldIdFromCompanyEmployee etc.)
        private string GetAPI(string getType, int limit, int page)
        {
            string targetURL = $"{ApiEndpoint}{getType}";
            if (limit > 0)
                targetURL = $"{ApiEndpoint}{getType}?limit={limit}&page={page}";

            var response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return response.IsSuccessStatusCode ? result : null;
        }

        private string GetAPI(string getType, int limit, int page, string extraQuery)
        {
            string targetURL = $"{ApiEndpoint}{getType}";
            if (limit > 0)
                targetURL = $"{ApiEndpoint}{getType}?limit={limit}&page={page}";

            if (!string.IsNullOrWhiteSpace(extraQuery))
            {
                extraQuery = extraQuery.TrimStart('?', '&');
                targetURL += targetURL.Contains("?") ? $"&{extraQuery}" : $"?{extraQuery}";
            }

            var response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return response.IsSuccessStatusCode ? result : null;
        }

        public string PostJSON(string DATA, string endPoint, out string errorMessage)
        {
            AuthHClient();
            errorMessage = "";
            return "";
        }

        public string GetKhFieldIdFromCompanyEmployee(string companyEmployeeId, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrWhiteSpace(companyEmployeeId))
            {
                errorMessage = "companyEmployeeId is required.";
                return null;
            }

            try
            {
                string url = $"{ApiEndpoint}personnel/company-employees/{companyEmployeeId}?external=false";
                var response = _hClient.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    var body = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    errorMessage = $"Status: {response.StatusCode}, Body: {body}";
                    return null;
                }

                var json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var customFields = JObject.Parse(json)["customFields"] as JArray;

                if (customFields == null) { errorMessage = "No customFields."; return null; }

                foreach (var cf in customFields)
                {
                    var fieldId = (string)cf["fieldId"];
                    if (!string.IsNullOrEmpty(fieldId) &&
                        ((string)cf["fieldExternalId"] == "2" ||
                         string.Equals((string)cf["field"], "KH-nummer", StringComparison.OrdinalIgnoreCase)))
                        return fieldId;
                }

                errorMessage = "KH-nummer field not found.";
                return null;
            }
            catch (Exception ex) { errorMessage = ex.ToString(); return null; }
        }

        public bool UpdateRecord(string data, string anchor, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(data)) { errorMessage = "No data."; return false; }
            if (string.IsNullOrEmpty(anchor)) { errorMessage = "No anchor."; return false; }
            if (_hClient == null) { errorMessage = "_hClient not initialized."; return false; }

            try
            {
                string url = $"{ApiEndpoint}personnel/work-contact-details/{anchor}?forceNoApprovalPolicy=false";
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = _hClient.PutAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult();
                var respData = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"Errorcode: {response.StatusCode}, {response.ReasonPhrase}, Response: {respData}";
                    return false;
                }
                return true;
            }
            catch (Exception ex) { errorMessage = ex.ToString(); return false; }
        }

        public bool UpdateContactDetailsWorkEmail(string data, string userId, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(data)) { errorMessage = "No data."; return false; }
            if (string.IsNullOrEmpty(userId)) { errorMessage = "No userId."; return false; }
            if (_hClient == null) { errorMessage = "_hClient not initialized."; return false; }

            try
            {
                string url = $"{ApiEndpoint}personnel/contact-details/{userId}?forceNoApprovalPolicy=false";
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = new StringContent(data, Encoding.UTF8, "application/json")
                };

                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                string body = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"Errorcode: {response.StatusCode}, {response.ReasonPhrase}, Response: {body}";
                    return false;
                }
                return true;
            }
            catch (Exception ex) { errorMessage = ex.ToString(); return false; }
        }

        public bool UpdateCompanyEmployeeKhNumber(string companyEmployeeId, string khFieldId, string khNumber, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrWhiteSpace(companyEmployeeId)) { errorMessage = "companyEmployeeId required."; return false; }
            if (string.IsNullOrWhiteSpace(khFieldId)) { errorMessage = "KhFieldId required."; return false; }
            if (string.IsNullOrWhiteSpace(khNumber)) { errorMessage = "KH-nummer value required."; return false; }
            if (_hClient == null) { errorMessage = "_hClient not initialized."; return false; }

            try
            {
                var payload = JsonConvert.SerializeObject(new
                {
                    customFields = new[] { new { fieldId = khFieldId, valueId = (string)null, value = khNumber } }
                });

                string url = $"{ApiEndpoint}personnel/company-employees/{companyEmployeeId}";
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };

                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                string body = response.Content?.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult() ?? "";

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"Status: {response.StatusCode}, Body: {body}";
                    return false;
                }
                return true;
            }
            catch (Exception ex) { errorMessage = ex.ToString(); return false; }
        }

        private string SendPostRequest(string baseURL, string data, string doMethod, string subUrl,
            out string errorMessage, out string responseData)
        {
            var code = "BadRequest";
            errorMessage = "";
            responseData = "";

            try
            {
                var content = new StringContent(data, Encoding.UTF8, "application/xml");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

                var request = new HttpRequestMessage
                {
                    Method = new HttpMethod(doMethod),
                    RequestUri = new Uri($"{baseURL}{subUrl}"),
                    Content = content
                };

                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                code = response.StatusCode.ToString();

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    throw new Exception(errorMessage);
                }

                responseData = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw new Exception(errorMessage);
            }

            return code;
        }

        #endregion

        #region Ecma

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
                SchemaDefs.Add("Person", CreateSchema<MIMObject>(_anchor, "Person"));
            return true;
        }

        public SchemaType CreateSchema<T>(string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            var specProps = new List<string> { anchor };
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                if (specProps.Contains(p.Name)) continue;
                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                schemaDef.Attributes.Add(
                    SchemaAttribute.CreateSingleValuedAttribute(p.Name, Util.GetType(type.Name)));
            }

            return schemaDef;
        }

        private SchemaType CreateSchemaJS(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            var cusdic = new Dictionary<string, string>();
            var jts = jsonData;

            if (!string.IsNullOrEmpty(jsonRoot))
                jts = JObject.Parse(jsonData)[jsonRoot].ToString();

            foreach (var empItem in JArray.Parse(jts))
            {
                foreach (var kv in MakeKV(JObject.Parse(empItem.ToString()), jsonRoot, true))
                    if (kv.Key != anchor) cusdic[kv.Key] = kv.Value;
            }

            foreach (var sc in cusdic)
                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(sc.Key, AttributeType.String));

            return schemaDef;
        }

        private void FillCSEntriesJS(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            EmpDic = new List<Dictionary<string, string>>();
            var jts = string.IsNullOrEmpty(jsonRoot)
                ? jsonData
                : JObject.Parse(jsonData)[jsonRoot].ToString();

            foreach (var empItem in JArray.Parse(jts))
            {
                var emfi = new Dictionary<string, string>();
                var csentry1 = CSEntryChange.Create();
                csentry1.ObjectModificationType = ObjectModificationType.Add;
                csentry1.ObjectType = objectType;

                foreach (var kv in MakeKV(JObject.Parse(empItem.ToString()), jsonRoot, false))
                {
                    emfi[kv.Key] = kv.Value;
                    if (kv.Key == anchor)
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(kv.Key, kv.Value));
                    else
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value));
                }

                CSEntries.Add(csentry1);
                EmpDic.Add(emfi);
            }
        }

        private CSEntryChange FillCSEntry<T>(T bol, string anchor, string objectType)
        {
            var csentry1 = CSEntryChange.Create();
            if (bol == null) return csentry1;

            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = objectType;

            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                var pValue = p.GetValue(bol);
                if (pValue == null) continue;

                var tp = p.PropertyType.Name;
                object ob = tp == "Boolean" ? pValue : pValue.ToString();
                if (tp == "DateTime")
                    ob = DateTime.Parse(pValue.ToString()).ToShortDateString();

                if (p.Name == anchor)
                    csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob.ToString()));
                else
                    csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
            }

            return csentry1;
        }

        private Dictionary<string, string> MakeKV(JObject emp, string objectType, bool schemaProps)
        {
            var kvs = new Dictionary<string, string>();

            foreach (KeyValuePair<string, JToken> baseItem in emp)
            {
                if (baseItem.Value.Type == JTokenType.Array)
                {
                    int roleCount = 0;
                    foreach (var fieldItem in JArray.Parse(baseItem.Value.ToString()))
                    {
                        if (fieldItem.Type == JTokenType.Object)
                        {
                            foreach (KeyValuePair<string, JToken> dataItem in JObject.Parse(fieldItem.ToString()))
                                kvs[$"{baseItem.Key.Replace(" ", "_")}{roleCount}-{dataItem.Key.Replace(" ", "_")}"] =
                                    dataItem.Value.ToString();
                        }
                        roleCount++;
                    }
                }
                else
                {
                    kvs[baseItem.Key.Replace(" ", "_")] = baseItem.Value.ToString();
                }
            }

            return kvs;
        }

        #endregion

        #region Helpers

        private static string GetEmploymentCustomFieldValueByName(
            EmploymentCustomfield[] customFields, string targetFieldName)
        {
            if (customFields == null || string.IsNullOrWhiteSpace(targetFieldName)) return "";

            var cf = customFields.FirstOrDefault(x =>
                string.Equals(x.field, targetFieldName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.fieldExternalName, targetFieldName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.fieldTemplateName, targetFieldName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.fieldTemplateExternalName, targetFieldName, StringComparison.OrdinalIgnoreCase));

            return cf?.valueExternalName ?? cf?.value ?? "";
        }

        private string SendPostRequest(string url, string data, string doMethod, out string errorMessage)
        {
            var code = "BadRequest";
            errorMessage = "";
            try
            {
                using (var client = new HttpClient(new HttpClientHandler()))
                {
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    content.Headers.Add("content_type", "application/json");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Api-Version", "v3");
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var request = new HttpRequestMessage
                    {
                        Method = new HttpMethod(doMethod),
                        RequestUri = new Uri(url),
                        Content = content
                    };

                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    code = response.StatusCode.ToString();
                    if (!response.IsSuccessStatusCode)
                        errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex) { errorMessage = ex.Message; code = "BadRequest"; }

            return code;
        }

        #endregion

        #region Text

        private string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try { return Marshal.PtrToStringBSTR(bstr); }
            finally { Marshal.FreeBSTR(bstr); }
        }

        #endregion
    }
}