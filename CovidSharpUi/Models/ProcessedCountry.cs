using CovidSharp.owiData.Models;
using CovidSharpUi.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidSharpUi.Models
{
    public class ProcessedCountry
    {
        public Dictionary<string, List<CalculatedValue>> OutputFiles { get; set; }
        public OwidCountry CoreCountryData { get; set; }

        public ProcessedCountry(OwidCountry coreCountry)
        {
            CoreCountryData = coreCountry;
            OutputFiles = new Dictionary<string, List<CalculatedValue>>();
        }

        public void ProcessCumulativeData(List<Metrics> metrics, bool isPerCapita = true)
        {
            foreach (Metrics metric in metrics)
            {
                // 1) Create a new List<CalculatedValue>
                List<CalculatedValue> processedValues = new List<CalculatedValue>();
                // 2) Look at the metric, pick out the appropriate data for that metric (and give it a name)
                List<CalculatedValue> allValues = new List<CalculatedValue>();
                string namedMetric = "world-cumulative-";

                switch (metric)
                {
                    case Metrics.Deaths:

                        namedMetric += "deaths";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            allValues.Add(new CalculatedValue(namedMetric, Convert.ToDouble(od.TotalDeaths), od.Date));
                        }
                        break;
                    case Metrics.Cases:

                        namedMetric += "positives";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            allValues.Add(new CalculatedValue(namedMetric, Convert.ToDouble(od.TotalCases), od.Date));
                        }
                        break;
                    case Metrics.Tests:
                        namedMetric += "tests";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            double tests = Convert.ToDouble(od.TotalTests);
                            allValues.Add(new CalculatedValue(namedMetric, tests, od.Date));
                        }
                        break;
                    case Metrics.PercentPositive:
                        namedMetric += "percentPositive";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            double percentPositive = Convert.ToDouble(od.TotalCases) / Convert.ToDouble(od.TotalTests);
                            allValues.Add(new CalculatedValue(namedMetric, percentPositive, od.Date));
                        }
                        break;
                    default:
                        break;
                }

                // 3) do the per capita calculation

                if (isPerCapita && metric != Metrics.PercentPositive)
                {
                    foreach (CalculatedValue cv in allValues)
                    {
                        processedValues.Add(new CalculatedValue(namedMetric, ((cv.Value / CoreCountryData.Population) * 100000), cv.Date));
                    }

                    namedMetric += "-per100K";
                }
                else
                {
                    processedValues = allValues;
                }

                OutputFiles.Add(namedMetric, processedValues);
            }
        }

        public void ProcessDailyData(List<Metrics> metrics, bool isRollingAverage = false, int rollingAverage = 7, bool isPerCapita = true)
        {
            //List<StateDay> sortedDays = CoreStateData.CovidData.OrderByDescending(d => d.DateInt).ToList();

            // 2) Look at the metric, pick out the appropriate data for that metric (and give it a name)
            foreach (Metrics metric in metrics)
            {
                List<CalculatedValue> processedValues = new List<CalculatedValue>();
                List<CalculatedValue> preProcess = new List<CalculatedValue>();
                string namedMetric = "world-dailychange-";

                switch (metric)
                {
                    case Metrics.Deaths:
                        namedMetric += "deaths";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            preProcess.Add(new CalculatedValue(namedMetric, Convert.ToDouble(od.TotalDeaths), od.Date));
                        }
                        break;
                    case Metrics.Cases:
                        namedMetric += "positives";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            preProcess.Add(new CalculatedValue(namedMetric, Convert.ToDouble(od.TotalCases), od.Date));
                            if(CoreCountryData.CountryName == "Germany")
                            {
                                Debug.WriteLine("Germany - " + od.DateString + " - positives - " + od.TotalCases + ", " + od.NewCases);
                            }
                        }
                        break;
                    case Metrics.Tests:
                        namedMetric += "tests";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            double tests = Convert.ToDouble(od.TotalCases) + Convert.ToDouble(od.TotalTests);
                            preProcess.Add(new CalculatedValue(namedMetric, tests, od.Date));
                        }
                        break;
                    case Metrics.PercentPositive:
                        namedMetric += "percentPositive";
                        foreach (OwidDay od in CoreCountryData.Data)
                        {
                            double percentPositive = Convert.ToDouble(od.TotalCases) / Convert.ToDouble(od.TotalTests);
                            preProcess.Add(new CalculatedValue(namedMetric, percentPositive, od.Date));
                        }
                        break;                   
                    case Metrics.CaseJerk:
                        namedMetric += "caseJerk";

                        break;
                    default:
                        break;
                }

                // 3) doing a rolling average? roll that average
                List<CalculatedValue> averageValues = new List<CalculatedValue>();
                foreach (CalculatedValue cv in preProcess)
                {
                    var comparativeDate = cv.Date - new TimeSpan(rollingAverage, 0, 0, 0);
                    var comparativeValue = preProcess.FirstOrDefault(val => val.Date.Date == comparativeDate);
                    if (comparativeValue == null)
                    {
                        averageValues.Add(new CalculatedValue(cv.MetricName, 0, cv.Date));
                    }
                    else if (cv.MetricName.Contains("percentPositive"))
                    {
                        var percentPos = GetPercentPositiveRollingAvg(cv.Date, rollingAverage);
                        averageValues.Add(new CalculatedValue(cv.MetricName + "-" + rollingAverage.ToString() + "dayavg", percentPos, cv.Date));
                    }
                    else if (cv.MetricName.Contains("currentHospital"))
                    {
                        var hospitalVals = preProcess.FindAll(val => val.Date.Date < cv.Date.Date && val.Date.Date >= comparativeValue.Date.Date);
                        double allHospitalVals = 0;
                        foreach (CalculatedValue otherCv in hospitalVals)
                        {
                            allHospitalVals += otherCv.Value;
                        }

                        allHospitalVals = allHospitalVals / rollingAverage;

                        averageValues.Add(new CalculatedValue(cv.MetricName + "-" + rollingAverage.ToString() + "dayavg", allHospitalVals, cv.Date));
                    }
                    else
                    {
                        var avgValue = (cv.Value - comparativeValue.Value) / rollingAverage;
                        averageValues.Add(new CalculatedValue(cv.MetricName + "-" + rollingAverage.ToString() + "dayavg", avgValue, cv.Date));
                    }
                }

                if (isRollingAverage)
                {
                    namedMetric += "-" + rollingAverage.ToString() + "dayavg";
                    preProcess = averageValues;
                }

                // 4) do a perCapita calculation
                if (isPerCapita && metric != Metrics.PercentPositive)
                {
                    foreach (CalculatedValue cv in preProcess)
                    {
                        processedValues.Add(new CalculatedValue(namedMetric, ((cv.Value / CoreCountryData.Population) * 100000), cv.Date));
                    }

                    namedMetric += "-per100K";
                }
                else
                {
                    processedValues = preProcess;
                }

                OutputFiles.Add(namedMetric, processedValues);
            }

        }

        private double GetPercentPositiveRollingAvg(DateTime date, int rollAvg)
        {
            var oldData = CoreCountryData.Data.FirstOrDefault(d => d.Date.Date == (date - new TimeSpan(rollAvg, 0, 0, 0)).Date);
            var newData = CoreCountryData.Data.FirstOrDefault(d => d.Date == date);

            var caseChange = Convert.ToDouble(newData.TotalCases - oldData.TotalCases);
            var testChange = Convert.ToDouble(newData.TotalTests) - Convert.ToDouble(oldData.TotalTests);

            return (caseChange / testChange) * 100;
        }
    }
}
