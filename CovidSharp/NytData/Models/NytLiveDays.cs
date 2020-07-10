using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.NytData.Models
{
    public class NytBaseLiveDay
    {
        public string Date { get; set; }
        public int Cases { get; set; }
        public int Deaths { get; set; }
        public int ConfirmedCases { get; set; }
        public int ConfirmedDeaths { get; set; }
        public int ProbableCases { get; set; }
        public int ProbableDeaths { get; set; }
    }

    public class NytStateLiveDay : NytBaseLiveDay
    {
        public string State { get; set; }
        public string Fips { get; set; }
    }

    public class NytCountyLiveDay : NytStateLiveDay
    {
        public string CountyName { get; set; }
    }
}
