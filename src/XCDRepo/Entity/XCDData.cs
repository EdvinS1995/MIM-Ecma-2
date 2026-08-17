using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCDRepo.Entity
{
    public class XCDRoot
    {
        public Record[] records { get; set; }
    }

    public class Record
    {
        public Attributes attributes { get; set; }
        public Xcdhcm__Employee_Details__R xcdhcm__Employee_Details__r { get; set; }
        public string xcdhcm__First_name__c { get; set; }
        public string xcdhcm__Last_name__c { get; set; }
        public object xcdhcm__Work_phone__c { get; set; }
        public object xcdhcm__Home_phone__c { get; set; }
        public object xcdhcm__Title__c { get; set; }
        public object xcdhcm__Personal_email__c { get; set; }
        public object Pronoun__c { get; set; }
        public Xcdhcm__Personal_Address_L__R xcdhcm__Personal_address_L__r { get; set; }
    }

    public class Attributes
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Xcdhcm__Employee_Details__R
    {
        public Attributes attributes { get; set; }
        public Xcdhcm__Job_Role__R xcdhcm__Job_Role__r { get; set; }
        public string New_joiner_first_name__c { get; set; }
        public string New_joiner_last_name__c { get; set; }
        public Xcdhcm__Office_Locations__R xcdhcm__Office_Locations__r { get; set; }
        public Xcdhcm__User__R xcdhcm__User__r { get; set; }
        public string xcdhcm__Employee_Number__c { get; set; }
        public string Employee_type__c { get; set; }
        public string Suggested_username__c { get; set; }
        public string Business_Role__c { get; set; }
        public string Management_role__c { get; set; }
        public string Personas__c { get; set; }
        public object Additional_access__c { get; set; }
        
    }

    public class Xcdhcm__Job_Role__R
    {
        public Attributes attributes { get; set; }
        public string xcdhcm__Location_no_link__c { get; set; }
        public string xcdhcm__Department_Name__c { get; set; }
        public string xcdhcm__Manager_F__c { get; set; }
        public string xcdhcm__Manager__c { get; set; }
    }


    public class Xcdhcm__Office_Locations__R
    {
        public Attributes attributes { get; set; }
        public string xcdhcm__Country__c { get; set; }
    }

    public class Xcdhcm__User__R
    {
        public Attributes attributes { get; set; }
        public string LanguageLocaleKey { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FederationIdentifier { get; set; }
    }


    public class Xcdhcm__Personal_Address_L__R
    {
        public Attributes attributes { get; set; }
        public object xcdhcm__Street__c { get; set; }
        public string xcdhcm__Postal_zipcode__c { get; set; }
        public string xcdhcm__City__c { get; set; }
        public string xcdhcm__State_province__c { get; set; }
        public string xcdhcm__Country__c { get; set; }
    }

}