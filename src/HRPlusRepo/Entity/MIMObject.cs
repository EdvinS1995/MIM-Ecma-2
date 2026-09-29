using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRPlusRepo.Entity
{
    public class MIMObject
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int EmployeeId { get; set; }
        public int WorkplaceId { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CellPhone { get; set; }
        public string EmailWork { get; set; }
        public string ResidentialAddress { get; set; }

        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string FullName { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EmployerCode { get; set; }
        public string EmployerName { get; set; }
        public string EmployerRegistrationNumber { get; set; }
        public string CompanyCode { get; set; }
        public string UnitCode { get; set; }
        public int PersonId { get; set; }
        public int PersonSequenceNumber { get; set; }
        public string EmploymentNumber { get; set; }
        public string WorkNumber { get; set; }

        public int EmploymentPositionId { get; set; }
        public string PositionCode { get; set; }
        public string Description { get; set; }
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
        public string WorkEvaluation { get; set; }
        public string AgreementDivisionCode { get; set; }
        public string AgreementDivisionDescription { get; set; }
        public string SNForbundAgreementCode { get; set; }

        public string PartTimeDescription { get; set; }
        public string WorkingHourTypeATADescription { get; set; }
        public string EmploymentRateReducedCause { get; set; }

        public string ManagerUserId { get; set; }
        public string DepartmentId { get; set; }
        public int DepartmentTechnicalId { get; set; }
        public string DepartmentDescription { get; set; }

        public string AccountPart1 { get; set; }
        public string AccountPart2 { get; set; }
        public string AccountPart3 { get; set; }

        public string OrganizationalConcept1 { get; set; }
        public string OrganizationalConcept2 { get; set; }
        public string OrganizationalConcept3 { get; set; }


        public string WorkplaceCode { get; set; }
        public string WorkplaceDescription { get; set; }
        public string Address { get; set; }
        public string CoAddress { get; set; }
        public string PostalCode { get; set; }
        public string PostAdderess { get; set; }
        public string GroupManagerFullName { get; set; }
        public string GroupManagerUserId { get; set; }
        public string DateValue1 { get; set; }
        public string DateValue2 { get; set; }
        public string DescriptionTitle { get; set; }
        public string Descriptiondivison { get; set; }

        public MIMObject()
        {
            // Default constructor
        }

        public MIMObject(PersonValue p, int daysBeforeStart)
        {
            if (p != null)
            {
                UserId = p.UserId;
                UserName = p.UserName;
                UserEmail = p.UserEmail;
                LastName = p.LastName;
                FirstName = p.FirstName;
                CellPhone = p.CellPhone;
                EmailWork = p.EmailWork;
                Gender = p.Gender;
                BirthDate = p.BirthDate;
                FullName = p.FullName;
                UserPhone = p.UserPhone;
                Id = p.Id;
                UnitCode = PickEmployee(p.Employees, daysBeforeStart)?.UnitCode;
                HireDate = PickEmployee(p.Employees, daysBeforeStart)?.HireDate.ToString("yyyy-MM-dd");
                EndDate = PickEmployee(p.Employees, daysBeforeStart)?.EndDate?.ToString("yyyy-MM-dd");
            }
        }

        private Employee PickEmployee(Employee[] employees, int daysBeforeStart)
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-daysBeforeStart).Date;
            DateTime endDate = DateTime.Now.Date.AddDays(daysBeforeStart).Date;

            var emps = employees.Where(x => x.HireDate.Date <= endDate && (x.EndDate == null || x.EndDate?.Date > startDate));
            var emp = emps.OrderByDescending(x => x.HireDate).FirstOrDefault();

            // Implement your logic to pick an employee from the list
            return emp;
        }
    }
}