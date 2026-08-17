using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit4Repo.Entity
{
    public class MIMObject
    {
        public string personIdHRM { get; set; }
        public string givenName { get; set; }
        public string familyName { get; set; }

        public string companyName { get; set; }

        public string nationalityCode { get; set; }

        public string employeeId { get; set; }
        public string mobilePhone { get; set; }
        public string email { get; set; }
        public string workPhone { get; set; }
        public string employeeType { get; set; }

        public string manager { get; set; }
        public string positionCode { get; set; }
        public string positionType { get; set; }

        public string workClassification { get; set; }
        public string businessNumber { get; set; }
        public string servicePlace { get; set; }

        public string positionStartDate { get; set; }

        public string positionEndDate { get; set; }

    }
}
