using BALLRepo.Entity;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BALLRepo.Repo
{
    public class BRepo
    {
        #region Properties

        public string BaseUrl { get; set; }
        public string RequestedBy { get; set; }
        public string RequestLanguage { get; set; }

        public string CompanyCode { get; set; }

        public string AnchorName { get; set; }

        public string AdHocCST { get; set; }

        public string GetAppGroups { get; set; }
        public string GetGroups { get; set; }

        public string GetOptions { get; set; }



        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }

        public List<BALLObject> Records { get; set; }

        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }

        /// <summary>
        /// definition of schemna
        /// </summary>
        public SchemaType SchemaDef { get; set; }


        /// private string _offsetToken;

        #endregion

        #region Get

        public BRepo()
        {

        }





        #endregion



        #region Handle

        public void Get()
        {
            var fo = GetAll();
            CSEntries = new List<CSEntryChange>();

            foreach (var ffo in fo)
            {
                CSEntries.Add(FillCSEntry(ffo));

                //if (!string.IsNullOrEmpty(ffo.employeeID))
                //{
                    
                //}
            }

            var x = CSEntries;
        }


        public void Put()
        {
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();

                var status = ErrValidate("BadRequest");
                string errorMessage = "";
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
                                    var boo = FillBallObject(csentryChange);
                                    var boJson = JsonConvert.SerializeObject(boo);

                                    var poststatus = PostJSON(boJson, out errorMessage);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "BallAPI error";
                                    }

                                    status = ErrValidate(poststatus);
                                    //attributeChanges.Add(AttributeChange.CreateAttributeAdd("employeeID", boo.employeeID));
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                    break;
                                }
                            case ObjectModificationType.Delete:
                                {

                                    //User Deletion Code
                                    var boo = FillBallObject(csentryChange);
                                    boo.DoDisableOperation = true;
                                    var boJson = JsonConvert.SerializeObject(boo);

                                    if (string.IsNullOrEmpty(boo.sAMAccountName))
                                    {
                                        ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "BallAPI error", "No accountname"));
                                        break;
                                    }
                                    var disstatus = PatchJSON(boo.sAMAccountName, boJson, out errorMessage);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "BallAPI error";
                                    }
                                    status = ErrValidate(disstatus);
                                    //attributeChanges.Add(AttributeChange.CreateAttributeDelete("employeeID"));
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                                    break;
                                }


                            case ObjectModificationType.Replace:
                                {
                                    //User update code
                                    var boo = FillBallObject(csentryChange);
                                    var boJson = JsonConvert.SerializeObject(boo);

                                    if (string.IsNullOrEmpty(boo.sAMAccountName))
                                    {
                                        ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "BallAPI error", "No accountname"));
                                        break;
                                    }

                                    var patchstatus = PatchJSON(boo.sAMAccountName, boJson, out errorMessage);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "BallAPI error";
                                    }

                                    status = ErrValidate(patchstatus);
                                    //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                                    break;
                                }
                            case ObjectModificationType.Update:
                                {
                                    //User update code
                                    var boo = FillBallObject(csentryChange);
                                    var boJson = JsonConvert.SerializeObject(boo);

                                    if (string.IsNullOrEmpty(boo.sAMAccountName))
                                    {
                                        ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "BallAPI error", "No accountname"));
                                        break;
                                    }

                                    var patchstatus = PatchJSON(boo.sAMAccountName, boJson, out errorMessage);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorName = "BallAPI error";
                                    }

                                    status = ErrValidate(patchstatus);
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
                        ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, "BALLRepo Error", ex.Message));
                    }
                }

            }

        }




        public void MakeSchema()
        {
            List<string> specProps = new List<string>();
            SchemaDef = Microsoft.MetadirectoryServices.SchemaType.Create("Person", false);
            SchemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute("sAMAccountName", AttributeType.String));

            specProps.Add("sAMAccountName");
            specProps.Add("RequestLanguage");

            if (AnchorName != "sAMAccountName")
            {
                specProps.Add(AnchorName);
                SchemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(AnchorName, AttributeType.String));
            }
            var props = typeof(BALLObject).GetProperties();

            foreach (PropertyInfo p in props)
            {
                if (!specProps.Contains(p.Name))
                {
                    Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    string typeName = type.Name;
                    if (typeName == "List`1")
                    {
                        SchemaDef.Attributes.Add(SchemaAttribute.CreateMultiValuedAttribute(p.Name, AttributeType.String));
                    }
                    else
                    {
                        SchemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(p.Name, GetType(typeName)));
                    }
                }
            }


        }




        #endregion

        #region URLHandle

        private string Token()
        {
            string tokenValue = "";
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("content_type", "application/json");
                var url = "https://rotdev.intility.no/token";
                var res = client.GetStringAsync(url).Result;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(res);
                XmlNodeList nlist = doc.GetElementsByTagName(("input"));
                foreach (XmlNode cl in nlist)
                {
                    var key = cl.Attributes["name"].Value;
                    if (key == "__RequestVerificationToken")
                    {
                        tokenValue = cl.Attributes["value"].Value;
                    }

                }
            }
            return tokenValue;
        }



        private List<BALLObject> GetAll()
        {
            var obj = new List<BALLObject>();
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (HttpClient client = new HttpClient(handler))
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add("content_type", "application/json");
                var url = $"{BaseUrl}{CompanyCode}/?anchorName={AnchorName}&getAppGroups={intToBool(GetAppGroups)}&getGroups={intToBool(GetGroups)}&getOptions={intToBool(GetOptions)}";
                try
                {
                    var res = client.GetStringAsync(url).Result;
                    var gobj = JsonConvert.DeserializeObject<List<BALLObject>>(res);
                    return gobj;
                }
                catch (Exception ex)
                {
                    return obj;
                }
                

                
            }
        }

        private async Task<List<BALLObject>> GetAllAync()
        {
            var obj = new List<BALLObject>();
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("content_type", "application/json");
                var url = $"{BaseUrl}{CompanyCode}/?anchorName={AnchorName}";
                var res = await client.GetStringAsync(url);


                obj = JsonConvert.DeserializeObject<List<BALLObject>>(res);
            }

            return obj;
        }


        private string SendPostRequest(string url, string data, string doMethod, out string errorMessage)
        {
            var code = "BadRequest";
            errorMessage = "";
            //return code;
            try
            {
                HttpClientHandler hand = new HttpClientHandler { UseDefaultCredentials = true, PreAuthenticate = true };

                using (HttpClient client = new HttpClient(hand))
                {
                    HttpContent requestContent = new StringContent(data, Encoding.UTF8, "application/json");


                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                code = "BadRequest";
            }

            return code;
        }


        public string PostJSON(string DATA, out string errorMessage)
        {
            string response = "";
            string URL = BaseUrl;

            response = SendPostRequest(URL, DATA, "Post", out errorMessage);

            return response;
        }


        public string PatchJSON(string id, string DATA, out string errorMessage)
        {
            string response = "";

            string URL = BaseUrl + id;
            response = SendPostRequest(URL, DATA, "PATCH", out errorMessage);

            return response;
        }


        #endregion




        #region Helpers


        private AttributeType GetType(string propname)
        {
            AttributeType typ = AttributeType.String;
            switch (propname)
            {
                case "String":
                    {
                        typ = AttributeType.String;
                        break;
                    }
                case "Boolean":
                    {
                        typ = AttributeType.Boolean;
                        break;
                    }
                case "DateTime":
                    {
                        typ = AttributeType.String;
                        break;
                    }
                default:
                    break;
            }
            return typ;
        }

        private MAExportError ErrValidate(string statusCode)
        {
            MAExportError er = new MAExportError();

            switch (statusCode)
            {
                case "OK":
                    {
                        er = MAExportError.Success;
                        break;
                    }
                case "Accepted":
                    {
                        er = MAExportError.Success;
                        break;
                    }
                case "BadRequest":
                    {
                        er = MAExportError.ExportErrorCustomStopRun;
                        break;
                    }
                case "NotFound":
                    {
                        er = MAExportError.ExportErrorInvalidDN;
                        break;
                    }
                case "NotAcceptable":
                    {
                        er = MAExportError.ExportErrorInvalidAttributeValue;
                        break;
                    }
                case "NotModified":
                    {
                        er = MAExportError.ExportErrorAmbiguousUpdate;
                        break;
                    }
                default:
                    break;
            }

            return er;
        }

        private BALLObject FillBallObject(CSEntryChange cse)
        {
            BALLObject bol = new BALLObject();

            var ballProps = typeof(BALLObject).GetProperties();

            foreach (PropertyInfo p in ballProps)
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

            if (string.IsNullOrEmpty(bol.RequestedBy))
            {
                bol.RequestedBy = RequestedBy;
            }

            bol.RequestLanguage = RequestLanguage;
            bol.CSTicketId = AdHocCST;
            bol.CompanyCode = CompanyCode;

            return bol;
        }

        private CSEntryChange FillCSEntry(BALLObject bol)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (bol == null)
            {
                return csentry1;
            }


            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = "Person";

            var ballProps = typeof(BALLObject).GetProperties();
            List<string> specProps = new List<string>();
            //specProps.Add(AnchorName);
            //specProps.Add("sAMAccountName");
            //specProps.Add("RequestedBy");
            specProps.Add("RequestLanguage");


            foreach (PropertyInfo p in ballProps)
            {
                var pValue = p.GetValue(bol);
                var tp = p.PropertyType.Name;

                if (pValue != null)
                {
                    var ob = pValue;
                    ob = tp == "Boolean" ? pValue : pValue.ToString();

                    if (p.Name == AnchorName)
                    {
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob.ToString()));
                    }
                    else
                    {
                        if (!specProps.Contains(p.Name))
                        {
                            csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
                        }
                    }
                }
            }


            return csentry1;
        }

        private CSEntryChange FillCSEntryX(BALLObject bol)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (bol == null)
            {
                return csentry1;
            }


            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = "Person";

            foreach (PropertyInfo p in typeof(BALLObject).GetProperties())
            {
                if (p.GetValue(bol) != null)
                {
                    var tp = p.PropertyType.Name;
                    switch (tp)
                    {
                        case "String":
                            {
                                var ob = (String)p.GetValue(bol);

                                if (!string.IsNullOrEmpty(ob))
                                {
                                    if (p.Name == AnchorName)
                                    {
                                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob));
                                    }
                                    else
                                    {
                                        if (p.Name != "RequestedBy" && p.Name != "RequestLanguage")
                                        {
                                            csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
                                        }
                                    }
                                }

                                break;
                            }
                    }

                }
            }


            return csentry1;
        }

        private void AddAttributeChanges<T>(ref CSEntryChange csEntryChange, T item)
        {
            if (csEntryChange == null)
            {
                throw new ArgumentNullException(nameof(csEntryChange));
            }
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanRead && x.CanWrite)
                .Where(x => x.PropertyType == typeof(string))
                .Where(x => x.GetGetMethod(true).IsPublic);

            string name, value;

            foreach (PropertyInfo info in properties)
            {
                value = info.GetValue(item, null) as string;

                if (string.IsNullOrEmpty(value) || info.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                name = $"{char.ToLower(info.Name[0])}{info.Name.Substring(1)}";
                csEntryChange.AttributeChanges.Add(
                    AttributeChange.CreateAttributeAdd(name, info.GetValue(item, null)));
            }
        }



        #endregion

        #region Mini 
        
        string intToBool(string cb)
        {
            if (cb == "1")
            {
                return "true";
            }

            return "false";
        }


        #endregion

        #region Classes


        #endregion
    }
}
