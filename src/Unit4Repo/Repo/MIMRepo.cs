using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Unit4Repo.Entity;
using MoreLinq;
using System.Reflection.Emit;

namespace Unit4Repo.Repo
{
    public class MIMRepo
    {
        #region Properties


        public string ApiEndpoint { get; set; }
        public string Username { get; set; }
        public System.Security.SecureString Password { get; set; }


        public string Token { get; set; }
        public string ClientID { get; set; }
        public System.Security.SecureString ClientSecret { get; set; }
        public string ServiceUrl { get; set; }
        public string LoginEndpoint { get; set; }

        public bool GetEmployees { get; set; }

        public string CompanyCodes { get; set; }

        public string EmployeeCodes { get; set; }

        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }

        /// <summary>
        /// all entries
        /// </summary>
        public List<MIMObject> MIMObjects { get; set; }

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
            var aut = Encoding.UTF8.GetBytes($"{Username}:{SecureStringToString(Password)}");
            string base6 = Convert.ToBase64String(aut);
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.Timeout = new TimeSpan(0, 5, 0);
            //_hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        //public void AuthPost()
        //{
        //    Token = null;
        //    HttpClient client = new HttpClient();

        //    HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
        //        {
        //            {"grant_type", "password"},
        //            {"client_id", ClientID},
        //            {"client_secret", SecureStringToString(ClientSecret)},
        //            {"username", Username},
        //            {"password", SecureStringToString(Password)}

        //        });

        //    HttpResponseMessage tokenResponse = client.PostAsync(LoginEndpoint, content).Result;
        //    var jsonContent = tokenResponse.Content.ReadAsStringAsync().Result;

        //    JObject obj = JObject.Parse(jsonContent);

        //    Token = (string)obj["access_token"];
        //    ServiceUrl = (string)obj["instance_url"];
        //    var x = ServiceUrl;
        //}

        #endregion

        #region Handlers
        public void Get()
        {
            AuthHClient();
           
            MIMObjects = new List<MIMObject>();
            if (GetEmployees)
            {
                List<string> eCodes = EmployeeCodes.Split(new char[] { ',' }).ToList();
                List<string> cCodes = CompanyCodes.Split(new char[] { ',' }).ToList();

                var columnsText = GetAPI("columns");
                var columns = JsonConvert.DeserializeObject<List<Unit4Class>>(columnsText);

                if (cCodes.Contains("NM")) //Flytoget
                {
                    GetFlytoget(columns);
                }

                if (cCodes.Contains("EB")) //Eidsiva
                {
                    GetEidsiva(cCodes,eCodes,columns);
                }


                
            }

            var xfdata = JsonRoot;
        }


        private void GetFlytoget(List<Unit4Class> columns)
        {
            var csen = new List<CSEntryChange>();
            var resultsText = GetAPI("results");
            JsonRoot = resultsText;

            var resultsDef = JsonConvert.DeserializeObject<Unit4EmployeesFlytoget>(resultsText);

            // All rows from API
            var emps = resultsDef.result.ToList();   // List<Unit4ResultFlytoget>
            System.Diagnostics.Debug.WriteLine("Flytoget: total rows from API: " + emps.Count);

            var fields = resultsDef.fields.ToList();

            Dictionary<string, string> fieldNameDic = new Dictionary<string, string>();
            foreach (var field in fields)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9-]");
                var colName = columns.FirstOrDefault(fi => fi.columnKey == field.columnKey)?.text ?? "";
                var str = rgx.Replace(colName, "");

