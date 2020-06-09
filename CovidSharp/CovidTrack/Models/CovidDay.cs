using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CovidTrack.Models
{
    public class CovidDay
    {
        [JsonProperty("date")]
        public int DateInt { get; set; }
        public DateTime Date { get {
                int d = DateInt % 100;
                int m = (DateInt / 100) % 100;
                int y = DateInt / 10000;

                return new DateTime(y, m, d); } }

        [JsonProperty("positive")]
        public int? Positive { get; set; }

        [JsonProperty("negative")]
        public int? Negative { get; set; }

        [JsonProperty("pending")]
        public int? Pending { get; set; }

        [JsonProperty("hospitalizedCurrently")]
        public int? HospitalizedCurrently { get; set; }

        [JsonProperty("hospitalizedCumulative")]
        public int? HospitalizedCumulative { get; set; }

        [JsonProperty("inIcuCurrently")]
        public int? InIcuCurrently { get; set; }

        [JsonProperty("inIcuCumulative")]
        public int? InIcuCumulative { get; set; }

        [JsonProperty("onVentilatorCurrently")]
        public int? OnVentilatorCurrently { get; set; }

        [JsonProperty("onVentilatorCumulative")]
        public int? OnVentilatorCumulative { get; set; }

        [JsonProperty("recovered")]
        public int? Recovered { get; set; }

        [JsonProperty("dateChecked")]
        public DateTime DateChecked { get; set; }

        [JsonProperty("death")]
        public int? Death { get; set; }

        [JsonProperty("hospitalized")]
        public int? Hospitalized { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("totalTestResults")]
        public int TotalTestResults { get; set; }

        [JsonProperty("posNeg")]
        public int PosNeg { get; set; }

        [JsonProperty("deathIncrease")]
        public int DeathIncrease { get; set; }

        [JsonProperty("hospitalizedIncrease")]
        public int HospitalizedIncrease { get; set; }

        [JsonProperty("negativeIncrease")]
        public int NegativeIncrease { get; set; }

        [JsonProperty("positiveIncrease")]
        public int PositiveIncrease { get; set; }

        [JsonProperty("totalTestResultsIncrease")]
        public int TotalTestResultsIncrease { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

    }
}
