using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentManagerRepo.Entity
{

    public class Employee
    {
        public Executiontime ExecutionTime { get; set; }
        public Customer Customer { get; set; }
        public Counts Counts { get; set; }
        public Sort Sort { get; set; }
        public List<EmployeeItem> Items { get; set; }
    }

    public class EmployeeItem
    {
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
        public Details Details { get; set; } = new Details();
    }

    public class Details
    {
        //public string SecondLastName { get; set; }
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
        //public string WebSite { get; set; }
        public int WorkHours { get; set; }
        public string WorkLocation { get; set; }
        public int WorkPercentage { get; set; }
        public DateTime AvailabilityDate { get; set; }
        //public string BankAccountNumber { get; set; }
        public string StaffCategory { get; set; }
        public string CostCenter { get; set; }
        public JobtitleDef JobTitle { get; set; }
        public JobtypeDef JobType { get; set; }
        //public object NoticeTime { get; set; }
        //public Probationperiod ProbationPeriod { get; set; }
        //public string EducationArea { get; set; }
        //public string PreferredLanguage { get; set; } = string.Empty;

        public string Rank { get; set; }
        public DateTime LastLoginDateTimeUtc { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime SeniorityDate { get; set; }
        public int ReasonForLeavingId { get; set; }
        public double FTE { get; set; }
        //public string BankRegistrationNumber { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public DateTime EmploymentEndDate { get; set; }
        public string Salary { get; set; }
        //public string ThirdPartyId { get; set; }
        //public Accesslevel[] AccessLevels { get; set; }
    }

    public class JobtitleDef
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class JobtypeDef
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Probationperiod
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class Accesslevel
    {
        public int Id { get; set; }
        public string ThirdPartyId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Rolelist[] RoleList { get; set; }
    }

    public class Rolelist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThirdPartyId { get; set; }
    }


}
