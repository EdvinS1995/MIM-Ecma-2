using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    public class SFFOGroups
    {
        public SFFOGroup d { get; set; }
    }

    public class SFFOGroup
    {
        public SFFOGroupResult[] results { get; set; }
        public string __next { get; set; }
    }

    public class SFFOGroupResult
    {
        public string externalCode { get; set; }
        public DateTime? effectiveStartDate { get; set; }
        // cust_Group uses externalName_* not name_*
        public string externalName_defaultValue { get; set; }
        public string externalName_en_GB { get; set; }
        public string externalName_nb_NO { get; set; }

        // Convenience aliases so the rest of the code can use name_* like other FO entities
        public string name_defaultValue => externalName_defaultValue;
        public string name_en_GB => externalName_en_GB;
        public string name_nb_NO => externalName_nb_NO;
    }
}