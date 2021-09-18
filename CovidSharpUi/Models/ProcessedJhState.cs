using CovidSharp.JohnsHopkins.Model;
using CovidSharpUi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidSharpUi.Models
{
    public class ProcessedJhState
    {
        public Dictionary<string, List<CalculatedValue>> OutputFiles { get; set; }
        public JhState CoreStateData { get; set; }

        public ProcessedJhState(JhState coreState)
        {
            CoreStateData = coreState;
            OutputFiles = new Dictionary<string, List<CalculatedValue>>();
        }

        public void ProcessCumulativeData(List<Metrics> metrics)
        {
            foreach (Metrics metric in metrics)
            {
                // 1) Create a new List<CalculatedValue>
                List<CalculatedValue> processedValues = new List<CalculatedValue>();
                // 2) Look at the metric, pick out the appropriate data for that metric (and give it a name)
                List<CalculatedValue> allValues = new List<CalculatedValue>();
                string namedMetric = "jh-cumulative-";

                switch (metric)
                {
                    case Metrics.Deaths:

                        namedMetric += "deaths";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            allValues.Add(new CalculatedValue(namedMetric, Convert.ToDouble(sd.Deaths), sd.Date));
                        }
                        break;
                    case Metrics.Cases:

                        namedMetric += "positives";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            allValues.Add(new CalculatedValue(namedMetric, Convert.ToDouble(sd.Positives), sd.Date));
                        }
                        break;
                    case Metrics.Tests:
                        namedMetric += "tests";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            double tests = Convert.ToDouble(sd.Tests);
                            allValues.Add(new CalculatedValue(namedMetric, tests, sd.Date));
                        }
                        break;
                    case Metrics.PercentPositive:
                        namedMetric += "percentPositive";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            double percentPositive = Convert.ToDouble(sd.Positives) / Convert.ToDouble(sd.Tests);
                            allValues.Add(new CalculatedValue(namedMetric, percentPositive, sd.Date));
                        }
                        break;
                    default:
                        break;
                }

                // Add all the cumulative items 
                OutputFiles.Add(namedMetric, allValues);

                // 3) do the per capita calculation
                if (metric != Metrics.PercentPositive)
                {
                    foreach (CalculatedValue cv in allValues)
                    {
                        processedValues.Add(new CalculatedValue(namedMetric, ((cv.Value / CoreStateData.State.Population) * 100000), cv.Date));
                    }

                    namedMetric += "-per100K";
                    OutputFiles.Add(namedMetric, processedValues);
                }
            }
        }

        public void ProcessDailyData(List<Metrics> metrics, bool isRollingAverage = true, int rollingAverage = 7) { 
            //List<StateDay> sortedDays = CoreStateData.CovidData.OrderByDescending(d => d.DateInt).ToList();

            // 2) Look at the metric, pick out the appropriate data for that metric (and give it a name)
            foreach (Metrics metric in metrics)
            {
                List<CalculatedValue> processedValues = new List<CalculatedValue>();
                List<CalculatedValue> preProcess = new List<CalculatedValue>();
                string namedMetric = "jh-daily-";

                switch (metric)
                {
                    case Metrics.Deaths:
                        namedMetric += "deaths";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            preProcess.Add(new CalculatedValue(namedMetric, sd.Deaths, sd.Date));
                        }
                        break;
                    case Metrics.Cases:
                        namedMetric += "positives";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            preProcess.Add(new CalculatedValue(namedMetric, sd.Positives, sd.Date));
                        }
                        break;
                    case Metrics.Tests:
                        namedMetric += "tests";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            double tests = sd.Tests;
                            preProcess.Add(new CalculatedValue(namedMetric, sd.Tests, sd.Date));
                        }
                        break;
                    case Metrics.PercentPositive:
                        namedMetric += "percentPositive";
                        foreach (JhCovidDay sd in CoreStateData.CovidData)
                        {
                            double percentPositive = sd.Positives / sd.Tests;
                            preProcess.Add(new CalculatedValue(namedMetric, percentPositive, sd.Date));
                        }
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

                OutputFiles.Add(namedMetric, preProcess);

                // 4) do a perCapita calculation
                if (metric != Metrics.PercentPositive)
                {
                    foreach (CalculatedValue cv in preProcess)
                    {
                        processedValues.Add(new CalculatedValue(namedMetric, ((cv.Value / CoreStateData.State.Population) * 100000), cv.Date));
                    }

                    namedMetric += "-per100K";

                    OutputFiles.Add(namedMetric, processedValues);
                }
            }

        }

        private double GetPercentPositiveRollingAvg(DateTime date, int rollAvg)
        {
            var oldData = CoreStateData.CovidData.FirstOrDefault(d => d.Date.Date == (date - new TimeSpan(rollAvg, 0, 0, 0)).Date);
            var newData = CoreStateData.CovidData.FirstOrDefault(d => d.Date == date);

            var caseChange = newData.Positives - oldData.Positives;
            var testChange = newData.Tests - oldData.Tests;

            return (caseChange / testChange) * 100;
        }


    }
}
