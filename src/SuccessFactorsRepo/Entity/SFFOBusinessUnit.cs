using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    public class SFFOBusinessUnits
    {
        public SFFOBusinessUnitD d { get; set; }
    }

    public class SFFOBusinessUnitD
    {
        public SFFOBusinessUnitResult[] results { get; set; }
    }

    public class SFFOBusinessUnitResult
    {
        public string externalCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string name_localized { get; set; }
        public DateTime endDate { get; set; }
        public string entityUUID { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime createdOn { get; set; }
        public string headOfUnit { get; set; }
        public string name_en_US { get; set; }
        public string name_defaultValue { get; set; }
        public string lastModifiedBy { get; set; }
        public string name_en_GB { get; set; }
        public string name_nb_NO { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public string createdBy { get; set; }
        public string name { get; set; }
        public string status { get; set; }

    }



}

