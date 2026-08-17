using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using BambooRepo.Entity;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace BambooRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string ApiEndpoint { get; set; }
        //public string Username { get; set; }

        public string CompanyCode { get; set; }

        public System.Security.SecureString ApiKey { get; set; }

        public string ReportName { get; set; }

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


        private HttpClient _hClient;

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

            var pw = "==81_indicate_INDICATE_string_88==";
            string apiu = SecureStringToString(ApiKey);
            //string pw = SecureStringToString(password);

            var aut = Encoding.UTF8.GetBytes($"{apiu}:{pw}");
            //var aut = Encoding.UTF8.GetBytes($"{apiu}");
            string base6 = Convert.ToBase64String(aut);
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
            _hClient.DefaultRequestHeaders.Add("apikey", apiu);
            _hClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _hClient.DefaultRequestHeaders.Accept.Clear();
            //_hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        #endregion

        #region Handlers
        public void Get()
        {
            AuthHClient();
            //GetAPI();
            CSEntries = new List<CSEntryChange>();
            if (GetEmployees)
            {
                var fo = GetPostAPI();
                JsonRoot = fo;

                CSEntries = MakeCSEntriesJson(fo, "employees", "employeeNumber", "Person");
            }

            var x = CSEntries;


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

                try
                {
                    switch (csentryChange.ObjectModificationType)
                    {
                        case ObjectModificationType.Add:
                            {
                                var poststatus = "BadRequest";
                                var errorMessage = "Not implemented";
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    errorName = "Bamboo error";
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
                                    errorName = "Bamboo error";
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
                                var boo = MakeObject(csentryChange, out anchor);

                                if (string.IsNullOrEmpty(anchor) || string.IsNullOrEmpty(boo))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                    break;
                                }

                                var patchstatus = UpdateRecord(boo, $"employees/{anchor}", out errorMessage);

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
                                var boo = MakeObject(csentryChange, out anchor);

                                if (string.IsNullOrEmpty(anchor) || string.IsNullOrEmpty(boo))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                    break;
                                }

                                var patchstatus = UpdateRecord(boo, $"employees/{anchor}", out errorMessage);

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
                }
                catch (Exception ex)
                {
                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, "Bamboo repo Error", ex.Message));
                }


            }
        }


        #endregion


        #region API

        public string GetAPI()
        {
            //var baseURL = "https://api.bamboohr.com/api/gateway.php/bahras/v1/employees/114/tables/jobInfo"; // ApiEndpoint;
            //var baseURL = "https://api.bamboohr.com/api/gateway.php/bahras/v1/meta/tables";
            var baseURL = "https://api.bamboohr.com/api/gateway.php/bahras/v1/employees/114/?fields=id%2CemployeeNumber%2CemploymentHistoryStatus%2Csupervisor%2Csupervisoreid&onlyCurrent=false%22";
            string result = "";

            var targetURL = $"{baseURL}";
            HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType.MediaType = "application/json";
                HttpContent content = response.Content;
                result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }

            return result;
        }

        public string GetPostAPI()
        {
            //\u00e5 = å
            //\u00f8 = ø;
            //\u00d8 = Ø
            //\u00e9 = è
            //\u00e6 = æ

            //AuthHClient();
            string errorMessage = "";
            string responseData = "";

            BamLastchanged bc = new BamLastchanged
            {
                includeNull = "no",
                value = DateTime.Now //.AddMonths(-5),
            };

            BamFilters bf = new BamFilters { lastChanged = bc };

            List<string> fields = new List<string>
            { "firstName", "lastName"};

            if (CompanyCode == "XF")
            {

                fields = new List<string>
            {
                "-38", "1", "10", "1037", "11", "12", "13", "1355", "1357", "1358", "1358", "14",
              "15", "1502", "154", "155",  "157",  "16", "16.1",
              "1610", "17", "18",  "1923", "1924", "1925", "1926", "1927", "1928",
              "1929", "1930", "1931", "1932", "1933", "1934", "1935", "1936", "2",
                "3", "387", "387", "3991", "3992",  "4", "4001", "4020",
              "4020.1", "4020.2", "4020.3", "4020.4", "4020.5", "4020.6", "4020.7",
               "4045", "4046", "4047", "4056", "4057", "4058",  "4175",
              "4293", "4294", "4295", "4296", "4297", "4298", "4299", "4300", "4301", "4302",
              "4313", "4314", "4315",  "4333",  "4409",
              "4490", "5", "6", "634", "635", "636",  "7",
              "8", "9", "91","supervisoreid","customjobtitleenglish","customjobtitleformal","customsignaturename","4497","3","supervisorId","supervisorEmail","supervisorEId","supervisor","hireDate","originalHireDate","bestEmail","homeEmail","customIwouldliketogetupdatesfromBAHRAlumni"
            };
            }
            if (CompanyCode == "QY")
            {
                fields = new List<string>
                {
                    "firstName", "lastName","-38", "1", "10", "1037", "11", "12", "13", "1355", "1357", "1358", "1358", "14",
              "15", "1502", "154", "155",  "157",  "16", "16.1",
              "1610", "17", "18",  "1923", "1924", "1925", "1926", "1927", "1928",
              "1929", "1930", "1931", "1932", "1933", "1934", "1935", "1936", "2",
                "3", "387", "387", "3991", "3992",  "4", "4001", "4020",
              "4020.1", "4020.2", "4020.3", "4020.4", "4020.5", "4020.6", "4020.7",
               "4045", "4046", "4047", "4056", "4057", "4058",  "4175",
              "4293", "4294", "4295", "4296", "4297", "4298", "4299", "4300", "4301", "4302",
              "4313", "4314", "4315",  "4333",  "4409","workPhone","fullName1",
              "4490", "5", "6", "634", "635", "636",  "7","supervisorId","supervisorEmail","supervisorEId","supervisor","hireDate","originalHireDate",
              "8", "9", "91", "3"
                };
            }
            


            BamReportPost pd = new BamReportPost
            {
                title = ReportName,
                filters = bf,
                fields = fields.ToArray()
            };

            var data = JsonConvert.SerializeObject(pd);

            var res = SendPostRequest(data, "POST", "reports/custom?format=JSON&onlyCurrent=false", out errorMessage, out responseData);

            return responseData;

        }

        public bool UpdateRecord(string data, string subUrl, out string errorMessage)
        {
            string responseData = "";
            var res = SendPostRequest(data, "POST", subUrl, out errorMessage, out responseData);

            if (res != "BadRequest")
            {
                return true;
            }

            return false;
        }

        private string SendPostRequest(string data, string doMethod, string subUrl, out string errorMessage, out string responseData)
        {
            var code = "BadRequest";
            errorMessage = "";
            responseData = "";
            var baseURL = ApiEndpoint;
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

        #region Converters

        private List<CSEntryChange> MakeCSEntriesJson(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            var csEntries = new List<CSEntryChange>();

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
                    if (baseItem.Key == anchor)
                    {
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(baseItem.Key, baseItem.Value.ToString()));
                    }
                    else
                    {
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(baseItem.Key, baseItem.Value.ToString()));
                    }
                }

                csEntries.Add(csentry1);
            }

            return csEntries;
        }


        public CSEntryChange MakeCSEntry<T>(T obj, string anchor, string objectType)
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
                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                var tp = type.Name;

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


        public string MakeObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";
            var cseDic = new Dictionary<string, string>();

            foreach (var a in cse.AnchorAttributes)
            {
                //cseDic[a.Name] = a.Value.ToString();
                anchor = a.Value.ToString();
            }

            List<string> specProps = new List<string>
            {
                "workPhone",
                "workEmail"
            };

            foreach (var p in cse.AttributeChanges)
            {
                string propName = p.Name;
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val != null && specProps.Contains(propName))
                {
                    cseDic[propName] = val.Value.ToString();
                }
            }

            var rootJson = JsonConvert.SerializeObject(cseDic,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            return rootJson;
        }


        #endregion


        #region Ecma



        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
            {
                SchemaDefs.Add("Person", CreateSchema<string>("employeeNumber", "Person"));
            }


            return true;
        }

        public bool MakeJSSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
            {
                AuthHClient();
                var fo = GetPostAPI();

                SchemaDefs.Add("Person", CreateSchemaJson(fo, "employees", "employeeNumber", "Person"));
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


        public SchemaType CreateSchemaJson(string jsonData, string jsonRoot, string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));
            JObject jj = JObject.Parse(jsonData);

            JToken jt = jj[jsonRoot];
            var jts = jt.ToString();
            JArray jaja = JArray.Parse(jts);
            var cusdic = new Dictionary<string, string>();

            foreach (var empItem in jaja)
            {
                JObject jEmp = JObject.Parse(empItem.ToString());
                foreach (KeyValuePair<string, JToken> baseItem in jEmp)
                {
                    cusdic[baseItem.Key] = baseItem.Value.ToString();
                }
            }

            foreach (var scUniq in cusdic)
            {
                if (scUniq.Key != anchor)
                {
                    schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(scUniq.Key, AttributeType.String));
                }
            }


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
