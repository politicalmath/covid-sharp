using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CovidTrack.Models
{
    public class StateMetadata
    {

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("covid19Site")]
        public string Covid19Site { get; set; }

        [JsonProperty("covid19SiteSecondary")]
        public string Covid19SiteSecondary { get; set; }

        [JsonProperty("covid19SiteTertiary")]
        public string Covid19SiteTertiary { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("covid19SiteOld")]
        public string Covid19SiteOld { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fips")]
        public string Fips { get; set; }

        [JsonProperty("pui")]
        public string Pui { get; set; }

        [JsonProperty("pum")]
        public bool Pum { get; set; }
    }
}
