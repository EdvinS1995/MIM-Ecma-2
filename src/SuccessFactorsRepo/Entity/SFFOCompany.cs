using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class SFFOCompanys
    {
        public SFFOCompanyD d { get; set; }
    }

    public class SFFOCompanyD
    {
        public SFFOCompanyResult[] results { get; set; }
    }

    public class SFFOCompanyResult
    {
        
        public string externalCode { get; set; }
        public DateTime startDate { get; set; }
        public string country { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string name_localized { get; set; }
        public DateTime endDate { get; set; }
        public DateTime createdDateTime { get; set; }
        public string standardHours { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public DateTime createdOn { get; set; }
        public string name_en_US { get; set; }
        public string currency { get; set; }
        public string name_defaultValue { get; set; }
        public string lastModifiedBy { get; set; }
        public string defaultPayGroup { get; set; }
        public string name_en_GB { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public string createdBy { get; set; }

    }





}
