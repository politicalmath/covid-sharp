using CovidSharp.Constants;
using CovidSharp.Models;
using CovidSharp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.JohnsHopkins.Model
{
    public class JhState
    {
        public StateBase State { get; set; }
        public string StateString { get; set; }
        public List<JhCovidDay> CovidData { get; set; }

        public JhState(string stateName) {
            State = StateMapHelper.StringToStateMap(stateName);
            StateString = stateName;
            CovidData = new List<JhCovidDay>();
        }
    }

    public class JhCovidDay
    {
        public JhCovidDay() { }

        public JhCovidDay(RawJhState rawData, string filePath)
        {
            int month = Int32.TryParse(filePath.Substring(0, 2), out int m) ? m : 1;
            int day = Int32.TryParse(filePath.Substring(3, 2), out int d) ? d : 1;
            int year = Int32.TryParse(filePath.Substring(6, 4), out int y) ? y : 1;
            Date = new DateTime(year, month, day);
            Positives = rawData.Confirmed;
            Deaths = rawData.Deaths;
            Recovered = rawData.Recovered;
            Active = rawData.Active;
            Tests = rawData.People_Tested;
        }

        public DateTime Date { get; set; }
        public double Positives { get; set; }
        public double Deaths { get; set; }
        public double Recovered { get; set; }
        public double Active { get; set; }
        public double Tests { get; set; }
    }

    public class RawJhState
    {
        public string Province_State { get; set; }
        public string Country_Region { get; set; }
        public string Last_Update { get; set; }
        public double Lat { get; set; }
        public double Long_ { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public int Active { get; set; }
        public int FIPS { get; set; }
        public double Incident_Rate { get; set; }
        public int People_Tested { get; set; }
        public int People_Hospitalized { get; set; }
        public double Mortality_Rate { get; set; }
        public int UID { get; set; }
        public string ISO3 { get; set; }
        public double Testing_Rate { get; set; }
        public double Hospitalization_Rate { get; set; }
    }

}
