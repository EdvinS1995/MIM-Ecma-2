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
using UBWRepo.Entity;
using System.Reflection;
using System.Diagnostics;

namespace UBWRepo.Repo
{
    public class MIMRepo
    {
        #region Properties


        public string ApiEndpoint { get; set; }
        public string Username { get; set; }
        public System.Security.SecureString Password { get; set; }

        public bool GetEmployees { get; set; }

        public string CompanyCode { get; set; }

        public int DaysBeforeStart { get; set; }

        public int DaysAfterDisable { get; set; }

        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }



        public string JsonRoot { get; set; }
        public string JsonHandled { get; set; }


        /// <summary>
        /// definition of schemna
        /// </summary>
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }



        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        public List<Dictionary<string, string>> EmpDic { get; set; }
        public string JsonEmpDic { get; set; }

        private HttpClient _hClient;


        #endregion

        #region Auth

        public MIMRepo()
        {
        }

        public void AuthHClient()
        {
            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var aut = Encoding.UTF8.GetBytes($"{Username}:{SecureStringToString(Password)}");
            string base6 = Convert.ToBase64String(aut);
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
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
            CSEntries = new List<CSEntryChange>();
            if (GetEmployees)
            {
                var realStart = DateTime.Now.AddDays(DaysBeforeStart);
                var realEnd = DateTime.Now.AddDays(-DaysAfterDisable);

                //var attrValuesJson = GetAPI("v1/objects/attribute-values"); //GetJString("flyattrs.json"); 
                //var attrValues = JsonConvert.DeserializeObject<List<UBWAttribute>>(attrValuesJson);

                //var attrRelationsJson = GetAPI("v1/objects/attribute-relations"); //GetJString("flyattrs.json"); 
                //var attrRelations = JsonConvert.DeserializeObject<List<UBWAttribute>>(attrRelationsJson);

                //var relationsJson = GetAPI("v1/objects/relations"); //GetJString("flyattrs.json"); 
                //var relations = JsonConvert.DeserializeObject<List<UBWRelation>>(relationsJson);


                //var oneBrum = GetAPI($"v1/employees/999999");

                JsonRoot = GetAPI("v1/objects/employees"); //GetJString("flytog.json");
                var allEmps = JsonConvert.DeserializeObject<List<UBWEntity>>(JsonRoot);
                var employees = allEmps.Where(x => realStart >= x.dateFrom && realEnd < x.dateTo).ToList(); //

                var mims = new List<NMMIMObject>();
                foreach (var e in employees)
                {
                    var oneUserJson = GetAPI($"v1/employees/{e.personId}");
                    var yu = JsonConvert.DeserializeObject<UBWUser>(oneUserJson);

                    var oneMim = new NMMIMObject(yu, employees);
                    mims.Add(oneMim);
                }
                JsonHandled = JsonConvert.SerializeObject(mims);

                //var allmims = JsonConvert.SerializeObject(mims);



                CSEntries = FillCSEntriesResult(mims, "EmployeeId", "Person");

                //JsonEmpDic = JsonConvert.SerializeObject(mims);
                var y = JsonEmpDic;
            }

 

         
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
                                errorName = "UBW error";
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
                                errorName = "UBW error";
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

        private string GetJString(string filename)
        {
            string text = System.IO.File.ReadAllText($"C:\\Data\\{filename}");
            return text;
        }


        private string GetAPI(string getType)
        {
            var baseURL = ApiEndpoint;

            var targetURL = $"{baseURL}{getType}";
            HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            response.Content.Headers.ContentType.MediaType = "application/json";
            HttpContent content = response.Content;
            string result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            //var pRes = JsonConvert.DeserializeObject<SFPerPersons>(result);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(result);
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
                SchemaDefs.Add("Person", CreateSchema<NMMIMObject>("EmployeeId", "Person"));
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

            var scJson = JsonConvert.SerializeObject(cusdic);

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

        private List<CSEntryChange> FillCSEntriesResult(List<NMMIMObject> result, string anchor, string objectType)
        {
            var csen = new List<CSEntryChange>();

            foreach (var empItem in result)
            {
                var csentry1 = MakeCSEntry(empItem, anchor, objectType);
                csen.Add(csentry1);
            }

            return csen;
        }

        private List<NMMIMObject> FillMIMObject(List<UBWEntity> employees, List<UBWAttribute> attributes)
        {
            var mims = new List<NMMIMObject>();
            foreach (var e in employees)
            {
                var oneMim = new NMMIMObject(e, attributes);
                mims.Add(oneMim);
            }

            return mims;
        }

        private CSEntryChange MakeCSEntry<T>(T obj, string anchor, string objectType)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (obj == null)
            {
                return csentry1;
            }

            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = objectType;

            var wdProps = typeof(T).GetProperties();

            foreach (PropertyInfo p in wdProps)
            {
                var pValue = p.GetValue(obj);
                var tp = p.PropertyType.Name;
                var propColName = p.Name;

                if (pValue != null)
                {
                    var ob = tp == "Boolean" ? pValue : pValue.ToString();
                    if (tp == "DateTime")
                    {
                        var tx = DateTime.Parse(pValue.ToString());
                        ob = tx.ToShortDateString();
                    }

                    if (propColName == anchor)
                    {
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(propColName, ob.ToString()));
                    }
                    else
                    {
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(propColName, ob));
                    }
                }
                else
                {
                    csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(propColName, ""));
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
                if (baseItem.Value.Type == JTokenType.Object)
                {
                    var m = MakeKVObject(baseItem.Value.ToString(), baseItem.Key, schemaProps);
                    m.ToList().ForEach(x => kvs.Add(x.Key, x.Value));
                    continue;
                }

                if (baseItem.Value.Type == JTokenType.Array)
                {
                    var m = MakeKVArray(baseItem.Value.ToString(), baseItem.Key, schemaProps);
                    m.ToList().ForEach(x => kvs.Add(x.Key, x.Value));
                    continue;
                }
                keys = new List<string>
                    {
                        baseItem.Key.Replace(" ", "_"),
                    };
                var baseKey = string.Join("-", keys);
                kvs[baseKey] = baseItem.Value.ToString();
                if (schemaProps)
                {
                    kvs[baseKey] = "";
                }
            }

            return kvs;
        }


        private Dictionary<string, string> MakeKVOld(JObject emp, string objectType, bool schemaProps)
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
                            int dataitemCount = 0;
                            foreach (KeyValuePair<string, JToken> dataItem in jfff)
                            {

                                if (dataItem.Value.Type == JTokenType.Object)
                                {
                                    JObject jdi = JObject.Parse(dataItem.Value.ToString());
                                    foreach (KeyValuePair<string, JToken> dataSubItem in jdi)
                                    {
                                        keys = new List<string>
                                            {
                                                $"{baseItem.Key.Replace(" ", "_")}{roleCount}",
                                            $"{dataItem.Key.Replace(" ", "_")}{dataitemCount}",
                                                dataSubItem.Key.Replace(" ", "_")

                                            };
                                        var ddretKey = string.Join("-", keys);
                                        kvs[ddretKey] = dataSubItem.Value.ToString();
                                    }
                                }

                                else
                                {
                                    keys = new List<string>
                                            {
                                                $"{baseItem.Key.Replace(" ", "_")}{roleCount}",
                                                $"{dataItem.Key.Replace(" ", "_")}{dataitemCount}",

                                            };
                                    var ddretKey = string.Join("-", keys);
                                    kvs[ddretKey] = dataItem.Value.ToString();
                                }
                                dataitemCount++;
                            }
                            continue;
                        }

                        if (fieldItem.Type == JTokenType.Array)
                        {
                            JArray jfl = JArray.Parse(fieldItem.ToString());
                            int listCount = 0;
                            foreach (var listItem in jfl)
                            {
                                if (listItem.Type == JTokenType.Object)
                                {
                                    JObject jffl = JObject.Parse(listItem.ToString());
                                    foreach (KeyValuePair<string, JToken> listdataItem in jffl)
                                    {
                                        keys = new List<string>
                                {
                                    $"{baseItem.Key.Replace(" ", "_")}{roleCount}",
                                    $"{listdataItem.Key.Replace(" ", "_")}{listCount}"

                                };
                                        var ddretKey = string.Join("-", keys);
                                        kvs[ddretKey] = listdataItem.ToString();

                                    }
                                }
                                listCount++;
                            }
                            continue;
                        }

                        keys = new List<string>
                        {
                            $"{baseItem.Key.Replace(" ", "_")}{roleCount}",

                        };
                        var ddretAKey = string.Join("-", keys);
                        kvs[ddretAKey] = baseItem.Value.ToString();
                        roleCount++;
                    }
                    continue;
                }

                if (baseItem.Value.Type == JTokenType.Object)
                {
                    JObject jfff = JObject.Parse(baseItem.Value.ToString());
                    foreach (KeyValuePair<string, JToken> basedataItem in jfff)
                    {
                        keys = new List<string>
                                {
                                    $"{baseItem.Key.Replace(" ", "_")}",
                                    //fieldItem.Key,
                                    basedataItem.Key.Replace(" ", "_")

                                };
                        var ddretKey = string.Join("-", keys);
                        kvs[ddretKey] = basedataItem.Value.ToString();

                    }
                    continue;
                }

                keys = new List<string>
                    {
                        baseItem.Key.Replace(" ", "_"),

                    };
                var ddretsKey = string.Join("-", keys);
                kvs[ddretsKey] = baseItem.Value.ToString();

            }

            return kvs;

        }


        private Dictionary<string, string> MakeKVObject(string jo, string baseName, bool schemaProps)
        {
            Dictionary<string, string> kvs = new Dictionary<string, string>();
            List<string> keys = new List<string>();

            JObject jos = JObject.Parse(jo);
            foreach (KeyValuePair<string, JToken> item in jos)
            {
                if (item.Value.Type == JTokenType.Object)
                {
                    var m = MakeKVObject(item.Value.ToString(), $"{baseName}-{item.Key.Replace(" ", "_")}", schemaProps);
                    m.ToList().ForEach(x => kvs.Add(x.Key, x.Value));
                    continue;
                }
                if (item.Value.Type == JTokenType.Array)
                {
                    var m = MakeKVArray(item.Value.ToString(), $"{baseName}-{item.Key.Replace(" ", "_")}", schemaProps);
                    m.ToList().ForEach(x => kvs.Add(x.Key, x.Value));
                    continue;
                }
                keys = new List<string>
                                {
                                    $"{baseName.Replace(" ", "_")}",
                                    item.Key.Replace(" ", "_")

                                };
                var ddretKey = string.Join("-", keys);
                kvs[ddretKey] = item.Value.ToString();
                if (schemaProps)
                {
                    kvs[ddretKey] = "";
                }
            }
            return kvs;
        }

        private Dictionary<string, string> MakeKVArray(string ja, string baseName,bool schemaProps)
        {

            Dictionary<string, string> kvs = new Dictionary<string, string>();
            List<string> keys = new List<string>();
            JArray jff = JArray.Parse(ja);
            int aCount = 0;
            if (jff.Count == 0)
            {
                kvs[baseName] = "";
            }

            foreach (var fieldItem in jff)
            {
                if (fieldItem.Type == JTokenType.Object)
                {
                    var m = MakeKVObject(fieldItem.ToString(), $"{baseName}{aCount}", schemaProps);
                    m.ToList().ForEach(x => kvs.Add(x.Key, x.Value));
                    aCount++;
                    continue;
                }
                //keys = new List<string>
                //    {
                //        baseName.Replace(" ", "_"),
                //        fieldItem.Value.Replace(" ", "_"),
                //    };
                //var baseKey = string.Join("-", keys);
                //kvs[baseKey] = baseItem.Value.ToString();
                aCount++;
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
