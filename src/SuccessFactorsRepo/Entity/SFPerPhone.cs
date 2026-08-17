using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class SFPerPhones
    {
        public SFPerPhoneD d { get; set; }
    }

    public class SFPerPhoneD
    {
        public SFPerPhoneResult[] results { get; set; }
        public string __next { get; set; }
    }

    public class SFPerPhoneResult
    {
        public string phoneType { get; set; }
        public string personIdExternal { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string phoneNumber { get; set; }
        public string countryCode { get; set; }
        public string createdBy { get; set; }
        public string extension { get; set; }
        public bool isPrimary { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime lastModifiedOn { get; set; }

    }



}
