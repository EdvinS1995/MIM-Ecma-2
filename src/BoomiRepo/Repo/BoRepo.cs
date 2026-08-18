using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BoomiRepo.Entity;
using Microsoft.MetadirectoryServices;

namespace BoomiRepo.Repo
{
    public class BoRepo
    {
        #region Properties

        public string ImportBaseUrl { get; set; }
        public string ImportClientId { get; set; }
        public string ImportClientSecret { get; set; }

        public string ExportBaseUrl { get; set; }
        public string ExportClientId { get; set; }
        public string ExportClientSecret { get; set; }

        public string XMLRecords { get; set; }




        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }

        public BoomO2.EmployeeList Records { get; set; }

        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        /// <summary>
        /// definition of schemna
        /// </summary>
        public SchemaType SchemaDef { get; set; }


        private string _offsetToken;

        #endregion

        #region Init

        public BoRepo()
        {

        }

        #endregion



        #region Handle


        public void Get()
        {
            _offsetToken = "";
            Records = new BoomO2.EmployeeList();
            var recs = new List<BoomO2.Employee>();
            CSEntries = new List<CSEntryChange>();

            bool dataOk = true;



            try
            {
                XMLRecords = GetXMLClient();
                var data = ConvertSFromXML(XMLRecords);

                foreach (var bad in data.Employees)
                {
                    recs.Add(bad);
                }

                Records.Employees = recs;
            }
            catch (Exception ex)
            {
                var e1 = ex.Message;
                dataOk = false;

            }

            if (dataOk)
            {
                foreach (var fo in recs)
                {
                    CSEntries.Add(FillCSEntry(fo));
                }
            }
        }

