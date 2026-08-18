using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBWRepo.Entity
{





    public class UBWUser
    {
        public DateTime dateFrom { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public DateTime birthDate { get; set; }
        public DateTime dateTo { get; set; }
        public UAdditionalinformation additionalInformation { get; set; }
        public string companyId { get; set; }
        public UCustominformation customInformation { get; set; }
        public string languageCode { get; set; }
        public ULastupdated lastUpdated { get; set; }
        public UNameinformation nameInformation { get; set; }
        public float partTimePct { get; set; }
        public UPaymentinformation paymentInformation { get; set; }
        public UBasePayrollinformation payrollInformation { get; set; }
        public UPersonalinformation personalInformation { get; set; }
        public string personId { get; set; }
        public string personnelType { get; set; }
        public UEmployment[] employments { get; set; }
        public UContactpoint[] contactPoints { get; set; }
        public UCustomfieldgroups customFieldGroups { get; set; }
        public UBaseRelatedvalue[] relatedValues { get; set; }
    }

    public class UAdditionalinformation
    {
        public int graduationYear { get; set; }
        public bool hasOvertime { get; set; }
        public bool isHourlyPaid { get; set; }
        public string message { get; set; }
        public int payYear { get; set; }
        public bool piecework { get; set; }
        public DateTime probationPeriod { get; set; }
        public string relatedPersonId { get; set; }
        public int retirementAge { get; set; }
        public string roomNo { get; set; }
        public DateTime startDate { get; set; }
        public string terminationReason { get; set; }
    }

    public class UCustominformation
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

    public class ULastupdated
    {
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
    }

    public class UNameinformation
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

    public class UPaymentinformation
    {
        public string bankAccount { get; set; }
        public string bankClearingCode { get; set; }
        public string contractId { get; set; }
        public string currencyCode { get; set; }
        public string iban { get; set; }
        public string otherAccount { get; set; }
        public string payMethod { get; set; }
        public string postalAccount { get; set; }
        public string supplierGroupId { get; set; }
        public string supplierId { get; set; }
        public string swift { get; set; }
    }

    public class UBasePayrollinformation
    {
        public int batchId { get; set; }
        public string customTaxCode { get; set; }
        public string notes { get; set; }
        public int payPeriod { get; set; }
        public string payStatus { get; set; }
        public string taxBasis { get; set; }
        public string taxCode { get; set; }
    }

    public class UPersonalinformation
    {
        public string citizenship { get; set; }
        public string gender { get; set; }
        public string maritialStatus { get; set; }
        public string nationalIdentityNo { get; set; }
    }

    public class UCustomfieldgroups
    {
        public UAnnethr[] annethr { get; set; }
        public UNotat[] notat { get; set; }
        public object[] rollerhr { get; set; }
        public object[] vervhr { get; set; }
    }

    public class UAnnethr
    {
        public int rowId { get; set; }
        public string annen_informasjon_hr_fx { get; set; }
        public DateTime dato_fra_fx { get; set; }
        public DateTime dato_til_fx { get; set; }
        public string kommentar_fx { get; set; }
    }

    public class UNotat
    {
        public int rowId { get; set; }
        public string informasjon_fx { get; set; }
        public DateTime dato_registrert_fx { get; set; }
        public string registrert_av_fx { get; set; }
        public DateTime dato_for_oppfoelging_fx { get; set; }
        public string status_fx { get; set; }
    }

    public class UEmployment
    {
        public string status { get; set; }
        public string workSchedule { get; set; }
        public string positionId { get; set; }
        public string positionCodeId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public float allocationPercentage { get; set; }
        public float customPercentage { get; set; }
        public string employmentType { get; set; }
        public string customAttributeId { get; set; }
        public string customValue { get; set; }
        public bool isMainPosition { get; set; }
        public UPayrollinformation payrollInformation { get; set; }
        public int sorting { get; set; }
        public URelatedvalue[] relatedValues { get; set; }
    }

    public class UPayrollinformation
    {
        public string payScale { get; set; }
        public int payScaleAlternative { get; set; }
        public string payStep { get; set; }
        public DateTime seniorityDate { get; set; }
        public bool excludeFromSeniorityAdvancement { get; set; }
        public DateTime lastAdvancementDate { get; set; }
        public string reasonChangeCode { get; set; }
    }

    public class URelatedvalue
    {
        public string status { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public float percentage { get; set; }
        public string positionId { get; set; }
        public string relationGroup { get; set; }
        public bool isDefaulted { get; set; }
        public bool useInFixedPd { get; set; }
        public bool useInAccountingCategories { get; set; }
        public bool useInRateCalculation { get; set; }
        public bool useInReport { get; set; }
        public string relationId { get; set; }
        public string relationName { get; set; }
        public string relatedValue { get; set; }
    }

    public class UContactpoint
    {
        public UAddress address { get; set; }
        public UAdditionalcontactinfo additionalContactInfo { get; set; }
        public string contactPointType { get; set; }
        public ULastupdated lastUpdated { get; set; }
        public UPhonenumbers phoneNumbers { get; set; }
        public int sortOrder { get; set; }
    }

    public class UAddress
    {
        public string countryCode { get; set; }
        public string place { get; set; }
        public string postcode { get; set; }
        public string province { get; set; }
        public string streetAddress { get; set; }
    }

    public class UAdditionalcontactinfo
    {
        public string contactPerson { get; set; }
        public string contactPosition { get; set; }
        public string eMail { get; set; }
        public string eMailCc { get; set; }
        public string gtin { get; set; }
        public string url { get; set; }
    }


    public class UPhonenumbers
    {
        public string telephone1 { get; set; }
        public string telephone2 { get; set; }
        public string telephone3 { get; set; }
        public string telephone4 { get; set; }
        public string telephone5 { get; set; }
        public string telephone6 { get; set; }
        public string telephone7 { get; set; }
    }

    public class UBaseRelatedvalue
    {
        public DateTime dateFrom { get; set; }
        public string status { get; set; }
        public DateTime dateTo { get; set; }
        public string description { get; set; }
        public float percentage { get; set; }
        public string relationGroup { get; set; }
        public string relationId { get; set; }
        public string relationName { get; set; }
        public string relatedValue { get; set; }
    }

}