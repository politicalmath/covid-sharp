using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.owiData.Models
{

    public class OwidCountry
    {
        public string CountryCode { get; set; }

        [JsonProperty("continent")]
        public string Continent { get; set; }

        [JsonProperty("location")]
        public string CountryName { get; set; }

        [JsonProperty("population")]
        public double Population { get; set; }

        [JsonProperty("population_density")]
        public double PopulationDensity { get; set; }

        [JsonProperty("median_age")]
        public double MedianAge { get; set; }

        [JsonProperty("aged_65_older")]
        public double Over65Percent { get; set; }

        [JsonProperty("aged_70_older")]
        public double Over70Percent { get; set; }

        [JsonProperty("gdp_per_capita")]
        public double GdpPerCapita { get; set; }

        [JsonProperty("extreme_poverty")]
        public double ExtremePovertyRate { get; set; }

        [JsonProperty("cardiovasc_death_rate")]
        public double CardioDeathRate { get; set; }

        [JsonProperty("diabetes_prevalence")]
        public double DiabetesPrevalence { get; set; }

        [JsonProperty("female_smokers")]
        public double FemaleSmokers { get; set; }

        [JsonProperty("male_smokers")]
        public double MaleSmokersPercent { get; set; }

        [JsonProperty("hospital_beds_per_thousand")]
        public double HospitalBedsPer1K { get; set; }

        [JsonProperty("life_expectancy")]
        public double LifeExpectancy { get; set; }

        [JsonProperty("human_development_index")]
        public double HumanDevelopmentIndext { get; set; }

        [JsonProperty("data")]
        public List<OwidDay> Data { get; set; }
    }

    public class OwidDay
    {
        [JsonProperty("date")]
        public string DateString { get; set; }
        //"date": "2019-12-31",
        public DateTime Date { get
            {
                int year = Convert.ToInt32(DateString.Substring(0, 4));
                int month = Convert.ToInt32(DateString.Substring(5, 2));
                int day = Convert.ToInt32(DateString.Substring(8, 2));

                return new DateTime(year, month, day); } }

        [JsonProperty("total_cases")]
        public double TotalCases { get; set; }

        [JsonProperty("new_cases")]
        public double NewCases { get; set; }

        [JsonProperty("new_cases_smoothed")]
        public double NewCasesSmoothed { get; set; }

        [JsonProperty("total_deaths")]
        public double TotalDeaths { get; set; }

        [JsonProperty("new_deaths")]
        public double NewDeaths { get; set; }

        [JsonProperty("new_deaths_smoothed")]
        public double NewDeathsSmoothed { get; set; }

        [JsonProperty("total_cases_per_million")]
        public double TotalCasesPerMillion { get; set; }

        [JsonProperty("new_cases_per_million")]
        public double NewCasesPerMillion { get; set; }

        [JsonProperty("new_cases_smoothed_per_million")]
        public double NewCasesSmoothedPerMillion { get; set; }

        [JsonProperty("total_deaths_per_million")]
        public double TotalDeathsPerMillion { get; set; }

        [JsonProperty("new_deaths_per_million")]
        public double NewDeathsPerMillion { get; set; }

        [JsonProperty("new_deaths_smoothed_per_million")]
        public double NewDeathsSmoothedPerMillion { get; set; }

        [JsonProperty("new_tests")]
        public double NewTests { get; set; }

        [JsonProperty("total_tests")]
        public double TotalTests { get; set; }

        [JsonProperty("total_tests_per_thousand")]
        public double TotalTestsPerThousand { get; set; }

        [JsonProperty("new_tests_per_thousand")]
        public double NewTestsPerThousand { get; set; }

        [JsonProperty("new_tests_smoothed")]
        public double NewTestsSmoothed { get; set; }

        [JsonProperty("new_tests_smoothed_per_thousand")]
        public double NewTestsSmoothedPerThousand { get; set; }

        [JsonProperty("tests_per_case")]
        public double TestsPerCase { get; set; }

        [JsonProperty("positive_rate")]
        public double PositiveRate { get; set; }

        [JsonProperty("tests_units")]
        public string TestsUnits { get; set; }

        [JsonProperty("stringency_index")]
        public double StringencyIndex { get; set; }
    }
}
 
