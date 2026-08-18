using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBWRepo.Entity
{

       public class UBWEntity
    {
        public int age { get; set; }
        public DateTime birthDate { get; set; }
        public string citizenship { get; set; }
        public string companyId { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string gender { get; set; }
        public string languageCode { get; set; }
        public string maritalStatus { get; set; }
        public string nationalIdentityNumber { get; set; }
        public string note { get; set; }
        public string personId { get; set; }
        public string personName { get; set; }
        public string personnelType { get; set; }
        public string status { get; set; }
        public string workflowState { get; set; }
        public Nameinformation nameInformation { get; set; }
        public Payrollinformation payrollInformation { get; set; }
        public Paymentinformation paymentInformation { get; set; }
        public Custominformation customInformation { get; set; }
        public Additionalinformation additionalInformation { get; set; }
        public Lastupdated lastUpdated { get; set; }
        public Employment[] employments { get; set; }
        public Contactpoint[] contactPoints { get; set; }
        public Relatedvalue[] relatedValues { get; set; }
        public Customfieldgroups customFieldGroups { get; set; }
    }

    public class Nameinformation
    {
        public string aliasName { get; set; }
        public string customNameField1 { get; set; }
        public string customNameField2 { get; set; }
        public string customNameField3 { get; set; }
        public string customNameField4 { get; set; }
        public string customNamePart { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public string title { get; set; }
    }

    public class Payrollinformation
    {
        public int batchId { get; set; }
        public string municipal { get; set; }
        public int payPeriod { get; set; }
        public string payStatus { get; set; }
        public string taxBasis { get; set; }
        public string taxNumber { get; set; }
    }

    public class Paymentinformation
    {
        public string bankAccount { get; set; }
        public string bankClearingCode { get; set; }
        public string currencyCode { get; set; }
        public string iban { get; set; }
        public string otherAccount { get; set; }
        public string payMethod { get; set; }
        public string postalAccount { get; set; }
        public string supplierGroupId { get; set; }
        public string supplierId { get; set; }
        public string swift { get; set; }
    }

    public class Custominformation
    {
        public string customCategoryField1 { get; set; }
        public string customCategoryField2 { get; set; }
        public string customCategoryField3 { get; set; }
        public string customCategoryField4 { get; set; }
        public string customCode1 { get; set; }
        public string customCode2 { get; set; }
        public string customCode3 { get; set; }
        public string customCode4 { get; set; }
        public string customCode5 { get; set; }
        public string customCode6 { get; set; }
        public string customCode7 { get; set; }
        public string customCode8 { get; set; }
        public DateTime customDate1 { get; set; }
        public DateTime customDate2 { get; set; }
        public DateTime customDate3 { get; set; }
        public bool customFlag1 { get; set; }
        public bool customFlag2 { get; set; }
    }

    public class Additionalinformation
    {
        public int graduationYear { get; set; }
        public bool hasOvertime { get; set; }
        public bool isHourlyPaid { get; set; }
        public string message { get; set; }
        public int payYear { get; set; }
        public bool piecework { get; set; }
        public DateTime probationPeriod { get; set; }
        public int retirementAge { get; set; }
        public string roomNo { get; set; }
        public DateTime startDate { get; set; }
        public string terminationReason { get; set; }
    }

    public class Lastupdated
    {
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
    }

    public class Customfieldgroups
    {
        public Annethr[] annethr { get; set; }
        public Notat[] notat { get; set; }
        public Rollerhr[] rollerhr { get; set; }
        public object[] vervhr { get; set; }
    }

    public class Annethr
    {
        public string annen_informasjon_hr_fx { get; set; }
        public DateTime dato_fra_fx { get; set; }
        public DateTime dato_til_fx { get; set; }
        public string kommentar_fx { get; set; }
    }

    public class Notat
    {
        public DateTime dato_for_oppfoelging_fx { get; set; }
        public DateTime dato_registrert_fx { get; set; }
        public string informasjon_fx { get; set; }
        public string registrert_av_fx { get; set; }
        public string status_fx { get; set; }
    }

    public class Rollerhr
    {
        public DateTime dato_fra_fx { get; set; }
        public DateTime dato_til_fx { get; set; }
        public string kommentar_fx { get; set; }
        public string roller_fx { get; set; }
    }

    public class Employment
    {
        public float allocationPercentage { get; set; }
        public string customAttributeId { get; set; }
        public float customPercentage { get; set; }
        public string customValue { get; set; }
        public string employmentDescription { get; set; }
        public DateTime employmentEndDate { get; set; }
        public int employmentId { get; set; }
        public DateTime employmentStartDate { get; set; }
        public string employmentStatus { get; set; }
        public string employmentType { get; set; }
        public DateTime employmentUpdatedAt { get; set; }
        public string employmentUpdatedBy { get; set; }
        public bool isMainPosition { get; set; }
        public DateTime lastAdvancementDate { get; set; }
        public string payScale { get; set; }
        public int payScaleAlternative { get; set; }
        public string payStep { get; set; }
        public string positionCode { get; set; }
        public string positionId { get; set; }
        public string reasonChangeCode { get; set; }
        public DateTime seniorityDate { get; set; }
        public int sorting { get; set; }
        public string workSchedule { get; set; }
    }

    public class Contactpoint
    {
        public string contactPointType { get; set; }
        public Address address { get; set; }
        public Phonenumbers phoneNumbers { get; set; }
        public Additionalcontactinfo additionalContactInfo { get; set; }
        public Lastupdated1 lastUpdated { get; set; }
    }

    public class Address
    {
        public string countryCode { get; set; }
        public string place { get; set; }
        public string postcode { get; set; }
        public string province { get; set; }
        public string streetAddress { get; set; }
    }

    public class Phonenumbers
    {
        public string telephone1 { get; set; }
        public string telephone2 { get; set; }
        public string telephone3 { get; set; }
        public string telephone4 { get; set; }
        public string telephone5 { get; set; }
        public string telephone6 { get; set; }
        public string telephone7 { get; set; }
    }

    public class Additionalcontactinfo
    {
        public string contactPerson { get; set; }
        public string contactPosition { get; set; }
        public string eMail { get; set; }
        public string eMailCc { get; set; }
        public string gtin { get; set; }
        public string url { get; set; }
    }

    public class Lastupdated1
    {
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
    }

    public class Relatedvalue
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public float percentage { get; set; }
        public string relatedValue { get; set; }
        public string relationGroup { get; set; }
        public string relationId { get; set; }
        public string relationName { get; set; }
        public float unitValue { get; set; }
    }


}