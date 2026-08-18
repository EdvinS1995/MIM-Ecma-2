using System;

namespace FourHumanRepo.Entity
{


    public class CompanyEmployeesResponse
    {
        public CompanyEmployeesItem[] items { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }

    public class CompanyEmployeesItem
    {
        public int id { get; set; }
        public string[] countriesOfEmployments { get; set; }
        public string userStatus { get; set; }
        public string userCalculatedStatus { get; set; }
        public float totalFteFactor { get; set; }
        public int employmentCount { get; set; }
        public string companyEmployeeCalculatedStatus { get; set; }
        public DateTime? statusDate { get; set; }
        public DateTime? companyEmployeeOffboardingDate { get; set; }
        public bool mainEmployer { get; set; }
        public bool hasMainEmployer { get; set; }
        public int companyId { get; set; }
        public string userId { get; set; }
        public string resourceType { get; set; }
        public DateTime? seniorityDate { get; set; }
        public Primaryemployment primaryEmployment { get; set; }
        public string employeeId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string salarySystemCompanyId { get; set; }
        public string selfSickness { get; set; }
        public Freeemployer freeEmployer { get; set; }
        public Primarylocation primaryLocation { get; set; }
        public bool? readyForPayment { get; set; }
        public string readyForPaymentMessage { get; set; }
        public string workStatus { get; set; }
        public string effectiveStatus { get; set; }
        public int referenceId { get; set; }
        public string uuid { get; set; }
        public DateTime? retirementDate { get; set; }
        public DateTime? maxDateFollowUpSickness { get; set; }
        public DateTime? maxDateSicknessRefund { get; set; }
        public string voluntaryPartTime { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
        public DateTime? effectiveStatusDate { get; set; }
    }

    public class Primaryemployment
    {
        public int id { get; set; }
        public string idExt { get; set; }
        public float? fteFactor { get; set; }
        public float? paymentPercentage { get; set; }
        public int orgUnitId { get; set; }
        public string orgUnitExternalId { get; set; }
    }

    public class Freeemployer
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }

    public class Primarylocation
    {
        public string companyNumber { get; set; }
        public string name { get; set; }
        public string externalId { get; set; }
    }



    public class UsersResponse
    {
        public UserItem[] items { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }

    public class UserItem
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string userPhoto { get; set; }
        public int instanceUserId { get; set; }
        public string employmentStatus { get; set; }
        public string employmentCalculatedStatus { get; set; }
        public int? remainingDays { get; set; }
        public string userStatus { get; set; }
        public string userCalculatedStatus { get; set; }
        public int? invitationDateTimestamp { get; set; }
        public string identityProvider { get; set; }
        public string email { get; set; }

        public string workEmail { get; set; }
        public bool hasUserEmail { get; set; }
        public string loginMethod { get; set; }
        public bool hasActiveDirectoryLogin { get; set; }
        public int employmentCount { get; set; }
        public bool employed { get; set; }
        public int companyIds { get; set; }
        public bool multipleCompanyEmployments { get; set; }
        public string[] employeeIds { get; set; }
        public string jobTitle { get; set; }
        public int companyId { get; set; }
        public int primaryEmploymentId { get; set; }
        public int redirectEmploymentId { get; set; }
        public string emailWork { get; set; }
        public string phoneNumberWork { get; set; }

        // NY: privat kontaktinfo også på UserItem hvis det faktisk kommer derfra
        public Privatecontactdetails privateContactDetails { get; set; }

    }


    public class EmploymentsResponse
    {
        public EmploymentItem[] items { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }

