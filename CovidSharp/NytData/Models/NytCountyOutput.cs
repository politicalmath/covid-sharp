using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.NytData.Models
{
    public class NytFileOutput
    {
        public string DateString { get; set; }
        public List<NytProcessedCounty> CountyData { get; set; }
        public bool OutputCases { get; set; }
        public bool OutputDeaths { get; set; }

        public NytFileOutput(bool cases, bool deaths)
        {
            CountyData = new List<NytProcessedCounty>();
            OutputCases = cases;
            OutputDeaths = deaths;
        }
    }

    public class NytProcessedCounty
    {
        public string DateString { get; set; }
        public string Fips { get; set; }
        public double Cases { get; set; }
        public double Deaths { get; set; }
        public string CasesString { get; set; }
        public string DeathsString { get; set; }
    }
}
