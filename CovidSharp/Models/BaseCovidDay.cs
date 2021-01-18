using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.Models
{
    public class BaseCovidDay
    {
        public DateTime Date { get; set; }
        public int Positive { get; set; }
        public int Deaths { get; set; }
        public int Tests { get; set; }
        public double PercentPositive => Convert.ToDouble(Positive / Tests);
        public int DosesDistributed { get; set; }
        public int DosesAdministered { get; set; }
        public int DosesDistributedPer100K { get; set; }
        public int DosesAdministeredPer100K { get; set; }
    }    
}
