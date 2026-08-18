using Microsoft.MetadirectoryServices;
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
using Newtonsoft.Json;
using Microsoft.Identity.Client;

namespace AzureRepo.Repo
{
    public class MIMRepo
    {
        #region Properties


        public string LoginEndpoint { get; set; } = "https://login.microsoftonline.com/9b5ff18e-53c0-45a2-8bc2-9c0c8f60b2c6";
        public string ApiEndpoint { get; set; }

        public string ClientId { get; set; }
        public string Scope { get; set; }
        public System.Security.SecureString ClientSecret { get; set; }

        public bool GetEmployees { get; set; }

        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }



        public string JsonRoot { get; set; }


        /// <summary>
        /// definition of schemna
        /// </summary>
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }



        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        public List<Dictionary<string, string>> EmpDic { get; set; }

        private HttpClient _hClient;

        #endregion

        #region Auth

        public MIMRepo()
        {
        }

        public void AuthHClient()
        {
            _hClient = new HttpClient();
            var secret = SecureStringToString(ClientSecret);
            var token = GetBearerToken(ClientId, secret, Scope, LoginEndpoint);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //_hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SecureStringToString(JWTToken)}");
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.Timeout = new TimeSpan(0, 5, 0);
            //_hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        private string GetBearerToken(string clientID, string clientSecret, string scope, string authorityUrl)
        {
            var scopes = new string[] { scope };
            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(clientID)
                .WithAuthority(authorityUrl)
                .WithClientSecret(clientSecret)
                .Build();
            try
            {
                var authResult = confidentialClient
                    .AcquireTokenForClient(scopes)
                    .ExecuteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                // Authentication failed
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #region Handlers
        public void Get()
        {
            AuthHClient();
            CSEntries = new List<CSEntryChange>();
            if (GetEmployees)
            {
                JsonRoot = GetAPI("");
                //FillCSEntriesJS(JsonRoot, "", "Employee ID", "Person");
            }

            var x = EmpDic;
        }


        public void Put()
        {
            AuthHClient();
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();

                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

                string objectType = "user";

                //try
                //{
                switch (csentryChange.ObjectModificationType)
                {
                    case ObjectModificationType.Add:
                        {
                            var poststatus = "BadRequest";
                            var errorMessage = "Not implemented";
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "XCD error";
                            }

                            status = Util.ErrValidate(poststatus);
                            //attributeChanges.Add(AttributeChange.CreateAttributeDelete("employeeID"));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));




                            //var errorMessage = "";
                            //var boo = FillRootObject(oneent, objectType, "guid", out errorMessage);

                            //if (!string.IsNullOrEmpty(errorMessage))
                            //{
                            //    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No Unit"));
                            //    break;
                            //}

                            ////User Creation Code

                            //var poststatus = PostJSON(boo, objectType, "POST", out errorMessage);
                            //if (!string.IsNullOrEmpty(errorMessage))
                            //{
                            //    errorName = "C1V3 error";
                            //}

                            //status = Util.ErrValidate(poststatus);
                            ////attributeChanges.Add(AttributeChange.CreateAttributeAdd("employeeID", boo.employeeID));
                            //ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }
                    case ObjectModificationType.Delete:
                        {

                            //User Deletion Code
                            var poststatus = "BadRequest";
                            var errorMessage = "Not implemented";
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "XCD error";
                            }

                            status = Util.ErrValidate(poststatus);
                            //attributeChanges.Add(AttributeChange.CreateAttributeDelete("employeeID"));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                            break;
                        }

                    case ObjectModificationType.Replace:
                        {
                            var anchor = "";
                            var errorMessage = "";
                            var boo = FillRootObject(csentryChange, out anchor);

                            if (string.IsNullOrEmpty(anchor) || string.IsNullOrEmpty(boo))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, objectType, anchor);

                            if (patchstatus)
                            {
                                status = MAExportError.Success;
                            }
                            //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                            break;
                        }

                    case ObjectModificationType.Update:
                        {
                            var anchor = "";
                            var errorMessage = "";
                            var boo = FillRootObject(csentryChange, out anchor);

                            if (string.IsNullOrEmpty(anchor) || string.IsNullOrEmpty(boo))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, objectType, anchor);

                            if (patchstatus)
                            {
                                status = MAExportError.Success;
                            }
                            //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                            break;
                        }
                    default:
                        break;
                }
                //}
                //catch (Exception ex)
                //{
                //    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, "Workday repo Error", ex.Message));
                //}


            }
        }


        private string FillRootObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";
            foreach (var a in cse.AnchorAttributes.Where(x => x.Name == "Employee_Details__r-User__c"))
            {
                anchor = a.Value.ToString();
            }

            var cseDic = new Dictionary<string, string>();

            string email = null;
            string username = null;

            foreach (var p in cse.AttributeChanges)
            {
                string propName = p.Name;
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val != null)
                {
                    cseDic[propName] = val.Value.ToString();
                    if (propName == "Employee_Details__r-Username__c")
                    {
                        username = val.Value.ToString();
                    }

                    if (propName == "Employee_Details__r-User__r-Email")
                    {
                        email = val.Value.ToString();
                    }
                }
            }

            var jup = new XCDUpdate { Email = email, Username = username };

            var rootJson = JsonConvert.SerializeObject(jup,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            if (rootJson == "{}")
            {
                rootJson = "";
            }

            return rootJson;
        }

        public class XCDUpdate
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }


        #endregion



        #region API


        private string GetAPI(string getType)
        {
            var baseURL = ApiEndpoint;

            string result = "";

            try
            {
                var targetURL = $"{baseURL}{getType}";
                HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();


                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    var x = result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public string PostJSON(string DATA, string endPoint, out string errorMessage)
        {
            AuthHClient();
            errorMessage = "";

            string response = SendPostRequest(ApiEndpoint, DATA, "Post", out errorMessage);

            return "";

        }

        public bool UpdateRecord(string updateMessage, string recordType, string recordId)
        {
            //AuthPost();
            HttpClient client = new HttpClient();
            HttpContent contentUpdate = new StringContent(updateMessage, Encoding.UTF8, "application/json");

            string uri = $"{ApiEndpoint}/sobjects/{recordType}/{recordId}?_HttpMethod=PATCH";

            HttpRequestMessage requestUpdate = new HttpRequestMessage(HttpMethod.Post, uri);
            requestUpdate.Headers.Add("Authorization", "Bearer ");
            requestUpdate.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestUpdate.Content = contentUpdate;

            HttpResponseMessage response = client.SendAsync(requestUpdate).Result;

            var res = response.Content.ReadAsStringAsync().Result;

            return response.IsSuccessStatusCode;

        }


        #endregion

        #region Ecma

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
            {
                AuthHClient();
                var empText = GetAPI("");
                SchemaDefs.Add("Person", CreateSchemaJS(empText, "", "Employee ID", "Person"));
            }

            return true;
        }


        private SchemaType CreateSchemaJS(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);

            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));

            var cusdic = new Dictionary<string, string>();

            var jts = jsonData;
            if (!string.IsNullOrEmpty(jsonRoot))
            {
                JObject jj = JObject.Parse(jsonData);

                JToken jt = jj[jsonRoot];
                jts = jt.ToString();
            }

            JArray jaja = JArray.Parse(jts);

            foreach (var empItem in jaja)
            {
                JObject jEmp = JObject.Parse(empItem.ToString());

                var keva = MakeKV(jEmp, jsonRoot, true);
                foreach (var kv in keva)
                {
                    if (kv.Key != anchor)
                    {
                        cusdic[kv.Key] = kv.Value;
                    }
                }
            }

            foreach (var scUniq in cusdic)
            {
                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));
            }

            return schemaDef;
        }

        private void FillCSEntriesJS(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            EmpDic = new List<Dictionary<string, string>>();

            var jts = jsonData;
            if (!string.IsNullOrEmpty(jsonRoot))
            {
                JObject jj = JObject.Parse(jsonData);

                JToken jt = jj[jsonRoot];
                jts = jt.ToString();
            }

            JArray jaja = JArray.Parse(jts);

            foreach (var empItem in jaja)
            {
                var emfi = new Dictionary<string, string>();
                CSEntryChange csentry1 = CSEntryChange.Create();
                csentry1.ObjectModificationType = ObjectModificationType.Add;
                csentry1.ObjectType = objectType;

                JObject jEmp = JObject.Parse(empItem.ToString());

                var keva = MakeKV(jEmp, jsonRoot, false);
                foreach (var kv in keva)
                {
                    if (kv.Key == anchor)
                    {
                        emfi[kv.Key] = kv.Value.ToString();
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(kv.Key, kv.Value.ToString()));
                        continue;
                    }
                    else
                    {
                        emfi[kv.Key] = kv.Value.ToString();
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value.ToString()));
                    }
                }


                CSEntries.Add(csentry1);
                EmpDic.Add(emfi);
            }
        }



        private Dictionary<string, string> MakeKV(JObject emp, string objectType, bool schemaProps)
        {

            Dictionary<string, string> kvs = new Dictionary<string, string>();
            List<string> keys = new List<string>();
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
                                keys = new List<string>
                                {
                                    $"{baseItem.Key.Replace(" ", "_")}{roleCount}",
                                    //fieldItem.Key,
                                    dataItem.Key.Replace(" ", "_")

                                };
                                var ddretKey = string.Join("-", keys);
                                kvs[ddretKey] = dataItem.Value.ToString();

                            }
                        }

                        else
                        {
                            keys = new List<string>
                            {
                                baseItem.Key,
                                //fieldItem.

                            };
                            var ddretKey = string.Join("-", keys);
                            //kvs[ddretKey.Replace("xcdhcm__", "")] = fieldItem.Value.ToString();
                        }
                        roleCount++;
                    }
                }

                else
                {
                    keys = new List<string>
                    {
                        baseItem.Key.Replace(" ", "_"),

                    };
                    var ddretKey = string.Join("-", keys);
                    kvs[ddretKey] = baseItem.Value.ToString();
                }
            }

            return kvs;

        }


        #endregion


        #region Helpers

        private string SendPostRequest(string url, string data, string doMethod, out string errorMessage)
        {
            var code = "BadRequest";
            errorMessage = "";
            try
            {

                HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                requestContent.Headers.Add("content_type", "application/json");

                _hClient.DefaultRequestHeaders.Accept.Clear();
                _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var method = new HttpMethod(doMethod);
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(url),
                    Content = requestContent
                };

                //client.Timeout = new TimeSpan(0, 3, 0);
                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                code = response.StatusCode.ToString();

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                code = "BadRequest";
            }

            return code;
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
