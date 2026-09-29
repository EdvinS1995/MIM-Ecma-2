using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBWRepo.Entity
{


    public class UBWRelation
    {
        public string attributeId { get; set; }
        public string attributeName { get; set; }
        public bool canHaveDuplicate { get; set; }
        public string companyId { get; set; }
        public bool enableDate { get; set; }
        public bool enabledFixedPd { get; set; }
        public bool enabledGlAnalysis { get; set; }
        public bool enabledRate { get; set; }
        public bool enabledReporting { get; set; }
        public bool enabledResource { get; set; }
        public bool fixedFixedPd { get; set; }
        public bool fixedGlAnalysis { get; set; }
        public bool fixedRate { get; set; }
        public bool fixedReporting { get; set; }
        public string flag { get; set; }
        public bool locked { get; set; }
        public string module { get; set; }
        public bool noDateCheck { get; set; }
        public bool percentwiseDistributionAllowed { get; set; }
        public string periodType { get; set; }
        public bool position { get; set; }
        public string relatedAttributeId { get; set; }
        public string relatedAttributeName { get; set; }
        public string relationGroup { get; set; }
        public string required { get; set; }
        public int sortOrder { get; set; }
        public string typeOfMaintenance { get; set; }
        public string typeOfUnit { get; set; }
        public DateTime updatedAt { get; set; }
        public string updatedBy { get; set; }
        public bool visibleOfficeHs { get; set; }
    }
}