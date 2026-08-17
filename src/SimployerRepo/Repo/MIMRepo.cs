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
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimployerRepo.Repo
{
    public class MIMRepo
    {
        #region Static Constructor (TLS fix)

        static MIMRepo()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;

            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = false;
        }

        #endregion

        #region Properties

        // ── HVILKEN API SOM BRUKES ───────────────────────────────────────────
        public bool UseSimployer { get; set; } = true;

        // ── SIMPLOYER HRCONNECT ───────────────────────────────────────────────
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; } = "https://hrconnect.simployer.com";
        public string AuthUrl { get; set; } = "https://simplauth.simployer.com/oauth/token";

        // ── GAMMELT API ──────────────────────────────────────────────────────
        public string ApiEndpoint { get; set; }
        public string ImportUnitIds { get; set; }
        public string ExportUnitId { get; set; }
        public string IncludeSV { get; set; }
        public string UnitIdType { get; set; }
        public string Username { get; set; }
        public string SubscriptionKey { get; set; }
        public string Anchor { get; set; }
        public string PublicCustomerId { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public bool UpdateUsername { get; set; }

        // ── FELLES ──────────────────────────────────────────────────────────
        public bool GetEmployees { get; set; } = true;

        public IList<CSEntryChange> CSEntries { get; set; }
        public string JsonRoot { get; set; }
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }
        public PutExportEntriesResults ExportEntriesResults { get; set; }
        public List<Dictionary<string, string>> EmpDic { get; set; }

        private HttpClient _hClient;
        private string _accessToken;
        private string _anchor = "PersonId";

        // ── SIMPLOYER DATA ───────────────────────────────────────────────────
        private List<JObject> _persons;
        private List<JObject> _employments;
        private List<JObject> _employees;
        private List<JObject> _organizations;
        private List<JObject> _managers;
        private List<JObject> _users;

        #endregion

        public MIMRepo()
        {
        }

        #region Auth

        public void AuthPost()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = false;

            _accessToken = null;

            try
            {
                var body = JsonConvert.SerializeObject(new
                {
                    client_id = ClientId,
                    client_secret = ClientSecret,
                    audience = BaseUrl,
                    grant_type = "client_credentials"
                });

                var webRequest = (HttpWebRequest)WebRequest.Create(AuthUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.Timeout = 120000;
                webRequest.KeepAlive = false;
                webRequest.ProtocolVersion = HttpVersion.Version11;

                var bodyBytes = Encoding.UTF8.GetBytes(body);
                webRequest.ContentLength = bodyBytes.Length;

                using (var stream = webRequest.GetRequestStream())
                    stream.Write(bodyBytes, 0, bodyBytes.Length);

                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var raw = reader.ReadToEnd();
                    var json = JObject.Parse(raw);
                    _accessToken = json["access_token"]?.ToString();

                    if (string.IsNullOrEmpty(_accessToken))
                        throw new Exception($"Token response inneholder ikke access_token. Body={raw}");

                    Log($"Simployer token hentet OK. Utløper om {json["expires_in"]} sekunder.");
                }

                _hClient = new HttpClient();
                _hClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
                _hClient.DefaultRequestHeaders.Accept.Clear();
                _hClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                _hClient.Timeout = TimeSpan.FromSeconds(120);
            }
            catch (WebException webEx)
            {
                var errorDetail = "";
                if (webEx.Response != null)
                    using (var reader = new StreamReader(webEx.Response.GetResponseStream()))
                        errorDetail = reader.ReadToEnd();

                Log($"AuthPost WebException: {webEx.Message} | {errorDetail}");
                throw new Exception($"Simployer auth feilet: {webEx.Message} | {errorDetail}", webEx);
            }
            catch (Exception ex)
            {
                Log($"AuthPost feil: {ex.Message}");
                throw new Exception($"Simployer auth feilet: {ex.Message}", ex);
            }
        }

        public void AuthHClient(string fromUnitId = "")
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            _hClient = new HttpClient();
            _hClient.BaseAddress = new Uri(ApiEndpoint);

            var aut = Encoding.UTF8.GetBytes($"{Username}:{Password}");
            string base6 = Convert.ToBase64String(aut);

            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
            _hClient.DefaultRequestHeaders.Add("X-ITAS-PublicCustomerId", PublicCustomerId);
            _hClient.DefaultRequestHeaders.Add("X-ITAS-ApiKey", ApiKey);
            _hClient.DefaultRequestHeaders.Add("X-ITAS-UnitIdType", UnitIdType);
            _hClient.DefaultRequestHeaders.Add("X-ITAS-IncludeSV", IncludeSV);
            _hClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
            _hClient.DefaultRequestHeaders.Add("X-ITAS-UpdateUserName", UpdateUsername.ToString());

            if (!string.IsNullOrEmpty(fromUnitId))
                _hClient.DefaultRequestHeaders.Add("X-ITAS-FromUnitId", fromUnitId);

            _hClient.DefaultRequestHeaders.Accept.Clear();
        }

        #endregion

        #region Handlers

        public void Get()
        {
            EmpDic = new List<Dictionary<string, string>>();
            CSEntries = new List<CSEntryChange>();

            if (!GetEmployees) return;

            if (UseSimployer)
            {
                AuthPost();
                LoadAllSimployerData();

                foreach (var person in _persons)
                {
                    try
                    {
                        var entry = FillCSEntry(person);
                        if (entry != null)
                            CSEntries.Add(entry);
                    }
                    catch (Exception ex)
                    {
                        Log($"Feil ved behandling av person {person["id"]}: {ex.Message}");
                    }
                }

                Log($"Simployer import fullført. {CSEntries.Count} personer importert.");
            }
            else
            {
                List<string> uns = ImportUnitIds.Split(',').ToList();
                var allEntries = new List<CSEntryChange>();

                foreach (string s in uns)
                {
                    AuthHClient(s);
                    var fo = GetAPI();
                    JsonRoot = fo;
                    var entries = MakeCSEntriesJson(fo, Anchor, "Person");
                    allEntries.AddRange(entries);
                }

                CSEntries = allEntries;
                Log($"Gammelt API import fullført. {CSEntries.Count} personer importert.");
            }
        }

        public void Put()
        {
            ExportEntriesResults = new PutExportEntriesResults();

            if (UseSimployer)
            {
                AuthPost();

                foreach (CSEntryChange csentryChange in CSEntries)
                {
                    IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                    var status = MAExportError.Success;
                    string errorName = "";

                    try
                    {
                        var personId = csentryChange.AnchorAttributes
                            .Cast<AnchorAttribute>()
                            .FirstOrDefault(a => a.Name == _anchor)?.Value?.ToString();

                        var emailChange = csentryChange.AttributeChanges
                            .Cast<AttributeChange>()
                            .FirstOrDefault(a => a.Name == "Epost");

                        var newEmail = emailChange?.ValueChanges
                            .FirstOrDefault(v => v.ModificationType == ValueModificationType.Add)
                            ?.Value?.ToString();

                        if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(newEmail))
                        {
                            var errorMessage = "";
                            var patchOk = PatchPersonEmail(personId, newEmail, out errorMessage);
                            if (patchOk)
                                status = MAExportError.Success;
                            else
                            {
                                errorName = "Simployer PATCH feil";
                                status = MAExportError.ExportErrorCustomContinueRun;
                            }

                            Log($"PATCH person {personId} epost={newEmail} -> {(patchOk ? "OK" : errorMessage)}");

                            ExportEntriesResults.CSEntryChangeResults.Add(
                                CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                                status, errorName, errorMessage));
                        }
                        else
                        {
                            ExportEntriesResults.CSEntryChangeResults.Add(
                                CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                                MAExportError.Success, "", ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        ExportEntriesResults.CSEntryChangeResults.Add(
                            CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                            MAExportError.ExportErrorCustomContinueRun, "Simployer write-back feil", ex.Message));
                    }
                }
            }
            else
            {
                AuthHClient(ExportUnitId);

                foreach (CSEntryChange csentryChange in CSEntries)
                {
                    IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                    var status = Util.ErrValidate("BadRequest");
                    string errorName = "";

                    try
                    {
                        switch (csentryChange.ObjectModificationType)
                        {
                            case ObjectModificationType.Add:
                            case ObjectModificationType.Delete:
                                {
                                    var errorMessage = "Not implemented";
                                    errorName = "Simployer error";
                                    ExportEntriesResults.CSEntryChangeResults.Add(
                                        CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                                        Util.ErrValidate("BadRequest"), errorName, errorMessage));
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
                                            MAExportError.ExportErrorInvalidDN, "Simployer error", "No anchor"));
                                        break;
                                    }

                                    var patchstatus = UpdateRecord(boo, "user", out errorMessage);
                                    if (patchstatus) status = MAExportError.Success;

                                    ExportEntriesResults.CSEntryChangeResults.Add(
                                        CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                                        status, errorName, errorMessage));
                                    break;
                                }

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExportEntriesResults.CSEntryChangeResults.Add(
                            CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges,
                            status, "Simployer repo Error", ex.Message));
                    }
                }
            }
        }

        #endregion

        #region Simployer API

        private void LoadAllSimployerData()
        {
            Log("Henter data fra Simployer API...");

            _persons = FetchAllPages("/v1/persons");
            _employments = FetchAllPages("/v1/employments");
            _employees = FetchAllPages("/v1/employees");
            _organizations = FetchAllPages("/v1/organizations");
            _managers = FetchAllPages("/v1/persons/managerStructure");
            _users = FetchAllPages("/v1/tenants/users");
            Log($"Lastet: {_persons.Count} personer, {_employments.Count} ansettelser, " +
                $"{_employees.Count} employees, {_organizations.Count} avdelinger, " +
                $"{_managers.Count} lederrelasjoner, {_users.Count} brukere.");
        }

        private List<JObject> FetchAllPages(string endpoint)
        {
            var allItems = new List<JObject>();
            int page = 1;
            bool hasMore = true;

            while (hasMore)
            {
                var url = $"{BaseUrl}{endpoint}?page={page}&pageSize=1000";
                var response = DoWebRequest("GET", url, null);

                if (response == null)
                {
                    Log($"Feil ved henting fra {endpoint} side {page}");
                    break;
                }

                var items = JArray.Parse(response);
                foreach (var item in items)
                    allItems.Add((JObject)item);

                hasMore = items.Count == 1000;
                page++;
            }

            return allItems;
        }

        private string DoWebRequest(string method, string url, string body)
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = method;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.Timeout = 120000;
                webRequest.KeepAlive = false;
                webRequest.ProtocolVersion = HttpVersion.Version11;
                webRequest.Headers.Add("Authorization", $"Bearer {_accessToken}");

                if (body != null)
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(body);
                    webRequest.ContentLength = bodyBytes.Length;
                    using (var stream = webRequest.GetRequestStream())
                        stream.Write(bodyBytes, 0, bodyBytes.Length);
                }
                else
                {
                    webRequest.ContentLength = 0;
                }

                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                    return reader.ReadToEnd();
            }
            catch (WebException webEx)
            {
                var errorDetail = "";
                if (webEx.Response != null)
                    using (var reader = new StreamReader(webEx.Response.GetResponseStream()))
                        errorDetail = reader.ReadToEnd();

                Log($"WebRequest feil [{method} {url}]: {webEx.Message} | {errorDetail}");
                return null;
            }
            catch (Exception ex)
            {
                Log($"DoWebRequest feil [{method} {url}]: {ex.Message}");
                return null;
            }
        }

        private bool PatchPersonEmail(string personId, string newEmail, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var url = $"{BaseUrl}/v1/persons/{personId}";
                var body = JsonConvert.SerializeObject(new { primaryEmail = newEmail });
                var response = DoWebRequest("PATCH", url, body);
                return response != null;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Bygger CSEntryChange fra Simployer-person
        /// </summary>
        private CSEntryChange FillCSEntry(JObject person)
        {
            var personId = person["id"]?.ToString();
            if (string.IsNullOrEmpty(personId))
            {
                Log("Hopper over person uten id");
                return null;
            }

            // Hent aktiv ansettelse: foretrekk den uten sluttdato, ellers siste startdato
            var personEmployments = _employments?
                .Where(e => e["personId"]?.ToString() == personId)
                .ToList() ?? new System.Collections.Generic.List<JObject>();

            JObject emp = null;
            if (personEmployments.Count > 0)
            {
                // Foretrekk aktiv (ingen sluttdato eller sluttdato i fremtiden)
                emp = personEmployments
                    .Where(e => string.IsNullOrEmpty(e["endDate"]?.ToString()) ||
                                DateTime.TryParse(e["endDate"]?.ToString(), out DateTime ed) && ed > DateTime.Now)
                    .OrderByDescending(e => e["startDate"]?.ToString())
                    .FirstOrDefault()
                    ?? personEmployments
                        .OrderByDescending(e => e["startDate"]?.ToString())
                        .First();
            }
            var empl = _employees?.FirstOrDefault(e => e["personId"]?.ToString() == personId);
            var mgr = _managers?.FirstOrDefault(m => m["personId"]?.ToString() == personId);
            var orgId = person["affiliatedOrganizationId"]?.ToString();
            var org = _organizations?.FirstOrDefault(o => o["id"]?.ToString() == orgId);
            var user = _users?.FirstOrDefault(u => u["personId"]?.ToString() == personId);

            var csentry = CSEntryChange.Create();
            csentry.ObjectModificationType = ObjectModificationType.Add;
            csentry.ObjectType = "Person";

            csentry.AnchorAttributes.Add(AnchorAttribute.Create(_anchor, personId));

            // ── PERSON ───────────────────────────────────────────────────────
            AddAttr(csentry, "Fornavn", person["firstName"]?.ToString());
            AddAttr(csentry, "Etternavn", person["lastName"]?.ToString());
            AddAttr(csentry, "PersonAktiv", person["active"]?.ToString());
            AddAttr(csentry, "Epost", person["primaryEmail"]?.ToString());
            AddAttr(csentry, "Telefon", person["primaryPhone"]?.ToString());
            AddAttr(csentry, "Fodselsdato", FormatDate(person["birthdate"]?.ToString()));

            // ── LEDER ────────────────────────────────────────────────────────
            AddAttr(csentry, "LederNavn", mgr?["managerName"]?.ToString());
            AddAttr(csentry, "LederId", mgr?["managerId"]?.ToString());

            // ── ORGANISASJON ─────────────────────────────────────────────────
            AddAttr(csentry, "AvdelingNavn", org?["name"]?.ToString());
            AddAttr(csentry, "AvdelingId", org?["id"]?.ToString());
            AddAttr(csentry, "AvdelingKode", org?["departmentCode"]?.ToString());
            AddAttr(csentry, "AvdelingAktiv", org?["active"]?.ToString());

            // ── EMPLOYMENT ───────────────────────────────────────────────────
            AddAttr(csentry, "Stilling", emp?["positionName"]?.ToString());
            AddAttr(csentry, "Personalkategori", emp?["categoryName"]?.ToString());
            AddAttr(csentry, "Ansattnummer", emp?["employeeNumber"]?.ToString());
            AddAttr(csentry, "Ansattdato", FormatDate(emp?["startDate"]?.ToString()));
            AddAttr(csentry, "Sluttdato", FormatDate(emp?["endDate"]?.ToString()));
            AddAttr(csentry, "Stillingsprosent", emp?["assignmentValue"]?.ToString());
            AddAttr(csentry, "Kategori", emp?["categoryName"]?.ToString());
            AddAttr(csentry, "EmploymentId", emp?["employmentId"]?.ToString());

            // ── EMPLOYEE ─────────────────────────────────────────────────────
            AddAttr(csentry, "EmployeeId", empl?["employeeId"]?.ToString());
            AddAttr(csentry, "EmployeeAktiv", empl?["isActive"]?.ToString());

            // ── BRUKER ───────────────────────────────────────────────────────
            AddAttr(csentry, "BrukerEpost", user?["email"]?.ToString());

            return csentry;
        }

        #endregion

        #region Schema

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();

            if (GetEmployees)
            {
                if (UseSimployer)
                {
                    var schemaDef = SchemaType.Create("Person", false);

                    schemaDef.Attributes.Add(
                        SchemaAttribute.CreateAnchorAttribute(_anchor, AttributeType.String));

                    // Person
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("PersonGUID", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Fornavn", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Etternavn", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("PersonAktiv", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Telefon", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Fodselsdato", AttributeType.String));

                    // Leder
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("LederNavn", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("LederId", AttributeType.String));

                    // Organisasjon
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("AvdelingNavn", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("AvdelingId", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("AvdelingKode", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("AvdelingAktiv", AttributeType.String));

                    // Employment
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Stilling", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Personalkategori", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Ansattnummer", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Ansattdato", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Sluttdato", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Stillingsprosent", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("Kategori", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("EmploymentId", AttributeType.String));

                    // Employee
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("EmployeeId", AttributeType.String));
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("EmployeeAktiv", AttributeType.String));

                    // Bruker
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("BrukerEpost", AttributeType.String));

                    // Epost — ImportExport for write-back fra AD
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(
                        "Epost", AttributeType.String, AttributeOperation.ImportExport));

                    SchemaDefs.Add("Person", schemaDef);
                }
                else
                {
                    AuthHClient(ExportUnitId);
                    var fo = GetAPI();
                    SchemaDefs.Add("Person", CreateSchemaJson(fo, Anchor, "Person"));
                }
            }

            return true;
        }

        public SchemaType CreateSchema<T>(string anchor, string objectType)
        {
            var schemaDef = SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            var specProps = new List<string> { anchor };
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
            var schemaDef = SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            var cusdic = new Dictionary<string, string>();
            foreach (var empItem in JArray.Parse(jsonData))
            {
                var keva = MakeKV(JObject.Parse(empItem.ToString()), "", true);
                foreach (var kv in keva)
                    if (kv.Key != anchor)
                        cusdic[kv.Key] = kv.Value;
            }

            foreach (var scUniq in cusdic)
                if (scUniq.Key != anchor)
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));

            return schemaDef;
        }

        #endregion

        #region API (gammelt - beholdt)

        private string GetAPI()
        {
            string result = "";
            var targetURL = $"{ApiEndpoint}MIMFunctions/ExportMIMData";

            if (ApiEndpoint == "https://simployeramw.azure-api.net/")
                targetURL = $"{ApiEndpoint}ItasAPI-V2/ExportPerson";

            HttpResponseMessage response = _hClient.GetAsync(targetURL)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType.MediaType = "application/json";
                result = response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false).GetAwaiter().GetResult();
            }

            return result;
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

                var response = _hClient.SendAsync(request)
                    .ConfigureAwait(false).GetAwaiter().GetResult();

                code = response.StatusCode.ToString();

                if (!response.IsSuccessStatusCode)
                    errorMessage = response.Content.ReadAsStringAsync()
                        .ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                code = "BadRequest";
            }

            return code;
        }

        #endregion

        #region Converters (gammelt - beholdt)

        private List<CSEntryChange> MakeCSEntriesJson(string jsonData, string anchor, string objectType)
        {
            var jemps = new List<Dictionary<string, string>>();
            var csEntries = new List<CSEntryChange>();

            foreach (var empItem in JArray.Parse(jsonData))
            {
                var emfi = new Dictionary<string, string>();
                var csentry = CSEntryChange.Create();
                csentry.ObjectModificationType = ObjectModificationType.Add;
                csentry.ObjectType = objectType;

                var keva = MakeKV(JObject.Parse(empItem.ToString()), "", false);
                DateTime? hDate = null;
                DateTime? eDate = null;

                foreach (var kv in keva)
                {
                    if (kv.Key == anchor)
                    {
                        emfi[kv.Key] = kv.Value;
                        csentry.AnchorAttributes.Add(AnchorAttribute.Create(kv.Key, kv.Value));
                        continue;
                    }

                    if ((kv.Key == "hireDate" || kv.Key == "employmentFromDate") && !string.IsNullOrEmpty(kv.Value))
                        hDate = DateTime.Parse(kv.Value);

                    if ((kv.Key == "expireDate" || kv.Key == "employmentToDate") && !string.IsNullOrEmpty(kv.Value))
                        eDate = DateTime.Parse(kv.Value);

                    emfi[kv.Key] = kv.Value;
                    csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value));
                }

                if ((hDate != null && hDate < DateTime.Now) && (eDate == null || eDate > DateTime.Now))
                {
                    csEntries.Add(csentry);
                    jemps.Add(emfi);
                }
            }

            if (EmpDic == null) EmpDic = new List<Dictionary<string, string>>();
            EmpDic.AddRange(jemps);
            return csEntries;
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
                if (val != null) cseDic[p.Name] = val.Value.ToString();
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
                    int roleCount = 0;
                    foreach (var fieldItem in JArray.Parse(baseItem.Value.ToString()))
                    {
                        if (fieldItem.Type == JTokenType.Object)
                        {
                            foreach (KeyValuePair<string, JToken> dataItem in JObject.Parse(fieldItem.ToString()))
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

        #region Helpers

        private void AddAttr(CSEntryChange entry, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                entry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(name, value));
        }

        private string FormatDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return null;
            if (DateTime.TryParse(dateStr, out DateTime dt))
                return dt.ToString("dd.MM.yyyy");
            return dateStr;
        }

        private void Log(string message)
        {
            try
            {
                var logDir = @"C:\Temp";

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                File.AppendAllText(
                    Path.Combine(logDir, "connector.log"),
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}");
            }
            catch { }
        }

        #endregion
    }
}