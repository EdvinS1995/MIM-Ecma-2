using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class SFPerAddressDEFLTs
    {
        public SFPerAddressDEFLTD d { get; set; }
    }

    public class SFPerAddressDEFLTD
    {
        public SFPerAddressDEFLTResult[] results { get; set; }
    }

    public class SFPerAddressDEFLTResult
    {
        public string addressType { get; set; }
        public string personIdExternal { get; set; }
        public DateTime startDate { get; set; }
        public string country { get; set; }
        public string customString18 { get; set; }
        public string zipCode { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string notes { get; set; }
        public string address3 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public DateTime endDate { get; set; }
        public string address1 { get; set; }
        public string lastModifiedBy { get; set; }
        public string county { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime createdOn { get; set; }
        public string customString15 { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public string customString1 { get; set; }
        public string province { get; set; }
        public string createdBy { get; set; }
        public string state { get; set; }

    }



}
