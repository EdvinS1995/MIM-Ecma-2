using HRPlusRepo.Entity;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StaticLib;
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
using System.Xml;
using System.Xml.Serialization;

namespace HRPlusRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string CompanyCode { get; set; }

        public string LoginEndpoint { get; set; }
        public string ApiEndpoint { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public bool GetEmployees { get; set; } = true;

        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }

        public string JsonRoot { get; set; }

        /// <summary>
        /// definition of schema
        /// </summary>
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }

        /// <summary>
        /// Result of export
        /// </summary>
        public int DaysBeforeStart { get; set; }

        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        public List<Dictionary<string, string>> EmpDic { get; set; }

        private HttpClient _hClient;
        private string Token;
        private string _anchor = "UserId";

        #endregion

        #region Auth

        public MIMRepo()
        {
        }

        public bool DecideAuth(string companyCode)
        {
            switch (companyCode)
            {
                case "ACPGET":
                    {
                        ApiEndpoint = "https://connecting.stage.hrplus.visma.se/odata/";
                        ClientId = "W+DIPcl6Cx6JtM3eavTuyg==";
                        ClientSecret = SecretRef.ACPGet_ClientSecret;
                        DaysBeforeStart = 14;
                        AuthHClient();
                        break;
                    }
                case "ACPPUT":
                    {
                        ApiEndpoint = "https://connecting.hrplus.visma.se/iapi/api/";
                        ClientId = "mZzwstCqOOPI97iL/WASVGOYEpBIK7IUmLiexQc9rMo=";
                        ClientSecret = SecretRef.ACPPut_ClientSecret;
                        AuthPost();
                        break;
                    }

                default:
                    break;
            }

            return true;
        }

        public void AuthPost()
        {
            Token = null;
            HttpClient client = new HttpClient();

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", ClientId},
                {"client_secret", ClientSecret},
            });

            HttpResponseMessage tokenResponse = client.PostAsync(ApiEndpoint + "getsecuretoken", content).Result;
            var jsonContent = tokenResponse.Content.ReadAsStringAsync().Result;

            JObject obj = JObject.Parse(jsonContent);
            Token = (string)obj["access_token"];

            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            _hClient.BaseAddress = new Uri(ApiEndpoint);

            // XML endpoint — do not use application/json
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public void AuthHClient()
        {
            var aut = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            string base6 = Convert.ToBase64String(aut);
            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base6}");
            _hClient.BaseAddress = new Uri(ApiEndpoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.Timeout = new TimeSpan(0, 5, 0);
        }

        #endregion


        #region Handlers

        public void Get()
        {
            DecideAuth(CompanyCode + "GET");
            CSEntries = new List<CSEntryChange>();

            if (GetEmployees)
            {
                var personToExpand = "ExtraDataField";

                var resultPerson = GetAPI($"Person?$expand={personToExpand}", 0, 0);
                var dataPerson = JsonConvert.DeserializeObject<PersonRoot>(resultPerson);
                var listPerson = dataPerson.value.ToList();

                var resultWorkplace = GetAPI($"Workplace", 0, 0);
                var dataWorkplace = JsonConvert.DeserializeObject<WorkplaceRoot>(resultWorkplace);
                var listWorkplace = dataWorkplace.value.ToList();

                var employmentToExpand = "Positions,SalaryAffecting,EmploymentGroups";
                var resultEmploymentCore = GetAPI($"Employment?$expand={employmentToExpand}", 0, 0);
                var dataEmploymentCore = JsonConvert.DeserializeObject<EmploymentRoot>(resultEmploymentCore);
                var listEmploymentCore = dataEmploymentCore.value.ToList();

                var mimEmployees = MakeMimObjects(listPerson, listEmploymentCore, listWorkplace);

                foreach (var emp in mimEmployees)
                {
                    var csentry = FillCSEntry(emp, _anchor, "Person");
                    CSEntries.Add(csentry);
                }
            }

            var x = CSEntries;
        }

        public void Put()
        {
            DecideAuth(CompanyCode + "PUT");
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

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
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }

                    case ObjectModificationType.Delete:
                        {
                            var poststatus = "BadRequest";
                            var errorMessage = "Not implemented";
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorName = "IFS error";
                            }
                            status = Util.ErrValidate(poststatus);
                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }

                    case ObjectModificationType.Replace:
                        {
                            var anchor = "";
                            var errorMessage = "";
                            var boo = FillRootObject(csentryChange, out anchor);

                            if (boo == null || string.IsNullOrEmpty(anchor))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "HR+ error", "No WorkNumber found"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, anchor, out errorMessage);

                            if (patchstatus)
                            {
                                status = MAExportError.Success;
                            }
                            else
                            {
                                errorName = "HR+ error";
                            }

                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }

                    case ObjectModificationType.Update:
                        {
                            var anchor = "";
                            var errorMessage = "";
                            var boo = FillRootObject(csentryChange, out anchor);

                            if (boo == null || string.IsNullOrEmpty(anchor))
                            {
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "HR+ error", "No WorkNumber found"));
                                break;
                            }

                            var patchstatus = UpdateRecord(boo, anchor, out errorMessage);

                            if (patchstatus)
                            {
                                status = MAExportError.Success;
                            }
                            else
                            {
                                errorName = "HR+ error";
                            }

                            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                            break;
                        }

                    default:
                        break;
                }
            }
        }

        private UpdateEmployment FillRootObject(CSEntryChange cse, out string anchor)
        {
            anchor = "";

            var header = new UpdateEmploymentHeader
            {
                MessageId = Guid.NewGuid().ToString(),
                SourceSystemTimeStamp = DateTime.UtcNow,
                SchemaVersion = 1,
                SourceSystem = "ADManager",
                BusinessAction = "EditEmployment",
                CorrelationId = Guid.NewGuid().ToString() // overwritten below once WorkNumber is known
            };

            var idCodes = new UpdateEmploymentIdentificationCodes
            {
                IdentificationType = "WorkNumberEmploymentNumber"
            };

            var change = new UpdateEmploymentChangesChange
            {
                AffectingDate = DateTime.UtcNow
            };

            foreach (var p in cse.AttributeChanges)
            {
                string propName = p.Name;
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val == null) continue;

                var strVal = val.Value.ToString();

                switch (propName)
                {
                    case "EmailWork":
                        change.EmailWork = strVal;
                        break;
                    case "EmployerCode":
                        idCodes.EmployerCode = int.Parse(strVal);
                        break;
                    case "WorkNumber":
                        idCodes.WorkNumber = int.Parse(strVal);
                        anchor = strVal;
                        break;
                    case "EmploymentNumber":
                        idCodes.EmploymentNumber = strVal;
                        break;
                    case "PositionCode":
                        idCodes.PositionCode = int.Parse(strVal);
                        break;
                }
            }

            if (string.IsNullOrEmpty(anchor))
                return null;

            // CorrelationId format: "{WorkNumber};{guid1};{guid2}"
            header.CorrelationId = $"{anchor};{Guid.NewGuid()};{Guid.NewGuid()}";

            return new UpdateEmployment
            {
                Header = header,
                Changes = new UpdateEmploymentChanges { Change = change },
                IdentificationCodes = idCodes
            };
        }

        #endregion


        #region Convert

        public string ConvertToXMLSimple<T>(T entry)
        {
            string xml = string.Empty;
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "Bluegarden.HRPlus.Schemas");

            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true };

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(writer, entry, ns);
                    xml = sw.ToString();
                }
            }

            string declaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            return declaration + xml;
        }

        public string ConvertToXML<T>(T entry)
        {
            string xmlStr = String.Empty;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.OmitXmlDeclaration = false;
            settings.NewLineChars = String.Empty;
            settings.NewLineHandling = NewLineHandling.None;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(string.Empty, string.Empty);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(xmlWriter, entry, xmlns);
                    xmlStr = stringWriter.ToString();
                    xmlWriter.Close();
                }
            }
            return xmlStr.ToString();
        }

        private List<MIMObject> MakeMimObjects(List<PersonValue> persons, List<EmploymentValue> employments, List<WorkplaceValue> workplaces)
        {
            var mimObjects = persons.Select(p => new MIMObject(p)).ToList();

            foreach (var mim in mimObjects)
            {
                var emp = employments.FirstOrDefault(x => x.PersonId == mim.Id);
                if (emp != null)
                {
                    mim.EmploymentNumber = emp.EmploymentNumber;
                    mim.StartDate = emp.StartDate;
                    mim.EndDate = emp.EndDate;
                    mim.UnitCode = emp.UnitCode;
                    mim.CompanyCode = emp.CompanyCode;
                    mim.EmployerCode = emp.EmployerCode;
                    mim.EmployerName = emp.EmployerName;
                    mim.EmployerRegistrationNumber = emp.EmployerRegistrationNumber;
                    mim.PersonSequenceNumber = emp.PersonSequenceNumber;
                    mim.PersonId = emp.PersonId;
                    mim.WorkNumber = emp.WorkNumber;
                    mim.WorkplaceId = emp.WorkplaceId ?? 0;
                    mim.EmployeeId = emp.EmployeeId;

                    if (emp.Positions != null && emp.Positions.Length > 1)
                    {
                        var posis = emp.Positions.Where(x => x.StartDate.Date <= DateTime.Now.Date && (x.EndDate == null || x.EndDate?.Date > DateTime.Now.Date));
                        var pos = posis.OrderBy(x => x.StartDate).FirstOrDefault();
                        mim.EmploymentPositionId = pos.EmploymentPositionId;
                        mim.DescriptionTitle = pos.Description;
                        mim.TextValue1 = pos.TextValue1;
                        mim.TextValue2 = pos.TextValue2;
                        mim.TextValue3 = pos.TextValue3;
                        mim.TextValue4 = pos.TextValue4;
                        mim.TextValue5 = pos.TextValue5;
                        mim.TextValue6 = pos.TextValue6;
                        mim.TextValue7 = pos.TextValue7;
                        mim.PositionCode = pos.PositionCode;
                        mim.SSYKCode = pos.SSYKCode;
                        mim.OccupactionEmploymentCodeProfessionCode = pos.OccupactionEmploymentCodeProfessionCode;
                        mim.OccupactionEmploymentCodeDescription = pos.OccupactionEmploymentCodeDescription;
                        mim.SNJobStatusExternalId = pos.SNJobStatusExternalId;
                        mim.SNJobStatusDescription = pos.SNJobStatusDescription;
                        mim.SNEmployeeCategoryExternalId = pos.SNEmployeeCategoryExternalId;
                        mim.SNEmployeeCategoryDescription = pos.SNEmployeeCategoryDescription;
                        mim.BESTACode = pos.BESTACode;
                        mim.WorkEvaluation = pos.WorkEvaluation;
                        mim.AgreementDivisionCode = pos.AgreementDivisionCode;
                        mim.AgreementDivisionDescription = pos.AgreementDivisionDescription;
                        mim.SNForbundAgreementCode = pos.SNForbundAgreementCode;
                    }

                    if (emp.SalaryAffecting != null && emp.SalaryAffecting.Length > 1)
                    {
                        var sals = emp.SalaryAffecting.Where(x => x.StartDate.Date <= DateTime.Now.Date && (x.EndDate == null || x.EndDate?.Date > DateTime.Now.Date));
                        var sal = sals.OrderBy(x => x.StartDate).FirstOrDefault();
                        mim.PartTimeDescription = sal.PartTimeDescription;
                        mim.WorkingHourTypeATADescription = sal.WorkingHourTypeATADescription;
                        mim.EmploymentRateReducedCause = sal.EmploymentRateReducedCause;
                        mim.ManagerUserId = sal.ManagerUserId;
                        mim.DepartmentId = sal.DepartmentId;
                        mim.DepartmentTechnicalId = sal.DepartmentTechnicalId;
                        mim.DepartmentDescription = sal.Description;
                        mim.AccountPart1 = sal.AccountPart1;
                        mim.AccountPart2 = sal.AccountPart2;
                        mim.AccountPart3 = sal.AccountPart3;
                        mim.OrganizationalConcept1 = sal.OrganizationalConcept1;
                        mim.OrganizationalConcept2 = sal.OrganizationalConcept2;
                        mim.OrganizationalConcept3 = sal.OrganizationalConcept3;
                        mim.Descriptiondivison = sal.Description;
                    }

                    if (emp.EmploymentGroups != null && emp.EmploymentGroups.Length > 0)
                    {
                        var emg = emp.EmploymentGroups[0];
                        mim.GroupManagerUserId = emg.GroupManagerUserId;
                        mim.GroupManagerFullName = emg.GroupManagerFullName;
                    }
                }

                var person = persons.FirstOrDefault(x => x.Id == mim.Id);
                if (person != null && person.ExtraDataField != null && person.ExtraDataField.Length > 0)
                {
                    var ex = person.ExtraDataField[0];
                    mim.DateValue1 = ex.DateValue1;
                    mim.DateValue2 = ex.DateValue2;
                }

                var wrkp = workplaces.FirstOrDefault(x => x.Id == mim.WorkplaceId);
                if (wrkp != null)
                {
                    mim.Address = wrkp.Address;
                    mim.WorkplaceDescription = wrkp.Description;
                    mim.WorkplaceCode = wrkp.WorkplaceId;
                    mim.CoAddress = wrkp.CoAddress;
                    mim.PostalCode = wrkp.PostalCode;
                    mim.PostAdderess = wrkp.PostAdderess;
                }
            }

            return mimObjects;
        }

        #endregion


        #region API

        private List<T> GetIFSObjects<T>(string getType)
        {
            var ifsObjects = new List<T>();

            var result = GetAPI(getType, 0, 100);
            var empRoot = JsonConvert.DeserializeObject<T>(result);
            ifsObjects.Add(empRoot);

            try { }
            catch (Exception ex) { throw ex; }

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
                response.Content.Headers.ContentType.MediaType = "application/json";
                HttpContent content = response.Content;
                result = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

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
            return "";
        }

        public bool UpdateRecord(UpdateEmployment data, string anchor, out string errorMessage)
        {
            errorMessage = "";
            string responseData = "";

            var xmlob = ConvertToXMLSimple<UpdateEmployment>(data);

            try
            {
                var subUrl = "Employment";
                SendPostRequest(ApiEndpoint, xmlob, "PUT", subUrl, out errorMessage, out responseData);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private string SendPostRequest(string baseURL, string data, string doMethod, string subUrl, out string errorMessage, out string responseData)
        {
            var code = "BadRequest";
            errorMessage = "";
            responseData = "";
            var targetURL = $"{baseURL}{subUrl}";

            try
            {
                HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/xml");
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

                var method = new HttpMethod(doMethod);
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(targetURL),
                    Content = requestContent
                };

                var response = _hClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                code = response.StatusCode.ToString();

                if (!response.IsSuccessStatusCode)
                {
                    var errorresp = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    errorMessage = $"Errorcode: {response.StatusCode}, {response.ReasonPhrase} for {response.RequestMessage} with response {errorresp}";
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
                SchemaDefs.Add("Person", CreateSchema<MIMObject>(_anchor, "Person"));
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
                        cusdic[kv.Key] = kv.Value;
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
            if (bol == null) return csentry1;

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
                                    dataItem.Key.Replace(" ", "_")
                                };
                                var ddretKey = string.Join("-", keys);
                                kvs[ddretKey] = dataItem.Value.ToString();
                            }
                        }
                        else
                        {
                            keys = new List<string> { baseItem.Key };
                            var ddretKey = string.Join("-", keys);
                        }
                        roleCount++;
                    }
                }
                else
                {
                    keys = new List<string> { baseItem.Key.Replace(" ", "_") };
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
