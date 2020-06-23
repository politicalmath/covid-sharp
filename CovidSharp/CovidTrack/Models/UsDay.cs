using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CovidTrack.Models
{
    public class UsDay : CovidDay
    {

        [JsonProperty("states")]
        public int States { get; set; }
                
    }
}
