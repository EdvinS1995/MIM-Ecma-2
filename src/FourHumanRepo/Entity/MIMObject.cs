using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourHumanRepo.Entity
{
    public class MIMObject
    {
        public string UserId { get; set; }

        public string EmployeeId { get; set; }

        public string CompanyEmployeeId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CompanyId { get; set; }

        public DateTime? SeniorityDate { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string BirthDate { get; set; }

        public string Gender { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public string CellPhone { get; set; }
        public string EmailWork { get; set; }
        public string ResidentialAddress { get; set; }
        public string FullName { get; set; }

        public string LoginMethod { get; set; }
        public string loginMethod { get; set; }
        public string email { get; set; }
        public string activeDirectoryLogin { get; set; }

        public string khNumber { get; set; }

        public string UserCustomField1 { get; set; }
        public string UserCustomField2 { get; set; }
        public string UserCustomField3 { get; set; }
        public string UserCustomField4 { get; set; }

        public string EmploymentNumber { get; set; }

        public bool IsPrimaryEmployment { get; set; }
        public string OrgUnitName { get; set; }
        public string OrgUnitExternalId { get; set; }

        public string Manager { get; set; }

        public float? FteFactor { get; set; }

        public string CompanyLocationName { get; set; }

        public string PositionTitle { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeTypeId { get; set; }   // typeId, eks. "F"
        public string EmploymentType { get; set; }
        public string EmploymentTypeStatus { get; set; }
        public string HRRole { get; set; }

        public string Job { get; set; }

        public string OccupationalCode { get; set; }

        public string EmploymentStartDate { get; set; }
        public string EmploymentEndDate { get; set; }
        public string FirstWorkingDay { get; set; }
        public string LastWorkingDay { get; set; }

        public string EmploymentCustomField1 { get; set; }
        public string EmploymentCustomField2 { get; set; }
        public string EmploymentCustomField3 { get; set; }
        public string EmploymentCustomField4 { get; set; }
        public string EmploymentCustomField5 { get; set; }
        public string EmploymentCustomField6 { get; set; }
        public string EmploymentCustomField7 { get; set; }
        public string EmploymentCustomField8 { get; set; }
        public string EmploymentCustomField9 { get; set; }
        public string EmploymentCustomField10 { get; set; }

        public string EmploymentCustomField15 { get; set; }
        public string EmploymentCustomField16 { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhone { get; set; }
        public string KhFieldId { get; set; }
        public string KhValue { get; set; }

        public MIMObject()
        {
            // Default constructor
        }

        public MIMObject(UserItem p)
        {
            if (p != null)
            {
                UserId = p.id;
                EmployeeId = p.employeeIds != null ? p.employeeIds.FirstOrDefault() : "";
                UserName = p.userName;
                UserEmail = p.email;
                CellPhone = p.phoneNumberWork;
                EmailWork = p.workEmail;
                CompanyId = p.companyId;

                email = p.email;
                activeDirectoryLogin = p.email;
                loginMethod = "ACTIVE_DIRECTORY";
                LoginMethod = "ACTIVE_DIRECTORY";
            }
        }
    }
}