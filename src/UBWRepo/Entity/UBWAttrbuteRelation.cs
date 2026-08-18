using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBWRepo.Entity
{
    public class UBWAttrbuteRelation
    {
        public string attributeId { get; set; }
        public string attributeName { get; set; }
        public string attributeValue { get; set; }
        public string attributeValueFrom { get; set; }
        public string attributeValueTo { get; set; }
        public string companyId { get; set; }
        public int percent { get; set; }
        public string relatedAttributeId { get; set; }
        public string relatedAttributeName { get; set; }
        public DateTime relationDateFrom { get; set; }
        public DateTime relationDateTo { get; set; }
        public string relationValue { get; set; }
        public string relationValueStatus { get; set; }
    }
}