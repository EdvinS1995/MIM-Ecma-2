using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TalentManagerRepo.Entity;

namespace TalentManagerRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string ApiEndpoint { get; set; }
        public string CustomerAlias { get; set; }

        public System.Security.SecureString ApiKey { get; set; }

        public bool GetEmployees { get; set; }

        public string Anchor { get; set; }

        public int DaysBeforeStart { get; set; }
        public int DaysAfterTerminate { get; set; }

        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }

        public string JsonRoot { get; set; }

        /// <summary>
        /// definition of schema
        /// </summary>
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }

        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        public List<Dictionary<string, string>> EmpDic { get; set; }

        private HttpClient _hClient;

        // ── Cached decrypted API key – set once in AuthHClient ──
        private string _apiKeyPlain;

        // ── Max concurrent employee-detail requests ──
        private const int MaxParallelRequests = 10;

        // ── Paging batch size ──
        private const int PageSize = 100;

        #endregion

        public MIMRepo()
        {
        }

        #region Auth

        public void AuthHClient()
        {
            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.Timeout = new TimeSpan(0, 3, 0);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _hClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Decrypt the secure string once and cache it
            _apiKeyPlain = SecureStringToString(ApiKey);
        }

        #endregion

        #region Handlers

        public void Get()
        {
            EmpDic = new List<Dictionary<string, string>>();
            AuthHClient();
            CSEntries = new List<CSEntryChange>();
            var allEmployees = new List<EmployeeItem>();

            if (!GetEmployees) return;

            CancellationToken ct = new CancellationToken();

            // ── Step 1: Fetch first page to find total count ──
            var firstJson = GetListAPI("&take=" + PageSize);
            var firstList = JsonConvert.DeserializeObject<EmployeeList>(firstJson);
            var empList = new List<SimpleItem>(firstList.Items);
            int totalCount = firstList.Counts.SearchCount;
            int loops = totalCount / PageSize + 1;

            // ── Step 2: Fetch remaining pages in parallel ──
            if (loops > 1)
            {
                var pageTasks = Enumerable.Range(1, loops - 1).Select(i =>
                    Task.Run(() => GetListAPI($"&skip={i * PageSize}&take={PageSize}"))
                ).ToList();

                var pages = Task.WhenAll(pageTasks).ConfigureAwait(false).GetAwaiter().GetResult();

                foreach (var page in pages)
                {
                    if (string.IsNullOrEmpty(page)) continue;
                    var nextList = JsonConvert.DeserializeObject<EmployeeList>(page);
                    if (nextList?.Items != null)
                        empList.AddRange(nextList.Items);
                }
            }

            // ── Step 3: Fetch employee details in dynamic batches with throttling ──
            allEmployees = GetAllEmpsThrottled(empList, ct)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            // ── Step 4: Convert to CSEntries ──
            var mimObjects = allEmployees.Select(m => new MIMObject(m)).ToList();
            var allentries = mimObjects.Select(emp => MakeCSEntry(emp, Anchor, "Person")).ToList();
            CSEntries = allentries;
        }

        /// <summary>
        /// Fetches all employee details with a SemaphoreSlim to throttle parallelism.
        /// Replaces the old hardcoded 3×100 batch pattern.
        /// </summary>
        private async Task<List<EmployeeItem>> GetAllEmpsThrottled(List<SimpleItem> items, CancellationToken ct)
        {
            var semaphore = new SemaphoreSlim(MaxParallelRequests);

            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync(ct);
                try
                {
                    return await GetOneEmpAsync(item.Id, ct);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var results = await Task.WhenAll(tasks);
            return results.SelectMany(x => x).ToList();
        }

        private async Task<List<EmployeeItem>> GetOneEmpAsync(int empId, CancellationToken ct)
        {
            var json = await GetEmpAPIAsync(empId.ToString(), ct);
            if (string.IsNullOrEmpty(json))
                return new List<EmployeeItem>();

            try
            {
                var oneEmp = JsonConvert.DeserializeObject<Employee>(json);
                return oneEmp?.Items ?? new List<EmployeeItem>();
            }
            catch (Exception ex)
            {
                // Log with enough context to diagnose issues in MIM event log
                Console.WriteLine($"[MIMRepo] Failed to deserialize employee {empId}: {ex.Message}");
                return new List<EmployeeItem>();
            }
        }

        public void Put()
        {
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

                string objectType = "user";

                try
                {
                    switch (csentryChange.ObjectModificationType)
                    {
                        case ObjectModificationType.Add:
                            {
                                var poststatus = "BadRequest";
                                var errorMessage = "Not implemented";
                                if (!string.IsNullOrEmpty(errorMessage))
                                    errorName = "Simployer error";

                                status = Util.ErrValidate(poststatus);
                                ExportEntriesResults.CSEntryChangeResults.Add(
                                    CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                break;
                            }

                        case ObjectModificationType.Delete:
                            {
                                var poststatus = "BadRequest";
                                var errorMessage = "Not implemented";
                                if (!string.IsNullOrEmpty(errorMessage))
                                    errorName = "Simployer error";

                                status = Util.ErrValidate(poststatus);
                                ExportEntriesResults.CSEntryChangeResults.Add(
                                    CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                break;
                            }

                        case ObjectModificationType.Replace:
                        case ObjectModificationType.Update:
                            {
                                var anchor = "";
                                var errorMessage = "";
                                var boo = MakeObject(csentryChange, out anchor);

                                if (string.IsNullOrEmpty(anchor) || string.IsNullOrEmpty(boo))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(
                                        CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                                            MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                    break;
                                }

                                var patchstatus = UpdateRecord(boo, objectType, out errorMessage);
                                if (patchstatus)
                                    status = MAExportError.Success;

                                ExportEntriesResults.CSEntryChangeResults.Add(
                                    CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                break;
                            }

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ExportEntriesResults.CSEntryChangeResults.Add(
                        CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status,
                            "Workday repo Error", ex.Message));
                }
            }
        }

        #endregion

        #region API

        private string GetListAPI(string takeSkip)
        {
            string result = "";
            int yd = 0 - DaysAfterTerminate;
            string yest = DateTime.Now.AddDays(yd).Date.ToString("yyyy-MM-dd");

            // Use cached API key
            var targetURL = $"{ApiEndpoint}{CustomerAlias}/{_apiKeyPlain}/Employees/json?incFormerEmpSince={yest}{takeSkip}";

            HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType.MediaType = "application/json";
                result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return result;
        }

        private string GetEmpAPI(string empId)
        {
            string result = "";
            // Use cached API key
            var targetURL = $"{ApiEndpoint}{CustomerAlias}/{_apiKeyPlain}/Employees/json?empid={empId}";

            HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType.MediaType = "application/json";
                result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return result;
        }

        private async Task<string> GetEmpAPIAsync(string empId, CancellationToken ct)
        {
            // Use cached API key
            var targetURL = $"{ApiEndpoint}{CustomerAlias}/{_apiKeyPlain}/Employees/json?empid={empId}";

            try
            {
                HttpResponseMessage response = await _hClient.GetAsync(targetURL, ct);
                if (response.IsSuccessStatusCode)
                {
                    response.Content.Headers.ContentType.MediaType = "application/json";
                    return await response.Content.ReadAsStringAsync();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MIMRepo] GetEmpAPIAsync error for empId {empId}: {ex.Message}");
                return string.Empty;
            }
        }

        public bool UpdateRecord(string data, string objectType, out string errorMessage)
        {
            var res = SendPostRequest(data, "POST", out errorMessage);
            return res != "BadRequest";
        }

        private string SendPostRequest(string data, string doMethod, out string errorMessage)
        {
            var code = "BadRequest";
            errorMessage = "";
            var targetURL = $"{ApiEndpoint}MIMFunctions/ImportMIMData";

            try
            {
                HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var request = new HttpRequestMessage()
                {
                    Method = new HttpMethod(doMethod),
                    RequestUri = new Uri(targetURL),
                    Content = requestContent
                };

                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                code = response.StatusCode.ToString();

                if (!response.IsSuccessStatusCode)
                    errorMessage = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                code = "BadRequest";
            }

            return code;
        }

        #endregion

        #region Converters

        private List<CSEntryChange> MakeCSEntriesJson(string jsonData, string anchor, string objectType)
        {
            var jemps = new List<Dictionary<string, string>>();
            var csEntries = new List<CSEntryChange>();

            JArray jaja = JArray.Parse(jsonData);

            foreach (var empItem in jaja)
            {
                var emfi = new Dictionary<string, string>();
                CSEntryChange csentry1 = CSEntryChange.Create();
                csentry1.ObjectModificationType = ObjectModificationType.Add;
                csentry1.ObjectType = objectType;

                JObject jEmp = JObject.Parse(empItem.ToString());
                var keva = MakeKV(jEmp, "", false);
                DateTime? hDate = null;
                DateTime? eDate = null;

                foreach (var kv in keva)
                {
                    if (kv.Key == anchor)
                    {
                        emfi[kv.Key] = kv.Value.ToString();
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(kv.Key, kv.Value.ToString()));
                        continue;
                    }

                    if ((kv.Key == "hireDate" || kv.Key == "employmentFromDate") && !string.IsNullOrEmpty(kv.Value))
                        hDate = DateTime.Parse(kv.Value.ToString());

                    if ((kv.Key == "expireDate" || kv.Key == "employmentToDate") && !string.IsNullOrEmpty(kv.Value))
                        eDate = DateTime.Parse(kv.Value.ToString());

                    emfi[kv.Key] = kv.Value.ToString();
                    csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value.ToString()));
                }

                if ((hDate != null && hDate < DateTime.Now) && (eDate == null || eDate > DateTime.Now))
                {
                    csEntries.Add(csentry1);
                    jemps.Add(emfi);
                }
            }

            EmpDic.AddRange(jemps);
            return csEntries;
        }

        private CSEntryChange MakeCSEntry<T>(T obj, string anchor, string objectType)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (obj == null) return csentry1;

            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = objectType;

            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                var pValue = p.GetValue(obj);
                if (pValue == null) continue;

                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                var tp = type.Name;

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

        private string MakeObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";
            var cseDic = new Dictionary<string, string>();

            foreach (var a in cse.AnchorAttributes)
            {
                cseDic[a.Name] = a.Value.ToString();
                anchor = a.Name;
            }

            foreach (var p in cse.AttributeChanges)
            {
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val != null)
                    cseDic[p.Name] = val.Value.ToString();
            }

            return JsonConvert.SerializeObject(
                new List<Dictionary<string, string>> { cseDic },
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private Dictionary<string, string> MakeKV(JObject emp, string objectType, bool schemaProps)
        {
            var kvs = new Dictionary<string, string>();

            foreach (KeyValuePair<string, JToken> baseItem in emp)
            {
                if (baseItem.Value.Type == JTokenType.Array)
                {
                    JArray jff = JArray.Parse(baseItem.Value.ToString());
                    int roleCount = 0;
                    foreach (var fieldItem in jff)
                    {
                        if (fieldItem.Type == JTokenType.Object)
                        {
                            JObject jfff = JObject.Parse(fieldItem.ToString());
                            foreach (KeyValuePair<string, JToken> dataItem in jfff)
                            {
                                var key = $"{baseItem.Key.Replace(" ", "_")}{roleCount}-{dataItem.Key.Replace(" ", "_")}";
                                kvs[key] = dataItem.Value.ToString();
                            }
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

        #region Ecma

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
                SchemaDefs.Add("Person", CreateSchema<MIMObject>(Anchor, "Person"));

            return true;
        }

        public SchemaType CreateSchema<T>(string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            var specProps = new HashSet<string> { anchor };

            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                if (specProps.Contains(p.Name)) continue;
                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(p.Name, Util.GetType(type.Name)));
            }

            return schemaDef;
        }

        public SchemaType CreateSchemaJson(string jsonData, string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));
            var cusdic = new Dictionary<string, string>();

            foreach (var empItem in JArray.Parse(jsonData))
            {
                var keva = MakeKV(JObject.Parse(empItem.ToString()), "", true);
                foreach (var kv in keva)
                    if (kv.Key != anchor) cusdic[kv.Key] = kv.Value;
            }

            foreach (var scUniq in cusdic)
                if (scUniq.Key != anchor)
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));

            return schemaDef;
        }

        public SchemaType CreateSchemaJsonOrg(string jsonData, string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));
            var cusdic = new Dictionary<string, string>();

            foreach (var empItem in JArray.Parse(jsonData))
                foreach (KeyValuePair<string, JToken> baseItem in JObject.Parse(empItem.ToString()))
                    cusdic[baseItem.Key] = baseItem.Value.ToString();

            foreach (var scUniq in cusdic)
                if (scUniq.Key != anchor)
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));

            return schemaDef;
        }

        #endregion

        #region Text

        private string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        #endregion
    }
}