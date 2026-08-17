using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit4Repo.Entity
{

    public class Unit4Columns
    {
        public Unit4Class[] Property1 { get; set; }
    }

    public class Unit4Class
    {
        public string objectId { get; set; }
        public string publicPath { get; set; }
        public bool isDescription { get; set; }
        public string propertyName { get; set; }
        public string dataType { get; set; }
        public string valueList { get; set; }
        public bool isSummable { get; set; }
        public string text { get; set; }
        public string columnKey { get; set; }
        public string propertyKey { get; set; }
        public int order { get; set; }
        public bool isHidden { get; set; }
        public string attributeId { get; set; }
        //public Unit4Linkinfo linkInfo { get; set; }
    }

    public class Unit4Linkinfo
    {
        public string accessId { get; set; }
        public string viewId { get; set; }
        public string clientColumnKey { get; set; }
        public string keyColumn { get; set; }
        public string title { get; set; }
        public string helpId { get; set; }
        public string sequenceNoColumnKey { get; set; }
        public bool disabled { get; set; }
        public string attributeId { get; set; }
    }


}
