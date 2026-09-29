using QuinyxRepo.QuinyxReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QuinyxRepo.Entity
{
    public class SetUpdateClass
    {

        public List<AgreementTemplateV2> Templs { get; set; }





        public UpdateEmployee SetEmp(EmployeeExt emp1)
        {
            UpdateEmployee up = new UpdateEmployee
            {
                address1 = emp1.address1,
                address2 = emp1.address2,
                badgeNo = emp1.badgeNo,
                cellPhone = emp1.cellPhone,
                city = emp1.city,
                country = emp1.country,
                email = emp1.email,
                familyName = emp1.familyName,
                givenName = emp1.givenName,
                reportingTo = emp1.reportingTo,
                socsecNo = emp1.socsecNo,
                zip = emp1.zip,
                //workInUnits = emp1.un
            };

            //if (emp1.role != null)
            //{
            //    up.role = emp1.role.Value;
            //}

            if (emp1.sex != null)
            {
                up.sex = int.Parse(emp1.sex);
                up.sexSpecified = true;
            }

            //if (emp1.staffCat != null)
            //{
            //    up.staffCat = emp1.staffCat.Value;
            //}

            if (emp1.dateOfBirth != null)
            {
                up.dateOfBirth = emp1.dateOfBirth.Value;
                up.dateOfBirthSpecified = true;
            }

            if (emp1.employedDate != null)
            {
                up.employedDate = emp1.employedDate.Value;
                up.industryDate = emp1.employedDate.Value;
                up.industryDateSpecified = true;
                up.employedDateSpecified = true;
            }

            if (emp1.active != null)
            {
                up.active = int.Parse(emp1.active);
                up.activeSpecified = true;
                if (emp1.active == "0")
                {
                    var ld = DateTime.Now; 
                    if (emp1.leaveDate != null) ld = emp1.leaveDate.Value;
                    up.leaveDate = ld;
                    up.leaveDateSpecified = true;
                }
            }

            return up;
        }


        public UpdateAgreementV2 SetAgr(EmployeeExt emp1, string operation, List<AgreementV2> allAgrs)
        {
            bool changed = false;
            UpdateAgreementV2 av2 = new UpdateAgreementV2
            {
                badgeNo = emp1.badgeNo
            };
            var extAgr = allAgrs.FirstOrDefault(x => x.badgeNo == emp1.badgeNo && x.isMainAgreement);

            if (!string.IsNullOrEmpty(emp1.TemplateExtId))
            {
                
                if (extAgr == null || extAgr?.extTemplateId != emp1.TemplateExtId)
                {
                    var temid = Templs.FirstOrDefault(x => x.extTemplateId == emp1.TemplateExtId);
                    if (temid != null)
                    {
                        av2.templateId = temid.id;
                        av2.templateIdSpecified = true;
                        av2.name = $"{emp1.givenName} {emp1.familyName}";
                        av2.isMainAgreement = true;
                        av2.isMainAgreementSpecified = true;
                        av2.fromDate = emp1.employedDate != null ? (DateTime)emp1.employedDate : DateTime.Now;

                        if (operation == "Update")
                        {
                            av2.fromDate = DateTime.Now;
                        }
                        av2.fromDateSpecified = true;

                        av2.extTemplateId = emp1.TemplateExtId;
                        changed = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(emp1.EmploymentPercent))
            {
                var creRate = emp1.EmploymentPercent.Replace("%", "");
                decimal intRate = 0;
                if (decimal.TryParse(creRate, out intRate))
                {
                    var rateChange = false;
                    var extRate = extAgr?.employmentRates.FirstOrDefault();
                    if (extRate != null)
                    {
                        rateChange = (extRate.rate * 100) != intRate;
                    }


                    if (extAgr == null || rateChange)
                    {
                        EmploymentRate r1 = new EmploymentRate
                        {
                            rate = intRate,
                            fromDate = new DateTime(1970, 1, 1)
                        };
                        var rateList = new List<EmploymentRate> { r1 };
                        av2.employmentRatesReplace = rateList.ToArray();
                        //av2.employmentRatesAdd = rateList.ToArray();
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                return av2;
            }
            return null;

        }



        /// <summary>
        /// Helper class to send custom SOAP requests.
        /// </summary>
        public static class SOAPHelper
        {
            /// <summary>
            /// Sends a custom sync SOAP request to given URL and receive a request
            /// </summary>
            /// <param name="url">The WebService endpoint URL</param>
            /// <param name="action">The WebService action name</param>
            /// <param name="parameters">A dictionary containing the parameters in a key-value fashion</param>
            /// <param name="soapAction">The SOAPAction value, as specified in the Web Service's WSDL (or NULL to use the url parameter)</param>
            /// <param name="useSOAP12">Set this to TRUE to use the SOAP v1.2 protocol, FALSE to use the SOAP v1.1 (default)</param>
            /// <returns>A string containing the raw Web Service response</returns>
            public static string SendSOAPRequest(string url, string action, Dictionary<string, string> parameters, string soapAction = null, bool useSOAP12 = false)
            {
                // Create the SOAP envelope
                XmlDocument soapEnvelopeXml = new XmlDocument();
                var xmlStr = (useSOAP12)
                    ? @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                      xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                      xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                      <soap12:Body>
                        <{0} xmlns=""{1}"">{2}</{0}>
                      </soap12:Body>
                    </soap12:Envelope>"
                    : @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                        <soap:Body>
                           <{0} xmlns=""{1}"">{2}</{0}>
                        </soap:Body>
                    </soap:Envelope>";
                string parms = string.Join(string.Empty, parameters.Select(kv => String.Format("<{0}>{1}</{0}>", kv.Key, kv.Value)).ToArray());
                var s = String.Format(xmlStr, action, new Uri(url).GetLeftPart(UriPartial.Authority) + "/", parms);
                soapEnvelopeXml.LoadXml(s);

                // Create the web request
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("SOAPAction", soapAction ?? url);
                webRequest.ContentType = (useSOAP12) ? "application/soap+xml;charset=\"utf-8\"" : "text/xml;charset=\"utf-8\"";
                webRequest.Accept = (useSOAP12) ? "application/soap+xml" : "text/xml";
                webRequest.Method = "POST";

                // Insert SOAP envelope
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                // Send request and retrieve result
                string result;
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                    }
                }
                return result;
            }
        }



    }
}