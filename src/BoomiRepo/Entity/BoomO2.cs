using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BoomiRepo.Entity
{
    public class BoomO2
    {
        [XmlRoot(ElementName = "employee")]
        public class Employee
        {
            [XmlElement(ElementName = "goldenRecordId")]
            public string GoldenRecordId { get; set; }
            [XmlElement(ElementName = "displayname")]
            public string Displayname { get; set; }
            [XmlElement(ElementName = "firstname")]
            public string Firstname { get; set; }
            [XmlElement(ElementName = "lastname")]
            public string Lastname { get; set; }
            [XmlElement(ElementName = "preferredname")]
            public string Preferredname { get; set; }
            [XmlElement(ElementName = "gender")]
            public string Gender { get; set; }
            [XmlElement(ElementName = "jobtitle")]
            public string Jobtitle { get; set; }
            [XmlElement(ElementName = "mobilephone")]
            public string Mobilephone { get; set; }
            [XmlElement(ElementName = "workemail")]
            public string Workemail { get; set; }
            [XmlElement(ElementName = "department")]
            public string Department { get; set; }
            [XmlElement(ElementName = "main_department")]
            public string MainDepartment { get; set; }
            [XmlElement(ElementName = "sub_department")]
            public string SubDepartment { get; set; }
            [XmlElement(ElementName = "business_unit")]
            public string BusinessUnit { get; set; }
            [XmlElement(ElementName = "location")]
            public string Location { get; set; }
            [XmlElement(ElementName = "supervisorname")]
            public string Supervisorname { get; set; }
            [XmlElement(ElementName = "photourl")]
            public string Photourl { get; set; }
            [XmlElement(ElementName = "photouploaded")]
            public string Photouploaded { get; set; }
            [XmlElement(ElementName = "hiredate")]
            public string Hiredate { get; set; }
            [XmlElement(ElementName = "terminationdate")]
            public string Terminationdate { get; set; }
            [XmlElement(ElementName = "status")]
            public string Status { get; set; }
            [XmlElement(ElementName = "streetaddress")]
            public string Streetaddress { get; set; }
            [XmlElement(ElementName = "state")]
            public string State { get; set; }
            [XmlElement(ElementName = "iamdescription")]
            public string Iamdescription { get; set; }
            [XmlElement(ElementName = "iamcorporatessid")]
            public string Iamcorporatessid { get; set; }
            [XmlElement(ElementName = "initials")]
            public string Initials { get; set; }
            [XmlElement(ElementName = "iamaccountname")]
            public string Iamaccountname { get; set; }
            [XmlElement(ElementName = "iamusername")]
            public string Iamusername { get; set; }
            [XmlElement(ElementName = "companyid")]
            public string CompanyId { get; set; }
            [XmlElement(ElementName = "company")]
            public string Company { get; set; }
            [XmlElement(ElementName = "otheremail")]
            public string Otheremail { get; set; }

            [XmlElement(ElementName = "countryname")]
            public string Countryname { get; set; }

            [XmlElement(ElementName = "countryshortname")]
            public string CountryShortName { get; set; }

            [XmlElement(ElementName = "city")]
            public string City { get; set; }
            [XmlElement(ElementName = "codepage")]
            public string Codepage { get; set; }
            [XmlElement(ElementName = "division")]
            public string Division { get; set; }
            [XmlElement(ElementName = "office")]
            public string Office { get; set; }
            [XmlElement(ElementName = "organization")]
            public string Organization { get; set; }
            [XmlElement(ElementName = "othername")]
            public string Othername { get; set; }
            [XmlElement(ElementName = "postalcode")]
            public string Postalcode { get; set; }
            [XmlElement(ElementName = "preferredlanguage")]
            public string Preferredlanguage { get; set; }
            [XmlElement(ElementName = "samaccountname")]
            public string Samaccountname { get; set; }
            [XmlElement(ElementName = "userprincipalname")]
            public string Userprincipalname { get; set; }
            [XmlElement(ElementName = "middlename")]
            public string Middlename { get; set; }
            [XmlElement(ElementName = "computertype")]
            public string Computertype { get; set; }
            [XmlElement(ElementName = "extraspecification")]
            public string Extraspecification { get; set; }
            [XmlElement(ElementName = "emailgroup")]
            public string Emailgroup { get; set; }
            [XmlElement(ElementName = "externalinternal")]
            public string Externalinternal { get; set; }
            [XmlElement(ElementName = "keyboardlanguage")]
            public string Keyboardlanguage { get; set; }
            [XmlElement(ElementName = "attachment")]
            public string Attachment { get; set; }
            [XmlElement(ElementName = "onboarded")]
            public string Onboarded { get; set; }
            [XmlElement(ElementName = "countrynumber")]
            public string Countrynumber { get; set; }
            [XmlElement(ElementName = "countrycode")]
            public string Countrycode { get; set; }
            [XmlElement(ElementName = "profiletemplates")]
            public string Profiletemplates { get; set; }
            [XmlElement(ElementName = "supervisoremail")]
            public string Supervisoremail { get; set; }
            [XmlElement(ElementName = "dateofbirth")]
            public string Dateofbirth { get; set; }
            [XmlElement(ElementName = "bamboohr_employeeid")]
            public string Bamboohr_employeeid { get; set; }
            [XmlElement(ElementName = "bamboohr_employeenumber")]
            public string Bamboohr_employeenumber { get; set; }
            
            
            [XmlElement(ElementName = "bamboohr_supervisorid")]
            public string Bamboohr_supervisorid { get; set; }

            public string qrilloverride { get; set; }
            [XmlElement(ElementName = "qrilloverride")]

            [XmlElement(ElementName = "bamboohr_supervisoremployeenumber")]
            public string Bamboohr_supervisoremployeenumber { get; set; }

            [XmlElement(ElementName = "supervisoremployeerecordid")]
            public string Supervisoremployeerecordid { get; set; }
            
           
            [XmlElement(ElementName = "paycom_employeeid")]
            public string Paycom_employeeid { get; set; }
            
            [XmlElement(ElementName = "paycom_supervisorid")]
            public string Paycom_supervisorid { get; set; }
            
            [XmlElement(ElementName = "ocshr_employeeid")]
            public string Ocshr_employeeid { get; set; }
            
            [XmlElement(ElementName = "ocshr_supervisorid")]
            public string Ocshr_supervisorid { get; set; }
            [XmlElement(ElementName = "createdby")]
            public string Createdby { get; set; }
            [XmlElement(ElementName = "keycardorderplaced")]
            public string Keycardorderplaced { get; set; }
            [XmlElement(ElementName = "fornebuporten_intranet")]
            public string Fornebuporten_intranet { get; set; }
            [XmlElement(ElementName = "keycards__fornebu___access_")]
            public string Keycards__fornebu___access_ { get; set; }
            [XmlElement(ElementName = "manager_role")]
            public string Manager_role { get; set; }
            [XmlElement(ElementName = "extrasoftwareneeded")]
            public string Extrasoftwareneeded { get; set; }
            [XmlElement(ElementName = "lifestyle_access_request_sent")]
            public string Lifestyle_access_request_sent { get; set; }
            [XmlElement(ElementName = "fornebu_intranet_access_request_sent")]
            public string Fornebu_intranet_access_request_sent { get; set; }
            [XmlElement(ElementName = "applicationlist")]
            public string Applicationlist { get; set; }
        }


        [XmlRoot(ElementName = "employees")]
        public class EmployeeList
        {
            [XmlElement(ElementName = "employee")]
            public List<Employee> Employees { get; set; }
        }
    }
}
