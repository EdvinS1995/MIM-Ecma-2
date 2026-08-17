using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using VismaHRMRepo.Entity;
using System.Net.Http.Headers;
using System.Net;
using MoreLinq;

namespace VismaHRMRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string LoginEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public string ClientID { get; set; }

        public string OrgFilePath { get; set; }
        public string CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public System.Security.SecureString ClientSecret { get; set; }

        public string Username { get; set; }
        public System.Security.SecureString Password { get; set; }

        public int DaysBeforeStart { get; set; }
        public int DaysAfterTerminate { get; set; }


        public string Token { get; set; }

        public string ServiceUrl { get; set; }

        public bool GetEmployees { get; set; }

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
            //_hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
            //_hClient.BaseAddress = new Uri(ApiEndpoint);
            //_hClient.Timeout = new TimeSpan(0, 5, 0);
            //_hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public void AuthPost()
        {
            Token = null;
            HttpClient client = new HttpClient();

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", ClientID},
                    {"client_secret", SecureStringToString(ClientSecret)},
                    {"username", Username},
                    {"password", SecureStringToString(Password)}

                });

            HttpResponseMessage tokenResponse = client.PostAsync(LoginEndpoint, content).Result;
            var jsonContent = tokenResponse.Content.ReadAsStringAsync().Result;

            JObject obj = JObject.Parse(jsonContent);

            Token = (string)obj["access_token"];
            ServiceUrl = (string)obj["instance_url"];
            var x = ServiceUrl;
        }

        #endregion

        #region Handlers
        public void Get()
        {
            AuthHClient();
            CSEntries = new List<CSEntryChange>();
            MIMObjects = new List<MIMObject>();
            if (GetEmployees)
            {
                var org = GetFile(OrgFilePath);
                var orgs = ConvertXML<charts>(org);

                if (string.IsNullOrEmpty(org))
                {
                    string tod = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    var webOrg = GetAPI($"hrm_ws/secure/organization/chart/{tod}/{CompanyId}");
                    if (!string.IsNullOrEmpty(webOrg))
                    {
                        orgs = ConvertXML<charts>(webOrg);
                    }
                }


                //var usrs = GetAPI("enterprise_ws/secure/user");
                //var u1 = GetAPI("enterprise_ws/secure/user/23472");
                //var users = ConvertXML<users>(usrs);
                //var emp = GetAPI("hrm_ws/secure/persons/userid/1142");
                //var emps = GetAPI($"hrm_ws/secure/persons/company/100/start-id/0/end-id/99999");

                //var emps2 = GetAPI($"hrm_ws/secure/persons/company/100/terminated/date-interval/2021-09-01/2021-11-05");
                //var emps = GetAPI("hrm_ws/secure/persons/company/100/start-id/11109/end-id/11113");
                //var persons = ConvertXML<personsXML>(emps);


                List<personsXMLPerson> allPers = new List<personsXMLPerson>();
                List<string> cSplit = CompanyId.Split(',').ToList();

                foreach (var c in cSplit)
                {
                    var ps = GetPersons(c);
                    allPers.AddRange(ps);
                }

                var disPers = allPers.DistinctBy(xpe => xpe.personIdHRM).ToList();


                foreach (var ffo in disPers)
                {
                    var mob = MakeMIMObject(ffo, orgs, "userId", "Person");
                    MIMObjects.Add(mob);
                    CSEntries.Add(MakeCSEntry(mob, "userId", "Person"));

                }
            }

            JsonRoot = JsonConvert.SerializeObject(MIMObjects);
            var x = JsonRoot;
        }


        private List<personsXMLPerson> GetPersons(string companyId)
        {
            List<personsXMLPerson> people = new List<personsXMLPerson>();

            var emps = GetAPI($"hrm_ws/secure/persons/company/{companyId}/start-id/0/end-id/99999");
            var persons = ConvertXML<personsXML>(emps);
            if (persons?.person != null)
            {
                people.AddRange(persons.person);
            }

            int yd = 0 - DaysAfterTerminate;


            string tod = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string yest = DateTime.Now.AddDays(yd).Date.ToString("yyyy-MM-dd");
            string tomw = DateTime.Now.AddDays(DaysBeforeStart).Date.ToString("yyyy-MM-dd");

            var empsQ = GetAPI($"hrm_ws/secure/persons/company/{companyId}/terminated/date-interval/{yest}/{tod}");
            var personsQ = ConvertXML<personsXML>(empsQ);
            if (personsQ?.person != null)
            {
                people.AddRange(personsQ.person);
            }

            var empsNS = GetAPI($"hrm_ws/secure/persons/company/{companyId}/not-started/date-interval/{tod}/{tomw}");
            var personsNS = ConvertXML<personsXML>(empsNS);
            if (personsNS?.person != null)
            {
                people.AddRange(personsNS.person);
            }


            return people.ToList();
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
                            var boo = FillMIMObject(csentryChange, out anchor);

                            if (string.IsNullOrEmpty(anchor))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, out errorMessage);

                            if (patchstatus)
                            {
                                status = MAExportError.Success;
                            }
                            else
                            {
                                status = MAExportError.ExportErrorInvalidAttributeValue;
                            }
                            //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                            break;
                        }

                    case ObjectModificationType.Update:
                        {
                            var anchor = "";
                            var errorMessage = "";
                            var boo = FillMIMObject(csentryChange, out anchor);

                            if (string.IsNullOrEmpty(anchor))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, out errorMessage);

                            if (patchstatus)
                            {
                                status = MAExportError.Success;
                            }
                            else
                            {
                                status = MAExportError.ExportErrorInvalidAttributeValue;
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

        private string GetAPI(string subUri)
        {
            var baseURL = ApiEndpoint;

            var targetURL = $"{baseURL}{subUri}";
            HttpResponseMessage response = _hClient.GetAsync(targetURL).ConfigureAwait(false).GetAwaiter().GetResult();
            HttpContent content = response.Content;
            string result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            //var pRes = JsonConvert.DeserializeObject<SFPerPersons>(result);
            //sfPerPersonList = pRes?.d.results.ToList();

            return result;
        }

        private string GetFile(string filepath)
        {
            try
            {
                string text = System.IO.File.ReadAllText(filepath);
                return text;
            }
            catch (Exception)
            {
                return "";

            }

        }

        public bool UpdateRecord(MIMObject ob, out string error)
        {
            AuthHClient();
            error = "";
            bool result = false;
            if (!string.IsNullOrEmpty(ob.email))
            {
                var uri = $"{ApiEndpoint}enterprise_ws/secure/user/{ob.userId}/email/WORK/{ob.email}";

                result = SendPostRequest(uri, "", "PUT", out error);
            }
            if (!string.IsNullOrEmpty(ob.UserAlias))
            {
                var uri = $"{ApiEndpoint}enterprise_ws/secure/user/{ob.userId}/username/{ob.UserAlias}";

                result = SendPostRequest(uri, "", "POST", out error);
            }


            return result;
        }


        private bool SendPostRequest(string url, string data, string doMethod, out string errorMessage)
        {
            errorMessage = "";
            bool retResult = false;
            //return code;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpContent contentUpdate = new StringContent(data, Encoding.ASCII, "application/json");
                var method = new HttpMethod(doMethod);
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(url),
                    //Content = requestContent
                };

                var response = _hClient.SendAsync(request).GetAwaiter().GetResult();
                retResult = response.IsSuccessStatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }


            }
            catch (Exception ex)
            {
                errorMessage = "BadRequest";
            }

            return retResult;
        }

        #endregion

        #region Converters

        private T ConvertXML<T>(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return default;
            }

            try
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
                var xml = response.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", "");
                xml = xml.Replace("<?xml version='1.0' encoding='UTF-8'?>", "");

                using (StringReader sr = new StringReader(xml))
                {
                    var objects = (T)xsSubmit.Deserialize(sr);
                    return objects;
                }
            }
            catch (Exception)
            {
                return default;

            }

        }

        private MIMObject MakeMIMObject(personsXMLPerson obj, charts orgs, string anchor, string objectType)
        {
            MIMObject mobj = new MIMObject
            {
                personIdHRM = obj.personIdHRM,
                userId = obj.authentication?.userId ?? "",
                UseruserName = obj.authentication?.username,
                givenName = obj.givenName ?? "",
                familyName = obj.familyName ?? "",
                companyName = obj.employments.employment.company.companyName ?? "",
                organizationNumber = obj.employments.employment.company.organizationNumber,
                nationalityCode = obj.nationalityCode ?? "",
                employeeId = obj.employments.employment.employeeId,
                categoryId = obj.employments.employment?.category?.id ?? "",
                categoryDescription = obj.employments.employment?.category?.description ?? "",
                mobilePhone = obj.contactInfo.mobilePhone ?? "",
                email = obj.contactInfo.email ?? "",
                workPhone = obj.contactInfo.workPhone ?? "",
                chartId = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.id ?? "",
                chartName = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.name ?? "",
                unitId = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.unit?.id ?? "",
                roleId = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.unit?.roles?.FirstOrDefault()?.id ?? "",
                economyAccess = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.unit?.roles?.FirstOrDefault(y => y.id == "27")?.name ?? "",
                unitName = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.unit?.name ?? "",
                dimension1 = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension1?.name ?? "",
                dimension2 = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension2?.name ?? "",
                dimension3 = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension3?.name ?? "",
                dimension4 = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension4?.name ?? "",
                dimension5 = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension5?.name ?? "",
                dimension1Value = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension1?.value ?? "",
                dimension2Value = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension2?.value ?? "",
                dimension3Value = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension3?.value ?? "",
                dimension4Value = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension4?.value ?? "",
                dimension5Value = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.costCentres?.dimension5?.value ?? "",
                manager = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.chart?.unit?.manager?.id.ToString() ?? "",
                positionCode = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionInfo?.positionCode?.positionCode ?? "",
                publicPositionCode = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionInfo?.publicPositionCode?.name ?? "",
                positionCodeName = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionInfo?.positionCode?.name ?? "",
                positionType = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionInfo?.positionType?.name ?? "",
                positionTypeValue = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionInfo?.positionType?.value ?? "",
                workClassification = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionStatistics?.workClassification?.name ?? "",
                businessNumber = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionStatistics?.businessNumber?.name ?? "",
                companyNumber = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionStatistics?.companyNumber?.name ?? "",
                servicePlace = obj.employments.employment.positions?.FirstOrDefault(x => x.isPrimaryPosition)?.positionStatistics?.servicePlace?.name ?? "",
                startDate = obj.employments.employment.startDate.ToString() ?? "",
                endDate = obj.employments.employment?.endDate.ToString() ?? "",
                address2 = obj.postalAddress?.address2 ?? "",
                address1 = obj.postalAddress?.address1 ?? "",
                postalArea = obj.postalAddress?.postalArea ?? "",
                postalCode = obj.postalAddress?.postalCode ?? "",
                countryCode = obj.postalAddress?.countryCode ?? "",
                OrgStructure = "",
            };

            mobj.UserAlias = "";
            var aliases = obj.authentication?.alias;
            if (aliases != null)
            {
                var alList = aliases.ToList();
                var coAlias = aliases.FirstOrDefault(x => x.ToLower().StartsWith(CompanyCode.ToLower()));
                if (coAlias == null)
                {
                    coAlias = aliases.FirstOrDefault();
                }
                if (coAlias != null)
                {
                    mobj.UserAlias = coAlias;
                }
            }

            if (!string.IsNullOrEmpty(mobj.unitId) && orgs != null)
            {
                mobj.OrgStructure = Unit2Org(mobj.unitId, orgs);
            }


            return mobj;
        }

        private string Unit2Org(string unitId, charts orgs)
        {
            var org = @"";

            var o1 = orgs.chart.units.FirstOrDefault(x => x.id == unitId);
            if (o1 != null)
            {
                org = $@"{o1.id} {o1.name}+";
                var ox = GetParent(orgs, o1.parentid);
                while (ox?.parentid >= 0)
                {
                    org = $@"{ox.id} {ox.name}+" + org;
                    ox = GetParent(orgs, ox.parentid);
                }
            }

            var oOrg = org.Replace("+", "\\");
            return oOrg;
        }



        private chartsChartUnit GetParent(charts orgs, ushort parentId)
        {
            var parOrg = orgs.chart.units.FirstOrDefault(x => x.kode == parentId);
            if (parOrg != null)
            {
                return parOrg;
            }
            return null;
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


        private MIMObject FillMIMObject(CSEntryChange cse, out string anchor)
        {
            MIMObject bol = new MIMObject();

            var ballProps = typeof(MIMObject).GetProperties();
            anchor = "";

            foreach (PropertyInfo p in ballProps)
            {
                string propName = p.Name;

                Type typex = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                string typeName = typex.Name;

                if (cse.AnchorAttributes.Contains(propName))
                {
                    anchor = propName;
                    var atch = cse.AnchorAttributes[propName];
                    if (atch != null)
                    {
                        var val = atch.Value;
                        p.SetValue(bol, val);
                    }
                }

                if (cse.AttributeChanges.Contains(propName))
                {
                    var atch = cse.AttributeChanges[propName];
                    var val = atch?.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                    if (val != null)
                    {
                        switch (typeName)
                        {
                            case "String":
                                {
                                    p.SetValue(bol, val.Value.ToString());
                                }
                                break;
                            case "DateTime":
                                {
                                    var dd = DateTime.Parse(val.Value.ToString());
                                    p.SetValue(bol, dd);
                                }
                                break;
                            case "Boolean":
                                {
                                    var dd = bool.Parse(val.Value.ToString());
                                    p.SetValue(bol, dd);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (atch?.ModificationType == AttributeModificationType.Delete)
                    {
                        p.SetValue(bol, "");
                    }
                }
            }



            return bol;
        }

        #endregion


        #region Ecma

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
            {
                SchemaDefs.Add("Person", CreateSchema<MIMObject>("userId", "Person"));
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
