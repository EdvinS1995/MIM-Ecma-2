using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Unit4Repo.Entity
{

    public class Unit4EmployeesEidsiva
    {
        public bool successful { get; set; }
        public Unit4Field[] fields { get; set; }
        public object[] notFoundProperties { get; set; }
        public int resultCount { get; set; }
        public Unit4ResultEidsiva[] result { get; set; }
    }

    public class Unit4EmployeesFlytoget
    {
        public bool successful { get; set; }
        public Unit4Field[] fields { get; set; }
        public object[] notFoundProperties { get; set; }
        public int resultCount { get; set; }
        public Unit4ResultFlytoget[] result { get; set; }
    }

    public class Unit4Field
    {
        public int index { get; set; }
        public string publicPath { get; set; }
        public bool isDescription { get; set; }
        public string propertyKey { get; set; }
        public string columnKey { get; set; }
    }

    public class Unit4ResultEidsiva
    {
        [JsonProperty("0")]
        public string _0 { get; set; }
        
        [JsonProperty("1")]
        public string _1 { get; set; }

        [JsonProperty("2")]
        public string _2 { get; set; }

        [JsonProperty("3")]
        public string _3 { get; set; }

        [JsonProperty("4")]
        public string _4 { get; set; }

        [JsonProperty("5")]
        public string _5 { get; set; }

        [JsonProperty("6")]
        public string _6 { get; set; }

        [JsonProperty("7")]
        public string _7 { get; set; }

        [JsonProperty("8")]
        public string _8 { get; set; }

        [JsonProperty("9")]
        public string _9 { get; set; }

        [JsonProperty("10")]
        public string _10 { get; set; }

        [JsonProperty("11")]
        public string _11 { get; set; }

        [JsonProperty("12")]
        public string _12 { get; set; }

        [JsonProperty("13")]
        public string _13 { get; set; }

        [JsonProperty("14")]
        public string _14 { get; set; }


        [JsonProperty("15")]
        public string _15 { get; set; }

        [JsonProperty("16")]
        public string _16 { get; set; }

        [JsonProperty("17")]
        public string _17 { get; set; }

        [JsonProperty("18")]
        public string _18 { get; set; }

        [JsonProperty("19")]
        public string _19 { get; set; }

        [JsonProperty("20")]
        public string _20 { get; set; }

        [JsonProperty("21")]
        public string _21 { get; set; }

        [JsonProperty("22")]
        public string _22 { get; set; }

        [JsonProperty("23")]
        public string _23 { get; set; }

        [JsonProperty("24")]
        public string _24 { get; set; }

        [JsonProperty("25")]
        public string _25 { get; set; }

        [JsonProperty("26")]
        public string _26 { get; set; }

        [JsonProperty("27")]
        public string _27 { get; set; }

        [JsonProperty("28")]
        public string _28 { get; set; }

        [JsonProperty("29")]
        public string _29 { get; set; }

        [JsonProperty("30")]
        public string _30 { get; set; }

        [JsonProperty("31")]
        public string _31 { get; set; }

        [JsonProperty("32")]
        public string _32 { get; set; }

        [JsonProperty("33")]
        public string _33 { get; set; }
    }

    public class Unit4ResultFlytoget
    {
        [JsonProperty("0")]
        public DateTime _0 { get; set; }

        [JsonProperty("1")]
        public DateTime _1 { get; set; }

        [JsonProperty("2")]
        public string _2 { get; set; }

        [JsonProperty("3")]
        public string _3 { get; set; }

        [JsonProperty("4")]
        public string _4 { get; set; }

        [JsonProperty("5")]
        public string _5 { get; set; }

        [JsonProperty("6")]
        public string _6 { get; set; }

        [JsonProperty("7")]
        public string _7 { get; set; }

        [JsonProperty("8")]
        public string _8 { get; set; }

        [JsonProperty("9")]
        public string _9 { get; set; }

        [JsonProperty("10")]
        public string _10 { get; set; }

        [JsonProperty("11")]
        public string _11 { get; set; }

        [JsonProperty("12")]
        public string _12 { get; set; }

        [JsonProperty("13")]
        public string _13 { get; set; }

        [JsonProperty("14")]
        public string _14 { get; set; }


        [JsonProperty("15")]
        public string _15 { get; set; }

        [JsonProperty("16")]
        public string _16 { get; set; }

        [JsonProperty("17")]
        public string _17 { get; set; }

        [JsonProperty("18")]
        public string _18 { get; set; }

        [JsonProperty("19")]
        public string _19 { get; set; }

        [JsonProperty("20")]
        public string _20 { get; set; }

        [JsonProperty("21")]
        public string _21 { get; set; }

        [JsonProperty("22")]
        public string _22 { get; set; }

      
    }



}
