using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecma2Connector.Repo
{
    /// <summary>
    /// Generic REST-backed repo for an ECMA2 connector. Flattens whatever JSON the
    /// target API returns into string key/value pairs, so schema and CSEntries are
    /// derived from the live response instead of a hand-written entity class.
    /// Swap FillCSEntriesJS/CreateSchemaJS's flattening logic if the target system
    /// needs typed handling (nested arrays with per-item semantics, dates, etc.).
    /// </summary>
    public class MIMRepo
    {
        #region Properties

        public string ApiEndpoint { get; set; }
        public string LoginEndpoint { get; set; }

        public string Username { get; set; }
        public SecureString Password { get; set; }
        public SecureString BearerToken { get; set; }

        public string AnchorAttributeName { get; set; } = "id";
        public string ObjectType { get; set; } = "Person";

        /// <summary>Path to the array of records within the API response, or "" if the response root is the array.</summary>
        public string JsonRoot { get; set; } = "";

        public IList<CSEntryChange> CSEntries { get; set; }
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        private HttpClient _hClient;

        #endregion

        #region Auth

        public void AuthHClient()
        {
            _hClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (BearerToken != null)
            {
                _hClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SecureStringToString(BearerToken));
            }
            else if (!string.IsNullOrEmpty(Username))
            {
                var raw = Encoding.UTF8.GetBytes($"{Username}:{SecureStringToString(Password)}");
                _hClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(raw));
            }

            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _hClient.Timeout = TimeSpan.FromMinutes(5);
        }

        #endregion

        #region Schema (used from IMAExtensible2GetSchema)

        public SchemaType MakeSchema(string sampleEndpoint)
        {
            AuthHClient();
            var json = GetAPI(sampleEndpoint);
            return CreateSchemaJS(json);
        }

        private SchemaType CreateSchemaJS(string jsonData)
        {
            var schemaDef = SchemaType.Create(ObjectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(AnchorAttributeName, AttributeType.String));

            var attributeNames = new Dictionary<string, string>();
            foreach (var record in ParseRecords(jsonData))
            {
                foreach (var kv in MakeKV(record))
                {
                    if (kv.Key != AnchorAttributeName)
                    {
                        attributeNames[kv.Key] = kv.Value;
                    }
                }
            }

            foreach (var name in attributeNames.Keys)
            {
                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(name, AttributeType.String));
            }

            return schemaDef;
        }

        #endregion

        #region Import (used from IMAExtensible2CallImport)

        public void Get(string listEndpoint)
        {
            AuthHClient();
            CSEntries = new List<CSEntryChange>();

            var json = GetAPI(listEndpoint);
            foreach (var record in ParseRecords(json))
            {
                var csentry = CSEntryChange.Create();
                csentry.ObjectModificationType = ObjectModificationType.Add;
                csentry.ObjectType = ObjectType;

                foreach (var kv in MakeKV(record))
                {
                    if (kv.Key == AnchorAttributeName)
                    {
                        csentry.AnchorAttributes.Add(AnchorAttribute.Create(kv.Key, kv.Value));
                    }
                    else if (!string.IsNullOrEmpty(kv.Value))
                    {
                        csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value));
                    }
                }

                CSEntries.Add(csentry);
            }
        }

        #endregion

        #region Export (used from IMAExtensible2CallExport)

        public void Put()
        {
            AuthHClient();
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (var csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                string errorName = "";
                string errorMessage = "";
                MAExportError status;

                switch (csentryChange.ObjectModificationType)
                {
                    case ObjectModificationType.Add:
                        errorMessage = "Not implemented";
                        errorName = "Connector error";
                        status = Util.ErrValidate("BadRequest");
                        break;

                    case ObjectModificationType.Delete:
                        errorMessage = "Not implemented";
                        errorName = "Connector error";
                        status = Util.ErrValidate("BadRequest");
                        break;

                    case ObjectModificationType.Replace:
                    case ObjectModificationType.Update:
                        var anchor = csentryChange.AnchorAttributes
                            .FirstOrDefault(a => a.Name == AnchorAttributeName)?.Value?.ToString();

                        if (string.IsNullOrEmpty(anchor))
                        {
                            status = MAExportError.ExportErrorInvalidDN;
                            errorName = "Connector error";
                            errorMessage = $"No {AnchorAttributeName} on entry";
                            break;
                        }

                        var body = BuildUpdateBody(csentryChange);
                        var patched = UpdateRecord(anchor, body, out errorMessage);
                        status = patched ? MAExportError.Success : MAExportError.ExportErrorInvalidAttributeValue;
                        break;

                    default:
                        status = MAExportError.Success;
                        break;
                }

                ExportEntriesResults.CSEntryChangeResults.Add(
                    CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
            }
        }

        private string BuildUpdateBody(CSEntryChange csentryChange)
        {
            var dict = new Dictionary<string, string>();
            foreach (var change in csentryChange.AttributeChanges)
            {
                var value = change.ValueChanges.FirstOrDefault(v => v.ModificationType == ValueModificationType.Add);
                if (value != null)
                {
                    dict[change.Name] = value.Value.ToString();
                }
            }

            return JsonConvert.SerializeObject(dict, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        #endregion

        #region API

        private string GetAPI(string subUri)
        {
            var response = _hClient.GetAsync(subUri).ConfigureAwait(false).GetAwaiter().GetResult();
            return response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private bool UpdateRecord(string recordId, string body, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), recordId) { Content = content };
                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion

        #region JSON flattening helpers

        private IEnumerable<JObject> ParseRecords(string jsonData)
        {
            var jts = jsonData;
            if (!string.IsNullOrEmpty(JsonRoot))
            {
                var root = JObject.Parse(jsonData);
                jts = root[JsonRoot]?.ToString() ?? "[]";
            }

            return JArray.Parse(jts).Select(t => (JObject)t);
        }

        /// <summary>Flattens a (possibly nested) JSON object into single-valued string attributes.</summary>
        private Dictionary<string, string> MakeKV(JObject record)
        {
            var kvs = new Dictionary<string, string>();

            foreach (var prop in record)
            {
                var key = prop.Key.Replace(" ", "_");

                if (prop.Value.Type == JTokenType.Array)
                {
                    var items = (JArray)prop.Value;
                    for (var i = 0; i < items.Count; i++)
                    {
                        if (items[i].Type == JTokenType.Object)
                        {
                            foreach (var nested in (JObject)items[i])
                            {
                                kvs[$"{key}{i}-{nested.Key.Replace(" ", "_")}"] = nested.Value?.ToString() ?? "";
                            }
                        }
                    }
                }
                else if (prop.Value.Type != JTokenType.Object)
                {
                    kvs[key] = prop.Value?.ToString() ?? "";
                }
            }

            return kvs;
        }

        #endregion

        #region Helpers

        private string SecureStringToString(SecureString value)
        {
            var bstr = Marshal.SecureStringToBSTR(value);
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
