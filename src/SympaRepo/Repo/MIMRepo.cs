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
using System.Reflection;
using SympaRepo.Entity;

namespace SympaRepo.Repo
{
    public class MIMRepo
    {
        #region Properties


        public string LoginEndpoint { get; set; }
        public string ApiEndpoint { get; set; }

        public string ClientId { get; set; }
        public System.Security.SecureString ClientSecret { get; set; }

        public bool GetEmployees { get; set; } = true;

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
        private string Token;

        #endregion

        #region  

        public MIMRepo()
        {
        }


        public void AuthPost()
        {
            Token = null;
            HttpClient client = new HttpClient();

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials"},
                    {"client_id", ClientId},
                    {"client_secret", SecureStringToString(ClientSecret)},
                    //{"username", Username},

                });

            HttpResponseMessage tokenResponse = client.PostAsync(LoginEndpoint, content).Result;
            var jsonContent = tokenResponse.Content.ReadAsStringAsync().Result;

            JObject obj = JObject.Parse(jsonContent);

            Token = (string)obj["access_token"];
            var x = Token;


            _hClient = new HttpClient();

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            //_hClient.DefaultRequestHeaders.Add("If-Match", "*");
            _hClient.BaseAddress = new Uri(ApiEndpoint);

            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //ServiceUrl = (string)obj["instance_url"];
            //var x = ServiceUrl;
        }

        public void AuthHClient()
        {
            var secApiKey = SecureStringToString(ClientSecret);

            var aut = Encoding.UTF8.GetBytes($"{ClientId}:{secApiKey}");
            string base6 = Convert.ToBase64String(aut);
            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //_hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SecureStringToString(JWTToken)}");
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {base6}");
            _hClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.Timeout = new TimeSpan(0, 5, 0);
            //_hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        #endregion


        #region Handlers
        public void Get()
        {
            AuthHClient();
            //AuthPost();
            CSEntries = new List<CSEntryChange>();
            if (GetEmployees)
            {
                //var usr = GetAPI("UserHandling.svc/Users", 0, 0);

                var resultAnsatte = GetAPI("AD_ansatte", 0, 0);
                //var resultLeger = GetAPI("AD_leger", 0, 0);

                FillCSEntriesJS(resultAnsatte, "value", "samaccountname", "Person");

            }

            var x = CSEntries;
        }


        public void Put()
        {
            AuthHClient();
            //AuthPost();
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
                                errorName = "IFS error";
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
                                errorName = "IFS error";
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

                            if (string.IsNullOrEmpty(anchor))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, anchor, out errorMessage);

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

                            if (string.IsNullOrEmpty(anchor))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, anchor, out errorMessage);

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


        private UserEmailUpdate FillRootObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";
            var userEmail = new UserEmailUpdate();
            //foreach (var a in cse.AnchorAttributes.Where(x => x.Name == "id"))
            //{
            //    anchor = a.Value.ToString();
            //}

            var cseDic = new Dictionary<string, string>();

            foreach (var p in cse.AttributeChanges)
            {
                string propName = p.Name;
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val != null)
                {
                    cseDic[propName] = val.Value.ToString();

                    if (propName == "mail")
                    {
                        userEmail.mail = val.Value.ToString();
                    }
                    if (propName == "samaccountname")
                    {
                        anchor = val.Value.ToString();
                    }
                }
            }

            return userEmail;

        }




        #endregion

        #region Convert

        private List<MIMObject> MakeMimObjects()
        {
            //ApiEndpoint = "https://elvia-cfg.ifs.cloud/main/ifsapplications/projection/v1/";

            //var empInfo = GetAPI("PersonnelFileEmployementInfoHandling.svc/AllEmployees");
            //var empPeriod = GetAPI("PersonnelFileEmployementPeriodsHandling.svc/AllEmployees");
            //var compPerson = GetAPI("PersonnelFileEmployementInfoHandling.svc/CompanyPersonSet(CompanyId='01',EmpNo='45195')");
            //var onecompPerson = GetAPI("PersonnelFileEmployementInfoHandling.svc/CompanyPersonSet?$skip=0&$top=20");

            //var personell = GetIFSPersonel("PersonnelFileEmployementInfoHandling.svc/CompanyPersonSet");

            //var empStatus = GetAPI("PersonnelFileEmployementPeriodsHandling.svc/ActiveEmployees");




            var mimObjects = new List<MIMObject>();

            //var users = GetAPI("UserHandling.svc/Users",0,20); ///Users
            //var userRoot = JsonConvert.DeserializeObject<UsersRoot>(users);
            //var userValues = userRoot.value;

            //mimObjects.AddRange(userValues.Select(user => new MIMObject(user)));
            //var emps = GetAPI("PersonHandling.svc/PersonInfoSet", 0, 20);
            //var empRoot = JsonConvert.DeserializeObject<PersonRoot>(emps);
            //var empValues = empRoot.value;

            //foreach (var mims in mimObjects)
            //{
            //    var uperson = empValues.FirstOrDefault(x => x.UserId == mims.Identity);
            //    if (uperson != null)
            //    {
            //        mims.Name = uperson.Name;
            //        mims.FirstName = uperson.FirstName;
            //        mims.LastName = uperson.LastName;
            //        mims.Title = uperson.Title;
            //        mims.JobTitle = uperson.JobTitle;
            //        mims.CreationDate = uperson.CreationDate;
            //        mims.Party = uperson.Party;
            //        mims.Country = uperson.Country;
            //        mims.DefaultLanguage = uperson.DefaultLanguage;
            //        mims.PartyType = uperson.PartyType;
            //        mims.PersonId = uperson.PersonId;
            //        mims.Inactive = uperson.Inactive;
            //    }
            //}



            return mimObjects;


        }

        #endregion



        #region API


        //private List<PersonelFile> GetIFSPersonel(string getType)
        //{
        //    var ifsObjects = new List<PersonelFile>();


        //    var result = GetAPI(getType, 0, 0);
        //    var empRoot = JsonConvert.DeserializeObject<RootPersonelFile>(result);
        //    ifsObjects.AddRange(empRoot.value);
        //    //int i = 0;
        //    //while (empRoot.value.Length > 0)
        //    //{
        //    //    i++;
        //    //    var skip = empRoot.value.Length;
        //    //    result = GetAPI(getType, skip * i, 100);

        //    //    empRoot = JsonConvert.DeserializeObject<RootPersonelFile>(result);
        //    //    ifsObjects.AddRange(empRoot.value);
        //    //}




        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return ifsObjects;
        //}



        private List<T> GetIFSObjects<T>(string getType)
        {
            var ifsObjects = new List<T>();


            var result = GetAPI(getType, 0, 100);
            var empRoot = JsonConvert.DeserializeObject<T>(result);
            ifsObjects.Add(empRoot);


            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ifsObjects;
        }


        private string GetAPI(string getType, int skip, int top)
        {
            var baseURL = ApiEndpoint;
            string result = "";

            try
            {
                var targetURL = $"{baseURL}{getType}";
                if (top > 0)
                {
                    targetURL = $"{baseURL}{getType}?$skip={skip}&$top={top}";
                }


                HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
                response.Content.Headers.ContentType.MediaType = "application/json"; // ;odata.metadata=minimal";
                HttpContent content = response.Content;
                result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                //var pRes = JsonConvert.DeserializeObject<SFPerPersons>(result);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(result);
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

            //string response = SendPostRequest(resourceUrl, DATA, "Post", out errorMessage);

            return "";

        }

        public bool UpdateRecord(UserEmailUpdate data, string anchor, out string errorMessage)
        {
            string responseData = "";

            JObject jObject = JObject.FromObject(data);
            jObject["@odata.type"] = "AD_ansatte";


            var rootJson = JsonConvert.SerializeObject(jObject,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            if (rootJson == "{}")
            {
                rootJson = "";
            }

            var subUrl = $"AD_ansatte({anchor})";
            var updateUser = SendPostRequest(ApiEndpoint, rootJson, "PATCH", subUrl, out errorMessage, out responseData);

            return false;
        }

        private string SendPostRequest(string baseURL, string data, string doMethod, string subUrl, out string errorMessage, out string responseData)
        {
            var code = "BadRequest";
            errorMessage = "";
            responseData = "";
            var targetURL = $"{baseURL}{subUrl}";
            try
            {
                HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var method = new HttpMethod(doMethod);
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(targetURL),
                    Content = requestContent
                };

                //client.Timeout = new TimeSpan(0, 3, 0);

                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                code = response.StatusCode.ToString();

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"Errorcode: {response.StatusCode}, {response.ReasonPhrase} for {response.RequestMessage}";
                    throw new Exception(errorMessage);
                }
                else
                {
                    responseData = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }


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
            {
                AuthPost();
                var result = GetAPI("PersonnelFileEmployementInfoHandling.svc/CompanyPersonSet", 0, 0);
                SchemaDefs.Add("Person", CreateSchemaJS(result, "value", "EmpNo", "Person"));
            }

            return true;
        }


        public SchemaType CreateSchema<T>(string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);

            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));


            List<string> specProps = new List<string>();
            specProps.Add(anchor);

            var wdProps = typeof(T).GetProperties();

            foreach (PropertyInfo p in wdProps)
            {
                if (specProps.Contains(p.Name)) continue;
                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                string typeName = type.Name;
                AttributeOperation op = new AttributeOperation();

                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(p.Name, Util.GetType(typeName)));
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

        private CSEntryChange FillCSEntry<T>(T bol, string anchor, string objectType)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (bol == null)
            {
                return csentry1;
            }

            //csentry1.DN = $"{bol.badgeNo}/{bol.UnitExtId}";
            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = objectType;

            var wdProps = typeof(T).GetProperties();

            foreach (PropertyInfo p in wdProps)
            {
                var pValue = p.GetValue(bol);
                var tp = p.PropertyType.Name;

                if (pValue != null)
                {
                    var ob = tp == "Boolean" ? pValue : pValue.ToString();
                    if (tp == "DateTime")
                    {
                        var tx = DateTime.Parse(pValue.ToString());
                        ob = tx.ToShortDateString();
                    }

                    if (p.Name == anchor)
                    {
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob.ToString()));
                    }
                    else
                    {
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
                    }
                }
            }
            return csentry1;
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
                HttpClientHandler hand = new HttpClientHandler();

                using (HttpClient client = new HttpClient(hand))
                {
                    HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                    requestContent.Headers.Add("content_type", "application/json");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Api-Version", "v3");

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var method = new HttpMethod(doMethod);
                    var request = new HttpRequestMessage()
                    {
                        Method = method,
                        RequestUri = new Uri(url),
                        Content = requestContent
                    };

                    //client.Timeout = new TimeSpan(0, 3, 0);
                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    code = response.StatusCode.ToString();

                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }

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
