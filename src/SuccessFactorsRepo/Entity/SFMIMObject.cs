using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    public class SFMIMObject
    {
        public DateTime? endDate { get; set; }

        public DateTime? originalStartDate { get; set; }

        public DateTime? endDateJob { get; set; }

        public DateTime? originalStartDateJob { get; set; }

        public DateTime? hireDate { get; set; }

        public DateTime? lastDateWorked { get; set; }

        public DateTime? customDate { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string businessPhone { get; set; }
        public string businessUnit { get; set; }

        public string BusinessUnitNoPath { get; set; }

        public string BusinessUnitEnPath { get; set; }

        public string BusinessCategory { get; set; }

        public string UnitCode { get; set; }
        public string businessUnitNoName { get; set; }
        public string businessUnitEnName { get; set; }
        public string businessUnitDefaultName { get; set; }
        public string company { get; set; }
        public string companyName { get; set; }
        public string country { get; set; }
        public string custom03 { get; set; }
        public string customString2 { get; set; }
        public string customString2EnName { get; set; }
        public string customString2DefaultName { get; set; }
        public string costCenter { get; set; }
        public string countryOfCompany { get; set; }
        public string jobCustomString2 { get; set; }
        public string jobCustomString27 { get; set; }
        public string department { get; set; }
        public string departmentNoName { get; set; }
        public string departmentEnName { get; set; }
        public string departmentDefaultName { get; set; }
        public string division { get; set; }
        public string divisionNoName { get; set; }
        public string divisionEnName { get; set; }
        public string divisionDefaultName { get; set; }
        public string emailAddress { get; set; }
        public string employeeSubGroup { get; set; }
        public string employeeType { get; set; }
        public string employeeNumber { get; set; }
        public string employementType { get; set; }
        public string employeeClassExternalCode { get; set; }
        public string employmentTypeExternalCode { get; set; }
        public string employeeClassEnName { get; set; }
        public string employmentTypeEnName { get; set; }
        public string employeeClassNoName { get; set; }
        public string employmentTypeNoName { get; set; }
        public string isContingentWorker { get; set; }
        public string ShowIntranetCode { get; set; }
        public string ShowIntranetNoName { get; set; }
        public string ShowIntranetEnName { get; set; }

        public string SystemAccessCode { get; set; }
        public string SystemAccessNoName { get; set; }
        public string SystemAccessEnName { get; set; }

        public string employeeTypeId { get; set; }
        public string employeeTypeText { get; set; }
        public string firstName { get; set; }
        public string homePhone { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string manager { get; set; }
        public string managerFullname { get; set; }
        public string mobile { get; set; }
        public string nationality { get; set; }
        public string office { get; set; }
        public string phone { get; set; }
        public string personCustomString1 { get; set; }
        public string personIdExternal { get; set; }
        public string status { get; set; }
        public string terminationAgreement { get; set; }
        public string titleNo { get; set; }
        public string titleEn { get; set; }
        public string titleDefault { get; set; }
        public string jobTitle { get; set; }
        public string userId { get; set; }
        public string username { get; set; }
        public string zipCode { get; set; }

        public string totalTeamSize { get; set; }

        public string Vendor { get; set; }
        public SFMIMObject()
        {

        }

        public SFMIMObject(SFEmpEmploymentResult s)
        {
            this.personIdExternal = s.personIdExternal;
            this.userId = s.userId;
            this.originalStartDate = s.originalStartDate;
            this.endDate = s.endDate;

        }

        public SFMIMObject(SFUserResult s)
        {
            this.userId = s.userId;
            this.username = s.username;
            this.firstName = s.firstName;
            this.lastName = s.lastName;
            this.hireDate = s.hireDate;
            this.status = s.status;
            this.location = StripPara(s.location);
            this.department = StripPara(s.department);
            this.titleNo = StripPara(s.title);
            this.emailAddress = s.email;
            this.division = StripPara(s.division);
            this.address1 = s.addressLine1;
            this.office = s.city;
            this.zipCode = s.zipCode;
            this.countryOfCompany = s.country;
            this.totalTeamSize = s.totalTeamSize;
            if (s.cellPhone != null)
            {
                this.mobile = s.cellPhone.Replace(" ", ""); ;
            }
            if (s.businessPhone != null)
            {
                this.businessPhone = s.businessPhone.Replace(" ", "");
            }
            s.custom01 = StripPara(s.custom01);
            s.custom02 = StripPara(s.custom02);
            s.custom03 = StripPara(s.custom03);
            s.custom04 = StripPara(s.custom04);
            s.custom05 = StripPara(s.custom05);
            s.custom06 = StripPara(s.custom06);
            s.custom07 = StripPara(s.custom07);
            s.custom08 = StripPara(s.custom08);
            s.custom09 = StripPara(s.custom09);
            s.custom10 = StripPara(s.custom10);

        }



        #region Helpers

        private string StripPara(string orgname)
        {
            if (orgname == null)
            {
                return orgname;
            }
            if (!orgname.Contains("("))
            {
                return orgname;
            }
            string[] spl = orgname.Split('(');
            string org = spl[0];

            return org.Trim();
        }

        #endregion
    }
}