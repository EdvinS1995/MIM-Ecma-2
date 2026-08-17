using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class SFPerPersonals
    {
        public SFPerPersonalD d { get; set; }
    }

    public class SFPerPersonalD
    {
        public SFPerPersonalResult[] results { get; set; }
        public string __next { get; set; }
    }

  

    public class SFPerPersonalResult
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string personIdExternal { get; set; }
        public DateTime startDate { get; set; }
        public string customString20 { get; set; }
        public string customString23 { get; set; }
        public string customString8 { get; set; }
        public Customstring23nav customString23Nav { get; set; }
        public Customstring20nav customString20Nav { get; set; }
    }

    public class Customstring23nav
    {
        public string externalCode { get; set; }
        public Picklistlabels picklistLabels { get; set; }
    }

    public class Customstring20nav
    {
        public string externalCode { get; set; }
        public Picklistlabels picklistLabels { get; set; }
    }


  
}
