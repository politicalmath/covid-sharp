using CovidSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CovidTrack.Models
{
    public class CalculatedValue
    {
        public string ValueName { get; set; }
        public DateTime ValueDate { get; set; }
        public double Value { get; set; }
    }

    public class CalculatedStateValue : CalculatedValue
    {
        public StateBase StateBase { get; set; } 
    }
}
