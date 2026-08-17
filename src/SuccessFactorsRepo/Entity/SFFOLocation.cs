using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    class SFFOLocation
    {
    }


    public class SFFOLocations
    {
        public SFFOLocationD d { get; set; }
    }

    public class SFFOLocationD
    {
        public SFFOLocationResult[] results { get; set; }
    }

    public class SFFOLocationResult
    {
        public string externalCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public DateTime endDate { get; set; }
        public object standardHours { get; set; }
        public string timezone { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime createdDateTime { get; set; }
        public object description { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public string createdBy { get; set; }
        public string name { get; set; }
        public object geozoneFlx { get; set; }
        public object locationGroup { get; set; }
        public string status { get; set; }

    }





}