                fieldNameDic["_" + field.index.ToString()] = str;
            }

            // DEBUG ONCE: see mapping _n -> column name
            foreach (var kvp in fieldNameDic)
            {
                System.Diagnostics.Debug.WriteLine("Flytoget field map: " + kvp.Key + " -> " + kvp.Value);
            }

            // Find which _n contains Ressurs-ID
            string ressursIdPropName = fieldNameDic.First(kvp => kvp.Value == "Ressurs-ID").Key; // e.g. "_13"

            System.Diagnostics.Debug.WriteLine("Flytoget mapping: Ressurs-ID=" + ressursIdPropName);

            // Helper: get Ressurs-ID value from a Unit4ResultFlytoget row
            string GetRessursId(Unit4ResultFlytoget r)
            {
                if (ressursIdPropName == "_2") return r._2;
                if (ressursIdPropName == "_3") return r._3;
                if (ressursIdPropName == "_4") return r._4;
                if (ressursIdPropName == "_5") return r._5;
                if (ressursIdPropName == "_6") return r._6;
                if (ressursIdPropName == "_7") return r._7;
                if (ressursIdPropName == "_8") return r._8;
                if (ressursIdPropName == "_9") return r._9;
                if (ressursIdPropName == "_10") return r._10;
                if (ressursIdPropName == "_11") return r._11;
                if (ressursIdPropName == "_12") return r._12;
                if (ressursIdPropName == "_13") return r._13;
                if (ressursIdPropName == "_14") return r._14;
                if (ressursIdPropName == "_15") return r._15;
                if (ressursIdPropName == "_16") return r._16;
                if (ressursIdPropName == "_17") return r._17;
                if (ressursIdPropName == "_18") return r._18;
                if (ressursIdPropName == "_19") return r._19;
                if (ressursIdPropName == "_20") return r._20;
                if (ressursIdPropName == "_21") return r._21;
                if (ressursIdPropName == "_22") return r._22;
                // If somehow Ressurs-ID is in _0 or _1 (DateTime), handle here:
                if (ressursIdPropName == "_0") return r._0.ToString("yyyy-MM-dd");
                if (ressursIdPropName == "_1") return r._1.ToString("yyyy-MM-dd");

                return null;
            }

            // Group by Ressurs-ID
            var groupedById = emps.GroupBy(r => GetRessursId(r)).ToList();
            System.Diagnostics.Debug.WriteLine("Flytoget: distinct Ressurs-ID: " + groupedById.Count);

            // For each group: if 1 row -> keep it, if 2+ rows -> keep the second
            var selectedRows = new List<Unit4ResultFlytoget>();

            foreach (var g in groupedById)
            {
                var rows = g.ToList();

                if (rows.Count == 1)
                {
                    selectedRows.Add(rows[0]);       // only one -> pick it
                }
                else
                {
                    selectedRows.Add(rows[1]);       // duplicate(s) -> pick the second
                }
            }

            System.Diagnostics.Debug.WriteLine("Flytoget: rows after per-ID selection: " + selectedRows.Count);

            // Sanity check: no duplicate Ressurs-ID remain
            var dupCheck = selectedRows
                .GroupBy(r => GetRessursId(r))
                .Where(gr => gr.Count() > 1)
                .ToList();

            if (dupCheck.Any())
            {
                var dupInfo = string.Join("; ", dupCheck.Select(gr => gr.Key + ": " + gr.Count()));
                throw new Exception("Flytoget: Duplicate Ressurs-ID after selection: " + dupInfo);
            }

            // Build CSEntries from the deduplicated list
            var jRange = FillCSEntriesResult(selectedRows, fieldNameDic, "Ressurs-ID", "Person");

            csen.AddRange(jRange);
            CSEntries = csen;
        }

        private void GetEidsiva(List<string> cCodes, List<string> eCodes, List<Unit4Class> columns)
        {

            var csen = new List<CSEntryChange>();
            var resultsText = GetAPI("results");
            JsonRoot = resultsText;

            var resultsDef = JsonConvert.DeserializeObject<Unit4EmployeesEidsiva>(resultsText);

            var emps = resultsDef.result.ToList();
            //Only companycode
            var limEmps = emps.ToList(); //Where(x => x._20 == CompanyCode)
            var userEmps = emps.Where(x => cCodes.Contains(x._20) && eCodes.Contains(x._2)).ToList();
            var fuserEmps = JsonConvert.SerializeObject(userEmps);

            var quitUsers = userEmps.Where(x => x._5 < DateTime.Now || x._4 > DateTime.Now).ToList();
            var fquitUsers = JsonConvert.SerializeObject(quitUsers);
            var activeUsers = userEmps.Where(x => x._5 > DateTime.Now && x._4 < DateTime.Now).ToList();
            var factiveUsers = JsonConvert.SerializeObject(activeUsers);

            var delEmps = quitUsers.DistinctBy(x => x._1).ToList();
            var fdelEmps = JsonConvert.SerializeObject(delEmps);
            var d = delEmps.Where(x => !activeUsers.Any(y => y._1 == x._1)).ToList();

            activeUsers.AddRange(d);
            var dup1 = activeUsers.GroupBy(x => x._1)
             .Where(g => g.Count() > 1)
             .Select(x => x.Key);

            var factiveUsersdist = JsonConvert.SerializeObject(activeUsers);

            //var distContUsers = contEmps.DistinctBy(x => new { x._1, x._20 }).ToList();
            //activeUsers.AddRange(distContUsers);
            var factiveUsersAdded = JsonConvert.SerializeObject(activeUsers);

            var distActEmps = activeUsers.OrderByDescending(t => t._4).DistinctBy(x => new { x._1, x._20 }).ToList();


            JsonRoot = JsonConvert.SerializeObject(distActEmps);


            var dup2 = activeUsers.GroupBy(x => x._1)
           .Where(g => g.Count() > 1)
           .Select(x => x.Key);

            var fields = resultsDef.fields.ToList();

            Dictionary<string, string> fieldNameDic = new Dictionary<string, string>();
            foreach (var field in fields)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9-]");
                var colName = columns.FirstOrDefault(fi => fi.columnKey == field.columnKey)?.text ?? "";
                var str = rgx.Replace(colName, "");

                fieldNameDic[$"_{field.index.ToString()}"] = str;
            }
            //var fdic = JsonConvert.SerializeObject(fieldNameDic);

            var jRange = FillCSEntriesResult(distActEmps, fieldNameDic, "Ressurs-ID", "Person");

            csen.AddRange(jRange);
            //var jscent = JsonConvert.SerializeObject(csen);
            CSEntries = csen;
        }



        public void Put()
        {
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
                                errorName = "Visma HRM error";
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
                                errorName = "Visma HRM error";
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
                            var boo = MakeObject(csentryChange, out anchor);

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
                response.Content.Headers.ContentType.MediaType = "application/json";
                HttpContent content = response.Content;
                result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                //var pRes = JsonConvert.DeserializeObject<SFPerPersons>(result);
                //sfPerPersonList = pRes?.d.results.ToList();

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


        private bool UpdateRecord(string data, string objectType, string anchor)
        {

            return true;
        }

        #endregion

        #region Converters

        private MIMObject MakeMIMObject(string obj, string anchor, string objectType)
        {
            MIMObject mobj = new MIMObject
            {

            };


            return mobj;
        }

        private List<CSEntryChange> FillCSEntriesJson(string jsonData, Dictionary<string,string> colNames, string anchor, string objectType)
        {
            var csen = new List<CSEntryChange>();

            JObject jj = JObject.Parse(jsonData);
            JToken jt = jj["result"];
            var jts = jt.ToString();
            JArray jaja = JArray.Parse(jts);

            foreach (var empItem in jaja)
            {
                CSEntryChange csentry1 = CSEntryChange.Create();
                csentry1.ObjectModificationType = ObjectModificationType.Add;
                csentry1.ObjectType = objectType;

                JObject jEmp = JObject.Parse(empItem.ToString());


                foreach (var colName in colNames)
                {
                    var jja = jEmp[colName.Key];

                    if (jja != null)
                    {
                        var y = jja.ToString();
                        if (colName.Value == anchor)
                        {
                            csentry1.AnchorAttributes.Add(AnchorAttribute.Create(colName.Key, jja.ToString()));
                        }
                        else
                        {
                            csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(colName.Key, jja.ToString()));
                        }
                    }
                    else
                    {
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(colName.Key, ""));
                    }


                }

                
                //foreach (KeyValuePair<string, JToken> baseItem in jEmp)
                //{
                    

                //    var colName = colNames[baseItem.Key];

                //    if (colName == anchor)
                //    {
                //        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(colName, baseItem.Value.ToString()));
                //    }
                //    else
                //    {
                //        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(colName, baseItem.Value.ToString()));
                //    }
                //}
                csen.Add(csentry1);
            }

            return csen;
        }

        private List<CSEntryChange> FillCSEntriesResult<T>(List<T> result, Dictionary<string, string> colNames, string anchor, string objectType)
        {
            var csen = new List<CSEntryChange>();

            foreach (var empItem in result)
            {
                var csentry1 = MakeCSEntry(empItem, colNames, anchor, objectType);
                csen.Add(csentry1);
            }

            return csen;
        }



        private CSEntryChange MakeCSEntry<T>(T obj, Dictionary<string, string> colNames, string anchor, string objectType)
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
                var propColName = colNames[p.Name];

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

        private string MakeObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";


            return "";
        }


        #endregion


        #region Ecma

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
            {

                AuthHClient();
                var columnsText = GetAPI("columns");
                var columns = JsonConvert.DeserializeObject<List<Unit4Class>>(columnsText);
                SchemaDefs.Add("Person", CreateSimpleSchema("Ressurs-ID", "Person", columns));
            }

            return true;
        }

        public SchemaType CreateSimpleSchema(string anchor, string objectType, List<Unit4Class> units)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);

            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));


            List<string> specProps = new List<string>();
            specProps.Add(anchor);
            foreach (var unit in units)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9-]");
                var colName = unit.text;
                var str = rgx.Replace(colName, "");

                string name = str;
                if (specProps.Contains(name)) continue;
                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(name, AttributeType.String));
            }

            return schemaDef;
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
