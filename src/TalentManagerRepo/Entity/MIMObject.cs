using System;

namespace TalentManagerRepo.Entity
{
    public class MIMObject
    {
        #region Constructors

        public MIMObject()
        {
        }

        public MIMObject(EmployeeItem e)
        {
            Id = e.Id;
            FirstName = e.FirstName;
            LastName = e.LastName;
            Email = e.Email;
            DepartmentId = e.DepartmentId;
            DepartmentName = e.DepartmentName;
            SecondEmail = e.Details.SecondEmail;
            Superior = e.Details.Superior;
            SuperiorId = e.Details.SuperiorId;
            EmploymentDatetime = e.EmploymentDatetime;
            LastUpdated = e.LastUpdated;
            Birthdate = e.Birthdate;
            Gender = e.Gender;
            AnniversaryDate = e.Details.AnniversaryDate;
            Nationality = e.Details.Nationality;
            Address = e.Details.Address;
            POBox = e.Details.POBox;
            SSN = e.Details.SSN;
            EmploymentId = e.Details.EmploymentId;
            Zip = e.Details.Zip;
            Country = e.Details.Country;
            PhoneFax = e.Details.PhoneFax;
            PhoneMobile = e.Details.PhoneMobile;
            PhonePrivate = e.Details.PhonePrivate;
            PhoneFax = e.Details.PhoneFax;
            WorkHours = e.Details.WorkHours;
            WorkLocation = e.Details.WorkLocation;
            WorkPercentage = e.Details.WorkPercentage;
            AvailabilityDate  = e.Details.AvailabilityDate;
            StaffCategory   = e.Details.StaffCategory;
            CostCenter = e.Details.CostCenter;
            JobTitle = e.Details.JobTitle?.Name;
            JobType = e.Details.JobType?.Name;
            //PreferredLanguage = e.Details.PreferredLanguage;
            Rank = e.Details.Rank;
            SeniorityDate = e.Details.SeniorityDate;
            ReasonForLeavingId = e.Details.ReasonForLeavingId;
            FTE = e.Details.FTE;
            LastWorkingDate = e.Details.LastWorkingDate;
            EmploymentEndDate = e.Details.EmploymentEndDate;
            Salary = e.Details.Salary;
        }


        #endregion


        #region Properties



        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public int Gender { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime EmploymentDatetime { get; set; }
        public DateTime LastUpdated { get; set; }
        public string SecondEmail { get; set; }
        public string Superior { get; set; }
        public int SuperiorId { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public object POBox { get; set; }
        public string SSN { get; set; }
        public string EmploymentId { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhonePrivate { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneFax { get; set; }

        public int WorkHours { get; set; }
        public string WorkLocation { get; set; }
        public int WorkPercentage { get; set; }
        public DateTime AvailabilityDate { get; set; }
        public string StaffCategory { get; set; }
        public string CostCenter { get; set; }
        public string JobTitle { get; set; }
        public string JobType { get; set; }
        public string PreferredLanguage { get; set; }

        public string Rank { get; set; }
        public DateTime LastLoginDateTimeUtc { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime SeniorityDate { get; set; }
        public int ReasonForLeavingId { get; set; }
        public double FTE { get; set; }
        public DateTime LastWorkingDate { get; set; }

        public DateTime EmploymentEndDate { get; set; }
        public string Salary { get; set; }

        #endregion
    }
}
