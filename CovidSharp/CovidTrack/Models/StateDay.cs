using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CovidTrack.Models
{
    public class StateDay : CovidDay
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("dataQualityGrade")]
        public string DataQualityGrade { get; set; }

        [JsonProperty("lastUpdateEt")]
        public string LastUpdateEt { get; set; }

        [JsonProperty("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonProperty("checkTimeEt")]
        public string CheckTimeEt { get; set; }

        [JsonProperty("fips")]
        public string Fips { get; set; }

        [JsonProperty("posNeg")]
        public int? posNeg { get; set; }

        [JsonProperty("commercialScore")]
        public int? CommercialScore { get; set; }

        [JsonProperty("negativeRegularScore")]
        public int? NegativeRegularScore { get; set; }

        [JsonProperty("negativeScore")]
        public int? NegativeScore { get; set; }

        [JsonProperty("positiveScore")]
        public int? PositiveScore { get; set; }

        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("grade")]
        public string Grade { get; set; }
    }
}
