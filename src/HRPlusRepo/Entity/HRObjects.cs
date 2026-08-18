using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRPlusRepo.Entity
{





    public class PersonRoot
    {
        public string odatacontext { get; set; }
        public PersonValue[] value { get; set; }
    }

    public class PersonValue
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public string SocialSecurityNumber { get; set; }
        public int? SequenceNumber { get; set; }
        public string ProtectedIdentity { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string HomeTelephone { get; set; }
        public bool HomeTelephoneSecret { get; set; }
        public string CellPhone { get; set; }
        public bool? CellPhoneSecret { get; set; }
        public string EmailWork { get; set; }
        public string ResidentialAddress { get; set; }
        public string CoAddress { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public int? CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool? IntegrationPayRoll { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string FullName { get; set; }
        public ExtraDataField[] ExtraDataField { get; set; }


    }

    public class ExtraDataField
    {
        public int Id { get; set; }

        // These fields do NOT exist in the JSON you posted, but I’m adding them
        // because they are in your desired class. They will just be null/0
        // when deserialized unless your JSON is updated to include them.
        public int GroupId { get; set; }
        public string GroupDescription { get; set; }
        public string GroupManagerUserId { get; set; }
        public string GroupManagerFullName { get; set; }
        public string GroupExternalId { get; set; }
        public string GroupEmployerId { get; set; }
        public string GroupEmployerCode { get; set; }

        // From your JSON
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        // If you also want to keep all Text/Numeric/Date values:
        public string TextValue1 { get; set; }
        public string TextValue2 { get; set; }
        public string TextValue3 { get; set; }
        public string TextValue4 { get; set; }
        public string TextValue5 { get; set; }
        public string TextValue6 { get; set; }
        public string TextValue7 { get; set; }
        public string TextValue8 { get; set; }
        public string TextValue9 { get; set; }
        public string TextValue10 { get; set; }
        public string TextValue11 { get; set; }
        public string TextValue12 { get; set; }
        public string TextValue13 { get; set; }
        public string TextValue14 { get; set; }
        public string TextValue15 { get; set; }
        public string TextValue16 { get; set; }

        public decimal NumericValue1 { get; set; }
        public decimal NumericValue2 { get; set; }
        public decimal NumericValue3 { get; set; }
        public decimal NumericValue4 { get; set; }
        public decimal NumericValue5 { get; set; }
        public decimal NumericValue6 { get; set; }
        public decimal NumericValue7 { get; set; }
        public decimal NumericValue8 { get; set; }
        public decimal NumericValue9 { get; set; }
        public decimal NumericValue10 { get; set; }
        public decimal NumericValue11 { get; set; }
        public decimal NumericValue12 { get; set; }
        public decimal NumericValue13 { get; set; }
        public decimal NumericValue14 { get; set; }
        public decimal NumericValue15 { get; set; }
        public decimal NumericValue16 { get; set; }

        public string DateValue1 { get; set; }
        public string DateValue2 { get; set; }
        public string DateValue3 { get; set; }
        public string DateValue4 { get; set; }
        public string DateValue5 { get; set; }
        public string DateValue6 { get; set; }
        public string DateValue7 { get; set; }
        public string DateValue8 { get; set; }
        public string DateValue9 { get; set; }
        public string DateValue10 { get; set; }
        public string DateValue11 { get; set; }
        public string DateValue12 { get; set; }
        public string DateValue13 { get; set; }
        public string DateValue14 { get; set; }
        public string DateValue15 { get; set; }
        public string DateValue16 { get; set; }
    }







    public class EmploymentRoot
    {
        public string odatacontext { get; set; }
        public EmploymentValue[] value { get; set; }
    }

    public class EmploymentValue
    { 
        public object SalaryIncreaseAmount { get; set; }
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string SocialSecurityNumber { get; set; }
        public int PersonSequenceNumber { get; set; }
        public string EmployerCode { get; set; }
        public string EmployerName { get; set; }
        public string CompanyCode { get; set; }
        public string UnitCode { get; set; }
        public string EmployerRegistrationNumber { get; set; }
        public string WorkNumber { get; set; }
        public int EmployeeId { get; set; }
        public int? WorkplaceId { get; set; }
        public string WorkplaceCode { get; set; }
        public string WorkplaceDescription { get; set; }
        public int? RetirementCauseId { get; set; }
        public string RetirementCauseCode { get; set; }
        public string RetirementCauseDescription { get; set; }
        public int? IdleEmploymentId { get; set; }
        public string IdleEmploymentExternalId { get; set; }
        public string IdleEmploymentDescription { get; set; }
        public int? TimeSystemId { get; set; }
        public string TimeSystemCode { get; set; }
        public string TimeSystemDescription { get; set; }
        public int? UnionAffiliationId { get; set; }
        public string UnionAffiliationUnionCode { get; set; }
        public string UnionAffiliationUnionDescription { get; set; }
        public object UnionDepartmentId { get; set; }
        public object UnionDepartmentDepartment { get; set; }
        public object UnionDepartmentDescription { get; set; }
        public object EndMarkEmploymentId { get; set; }
        public object EndMarkEmploymentExternalId { get; set; }
        public object EndMarkEmploymentDescription { get; set; }
        public int? FinalSalaryPaidId { get; set; }
        public string FinalSalaryPaidExternalId { get; set; }
        public string FinalSalaryPaidDescription { get; set; }
        public int? LASNotEligibleId { get; set; }
        public string LasNotEligibleCode { get; set; }
        public string LasNotEligibleDescription { get; set; }
        public int? RecruitmentPathId { get; set; }
        public string RecruitmentPathExternalId { get; set; }
        public string RecruitmentPathDescription { get; set; }
        public object BenefitGroupSalaryFormId { get; set; }
        public object BenefitGroupSalaryFormExternalId { get; set; }
        public object BenefitGroupSalaryFormDescription { get; set; }
        public object PaidVacationCompensationId { get; set; }
        public object PaidVacationCompensationExternalId { get; set; }
        public object PaidVacationCompensationDescription { get; set; }
        public object ManagerLevelId { get; set; }
        public object ManagerLevelCode { get; set; }
        public object ManagerLevelDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AltStartDateVacation { get; set; }
        public object AltEndDateVacation { get; set; }
        public object VacationTransitionDate { get; set; }
        public object ActivityCompensationEndDate { get; set; }
        public object LastWorkDay { get; set; }
        public object CurrentCompensationDateForTimeLimitedSickness { get; set; }
        public object ForaWorkerToOfficial { get; set; }
        public object AbsenceEndDate { get; set; }
        public object LASEndDate { get; set; }
        public int? SalaryYear { get; set; }
        public object RevisionNumberAnnualSalaryPosition { get; set; }
        public object OccOpenEndedEmployment { get; set; }
        public object SchemeLoadingType { get; set; }
        public bool? TimeLimitedActivityCompensation { get; set; }
        public object ActivityCompensationInclude { get; set; }
        public bool? VacationTransition { get; set; }
        public bool? ManualVacationCalculation { get; set; }
        public object EarlierVacationEarnings { get; set; }
       
        public object VacationEarningsOtherAuthoritySPV { get; set; }
        public object VacationConversion12ExecutedBeforeSPV { get; set; }
        public string VacationGroup { get; set; }
        public bool? BasicPosition { get; set; }
        public object RefMainEmployment { get; set; }
        public string EmploymentNumber { get; set; }
        public object SalaryRevisionChange { get; set; }
        public object CarAgreement { get; set; }
        public bool? MainEmployment { get; set; }
        public bool? CEO { get; set; }
        public object AbsencePercent { get; set; }
        public bool? PostphonedCalendar { get; set; }
        public object DeviatingVacationAndLeaveFactors { get; set; }
        public object PositionType { get; set; }
        public object BasicPositionPercent { get; set; }
        public object SpecialSchoolCaring { get; set; }
        public object PartialPensionChangeSSG { get; set; }
        public object EarlyRetirePension { get; set; }
        public object CurrentCompensationTimeLimitedSickness { get; set; }
        public object PartialReasonForAbsence { get; set; }
        public object CompMax { get; set; }
        public object ManualCompMax { get; set; }
        public object CompMin { get; set; }
        public object ManualCompMin { get; set; }
        public int? DaysVacationOverride { get; set; }
        public int? EntitledToParentalBenefit { get; set; }
        public bool? EmolumentRecipient { get; set; }
        public bool? IncludeInTalentIntegration { get; set; }
        public object TrackEmployment { get; set; }
        public string ServiceCertificate { get; set; }
        public object ExternalSystemGuid { get; set; }
        public string AgreementCategoryCode { get; set; }
        public string AgreementCategoryName { get; set; }
        public int? SalaryFormId { get; set; }
        public string SalaryFormDescription { get; set; }
        public Position[] Positions { get; set; }
        public Salaryaffecting[] SalaryAffecting { get; set; }
        public EmploymentGroups[] EmploymentGroups { get; set; }
    }

    public class Position
    {
        public int Id { get; set; }
        public int EmploymentPositionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PositionCode { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string TextValue1 { get; set; }
        public string TextValue2 { get; set; }
        public string TextValue3 { get; set; }
        public string TextValue4 { get; set; }
        public string TextValue5 { get; set; }
        public string TextValue6 { get; set; }
        public string TextValue7 { get; set; }
        public string SSYKCode { get; set; }
        public string OccupactionEmploymentCodeProfessionCode { get; set; }
        public string OccupactionEmploymentCodeDescription { get; set; }
        public string SNJobStatusExternalId { get; set; }
        public string SNJobStatusDescription { get; set; }
        public string SNEmployeeCategoryExternalId { get; set; }
        public string SNEmployeeCategoryDescription { get; set; }
        public string BESTACode { get; set; }
        public object AIDLabelCode { get; set; }
        public object AIDLabelDescription { get; set; }
        public string WorkEvaluation { get; set; }
        public string AgreementDivisionCode { get; set; }
        public string AgreementDivisionDescription { get; set; }
        public string SNForbundAgreementCode { get; set; }
        public object Text1 { get; set; }
        public object Text2 { get; set; }
        public object Text3 { get; set; }
        public object Text4 { get; set; }
        public object Text5 { get; set; }
        public object Text6 { get; set; }
        public object Text7 { get; set; }
        public object Text8 { get; set; }
        public object Text9 { get; set; }
    }

    public class Salaryaffecting
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float ActualWorkHours { get; set; }
        public float FullTimeWorkHours { get; set; }
        public float EmploymentRate { get; set; }
        

        public object WeekdayFactor { get; set; }
        public int? PartTimeTypeId { get; set; }
        public string PartTimeExternalId { get; set; }
        public string PartTimeDescription { get; set; }
        public int? WorkingHourTypeATAId { get; set; }
        public string WorkingHourTypeATADescription { get; set; }
        public object Reserve { get; set; }
        
        
        public string EmploymentRateReducedCause { get; set; }
        
        public object PayGrade { get; set; }
        public int? EmployerId { get; set; }
        public string EmployerCode { get; set; }
        public string CompanyCode { get; set; }
        public string UnitCode { get; set; }
        public string EmployerRegistrationNumber { get; set; }
        public string EmployerName { get; set; }
        public string ManagerUserId { get; set; }
        public string DepartmentId { get; set; }
        public int DepartmentTechnicalId { get; set; }
        public string DepartmentStartDate { get; set; }
        public object DepartmentEndDate { get; set; }
        public string Description { get; set; }
        public object CostCenter { get; set; }
        public string AccountPart1 { get; set; }
        public string AccountPart2 { get; set; }
        public string AccountPart3 { get; set; }
        public int? SalaryFormId { get; set; }
        public string SalaryFormDescription { get; set; }
        public string OrganizationalConcept1 { get; set; }
        public string OrganizationalConcept2 { get; set; }
        public string OrganizationalConcept3 { get; set; }
    }


    public class EmploymentGroups
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string GroupDescription { get; set; }
        public string GroupManagerUserId { get; set; }
        public string GroupManagerFullName { get; set; }
        public string GroupExternalId { get; set; }
        public string GroupEmployerId { get; set; }
        public string GroupEmployerCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
  
    }

    public class WorkplaceRoot
    {
        public string odatacontext { get; set; }
        public WorkplaceValue[] value { get; set; }
    }

    public class WorkplaceValue
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public string EmployerCode { get; set; }
        public string CompanyCode { get; set; }
        public string UnitCode { get; set; }
        public string EmployerName { get; set; }
        public string EmployerRegistrationNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string WorkplaceId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string CoAddress { get; set; }
        public string PostalCode { get; set; }
        public string PostAdderess { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string SCBCFANumber { get; set; }
        public string SNWorkplaceNumber { get; set; }
        public object SNWorkplace { get; set; }
        public object SCBWorklocation { get; set; }
        public object MunicipalityCode { get; set; }
        public object CitypartCode { get; set; }
        public string Address2 { get; set; }
        public object CoAddress2 { get; set; }
        public string PostalCode2 { get; set; }
        public string PostAdderess2 { get; set; }
        public object Address3 { get; set; }
        public object CoAddress3 { get; set; }
        public object PostalCode3 { get; set; }
        public object PostAdderess3 { get; set; }
        public string Homepage { get; set; }
        public bool? RegionalAid { get; set; }
    }


}
