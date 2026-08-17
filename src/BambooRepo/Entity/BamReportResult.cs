using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooRepo.Entity
{

    public class BamReportResult
    {
        public string title { get; set; }
        public BamField[] fields { get; set; }
        public BamEmployee[] employees { get; set; }
    }

    public class BamField
    {
        public object id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class BamEmployee
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string employeeNumber { get; set; }
        public string department { get; set; }
        public string employmentHistoryStatus { get; set; }
        public string hireDate { get; set; }
        public string _91 { get; set; }
        public string jobTitle { get; set; }
    }

    public class BamWEmployee
    {
        public string workPhone { get; set; }

        public string employeeNumber { get; set; }
    }

}
