using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class SFPerPersons
    {
        public SFPerPersonD d { get; set; }
    }

    public class SFPerPersonD
    {
        public SFPerPersonResult[] results { get; set; }
        public string __next { get; set; }
    }

    public class SFPerPersonResult
    {
        public string personIdExternal { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string perPersonUuid { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public string createdBy { get; set; }
        public string personId { get; set; }
        public string customString1 { get; set; }
    }
}
