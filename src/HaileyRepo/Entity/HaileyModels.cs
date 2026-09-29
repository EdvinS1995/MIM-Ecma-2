using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HaileyRepo.Entity
{
    /// <summary>
    /// Wire DTO matching GET /Employees list items exactly (HaileyHR "EmployeeListItem").
    /// CustomFieldsData is company-defined (keys/types vary per tenant) so it is kept as a
    /// raw dictionary here and flattened into CustomField_* attributes separately in MIMRepo,
    /// rather than reflected as a normal property on MIMPerson.
    /// </summary>
    public class EmployeeListItemDto
    {
        public string EmployeeId { get; set; }
        public string AccountStatus { get; set; }
        public string LastUpdated { get; set; }
        public string WorkPhone { get; set; }
        public List<string> TitleIds { get; set; }
        public string CompanyEmail { get; set; }
        public string DepartmentId { get; set; }
        public string LocationId { get; set; }
        public string LegalEntityId { get; set; }
        public string BusinessAreaId { get; set; }
        public string CostCenterId { get; set; }
        public string ManagerEmployeeId { get; set; }
        public List<string> TeamIds { get; set; }
        public string DateOfJoining { get; set; }
        public string LastDayOfEmployment { get; set; }
        public string LastWorkingDay { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentType { get; set; }
        public string NoticePeriod { get; set; }
        public string NoticePeriodUnit { get; set; }
        public string EndOfProbationaryPeriod { get; set; }
        public string FixedTermType { get; set; }
        public string EndOfFixedTerm { get; set; }
        public string ScopePercentage { get; set; }
        public string ScopeHours { get; set; }
        public string VacationDays { get; set; }
        public string SubstitutingForEmployeeId { get; set; }
        public string SustituteReasonId { get; set; }
        public string EmploymentNumber { get; set; }
        public string BankName { get; set; }
        public string ClearingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Swift { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string PersonalIdentityNumber { get; set; }
        public string PrivateEmail { get; set; }
        public string PrivatePhone { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IceName { get; set; }
        public string IcePhone { get; set; }
        public string IceRelation { get; set; }
        public string EmploymentSequenceNumber { get; set; }

        [JsonProperty("customFieldsData")]
        public Dictionary<string, object> CustomFieldsData { get; set; }
    }

    /// <summary>
    /// One record from GET /Employees/{employeeId} -> jobData.employment.employments[].
    /// Only the fields needed to pick and flatten the "effectual" employment are modeled;
    /// organizationalInformation/terms/customFieldsData on this record duplicate fields
    /// already present on EmployeeListItemDto and are intentionally not pulled in again.
    /// </summary>
    public class EmploymentVmDto
    {
        public string EmploymentId { get; set; }
        public int? Priority { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LastWorkingDay { get; set; }
        public string Status { get; set; } // "WillStart", "Active" or "Ended"
    }

    /// <summary>
    /// One record from GET /Employees/{employeeId} -> salaries[]. History and the various
    /// pendingApproval sub-objects are intentionally not synced (approval-workflow detail,
    /// not identity data) — see README known limitations.
    /// </summary>
    public class SalaryDto
    {
        public string SalaryId { get; set; }
        public string Title { get; set; }
        public string SalaryTypeName { get; set; }
        public string PayPeriod { get; set; }
        public string CurrencyCode { get; set; }
        public string EndDate { get; set; }
    }

    /// <summary>
    /// POST /Employee ("Quick add Employee") request body. Only fields Hailey's quick-add
    /// endpoint actually accepts — this is deliberately far narrower than MIMPerson, see
    /// README known limitations.
    /// </summary>
    public class AddEmployeeRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyEmail { get; set; }
        public string StartDate { get; set; }
        public string DepartmentId { get; set; }
        public string LegalEntityId { get; set; }
        public string CostCenterId { get; set; }
        public string BusinessAreaId { get; set; }
        public string LocationId { get; set; }
        public string ReportsToEmployeeId { get; set; }
        public List<string> TeamIds { get; set; }
        public List<string> TitleIds { get; set; }
    }

    /// <summary>PUT /Employee/{employeeId}/CompanyEmail request body.</summary>
    public class UpdateCompanyEmailRequestDto
    {
        public string CompanyEmail { get; set; }
    }

    /// <summary>PUT /Employee/{employeeId}/CustomEmployeeField request body.</summary>
    public class UpdateCustomEmployeeFieldsRequestDto
    {
        public Dictionary<string, object> CustomFieldsData { get; set; }
    }

    /// <summary>
    /// Merged MIM object: base fields from the /Employees list, plus the picked "effectual"
    /// employment and current salary from the per-employee /Employees/{id} detail call.
    /// Schema is built by reflecting over this type (Pattern B) — see MIMRepo.CreateSchema.
    /// </summary>
    public class MIMPerson
    {
        public string EmployeeId { get; set; }
        public string AccountStatus { get; set; }
        public string LastUpdated { get; set; }
        public string WorkPhone { get; set; }
        public List<string> TitleIds { get; set; }
        public string CompanyEmail { get; set; }
        public string DepartmentId { get; set; }
        public string LocationId { get; set; }
        public string LegalEntityId { get; set; }
        public string BusinessAreaId { get; set; }
        public string CostCenterId { get; set; }
        public string ManagerEmployeeId { get; set; }
        public List<string> TeamIds { get; set; }
        public string DateOfJoining { get; set; }
        public string LastDayOfEmployment { get; set; }
        public string LastWorkingDay { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentType { get; set; }
        public string NoticePeriod { get; set; }
        public string NoticePeriodUnit { get; set; }
        public string EndOfProbationaryPeriod { get; set; }
        public string FixedTermType { get; set; }
        public string EndOfFixedTerm { get; set; }
        public string ScopePercentage { get; set; }
        public string ScopeHours { get; set; }
        public string VacationDays { get; set; }
        public string SubstitutingForEmployeeId { get; set; }
        public string SustituteReasonId { get; set; }
        public string EmploymentNumber { get; set; }
        public string BankName { get; set; }
        public string ClearingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Swift { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string PersonalIdentityNumber { get; set; }
        public string PrivateEmail { get; set; }
        public string PrivatePhone { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IceName { get; set; }
        public string IcePhone { get; set; }
        public string IceRelation { get; set; }
        public string EmploymentSequenceNumber { get; set; }

        // Additive fields merged in from the per-employee detail call (jobData.employment.employments[]).
        public string EffectualEmploymentId { get; set; }
        public string EffectualEmploymentStartDate { get; set; }
        public string EffectualEmploymentEndDate { get; set; }
        public string EffectualEmploymentLastWorkingDay { get; set; }
        public string EffectualEmploymentStatus { get; set; }
        public string EffectualEmploymentPriority { get; set; }

        // Additive fields merged in from the per-employee detail call (salaries[]).
        public string CurrentSalaryId { get; set; }
        public string CurrentSalaryTitle { get; set; }
        public string CurrentSalaryTypeName { get; set; }
        public string CurrentSalaryPayPeriod { get; set; }
        public string CurrentSalaryCurrencyCode { get; set; }
        public string CurrentSalaryEndDate { get; set; }

        public MIMPerson() { }

        public MIMPerson(EmployeeListItemDto e)
        {
            EmployeeId = e.EmployeeId;
            AccountStatus = e.AccountStatus;
            LastUpdated = e.LastUpdated;
            WorkPhone = e.WorkPhone;
            TitleIds = e.TitleIds;
            CompanyEmail = e.CompanyEmail;
            DepartmentId = e.DepartmentId;
            LocationId = e.LocationId;
            LegalEntityId = e.LegalEntityId;
            BusinessAreaId = e.BusinessAreaId;
            CostCenterId = e.CostCenterId;
            ManagerEmployeeId = e.ManagerEmployeeId;
            TeamIds = e.TeamIds;
            DateOfJoining = e.DateOfJoining;
            LastDayOfEmployment = e.LastDayOfEmployment;
            LastWorkingDay = e.LastWorkingDay;
            EmploymentStatus = e.EmploymentStatus;
            EmploymentType = e.EmploymentType;
            NoticePeriod = e.NoticePeriod;
            NoticePeriodUnit = e.NoticePeriodUnit;
            EndOfProbationaryPeriod = e.EndOfProbationaryPeriod;
            FixedTermType = e.FixedTermType;
            EndOfFixedTerm = e.EndOfFixedTerm;
            ScopePercentage = e.ScopePercentage;
            ScopeHours = e.ScopeHours;
            VacationDays = e.VacationDays;
            SubstitutingForEmployeeId = e.SubstitutingForEmployeeId;
            SustituteReasonId = e.SustituteReasonId;
            EmploymentNumber = e.EmploymentNumber;
            BankName = e.BankName;
            ClearingNumber = e.ClearingNumber;
            AccountNumber = e.AccountNumber;
            Iban = e.Iban;
            Bic = e.Bic;
            Swift = e.Swift;
            FirstName = e.FirstName;
            LastName = e.LastName;
            Gender = e.Gender;
            DateOfBirth = e.DateOfBirth;
            PersonalIdentityNumber = e.PersonalIdentityNumber;
            PrivateEmail = e.PrivateEmail;
            PrivatePhone = e.PrivatePhone;
            StreetAddress = e.StreetAddress;
            PostalCode = e.PostalCode;
            City = e.City;
            Country = e.Country;
            IceName = e.IceName;
            IcePhone = e.IcePhone;
            IceRelation = e.IceRelation;
            EmploymentSequenceNumber = e.EmploymentSequenceNumber;
        }

        /// <summary>
        /// Picks the employment that should represent "the current job" for this person and
        /// flattens it: prefer Active (lowest Priority number wins, i.e. highest priority),
        /// else the WillStart record with the earliest StartDate (next upcoming), else the
        /// Ended record with the latest EndDate (most recently ended). Returns the picked
        /// record's EmploymentId (or null if the list was empty) so the caller can log it.
        /// </summary>
        public string ApplyEffectualEmployment(List<EmploymentVmDto> employments)
        {
            if (employments == null || employments.Count == 0) return null;

            var picked =
                employments.Where(x => x.Status == "Active")
                    .OrderBy(x => x.Priority ?? int.MaxValue)
                    .ThenByDescending(x => x.StartDate)
                    .FirstOrDefault()
                ?? employments.Where(x => x.Status == "WillStart")
                    .OrderBy(x => x.StartDate)
                    .FirstOrDefault()
                ?? employments.Where(x => x.Status == "Ended")
                    .OrderByDescending(x => x.EndDate)
                    .FirstOrDefault()
                ?? employments.OrderByDescending(x => x.StartDate).FirstOrDefault();

            if (picked == null) return null;

            EffectualEmploymentId = picked.EmploymentId;
            EffectualEmploymentStartDate = picked.StartDate;
            EffectualEmploymentEndDate = picked.EndDate;
            EffectualEmploymentLastWorkingDay = picked.LastWorkingDay;
            EffectualEmploymentStatus = picked.Status;
            EffectualEmploymentPriority = picked.Priority?.ToString();

            return picked.EmploymentId;
        }

        /// <summary>
        /// Picks the salary that should represent "the current salary": the first one with no
        /// EndDate (still ongoing), else the one with the latest EndDate (most recently ended).
        /// Returns the picked record's SalaryId (or null if the list was empty) so the caller
        /// can log it, and log a warning if more than one ongoing salary was found (ambiguous).
        /// </summary>
        public string ApplyCurrentSalary(List<SalaryDto> salaries, out bool ambiguous)
        {
            ambiguous = false;
            if (salaries == null || salaries.Count == 0) return null;

            var ongoing = salaries.Where(x => string.IsNullOrEmpty(x.EndDate)).ToList();
            ambiguous = ongoing.Count > 1;

            var picked = ongoing.FirstOrDefault()
                ?? salaries.OrderByDescending(x => x.EndDate).FirstOrDefault();

            if (picked == null) return null;

            CurrentSalaryId = picked.SalaryId;
            CurrentSalaryTitle = picked.Title;
            CurrentSalaryTypeName = picked.SalaryTypeName;
            CurrentSalaryPayPeriod = picked.PayPeriod;
            CurrentSalaryCurrencyCode = picked.CurrencyCode;
            CurrentSalaryEndDate = picked.EndDate;

            return picked.SalaryId;
        }
    }
}
