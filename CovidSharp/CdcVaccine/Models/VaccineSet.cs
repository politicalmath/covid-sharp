using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CdcVaccine.Models
{
    public class VaccineSet
    {
        [JsonProperty("runid")]
        public int RunID { get; set; }

        [JsonProperty("vaccination_data")]
        public List<VaccineStateDay> VaccineDay {get; set;}

        public DateTime VaccineSetDate { get; set; }
    }
}