    public class EmploymentItem
    {
        public int id { get; set; }
        public string number { get; set; }
        public string uuid { get; set; }
        public string externalId { get; set; }
        public string employeeId { get; set; }
        public string employmentStatus { get; set; }
        public string employmentCalculatedStatus { get; set; }
        public DateTime? statusDate { get; set; }
        //public string documentStatus { get; set; }
        public string country { get; set; }
        public bool isPrimaryEmployment { get; set; }
        public bool isLastEmployment { get; set; }
        public string reasonForEmployment { get; set; }
        public string remainingDays { get; set; }
        public DateTime? terminationOrOffboardingDate { get; set; }
        public string terminateReason { get; set; }
        public User user { get; set; }
        public Orgunit orgUnit { get; set; }
        public Company company { get; set; }
        public bool hasEmploymentManager { get; set; }
        public Manager manager { get; set; }
        public float? fteFactor { get; set; }
        public string presentFteFactor { get; set; }
        public bool hasEmploymentLocation { get; set; }
        public Companylocation companyLocation { get; set; }
        public Job job { get; set; }
        public string positionTitle { get; set; }
        public string personalJobDescription { get; set; }
        public Employeetype employeeType { get; set; }
        public string comment { get; set; }
        public Hrrole hrRole { get; set; }
        public Hrrole1[] hrRoles { get; set; }
        public DateTime createdAt { get; set; }
        public int? invitationDateTimestamp { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? firstWorkingDay { get; set; }
        public DateTime? lastWorkingDay { get; set; }
        public string noticePeriod { get; set; }
        public string probationPeriod { get; set; }
        public string noticePeriodInProbation { get; set; }
        public Mainsalary mainSalary { get; set; }
        public Salaryrate salaryRate { get; set; }
        public bool hasOccupationalCode { get; set; }
        public Occupationalcode occupationalCode { get; set; }
        public string workRelationType { get; set; }
        public string workingHoursArrangement { get; set; }
        public float? workingHoursPerWeek { get; set; }
        public string salarySystemCompanyId { get; set; }
        public DateTime? salarySeniorityDate { get; set; }
        public string salaryType { get; set; }
        public string overtimeAllowance { get; set; }
        public string effectiveStatus { get; set; }
        public DateTime effectiveStatusDate { get; set; }
        public int referenceId { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string photo { get; set; }
        public string status { get; set; }
        public string calculatedStatus { get; set; }
    }

    public class Orgunit
    {
        public int id { get; set; }
        public string externalId { get; set; }
        public string name { get; set; }
        public int managerId { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Manager
    {
        public int id { get; set; }
        public string employeeId { get; set; }
    }

    public class Companylocation
    {
        public string companyNumber { get; set; }
        public string name { get; set; }
        public Visitaddress visitAddress { get; set; }
        public string externalId { get; set; }
    }

    public class Visitaddress
    {
        public string address { get; set; }
        public string zipCode { get; set; }
        public string city { get; set; }
        public string municipality { get; set; }
    }

    public class Job
    {
        public int id { get; set; }
        public string number { get; set; }
        public string evolutionId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public Occupationalcode[] occupationalCodes { get; set; }
    }

    public class Occupationalcode
    {
        public string country { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Employeetype
    {
        public string id { get; set; }
        public string typeId { get; set; }
        public string employeeType { get; set; }
        public string employmentType { get; set; }
        public string employmentTypeId { get; set; }
        public string status { get; set; }
    }

    public class Hrrole
    {
        public string id { get; set; }
    }

    public class Mainsalary
    {
        public int id { get; set; }
        public string name { get; set; }
        public string period { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
        public string currency { get; set; }

    }




    public class AdditionalSalary
    {
        public int id { get; set; }
        public string externalId { get; set; }
        public string name { get; set; }
        public string period { get; set; }
        public string type { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public object details { get; set; }
        public string valueType { get; set; }
        public bool additionalMainSalary { get; set; }
        public bool hasChangesDuringSnapshot { get; set; }
        public string status { get; set; }
        public bool hasChanged { get; set; }
    }


    public class Salaryrate
    {
        public string hourlyRate { get; set; }
        public string monthlyRate { get; set; }
        public string yearlyRate { get; set; }
        public string calculatedMonthlyRate { get; set; }
        public string calculatedYearlyRate { get; set; }
    }




    public class UserProfileResponse
    {
        public string id { get; set; }
        public string instanceEmployeeId { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public string profilePicture { get; set; }
        public Personalidentification personalIdentification { get; set; }
        public Paymentdetails paymentDetails { get; set; }
        public Nextofkin[] nextOfKin { get; set; }
        public Child[] children { get; set; }
        public Customfield[] customFields { get; set; }
        public Workcontactdetails workContactDetails { get; set; }

        // NY: privat kontaktinfo (matcher "privateContactDetails" i profile-JSON)
        public Privatecontactdetails privateContactDetails { get; set; }

        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
        public string referenceId { get; set; }
        public string registeredBy { get; set; }
        public string effectiveStatus { get; set; }
        public DateTime effectiveStatusDate { get; set; }
        public string uuid { get; set; }
        public string previousSnapshotId { get; set; }
        public string nextSnapshotId { get; set; }
        public Identification[] identification { get; set; }
        public Connectedemployeedata[] connectedEmployeeData { get; set; }
        public Integrations integrations { get; set; }
        public Loginmethod loginMethod { get; set; }
        public Usercalculatedstatus userCalculatedStatus { get; set; }
    }

    public class Personalidentification
    {
        public string idType { get; set; }
        public string idCountry { get; set; }
        public string idNumber { get; set; }
        public string passportCountry { get; set; }
        public string passportNumber { get; set; }
    }

    public class Paymentdetails
    {
        public string countryOfBank { get; set; }
        public string accountNumber { get; set; }
    }

    public class Workcontactdetails
    {
        public Addressinfo[] addressInfo { get; set; }
        public Contactinfo contactInfo { get; set; }
    }

    // NY: privat kontakt-details-klasse (samme struktur som workContactDetails)
    public class Privatecontactdetails
    {
        public Addressinfo[] addressInfo { get; set; }
        public Contactinfo contactInfo { get; set; }
    }

    public class Contactinfo
    {
        public Mobilephone mobilePhone { get; set; }
        public Phone phone { get; set; }
        public string email { get; set; }
        public string[] additionalContact { get; set; }
    }

    public class Mobilephone
    {
        public string phoneNumber { get; set; }
        public string dialCode { get; set; }
        public string countryCode { get; set; }
    }

    public class Addressinfo
    {
        public string id { get; set; }
        public string type { get; set; }
        public bool isCompanyLocation { get; set; }
        public bool isPostAddressAvailable { get; set; }
        public int? companyId { get; set; }
        public string name { get; set; }
        public Postaddress postAddress { get; set; }
        public Visitaddress visitAddress { get; set; }
    }

    public class Postaddress
    {
        public string address { get; set; }
        public string city { get; set; }
        public string zipCode { get; set; }
        public string country { get; set; }
        public string municipality { get; set; }
        public string buildingEntrance { get; set; }
    }




    public class Phone
    {
        public string phoneNumber { get; set; }
        public string dialCode { get; set; }
        public string countryCode { get; set; }
    }





    public class Integrations
    {
        public V4 v4 { get; set; }
    }

    public class V4
    {
        public bool enabled { get; set; }
    }

    public class Loginmethod
    {
        public string loginMethod { get; set; }
        public string email { get; set; }
        public string activeDirectoryLogin { get; set; }
    }

    public class Usercalculatedstatus
    {
        public string status { get; set; }
        public string remainingDays { get; set; }
    }

    public class Nextofkin
    {
        public string id { get; set; }
        public string externalId { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string relationshipType { get; set; }
        public Address address { get; set; }
    }

    public class Address
    {
        public Phone phone { get; set; }
    }



    public class Customfield
    {
        public string fieldId { get; set; }
        public string value { get; set; }
        public string fieldTemplateId { get; set; }
        public string valueId { get; set; }
        public string field { get; set; }
        public string fieldExternalId { get; set; }
        public string valueExternalId { get; set; }
        public string fieldExternalName { get; set; }
        public string valueExternalName { get; set; }
        //public DateTime fieldCreatedAt { get; set; }
        //public DateTime fieldUpdatedAt { get; set; }
        //public DateTime valueCreatedAt { get; set; }
        //public DateTime valueUpdatedAt { get; set; }
        //public DateTime createdAt { get; set; }
        //public DateTime updatedAt { get; set; }
        //public string  inheritedValue { get; set; }
        //public bool approvalRequired { get; set; }
    }

    public class Identification
    {
        public int id { get; set; }
        public string idType { get; set; }
        public string idNumber { get; set; }
        public string idCountry { get; set; }
    }

    public class Connectedemployeedata
    {
        public string employeeId { get; set; }
        public string organizationNumber { get; set; }
    }



    public class PersonEmploymentResponse
    {
        public string id { get; set; }
        public string number { get; set; }
        public string uuid { get; set; }
        public string externalId { get; set; }
        public string employeeId { get; set; }
        public string employmentStatus { get; set; }
        public string employmentCalculatedStatus { get; set; }
        public DateTime? statusDate { get; set; }
        //public string documentStatus { get; set; }
        public string country { get; set; }
        public bool isPrimaryEmployment { get; set; }
        public bool isLastEmployment { get; set; }
        public string reasonForEmployment { get; set; }
        public string terminationOrOffboardingDate { get; set; }
        public string terminateReason { get; set; }
        public User user { get; set; }
        public Orgunit orgUnit { get; set; }
        public Company company { get; set; }
        public bool hasEmploymentManager { get; set; }
        public Manager manager { get; set; }
        public float? fteFactor { get; set; }
        public string paymentPercentage { get; set; }
        public string presentFteFactor { get; set; }
        public string mainWorkplaceId { get; set; }
        public string managerForUnit { get; set; }
        public bool hasEmploymentLocation { get; set; }
        public Companylocation companyLocation { get; set; }
        public Job job { get; set; }
        public string positionTitle { get; set; }
        public string personalJobDescription { get; set; }
        public Employeetype employeeType { get; set; }
        public string comment { get; set; }
        public Hrrole hrRole { get; set; }
        public Hrrole1[] hrRoles { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? firstWorkingDay { get; set; }
        public DateTime? lastWorkingDay { get; set; }
        public string noticePeriod { get; set; }
        public string probationPeriod { get; set; }
        public string noticePeriodInProbation { get; set; }
        public Mainsalary mainSalary { get; set; }
        public AdditionalSalary[] additionalSalaries { get; set; }
        public string[] benefits { get; set; }
        public Deduction[] deductions { get; set; }
        public Salaryrate salaryRate { get; set; }
        public bool hasOccupationalCode { get; set; }
        public Occupationalcode occupationalCode { get; set; }
        public string workRelationType { get; set; }
        public string workingHoursArrangement { get; set; }
        public float? workingHoursPerWeek { get; set; }
        public string salarySystemCompanyId { get; set; }
        public DateTime? salarySeniorityDate { get; set; }
        public string salaryType { get; set; }
        public string overtimeAllowance { get; set; }
        public string costPlace { get; set; }
        public EmploymentCustomfield[] customFields { get; set; }
        //public string orgUnitsWithAdvancedAccess { get; set; }
        public string effectiveStatus { get; set; }
        public DateTime effectiveStatusDate { get; set; }
        public int referenceId { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
        public string previousSnapshotId { get; set; }
        public string internalTerminationReasonId { get; set; }
    }





    public class Deduction
    {
        public int id { get; set; }
        public string externalId { get; set; }
        public string name { get; set; }
        public string period { get; set; }
        public string type { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public object details { get; set; }
        public string valueType { get; set; }
        public bool additionalMainSalary { get; set; }
        public bool hasChangesDuringSnapshot { get; set; }
        public string status { get; set; }
        public bool hasChanged { get; set; }
    }




    public class Hrrole1
    {
        public int id { get; set; }
        public string hrRoleId { get; set; }
        public string title { get; set; }
        public bool isMain { get; set; }
    }

    public class EmploymentCustomfield
    {
        public string fieldId { get; set; }
        public string valueId { get; set; }
        public string fieldTemplateId { get; set; }
        public string field { get; set; }
        public string value { get; set; }
        public string fieldTemplateName { get; set; }
        public string fieldExternalId { get; set; }
        public string valueExternalId { get; set; }
        public string fieldTemplateExternalId { get; set; }
        public string fieldExternalName { get; set; }
        public string valueExternalName { get; set; }
        public string fieldTemplateExternalName { get; set; }
        public bool hasCustomFields { get; set; }
    }


    public class Child
    {
        public string id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        //public Chronicallysick chronicallySick { get; set; }
        public DateTime? birthDate { get; set; }
        public bool singleParent { get; set; }
        public string phoneNumber { get; set; }
    }

    public class Chronicallysick
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }


}
