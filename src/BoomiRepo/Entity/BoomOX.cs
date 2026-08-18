using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BoomiRepo.Entity
{
    public class BoomOX
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
            [XmlElement(ElementName = "gender")]
            public string Gender { get; set; }
            [XmlElement(ElementName = "jobtitle")]
            public string Jobtitle { get; set; }
            [XmlElement(ElementName = "workphone")]
            public string Workphone { get; set; }
            [XmlElement(ElementName = "mobilephone")]
            public string Mobilephone { get; set; }
            [XmlElement(ElementName = "workemail")]
            public string Workemail { get; set; }
            [XmlElement(ElementName = "department")]
            public string Department { get; set; }
            [XmlElement(ElementName = "location")]
            public string Location { get; set; }
            [XmlElement(ElementName = "linkedin")]
            public string Linkedin { get; set; }
            [XmlElement(ElementName = "workphoneextension")]
            public string Workphoneextension { get; set; }
            [XmlElement(ElementName = "supervisorname")]
            public string Supervisorname { get; set; }
            [XmlElement(ElementName = "photourl")]
            public string Photourl { get; set; }
            [XmlElement(ElementName = "canuploadphoto")]
            public string Canuploadphoto { get; set; }
            [XmlElement(ElementName = "photouploaded")]
            public string Photouploaded { get; set; }
            [XmlElement(ElementName = "username")]
            public string Username { get; set; }
            [XmlElement(ElementName = "employmenttype")]
            public string Employmenttype { get; set; }
            [XmlElement(ElementName = "hiredate")]
            public string Hiredate { get; set; }
            [XmlElement(ElementName = "terminationdate")]
            public string Terminationdate { get; set; }
            [XmlElement(ElementName = "status")]
            public string Status { get; set; }
            [XmlElement(ElementName = "streetaddress")]
            public string Streetaddress { get; set; }
            [XmlElement(ElementName = "postaladdress")]
            public string Postaladdress { get; set; }
            [XmlElement(ElementName = "postofficebox")]
            public string Postofficebox { get; set; }
            [XmlElement(ElementName = "state")]
            public string State { get; set; }
            [XmlElement(ElementName = "iamdescription")]
            public string Iamdescription { get; set; }
            [XmlElement(ElementName = "iamcorporatessid")]
            public string Iamcorporatessid { get; set; }
            [XmlElement(ElementName = "birthdate")]
            public string Birthdate { get; set; }
            [XmlElement(ElementName = "initials")]
            public string Initials { get; set; }
            [XmlElement(ElementName = "iamaccountname")]
            public string Iamaccountname { get; set; }
            [XmlElement(ElementName = "iamusername")]
            public string Iamusername { get; set; }
            [XmlElement(ElementName = "company")]
            public string Company { get; set; }
            [XmlElement(ElementName = "otheremail")]
            public string Otheremail { get; set; }
            [XmlElement(ElementName = "countryname")]
            public string Countryname { get; set; }
            [XmlElement(ElementName = "deleted")]
            public string Deleted { get; set; }
            [XmlElement(ElementName = "cannotchangepassword")]
            public string Cannotchangepassword { get; set; }
            [XmlElement(ElementName = "city")]
            public string City { get; set; }
            [XmlElement(ElementName = "codepage")]
            public string Codepage { get; set; }
            [XmlElement(ElementName = "division")]
            public string Division { get; set; }
            [XmlElement(ElementName = "enabled")]
            public string Enabled { get; set; }
            [XmlElement(ElementName = "fax")]
            public string Fax { get; set; }
            [XmlElement(ElementName = "homedirectory")]
            public string Homedirectory { get; set; }
            [XmlElement(ElementName = "homedrive")]
            public string Homedrive { get; set; }
            [XmlElement(ElementName = "homepage")]
            public string Homepage { get; set; }
            [XmlElement(ElementName = "homephone")]
            public string Homephone { get; set; }
            [XmlElement(ElementName = "instancetype")]
            public string Instancetype { get; set; }
            [XmlElement(ElementName = "internetencoding")]
            public string Internetencoding { get; set; }
            [XmlElement(ElementName = "lastbadpasswordattempt")]
            public string Lastbadpasswordattempt { get; set; }
            [XmlElement(ElementName = "lastlogondate")]
            public string Lastlogondate { get; set; }
            [XmlElement(ElementName = "lockedout")]
            public string Lockedout { get; set; }
            [XmlElement(ElementName = "logoncount")]
            public string Logoncount { get; set; }
            [XmlElement(ElementName = "office")]
            public string Office { get; set; }
            [XmlElement(ElementName = "officephone")]
            public string Officephone { get; set; }
            [XmlElement(ElementName = "passwordexpired")]
            public string Passwordexpired { get; set; }
            [XmlElement(ElementName = "passwordlastset")]
            public string Passwordlastset { get; set; }
            [XmlElement(ElementName = "passwordnotrequired")]
            public string Passwordnotrequired { get; set; }
            [XmlElement(ElementName = "organization")]
            public string Organization { get; set; }
            [XmlElement(ElementName = "pobox")]
            public string Pobox { get; set; }
            [XmlElement(ElementName = "postalcode")]
            public string Postalcode { get; set; }
            [XmlElement(ElementName = "preferredlanguage")]
            public string Preferredlanguage { get; set; }
            [XmlElement(ElementName = "samaccountname")]
            public string Samaccountname { get; set; }
            [XmlElement(ElementName = "telephonenumber")]
            public string Telephonenumber { get; set; }
            [XmlElement(ElementName = "userprincipalname")]
            public string Userprincipalname { get; set; }
            [XmlElement(ElementName = "managerrole")]
            public string Managerrole { get; set; }
            [XmlElement(ElementName = "middlename")]
            public string Middlename { get; set; }
            [XmlElement(ElementName = "transfernumber")]
            public string Transfernumber { get; set; }
            [XmlElement(ElementName = "computertype")]
            public string Computertype { get; set; }
            [XmlElement(ElementName = "phonetype")]
            public string Phonetype { get; set; }
            [XmlElement(ElementName = "extraneeds")]
            public string Extraneeds { get; set; }
            [XmlElement(ElementName = "extraspecification")]
            public string Extraspecification { get; set; }
            [XmlElement(ElementName = "emailgroup")]
            public string Emailgroup { get; set; }
            [XmlElement(ElementName = "extrasoftwareneeded")]
            public string Extrasoftwareneeded { get; set; }
            [XmlElement(ElementName = "externalinternal")]
            public string Externalinternal { get; set; }
            [XmlElement(ElementName = "startdate")]
            public string Startdate { get; set; }
            [XmlElement(ElementName = "needssimcard")]
            public string Needssimcard { get; set; }
            [XmlElement(ElementName = "phonenumberearlierowner")]
            public string Phonenumberearlierowner { get; set; }
            [XmlElement(ElementName = "keyboardlanguage")]
            public string Keyboardlanguage { get; set; }
            [XmlElement(ElementName = "additionalinformation")]
            public string Additionalinformation { get; set; }
            [XmlElement(ElementName = "attachment")]
            public string Attachment { get; set; }
            [XmlElement(ElementName = "onboarded")]
            public string Onboarded { get; set; }
            [XmlElement(ElementName = "checkpointvpnclient")]
            public string Checkpointvpnclient { get; set; }
            [XmlElement(ElementName = "crystalreportseditor")]
            public string Crystalreportseditor { get; set; }
            [XmlElement(ElementName = "endnote")]
            public string Endnote { get; set; }
            [XmlElement(ElementName = "jeeves")]
            public string Jeeves { get; set; }
            [XmlElement(ElementName = "nicelabel")]
            public string Nicelabel { get; set; }
            [XmlElement(ElementName = "ocshr")]
            public string Ocshr { get; set; }
            [XmlElement(ElementName = "powerbi")]
            public string Powerbi { get; set; }
            [XmlElement(ElementName = "readsoftonline")]
            public string Readsoftonline { get; set; }
            [XmlElement(ElementName = "salesforce")]
            public string Salesforce { get; set; }
            [XmlElement(ElementName = "teamviewer")]
            public string Teamviewer { get; set; }
            [XmlElement(ElementName = "thinkcell")]
            public string Thinkcell { get; set; }
            [XmlElement(ElementName = "tmmaster")]
            public string Tmmaster { get; set; }
            [XmlElement(ElementName = "qualityone")]
            public string Qualityone { get; set; }
            [XmlElement(ElementName = "visio365")]
            public string Visio365 { get; set; }
            [XmlElement(ElementName = "dnbfiles")]
            public string Dnbfiles { get; set; }
            [XmlElement(ElementName = "projectonlineprofessional365")]
            public string Projectonlineprofessional365 { get; set; }
            [XmlElement(ElementName = "countrynumber")]
            public string Countrynumber { get; set; }
            [XmlElement(ElementName = "countrycode")]
            public string Countrycode { get; set; }
            [XmlElement(ElementName = "adobeacrobat")]
            public string Adobeacrobat { get; set; }
            [XmlElement(ElementName = "computerscreensize")]
            public string Computerscreensize { get; set; }
            [XmlElement(ElementName = "jmp")]
            public string Jmp { get; set; }
            [XmlElement(ElementName = "navisworksfreedom")]
            public string Navisworksfreedom { get; set; }
            [XmlElement(ElementName = "paycom")]
            public string Paycom { get; set; }
            [XmlElement(ElementName = "profiletemplates")]
            public string Profiletemplates { get; set; }
            [XmlElement(ElementName = "supervisoremail")]
            public string Supervisoremail { get; set; }
            [XmlElement(ElementName = "autodeskdwg")]
            public string Autodeskdwg { get; set; }
            [XmlElement(ElementName = "dateofbirth")]
            public string Dateofbirth { get; set; }
            [XmlElement(ElementName = "bamboohr_employeeid")]
            public string Bamboohr_employeeid { get; set; }
            [XmlElement(ElementName = "bamboohr_employeenumber")]
            public string Bamboohr_employeenumber { get; set; }
            [XmlElement(ElementName = "bamboohr_supervisorid")]
            public string Bamboohr_supervisorid { get; set; }
            [XmlElement(ElementName = "bamboohr_supervisoremployeenumber")]
            public string Bamboohr_supervisoremployeenumber { get; set; }
            [XmlElement(ElementName = "bamboohr_last_updated")]
            public string Bamboohr_last_updated { get; set; }
            [XmlElement(ElementName = "paycom_employeeid")]
            public string Paycom_employeeid { get; set; }
            [XmlElement(ElementName = "paycom_last_updated")]
            public string Paycom_last_updated { get; set; }
            [XmlElement(ElementName = "ocshr_employeeid")]
            public string Ocshr_employeeid { get; set; }
            [XmlElement(ElementName = "ocshr_supervisorid")]
            public string Ocshr_supervisorid { get; set; }
            [XmlElement(ElementName = "ocshr_last_updated")]
            public string Ocshr_last_updated { get; set; }
        }

        [XmlRoot(ElementName = "employees")]
        public class EmployeeList
        {
            [XmlElement(ElementName = "employee")]
            public List<Employee> Employees { get; set; }
        }
    }
}
