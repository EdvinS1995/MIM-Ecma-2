using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace XCDRepo.Repo
{
    public class MIMRepo : IDisposable
    {
        #region Constants

        public const string ContactQuery =
            "SELECT xcdhcm__Employee_Details__r.xcdhcm__User__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Job_Role__r.xcdhcm__Location__r.Name, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Job_Role__r.xcdhcm__Department__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Employee_Number__c, " +
            "xcdhcm__Employee_Details__r.Employee_type__c, " +
            "xcdhcm__First_name__c, Organisational_hierarchy__c, Department_Team__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Office_Locations__r.xcdhcm__City__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Job_Role__r.xcdhcm__Manager_F__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Job_Role__r.xcdhcm__Manager__c, " +
            "xcdhcm__Work_mobile__c, xcdhcm__Home_phone__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Office_Locations__r.xcdhcm__Postcode__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Office_Locations__r.xcdhcm__State_Province__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Office_Locations__r.xcdhcm__Street__c, " +
            "xcdhcm__Last_name__c, xcdhcm__Work_phone__c, Job_Title__c, Pronoun__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Start_Date__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Office_Locations__r.xcdhcm__Country__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__User__r.LanguageLocaleKey, " +
            "xcdhcm__Employee_Details__r.xcdhcm__User__r.Email, " +
            "xcdhcm__Employee_Details__r.Username__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__User__r.FederationIdentifier, " +
            "xcdhcm__Leaving_date__c, " +
            "xcdhcm__Employee_Details__r.Business_Role__c, " +
            "xcdhcm__Employee_Details__r.Management_role__c, " +
            "xcdhcm__Employee_Details__r.Personas__c, " +
            "xcdhcm__Employee_Details__r.Additional_access__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Job_Role__r.xcdhcm__Division__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__Job_Role__r.xcdhcm__Team__c, " +
            "xcdhcm__Employee_Details__r.xcdhcm__User__r.IT_Display_Name__c, " +
            "xcdhcm__Employee_Details__r.IT_deactivation_date__c " +
            "FROM xcdhcm__Personal__c";

        private const int HttpTimeoutSeconds = 30;
        private const string AnchorAttributeName = "xcdhcm__Employee_Details__r-xcdhcm__User__c";
        private const string ObjectTypePerson = "Person";
        private const string ObjectTypeUser = "user";

        #endregion

        #region Properties

        public string LoginEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public string ClientID { get; set; }
        public SecureString ClientSecret { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }
        public SecureString SecToken { get; set; }
        public bool GetEmployees { get; set; }

        public string Token { get; private set; }
        public string ServiceUrl { get; private set; }
        public string JsonRoot { get; private set; }

        public IList<CSEntryChange> CSEntries { get; set; }
        public List<Dictionary<string, string>> EmpDic { get; private set; }
        public SchemaType SchemaDef { get; private set; }
        public PutExportEntriesResults ExportEntriesResults { get; private set; }

        // Shared, reusable HttpClient — avoids socket exhaustion from repeated instantiation
        private readonly HttpClient _httpClient;

        #endregion

        #region Constructor / Dispose

        public MIMRepo()
        {
            // Force TLS 1.2 globally — must be set before any outbound HTTPS request
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var handler = new HttpClientHandler
            {
                // Allow the handler to automatically decompress responses
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds)
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        #endregion

        #region Authentication

        /// <summary>
        /// Authenticates against the Salesforce OAuth2 password flow and
        /// stores the bearer token and instance URL for subsequent calls.
        /// </summary>
        public void AuthPost()
        {
            Token = null;
            ServiceUrl = null;

            string password = SecureStringToString(Password);
            string clientSecret = SecureStringToString(ClientSecret);
            string securityToken = SecureStringToString(SecToken);

            // Salesforce requires the security token appended directly to the password
            string fullPassword = password + securityToken;

            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type",    "password"    },
                { "client_id",     ClientID      },
                { "client_secret", clientSecret  },
                { "username",      Username      },
                { "password",      fullPassword  }
            });

            HttpResponseMessage response;
            try
            {
                response = _httpClient.PostAsync(LoginEndpoint, formContent).GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                throw new InvalidOperationException(
                    $"Authentication request to '{LoginEndpoint}' timed out after {HttpTimeoutSeconds} seconds. " +
                    "Check that the endpoint is reachable from this server.");
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException(
                    $"Authentication request to '{LoginEndpoint}' failed: {ex.Message}", ex);
            }

            string jsonContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Authentication failed with status {(int)response.StatusCode} ({response.StatusCode}). " +
                    $"Response: {jsonContent}");
            }

            JObject obj = JObject.Parse(jsonContent);

            Token = (string)obj["access_token"]
                ?? throw new InvalidOperationException("Authentication response did not contain an access_token.");

            ServiceUrl = (string)obj["instance_url"]
                ?? throw new InvalidOperationException("Authentication response did not contain an instance_url.");
        }

        #endregion

        #region Import (Get)

        /// <summary>
        /// Authenticates and imports all employee records as CSEntryChanges.
        /// </summary>
        public void Get()
        {
            AuthPost();
            CSEntries = new List<CSEntryChange>();

            if (GetEmployees)
            {
                JsonRoot = FetchJson(ContactQuery);
                FillCSEntriesFromJson(JsonRoot, "records", AnchorAttributeName, ObjectTypePerson);
            }
        }

        #endregion

        #region Export (Put)

        /// <summary>
        /// Processes all pending export entries (Add/Delete/Replace/Update).
        /// Add and Delete are not implemented and will return a BadRequest result.
        /// </summary>
        public void Put()
        {
            AuthPost();
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                ProcessExportEntry(csentryChange);
            }
        }

        private void ProcessExportEntry(CSEntryChange csentryChange)
        {
            IList<AttributeChange> attributeChanges = new List<AttributeChange>();

            switch (csentryChange.ObjectModificationType)
            {
                case ObjectModificationType.Add:
                    AddNotImplementedResult(csentryChange, attributeChanges);
                    break;

                case ObjectModificationType.Delete:
                    AddNotImplementedResult(csentryChange, attributeChanges);
                    break;

                case ObjectModificationType.Replace:
                case ObjectModificationType.Update:
                    ProcessUpdateEntry(csentryChange, attributeChanges);
                    break;

                default:
                    break;
            }
        }

        private void AddNotImplementedResult(CSEntryChange csentryChange, IList<AttributeChange> attributeChanges)
        {
            ExportEntriesResults.CSEntryChangeResults.Add(
                CSEntryChangeResult.Create(
                    csentryChange.Identifier,
                    attributeChanges,
                    Util.ErrValidate("BadRequest"),
                    "XCD error",
                    "Not implemented"));
        }

        private void ProcessUpdateEntry(CSEntryChange csentryChange, IList<AttributeChange> attributeChanges)
        {
            string anchor;
            string payload = BuildUpdatePayload(csentryChange, out anchor);

            if (string.IsNullOrEmpty(anchor) || string.IsNullOrEmpty(payload))
            {
                ExportEntriesResults.CSEntryChangeResults.Add(
                    CSEntryChangeResult.Create(
                        csentryChange.Identifier,
                        attributeChanges,
                        MAExportError.ExportErrorInvalidDN,
                        "XCD error",
                        "No anchor value (Employee_Details__r-User__c) or no attributes to update."));
                return;
            }

            bool success = UpdateRecord(payload, ObjectTypeUser, anchor);
            MAExportError status = success ? MAExportError.Success : Util.ErrValidate("BadRequest");

            ExportEntriesResults.CSEntryChangeResults.Add(
                CSEntryChangeResult.Create(
                    csentryChange.Identifier,
                    attributeChanges,
                    status,
                    success ? "" : "XCD error",
                    success ? "" : "Update request failed."));
        }

        #endregion

        #region Schema

        /// <summary>
        /// Authenticates and builds the connector schema by inspecting a live query response.
        /// </summary>
        public bool MakeSchema()
        {
            AuthPost();

            if (GetEmployees)
            {
                string empJson = FetchJson(ContactQuery);
                SchemaDef = BuildSchemaFromJson(empJson, "records", AnchorAttributeName, ObjectTypePerson);
            }

            return true;
        }

        private SchemaType BuildSchemaFromJson(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            SchemaType schemaDef = SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            JArray records = ParseRecordsArray(jsonData, jsonRoot);

            // Collect all unique attribute keys across all records
            var uniqueKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (JToken record in records)
            {
                var kv = FlattenJsonObject(JObject.Parse(record.ToString()));
                foreach (string key in kv.Keys)
                {
                    if (!key.Equals(anchor, StringComparison.OrdinalIgnoreCase))
                        uniqueKeys.Add(key);
                }
            }

            foreach (string key in uniqueKeys)
            {
                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(key, AttributeType.String));
            }

            return schemaDef;
        }

        #endregion

        #region API Helpers

        /// <summary>
        /// Executes a SOQL query against the Salesforce REST API and returns raw JSON.
        /// </summary>
        private string FetchJson(string soqlQuery)
        {
            string url = $"{ServiceUrl}{ApiEndpoint}/query?q={Uri.EscapeDataString(soqlQuery)}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            try
            {
                response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                throw new InvalidOperationException(
                    $"Query request timed out after {HttpTimeoutSeconds} seconds.");
            }

            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Query failed with status {(int)response.StatusCode} ({response.StatusCode}). " +
                    $"Response: {content}");
            }

            return content;
        }

        /// <summary>
        /// Sends a PATCH (via POST + _HttpMethod override) to update a Salesforce record.
        /// </summary>
        private bool UpdateRecord(string jsonPayload, string objectType, string recordId)
        {
            string uri = $"{ServiceUrl}{ApiEndpoint}/sobjects/{objectType}/{recordId}?_HttpMethod=PATCH";

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            try
            {
                response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                throw new InvalidOperationException(
                    $"Update request for {objectType}/{recordId} timed out after {HttpTimeoutSeconds} seconds.");
            }

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                // Log or surface the error body — for now returned as false so caller can handle it
                return false;
            }

            return true;
        }

        #endregion

        #region JSON → CSEntry Mapping

        private void FillCSEntriesFromJson(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            EmpDic = new List<Dictionary<string, string>>();
            JArray records = ParseRecordsArray(jsonData, jsonRoot);

            foreach (JToken record in records)
            {
                CSEntryChange csentry = CSEntryChange.Create();
                csentry.ObjectModificationType = ObjectModificationType.Add;
                csentry.ObjectType = objectType;

                var kvPairs = FlattenJsonObject(JObject.Parse(record.ToString()));
                var empDict = new Dictionary<string, string>();

                foreach (var kv in kvPairs)
                {
                    empDict[kv.Key] = kv.Value;

                    if (kv.Key.Equals(anchor, StringComparison.OrdinalIgnoreCase))
                    {
                        csentry.AnchorAttributes.Add(AnchorAttribute.Create(kv.Key, kv.Value));
                    }
                    else
                    {
                        csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value));
                    }
                }

                CSEntries.Add(csentry);
                EmpDic.Add(empDict);
            }
        }

        private static JArray ParseRecordsArray(string jsonData, string jsonRoot)
        {
            JObject root = JObject.Parse(jsonData);
            JToken token = root[jsonRoot]
                ?? throw new InvalidOperationException($"JSON root key '{jsonRoot}' not found in response.");

            return JArray.Parse(token.ToString());
        }

        /// <summary>
        /// Recursively flattens a nested Salesforce JSON object into a flat key-value dictionary.
        /// Nested keys are joined with '-' and the 'xcdhcm__' namespace prefix is stripped.
        /// </summary>
        private static Dictionary<string, string> FlattenJsonObject(JObject obj, string prefix = "")
        {
            var result = new Dictionary<string, string>();

            foreach (var property in obj)
            {
                if (property.Key == "attributes") continue;

                string key = string.IsNullOrEmpty(prefix)
                    ? property.Key
                    : $"{prefix}-{property.Key}";

                if (property.Value.Type == JTokenType.Object)
                {
                    var nested = FlattenJsonObject((JObject)property.Value, key);
                    foreach (var kv in nested)
                        result[kv.Key.Replace("xcdhcm__", "")] = kv.Value;
                }
                else
                {
                    result[key.Replace("xcdhcm__", "")] = property.Value?.ToString() ?? "";
                }
            }

            return result;
        }

        #endregion

        #region Export Payload Builder

        private string BuildUpdatePayload(CSEntryChange csentryChange, out string anchor)
        {
            anchor = "";

            foreach (var a in csentryChange.AnchorAttributes
                .Where(x => x.Name == "Employee_Details__r-User__c"))
            {
                anchor = a.Value.ToString();
            }

            string email = null;
            string username = null;

            foreach (var attributeChange in csentryChange.AttributeChanges)
            {
                var addedValue = attributeChange.ValueChanges
                    .FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);

                if (addedValue == null) continue;

                if (attributeChange.Name == "Employee_Details__r-Username__c")
                    username = addedValue.Value.ToString();

                if (attributeChange.Name == "Employee_Details__r-User__r-Email")
                    email = addedValue.Value.ToString();
            }

            var update = new XCDUpdate { Email = email, Username = username };

            string json = JsonConvert.SerializeObject(update, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            // Return empty string if there's nothing to update
            return json == "{}" ? "" : json;
        }

        public class XCDUpdate
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }

        #endregion

        #region Helpers

        private static string SecureStringToString(SecureString value)
        {
            if (value == null) return string.Empty;

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