using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    class SFFODivision
    {
    }


    public class SFFODivisions
    {
        public SFFODivisionD d { get; set; }
    }

    public class SFFODivisionD
    {
        public SFFODivisionResult[] results { get; set; }
    }

    public class SFFODivisionResult
    {
        public string externalCode { get; set; }
        public DateTime startDate { get; set; }
        public object parent { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string name_localized { get; set; }
        public DateTime endDate { get; set; }
        public string entityUUID { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime createdOn { get; set; }
        public object headOfUnit { get; set; }
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
