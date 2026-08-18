using Newtonsoft.Json;
using System;

namespace C1V3Repo.Entity
{
    public class RootObject
    {
        public Employee[] employees { get; set; }
    }

    public class Employee
    {
        public string guid { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string employeeId { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? lastModified { get; set; }
        public string hasPlannedChange { get; set; }
        public Field field { get; set; }
    }



    public class Field
    {
        [JsonProperty("24")]
        public CEmpBase _24 { get; set; }

        [JsonProperty("1")]
        public CEmpBase _1 { get; set; }

        [JsonProperty("8")]
        public CEmpBase _8 { get; set; }

        [JsonProperty("2")]
        public CEmpBase _2 { get; set; }

        [JsonProperty("1102")]
        public CEmpBase _1102 { get; set; }

        [JsonProperty("15")]
        public CEmpBase _15 { get; set; }

        [JsonProperty("7")]
        public CEmpBase _7 { get; set; }

        [JsonProperty("3")]
        public CEmpBase _3 { get; set; }

        [JsonProperty("14")]
        public CEmpBase _14 { get; set; }

        [JsonProperty("9")]
        public CEmpBase _9 { get; set; }

        [JsonProperty("0")]
        public CEmpBase _0 { get; set; }

        [JsonProperty("1094")]
        public CEmpBase _1094 { get; set; }
    }

    public class CEmpBase
    {
        public string name { get; set; }
        public DateTime? lastModified { get; set; }
        public string dataValidFrom { get; set; }
        public string type { get; set; }
        public string typeId { get; set; }
        public CData data { get; set; }
        public string visible { get; set; }
    }

    public class CData
    {
        public string guid { get; set; }
        public string value { get; set; }
        public string alternativeExportValue { get; set; }
        public string username { get; set; }
        public string employeeId { get; set; }
        public string uniqueImportId { get; set; }
    }







}
