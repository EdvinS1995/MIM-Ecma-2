using QuinyxRepo.QuinyxReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuinyxRepo.Entity
{

    public class EmployeeExt
    {

        public EmployeeExt()
        {
        }

        public EmployeeExt(Employee emp1)
        {
            active = emp1.active.ToString();
            leaveDate = null;
            if (emp1.active.ToString() == "0")
            {
                leaveDate = emp1.leaveDate;
            }
            address1 = emp1.address1;
            address2 = emp1.address2;
            badgeNo = emp1.badgeNo;
            cellPhone = emp1.cellPhone;
            city = emp1.city;
            country = emp1.country;
            dateOfBirth = emp1.dateOfBirth;
            email = emp1.email;
            employedDate = emp1.employedDate;
            familyName = emp1.familyName;
            givenName = emp1.givenName;
            reportingTo = emp1.reportingTo;
            role = emp1.role.ToString();
            sex = emp1.sex.ToString();
            socsecNo = emp1.socsecNo;
            zip = emp1.zip;
            restId = emp1.restId.ToString();
            staffCat = emp1.staffCat.ToString();
            staffCatName = emp1.staffCatName;
            cardNo = emp1.cardNo;
            id = emp1.id;
            costCentre = emp1.costCentre;
            costCentreExtCode = emp1.costCentreExtCode;
            
            lastDayOfEmployment = emp1.lastDayOfEmployment;
        }

        //from employee

        public string badgeNo { get; set; }
        public string familyName { get; set; }

        public string givenName { get; set; }

        public string cellPhone { get; set; }

        public DateTime? dateOfBirth { get; set; }

        public DateTime? employedDate { get; set; }

        public DateTime? lastDayOfEmployment { get; set; }

        public DateTime? leaveDate { get; set; }

        public string email { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }

        public string zip { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string active { get; set; }

        public string socsecNo { get; set; }

        public string role { get; set; }

        public string sex { get; set; }

        public string reportingTo { get; set; }

        public string restId { get; set; }

        public string staffCat { get; set; }
        public string staffCatName { get; set; }

        public string cardNo { get; set; }

        public int costCentre { get; set; }
        public string costCentreExtCode { get; set; }

        //extra

        public string UnitName { get; set; }

        public string UnitExtId { get; set; }

        //from agreement

        public string agreementId { get; set; }

        public string templateId { get; set; }
        public decimal? rate { get; set; }

        public string TemplateName { get; set; }

        public string TemplateExtId { get; set; }

        public string EmploymentPercent { get; set; }

        public int FullEmploymentHrs { get; set; }

        public int id { get; set; }

        //from balance

        public decimal SaldoFerie { get; set; }
        public decimal SaldoTidbank { get; set; }
        public decimal SaldoJubileumsuke { get; set; }

        public decimal SaldoSyktBarn { get; set; }

        public decimal SumOvertid { get; set; }
    }
}
