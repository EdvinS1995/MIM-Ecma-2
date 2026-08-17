using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    public class SFEmpJobs
    {
        public SFEmpJob d { get; set; }
    }

    public class SFEmpJob
    {
        public SFEmpJobResult[] results { get; set; }
        public string __next { get; set; }
    }

    public class SFEmpJobResultX
    {
        public DateTime createdDateTime { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime endDate { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? companyEntryDate { get; set; }
        public DateTime? contractEndDate { get; set; }
        public DateTime? departmentEntryDate { get; set; }
        public bool isFulltimeEmployee { get; set; }
        public object customString10 { get; set; }
        public object expectedReturnDate { get; set; }
        public string _event { get; set; }
        public string businessUnit { get; set; }
        public string company { get; set; }
        public string contractType { get; set; }
        public string costCenter { get; set; }
        public string countryOfCompany { get; set; }
        public string createdBy { get; set; }
        public string customDouble7 { get; set; }
        public string customDouble8 { get; set; }
        public string customDouble9 { get; set; }
        public string customString1 { get; set; }
        public string customString2 { get; set; }
        public string customString7 { get; set; }
        public string customString14 { get; set; }
        public string customString15 { get; set; }
        public string customString20 { get; set; }
        public string customString22 { get; set; }
        public string customString28 { get; set; }
        public string customString27 { get; set; }
        public string customString29 { get; set; }
        public string customString32 { get; set; }
        public string customString4 { get; set; }
        public string customString48 { get; set; }
        public string customString49 { get; set; }
        public string customString5 { get; set; }
        public string customString50 { get; set; }
        public string customString51 { get; set; }
        public string customString52 { get; set; }
        public string customString53 { get; set; }
        public string customString8 { get; set; }
        public string department { get; set; }
        public string division { get; set; }
        public string emplStatus { get; set; }
        public string eventReason { get; set; }
        public string fte { get; set; }
        public string jobCode { get; set; }
        public string jobTitle { get; set; }
        public string lastModifiedBy { get; set; }
        public string location { get; set; }
        public string managerId { get; set; }
        public string payScaleArea { get; set; }
        public string payScaleGroup { get; set; }
        public string payScaleType { get; set; }
        public string position { get; set; }
        public string seqNumber { get; set; }
        public string timezone { get; set; }
        public string userId { get; set; }
        public string workscheduleCode { get; set; }
    }

    public class SFEmpJobResult
    {
        public string userId { get; set; }
        public DateTime startDate { get; set; }
        public string businessUnit { get; set; }
        public DateTime endDate { get; set; }
        public DateTime? customDate1 { get; set; }
        public DateTime? contractEndDate { get; set; }
        public string jobTitle { get; set; }
        public string customString2 { get; set; }
        public string managerId { get; set; }
        public string customString7 { get; set; }
        public string customString27 { get; set; }
        public string customString48 { get; set; }
        public string customString49 { get; set; }
        public string countryOfCompany { get; set; }
        public string costCenter { get; set; }
        public string division { get; set; }
        public string company { get; set; }
        public string location { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        // customString2Nav — used to get externalCode and effectiveStartDate
        // for individual cust_Group lookup to always get the current name
        public Customstring2nav customString2Nav { get; set; }
        public CostcenterNav costCenterNav { get; set; }
    }

    // cust_Group nav — we use externalCode + effectiveStartDate as keys
    // to look up the current name directly from cust_Group API
    public class Customstring2nav
    {
        public string externalCode { get; set; }
        public DateTime? effectiveStartDate { get; set; }
        public string externalName_nb_NO { get; set; }
        public string externalName_en_GB { get; set; }
        public string externalName_defaultValue { get; set; }
    }

    public class CostcenterNav
    {
        public string name { get; set; }
    }
}