using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.NytData
{
    public class SourceConfig
    {
        public const string BaseUrlString = "https://raw.githubusercontent.com/nytimes/covid-19-data/master/";

        public const string LiveString = "live/";
        public const string UsString = "us-counties.csv";
        public const string StatesString = "us-states.csv";
        public const string CountiesString = "us-counties.csv";
    }
}