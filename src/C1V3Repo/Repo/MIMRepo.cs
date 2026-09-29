using C1V3Repo.Entity;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace C1V3Repo.Repo
{
    public class MIMRepo
    {
        #region Properties


        public string BaseUrl { get; set; }
        public string ClientID { get; set; }
        public System.Security.SecureString ClientSecret { get; set; }

        public string Token { get; set; }

        public bool GetAbsence { get; set; }

        public bool GetEmployees { get; set; }

        public bool IncludeDisabled { get; set; }

        public bool IncludeInactive { get; set; }

        /// <summary>
        /// set to show name of prop i attribute
        /// </summary>
        public bool HidePropName { get; set; }

        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }


        public RootObject C1Objects { get; set; }

        public string JsonC1Root { get; set; }


        /// <summary>
        /// definition of schemna
        /// </summary>
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }

        private Dictionary<string, string> _schemDic;


        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        #endregion

        #region Auth

        public MIMRepo()
        {
        }


        public class CatToken
        {
            public string access_token { get; set; }
        }

        public void AuthV3()
        {
            var OAuthUrl = BaseUrl + "/api/accesstoken";

            Token = null;
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add("client-id", ClientID);
                client.DefaultRequestHeaders.Add("client-secret", SecureStringToString(ClientSecret));
                client.DefaultRequestHeaders.Add("grant-type", "client_credentials");
                client.DefaultRequestHeaders.Add("Api-Version", "v3");

                var result = client.GetAsync(requestUri: OAuthUrl).Result;

                if (result.IsSuccessStatusCode)
                {
                    string content = result.Content.ReadAsStringAsync().Result;
                    CatToken contentDynRes = JsonConvert.DeserializeObject<CatToken>(content);
                    Token = contentDynRes.access_token;
                }
            }

        }

        #endregion


        #region Handlers
        public void Get()
        {
            AuthV3();
            var csen = new List<CSEntryChange>();
            if (GetEmployees)
            {
                string empJson = FetchJson("employees", true,false, IncludeInactive);
                csen.AddRange(FillCSEntriesJSObj(empJson, "employees", "guid", "Person"));
                if (IncludeDisabled)
                {
                    empJson = FetchJson("employees", false, true, false);
                    csen.AddRange(FillCSEntriesJSObj(empJson, "employees", "guid", "Person"));
                }
            }
            if (GetAbsence)
            {
                string empJson = FetchJson("absences", true,false, false);
                csen.AddRange(FillCSEntriesJSObj(empJson, "absences", "guid", "absence"));
            }

            CSEntries = csen;
        }



        public void Put()
        {
            AuthV3();
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();

                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

                string objectType = "employees";
                if (csentryChange.ObjectType == "absence")
                {
                    objectType = "absences";
                }

                IList<CSEntryChange> oneent = new List<CSEntryChange>();
                oneent.Add(csentryChange);

                //try
                //{
                switch (csentryChange.ObjectModificationType)
                {
                    case ObjectModificationType.Add:
                        {
                            var errorMessage = "";
                            var boo = FillRootObject(oneent, objectType, "guid", out errorMessage);

                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No Unit"));
                                break;
                            }

                            //User Creation Code
                            
                            var poststatus = PostJSON(boo,objectType,"POST", out errorMessage);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "C1V3 error";
                            }

                            status = Util.ErrValidate(poststatus);
                            //attributeChanges.Add(AttributeChange.CreateAttributeAdd("employeeID", boo.employeeID));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }
                    case ObjectModificationType.Delete:
                        {

                            //User Deletion Code
                            var poststatus = "BadRequest";
                            var errorMessage = "Not implemented";
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "C1V3 error";
                            }

                            status = Util.ErrValidate(poststatus);
                            //attributeChanges.Add(AttributeChange.CreateAttributeDelete("employeeID"));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                            break;
                        }

                    case ObjectModificationType.Replace:
                        {
                            var errorMessage = "";
                            var boo = FillRootObject(oneent, objectType, "guid", out errorMessage);


                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = PostJSON(boo, objectType, "PATCH", out errorMessage);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "C1V3 error";
                            }

                            status = Util.ErrValidate(patchstatus);
                            //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                            break;
                        }

                    case ObjectModificationType.Update:
                        {
                            var errorMessage = "";
                            var boo = FillRootObject(oneent, objectType, "guid", out errorMessage);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }
                            var patchstatus = PostJSON(boo, objectType, "PATCH", out errorMessage);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "C1V3 error";
                            }

                            status = Util.ErrValidate(patchstatus);
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

        public void PutOld()
        {
            AuthV3();
            ExportEntriesResults = new PutExportEntriesResults();

            string errorName = "";
            string errorMessage = "";
            var status = Util.ErrValidate("BadRequest");

            var rootJson = FillRootObject(CSEntries, "employees", "guid", out errorMessage);
            string res = PostJSON(rootJson, "employees", "Patch", out errorMessage);

            //if (root?.USERS != null)
            //{
            //    var rootJson = JsonConvert.SerializeObject(root,
            //        Newtonsoft.Json.Formatting.None,
            //        new JsonSerializerSettings
            //        {
            //            NullValueHandling = NullValueHandling.Ignore
            //        });

            //    string res = PostJSON(rootJson, out errorMessage);

            //    status = Util.ErrValidate(res);
            //}

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorName = "Catalyst Repo error";
            }


            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(Guid.NewGuid(), new List<AttributeChange>(), status, errorName, errorMessage));

        }

        #endregion



        #region API


        private string FetchJson(string endPoint, bool startdate, bool getDisabled, bool getInactive)
        {
            var resourceUrl = $"{BaseUrl}/api/{endPoint}"; ///?guid=81122de3-e227-4c32-969e-245569d6a907"; 
            if (startdate)
            {
                resourceUrl += $"?startDate=1900-01-01&includeInactive={getInactive.ToString().ToLower()}";
            }
            if (getDisabled)
            {
                resourceUrl += $"?deactivatedSince=1900-01-01&employmentStatus=30";
            }
            //if (getInactive)
            //{
            //    resourceUrl += "&includeInactive=true";
            //}
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add("Access-Token", Token);
                client.DefaultRequestHeaders.Add("Api-Version", "v3");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                HttpResponseMessage response = client.GetAsync(resourceUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                response.Content.Headers.ContentType.MediaType = "application/json";
                HttpContent content = response.Content;
                string result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                //var res = client.GetStringAsync(resourceUrl).Result;

                return result;
            }
        }

        public string PostJSON(string DATA, string endPoint, string method, out string errorMessage)
        {
            AuthV3();
            string response = SendPostRequest(BaseUrl, endPoint, DATA, method, out errorMessage);

            return response;

        }


        #endregion

        #region Ecma

        public bool MakeSchema()
        {
            AuthV3();
            SchemaDefs = new Dictionary<string, SchemaType>();
            _schemDic = new Dictionary<string, string>();
            if (GetEmployees)
            {
                var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create("Person", false);
                string empJson = FetchJson("employees", true, false, IncludeInactive);
                var a1 = CreateSchemaJSObj(empJson, "employees", "guid", "Person");
                //if (IncludeDisabled)
                //{
                //    empJson = FetchJson("employees", false, true);
                //    a1 = CreateSchemaJSObj(empJson, "employees", "guid", "Person");
                //}

                foreach (var scUniq in _schemDic)
                {
                    if (scUniq.Key != "guid")
                    {
                        schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));
                    }

                }

                SchemaDefs.Add("Person", a1);
            }
            if (GetAbsence)
            {
                string empJson = FetchJson("absences", true, false, false);
                SchemaDefs.Add("absence", CreateSchemaJSObj(empJson, "absences", "guid", "absence"));
            }

            return true;
        }

        private SchemaType CreateSchemaJSObj(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            var cusdic = new Dictionary<string, string>();

            string oAnchor = $"{jsonRoot}_base_{anchor}";

            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);

            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(oAnchor, AttributeType.String));
            JObject jj = JObject.Parse(jsonData);

            foreach (JToken jjj in jj[jsonRoot])
            {
                //JToken jt = jj["employees"].First;
                JObject j1 = JObject.Parse(jjj.ToString());

                foreach (KeyValuePair<string, JToken> baseItem in j1)
                {
                    var keva = MakeKV(baseItem, jsonRoot, true);
                    foreach (var kv in keva)
                    {
                        cusdic[kv.Key] = kv.Value;
                        _schemDic[kv.Key] = kv.Value;
                    }

                }
            }


            foreach (var scUniq in cusdic)
            {
                if (scUniq.Key != oAnchor)
                {
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));
                }

            }

            string svchj = JsonConvert.SerializeObject(cusdic);

            return schemaDef;
        }

        private List<CSEntryChange> FillCSEntriesJSObj(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            string oAnchor = $"{jsonRoot}_base_{anchor}";

            var csen = new List<CSEntryChange>();

            JObject jj = JObject.Parse(jsonData);

            JToken jt = jj[jsonRoot];
            var jts = jt.ToString();
            JArray jaja = JArray.Parse(jts);

            foreach (var empItem in jaja)
            {
                CSEntryChange csentry1 = CSEntryChange.Create();
                csentry1.ObjectModificationType = ObjectModificationType.Add;
                csentry1.ObjectType = objectType;

                JObject jEmp = JObject.Parse(empItem.ToString());
                foreach (KeyValuePair<string, JToken> baseItem in jEmp)
                {
                    var keva = MakeKV(baseItem, jsonRoot, false);
                    foreach (var kv in keva)
                    {
                        if ($"{kv.Key}" == oAnchor)
                        {
                            csentry1.AnchorAttributes.Add(AnchorAttribute.Create($"{kv.Key}", kv.Value.ToString()));
                        }
                        else
                        {
                            csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(kv.Key, kv.Value.ToString()));
                        }
                    }
                }
                csen.Add(csentry1);

            }
            return csen;
        }

        public string MakeRootJson(List<Dictionary<string, string>> users, string objectType, string anchor)
        {
            string oAnchor = $"{objectType}_{anchor}";
            JArray empArray = new JArray();

            foreach (var user in users)
            {
                var j = MakeCJobject(user);
                empArray.Add(j);
            }

            JObject root = new JObject(new JProperty(objectType, empArray));

            var jj = root.ToString();

            return jj;
        }

        private JObject MakeCJobject(Dictionary<string, string> fields)
        {
            JObject j = new JObject();
            foreach (var item in fields)
            {
                string[] keys = item.Key.Split('_');
                if (keys.Length == 3)
                {
                    j.Add(keys[2], item.Value);
                }
                if (keys.Length == 6)
                {
                    JProperty jpData = new JProperty(keys[2],
                            new JObject(new JProperty(keys[3],
                            new JObject(new JProperty(keys[4],
                            new JObject(new JProperty(keys[5], item.Value)))))));
                    JObject joData = new JObject(jpData);

                    if (j.ContainsKey(keys[2]))
                    {
                        var joStruct = (JObject)j[keys[1]];

                        var joStruct2 = (JObject)j[keys[2]];
                        var joData2 = (JObject)joData[keys[2]];
                        if (joStruct2.ContainsKey(keys[3]))
                        {
                            JObject joData3 = (JObject)joData[keys[2]][keys[3]];
                            JObject joStruct3 = (JObject)j[keys[2]][keys[3]];
                            if (joStruct3.ContainsKey(keys[4]))
                            {
                                JObject joData4 = (JObject)joData[keys[2]][keys[3]][keys[4]];
                                JObject joStruct4 = (JObject)j[keys[2]][keys[3]][keys[4]];
                                if (joStruct4.ContainsKey(keys[5]))
                                {

                                }
                                else
                                {
                                    joStruct4.Add(joData4.First);
                                }
                            }
                            else
                            {
                                joStruct3.Add(joData3.First);
                            }
                        }
                        else
                        {
                            joStruct2.Add(joData2.First);
                        }
                    }
                    else
                    {
                        j.Add(jpData);
                    }

                }
                if (keys.Length == 4)
                {
                    JProperty jp2 = new JProperty(keys[3], item.Value);
                    JObject j2 = new JObject(jp2);

                    JObject j1 = (JObject)j[keys[2]];
                    JProperty jp1 = new JProperty(keys[2], j2);
                    if (j1 == null)
                    {
                        j.Add(jp1);
                    }
                    else
                    {
                        j1.Add(jp2);
                    }

                }
            }

            return j;
        }

        private JObject FillCatObject(CSEntryChange cse, string objectType, string anchor)
        {
            string oAnchor = $"{objectType}_base_{anchor}";

            var cseDic = new Dictionary<string, string>();

            foreach (var a in cse.AnchorAttributes)
            {
                cseDic[a.Name] = a.Value.ToString();
            }

            //if (cse.AnchorAttributes.Contains(oAnchor))
            //{
            //    var atch = cse.AnchorAttributes[oAnchor];
            //    if (atch != null)
            //    {
            //        var val = atch.Value;
            //        cseDic[oAnchor] = val.ToString();
            //    }
            //}

            foreach (var p in cse.AttributeChanges)
            {
                string propName = p.Name;
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val != null)
                {
                    cseDic[propName] = val.Value.ToString();
                }
            }

            JObject j = MakeCJobject(cseDic);

            return j;
        }

        private string FillRootObject(IList<CSEntryChange> csEntries, string objectType, string anchor, out string errorMessage)
        {
            string r = "";

            errorMessage = "";
            try
            {

                JArray empArray = new JArray();

                foreach (var em in csEntries)
                {
                    var oneEmp = FillCatObject(em, objectType, anchor);
                    empArray.Add(oneEmp);
                }

                JObject root = new JObject(new JProperty(objectType, empArray));

                r = root.ToString();

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

            }

            return r;
        }

        #endregion


        #region Helpers

        private string SendPostRequest(string baseurl, string endpoint, string data, string doMethod, out string errorMessage)
        {
            var code = "BadRequest";
            errorMessage = "";
            string url = $"{baseurl}/api/{endpoint}";
            try
            {
                HttpClient client = new HttpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Access-Token", Token);
                client.DefaultRequestHeaders.Add("Api-Version", "v3");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var method = new HttpMethod(doMethod);
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(url),
                    Content = requestContent
                };

                //client.Timeout = new TimeSpan(0, 3, 0);
                var response = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                code = response.StatusCode.ToString();

                var mess = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

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


        private Dictionary<string, string> MakeKV(KeyValuePair<string, JToken> baseItem, string objectType, bool schemaProps)
        {

            Dictionary<string, string> kvs = new Dictionary<string, string>();
            List<string> keys = new List<string>();

            if (baseItem.Value.Type == JTokenType.Object)
            {
                JObject jff = JObject.Parse(baseItem.Value.ToString());
                foreach (KeyValuePair<string, JToken> fieldItem in jff)
                {
                    if (fieldItem.Value.Type == JTokenType.Object)
                    {
                        JObject jfd = JObject.Parse(fieldItem.Value.ToString());
                        foreach (KeyValuePair<string, JToken> dataItem in jfd)
                        {
                            if (dataItem.Value.Type == JTokenType.Object)
                            {
                                JObject jfdd = JObject.Parse(dataItem.Value.ToString());
                                foreach (KeyValuePair<string, JToken> ddataItem in jfdd)
                                {
                                    if ((ddataItem.Value != null || schemaProps) && ddataItem.Key != "guid")
                                    {
                                        keys = new List<string>
                                        {
                                            baseItem.Key,
                                            fieldItem.Key,
                                            dataItem.Key,
                                            ddataItem.Key
                                        };
                                        var str = "";
                                        if (!HidePropName)
                                        {
                                            Regex rgx = new Regex("[^a-zA-Z0-9-]");
                                            var str1 = rgx.Replace(fieldItem.Value["name"].ToString(), "");
                                            str = $"{str1}";
                                        }


                                        var ddretKey = string.Join("_", keys);
                                        var dataI = "";
                                        if (!schemaProps)
                                        {
                                            dataI = ddataItem.Value.ToString();
                                        }
                                        kvs[$"{objectType}_{str}_{ddretKey}"] = dataI;
                                    }
                                }
                            }
                            if (dataItem.Value.Type == JTokenType.Array)
                            {
                                foreach (var item in dataItem.Value)
                                {

                                    JObject jfdd = JObject.Parse(item.ToString());
                                    string arrValue = "";
                                    foreach (KeyValuePair<string, JToken> ddataItem in jfdd)
                                    {
                                        
                                        if ((ddataItem.Value != null || schemaProps) && ddataItem.Key != "guid")
                                        {
                                            keys = new List<string>
                                        {
                                            baseItem.Key,
                                            fieldItem.Key,
                                            dataItem.Key,
                                            ddataItem.Key
                                        };
                                            var str = "";
                                            if (!HidePropName)
                                            {
                                                Regex rgx = new Regex("[^a-zA-Z0-9-]");
                                                var str1 = rgx.Replace(fieldItem.Value["name"].ToString(), "");
                                                str = $"{str1}";
                                            };

                                            var ddretKey = string.Join("_", keys);
                                            if (!schemaProps)
                                            {
                                                arrValue = arrValue + ddataItem.Value + " ";
                                            }
                                            

                                            kvs[$"{objectType}_{str}_{ddretKey}"] = arrValue.Trim();
                                        }
                                    }
                                }


                            }

                        }
                    }
                    else
                    {
                        keys = new List<string>
                                {
                                    baseItem.Key,
                                    fieldItem.Key
                                };
                        var ddretKey = string.Join("_", keys);
                        kvs[$"{objectType}_e_{ddretKey}"] = fieldItem.Value.ToString();
                    }
                }
                return kvs;
            }

            keys.Add(baseItem.Key);
            var retKey = string.Join("_", keys);
            kvs[$"{objectType}_base_{retKey}"] = baseItem.Value.ToString();
            return kvs;



        }


        #endregion

        #region Classes

        public class C1KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        #endregion

    }
}
