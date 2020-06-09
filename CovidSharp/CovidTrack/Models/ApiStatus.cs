using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CovidTrack.Models
{
    public class ApiStatus
    {

        [JsonProperty("buildTime")]
        public DateTime BuildTime { get; set; }

        [JsonProperty("production")]
        public bool Production { get; set; }

        [JsonProperty("runNumber")]
        public string RunNumber { get; set; }
     
    }
}
