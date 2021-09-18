using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.NytData.Models
{
    public class CountyPop
    {
        public string Fips { get; set; }
        public string State { get; set; }
        public string CountyName { get; set; }
        public string PopulationString { get; set; }
        public int Population { get
            {
                return PopStringToInt(PopulationString);
            } }

        public static int PopStringToInt(string popString)
        {
            popString = popString.Replace(",", "");
            var pop = Int32.TryParse(popString, out int p) ? p : 0;
            return pop;
        }
    }

    

}
