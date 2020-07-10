using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.NytData.Models
{
    public class NytBaseDay
    {
        public string DateString { get; set; }
        public int Cases { get; set; }
        public int Deaths { get; set; }
    }

    public class NytStateDay : NytBaseDay
    {
        public string State { get; set; }
        public string Fips { get; set; }
    }

    public class NytCountyDay : NytStateDay
    {
        public string CountyName { get; set; }
    }

}
