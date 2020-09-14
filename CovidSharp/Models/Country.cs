using CovidSharp.owiData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.Models
{
    public class Country
    {
        public string CountryCode;
        public string CountryName;
        public int Population;

        public List<OwidDay> CovidData { get; set; }


    }
}
