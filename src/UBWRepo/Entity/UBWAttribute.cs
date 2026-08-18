using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBWRepo.Entity
{
    public class UBWAttribute
    {
        public string attributeId { get; set; }
        public string attributeName { get; set; }
        public string attributeValue { get; set; }
        public string companyId { get; set; }
        public float customValue { get; set; }
        public string description { get; set; }
        public string owner { get; set; }
        public string ownerAttributeId { get; set; }
        public string ownerAttributeName { get; set; }
        public int periodFrom { get; set; }
        public int periodTo { get; set; }
        public string status { get; set; }
        public Lastupdated lastUpdated { get; set; }
    }


}