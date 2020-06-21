using CovidSharp.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CovidSharp.Models
{
    public class CountyBase
    {
        public int StateInt { get; set; }
        public int CountyInt { get; set; }        
        public string StateString { get; set; }
        public string CountyName { get; set; }
        public int Population { get; set; }
    }

    public class County
    {
        public CountyBase CountyDetails { get; set; }
        public string Fips { get; set; }
        public StateName StateName { get; set; }
        public StateCode StateCode { get; set; }
        public County(CountyBase cBase, List<StateBase> states)
        {
            CountyDetails = cBase;
            var state = states.FirstOrDefault(s => s.Name.ToString().ToLower() == CountyDetails.StateString.ToLower());
            if(state != null)
            {
                StateName = state.Name;
                StateCode = state.Code;
            }

            Fips = cBase.StateInt.ToString("D2") + cBase.CountyInt.ToString("D3");

        }

    }
}