        public void PutMulti()
        {
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                List<AttributeChange> attributeChanges = new List<AttributeChange>();
                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

                if (csentryChange.ObjectType == "Person")
                {
                    try
                    {
                        switch (csentryChange.ObjectModificationType)
                        {
                            case ObjectModificationType.Add:
                                {
                                    //User Creation Code
                                    var poststatus = "BadRequest";
                                    var errorMessage = "Not implemented";
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "Workday error";
                                    }

                                    status = Util.ErrValidate(poststatus);
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                    break;
                                }
                            case ObjectModificationType.Delete:
                                {
                                    //User delete Code
                                    var poststatus = "BadRequest";
                                    var errorMessage = "Not implemented";
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "Workday error";
                                    }

                                    status = Util.ErrValidate(poststatus);
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                    break;
                                }

                            case ObjectModificationType.Replace:

                            case ObjectModificationType.Update:
                                {
                                    var boo = FillBoomiEntry(csentryChange);

                                    if (string.IsNullOrEmpty(boo.GoldenRecordId))
                                    {
                                        ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Boomi error", "No employee_id"));
                                        break;
                                    }

                                    var xml = ConvertToXML(boo);


                                    var errorMessage = "";
                                    var patchstatus = PostXMLClient(xml, out errorMessage);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "Boomi error";
                                    }

                                    status = Util.ErrValidate(patchstatus);
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                                    break;
                                }
                            default:
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, "BoomiRepo error", ex.Message));

                    }
                }

            }

        }


        public void Put()
        {
            ExportEntriesResults = new PutExportEntriesResults();

            string errorName = "";
            string errorMessage = "";
            var status = Util.ErrValidate("BadRequest");

            var root = FillBoomiEntries(CSEntries, out errorMessage);

            if (root != null)
            {
                string xmlEmps = ConvertListToXML(root);

                string res = PostXMLClient(xmlEmps, out errorMessage);

                status = Util.ErrValidate(res);
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorName = "Boomi Repo error";
            }


            ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(Guid.NewGuid(), new List<AttributeChange>(), status, errorName, errorMessage));
        }


        #endregion

        #region XMLHandle


        private string GetXMLClient()
        {
            var url = ImportBaseUrl;
            string res = "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                String username = ImportClientId;
                String password = ImportClientSecret;
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
                res = client.GetStringAsync(url).Result;

            }

            return res;
        }



        private string GetXML()
        {
            string response = "";

            try
            {
                string URL = ImportBaseUrl;

                string DATA = "<RecordQueryRequest limit=\"\" offsetToken=\"" + _offsetToken + "\"></RecordQueryRequest>";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.ContentLength = DATA.Length;

                ServicePointManager.Expect100Continue = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                String username = ImportClientId;
                String password = ImportClientSecret;
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);

                request.PreAuthenticate = true;

                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            response = responseReader.ReadToEnd();
                        }
                    }
                }
            }

            catch (Exception e1)
            {
                response = "";
            }

            return response;

        }

        private string SendPostRequest(string url, string data, string doMethod, out string errorMessage)
        {
            var code = "BadRequest";
            errorMessage = "";
            //return code;
            //try
            //{
                HttpClientHandler hand = new HttpClientHandler { UseDefaultCredentials = true, PreAuthenticate = true };

                using (HttpClient client = new HttpClient(hand))
                {
                    HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/xml");


                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    String username = ExportClientId;
                    String password = ExportClientSecret;
                    String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);

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
            //}
            //catch (Exception ex)
            //{
            //    errorMessage = ex.Message;
            //    code = "BadRequest";
            //}

            return code;
        }

        private string SendPostRequestx(string url, string data)
        {
            var code = "Failed";

            //try
            //{

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                var requestContent = new StringContent(data);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                String username = ExportClientId;
                String password = ExportClientSecret;
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);

                var response = client.PostAsync(url, requestContent).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;

                if (responseString.Contains("<status>Ok</status>"))
                {
                    code = "OK";
                }

            }

            //}
            //catch (Exception ex)
            //{
            //	code = ex.Message;
            //}

            return code;
        }


        public string PostXMLClient(string DATA, out string errorMessage)
        {
            string url = ExportBaseUrl;

            string response = SendPostRequest(url, DATA, "Post", out errorMessage);

            return response;
        }


        #endregion




        #region Convert



  

        private BoomO2.EmployeeList ConvertSFromXML(string response)
        {
            var objects = new BoomO2.EmployeeList();

            XmlSerializer xsSubmit = new XmlSerializer(typeof(BoomO2.EmployeeList));
            var xml = response.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", "");
            xml = xml.Replace("<?xml version='1.0' encoding='UTF-8'?>", "");

            //var bxml = $"<BoomiRecord>{xml}</BoomiRecord>";

            using (StringReader sr = new StringReader(xml))
            {
                objects = (BoomO2.EmployeeList)xsSubmit.Deserialize(sr);
            }

            return objects;
        }


        public string ConvertToXML(BoomO2.Employee entry)
        {
            string xmlStr = String.Empty;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.OmitXmlDeclaration = true;
            settings.NewLineChars = String.Empty;
            settings.NewLineHandling = NewLineHandling.None;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(string.Empty, string.Empty);
                    XmlSerializer serializer = new XmlSerializer(typeof(BoomO2.Employee));
                    serializer.Serialize(xmlWriter, entry, xmlns);
                    xmlStr = stringWriter.ToString();
                    xmlWriter.Close();
                }
            }
            return xmlStr.ToString();

        }

        public string ConvertListToXML(BoomO2.EmployeeList entry)
        {
            string xmlStr = String.Empty;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.OmitXmlDeclaration = true;
            settings.NewLineChars = String.Empty;
            settings.NewLineHandling = NewLineHandling.None;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(string.Empty, string.Empty);
                    XmlSerializer serializer = new XmlSerializer(typeof(BoomO2.EmployeeList));
                    serializer.Serialize(xmlWriter, entry, xmlns);
                    xmlStr = stringWriter.ToString();
                    xmlWriter.Close();
                }
            }
            return xmlStr.ToString();

        }


        #endregion



        #region Ecma

        public bool MakeSchema()
        {
            SchemaDef = CreateSchema();

            return true;
        }

        private SchemaType CreateSchema()
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create("Person", false);

            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute("GoldenRecordId", AttributeType.String));
            //schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute("SupervisorId", AttributeType.Reference));

            List<string> specProps = new List<string>();
            specProps.Add("GoldenRecordId");
            //specProps.Add("SupervisorId");


            var boProps = typeof(BoomO2.Employee).GetProperties();

            foreach (PropertyInfo p in boProps)
            {
                if (specProps.Contains(p.Name)) continue;
                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                string typeName = type.Name;

                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(p.Name, Util.GetType(typeName)));
            }

            return schemaDef;
        }


        public BoomO2.EmployeeList FillBoomiEntries(IList<CSEntryChange> csEntries, out string errorMessage)
        {
            errorMessage = "";
            BoomO2.EmployeeList booms = new BoomO2.EmployeeList();
            List<BoomO2.Employee> empList = new List<BoomO2.Employee>();
            foreach (var item in csEntries)
            {
                empList.Add(FillBoomiEntry(item));

            }
            booms.Employees = empList;


            return booms;
        }


        public BoomO2.Employee FillBoomiEntry(CSEntryChange cse)
        {
            BoomO2.Employee boom = new BoomO2.Employee();
            var boProps = typeof(BoomO2.Employee).GetProperties();

            foreach (PropertyInfo p in boProps)
            {
                string propName = p.Name;

                Type typex = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                string typeName = typex.Name;

                if (cse.AnchorAttributes.Contains(propName))
                {
                    var atch = cse.AnchorAttributes[propName];
                    if (atch != null)
                    {
                        var val = atch.Value;
                        p.SetValue(boom, val);
                    }
                }

                if (cse.AttributeChanges.Contains(propName))
                {
                    var atch = cse.AttributeChanges[propName];
                    var val = atch.ValueChanges.FirstOrDefault();
                    if (val != null)
                    {
                        switch (typeName)
                        {
                            case "String":
                                {
                                    p.SetValue(boom, val.Value.ToString());
                                }
                                break;
                            case "DateTime":
                                {
                                    var dd = DateTime.Parse(val.Value.ToString());
                                    p.SetValue(boom, dd);
                                }
                                break;
                            case "Boolean":
                                {
                                    var dd = bool.Parse(val.Value.ToString());
                                    p.SetValue(boom, dd);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return boom;
        }


        private CSEntryChange FillCSEntry(BoomO2.Employee cat)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (cat == null)
            {
                return csentry1;
            }

            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = "Person";

            var boProps = typeof(BoomO2.Employee).GetProperties();

            if (cat != null)
            {
                foreach (PropertyInfo p in boProps)
                {
                    var pValue = p.GetValue(cat);
                    var tp = p.PropertyType.Name;

                    if (pValue != null)
                    {
                        var ob = tp == "Boolean" ? pValue : pValue.ToString();

                        if (p.Name == "GoldenRecordId")
                        {
                            csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob.ToString()));
                        }
                        else
                        {
                            csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
                        }
                    }
                }
            }

            return csentry1;
        }



        #endregion
    }
}


