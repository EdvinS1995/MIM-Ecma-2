using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooRepo.Entity
{

    public class BamReportPost
    {
        public string title { get; set; }
        public BamFilters filters { get; set; }
        public string[] fields { get; set; }
    }

    public class BamFilters
    {
        public BamLastchanged lastChanged { get; set; }
    }

    public class BamLastchanged
    {
        public string includeNull { get; set; }
        public DateTime value { get; set; }
    }

}
