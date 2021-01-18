using CovidSharp.CdcVaccine.Models;
using CovidSharpUi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidSharpUi.Models
{
    public class ProcessedVaccine
    {
        public Dictionary<string, List<CalculatedValue>> OutputFiles { get; set; }
        public VaccineState CoreVaccineStateData { get; set; }

        public ProcessedVaccine(VaccineState coreState)
        {
            CoreVaccineStateData = coreState;
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
                string namedMetric = "cumulative-";

                switch (metric)
                {
                    case Metrics.DosesAdministered:
                        namedMetric += "dosesAdmin";
                        foreach (VaccineDay vs in CoreVaccineStateData.VaccineData)
                        {
                            if (isPerCapita)
                            {
                                allValues.Add(new CalculatedValue(namedMetric + "-per100K", Convert.ToDouble(vs.AdministeredPer100K), vs.Date));
                                
                            }
                            else
                            {
                                allValues.Add(new CalculatedValue(namedMetric, Convert.ToDouble(vs.DosesAdministered), vs.Date));
                            }
                        }
                        break;
                    case Metrics.DosesDistributed:
                        namedMetric += "dosesDist";
                        foreach (VaccineDay vs in CoreVaccineStateData.VaccineData)
                        {
                            if (isPerCapita)
                            {
                                allValues.Add(new CalculatedValue(namedMetric + "-per100K", Convert.ToDouble(vs.DistributedPer100K), vs.Date));
                            }
                            else
                            {
                                allValues.Add(new CalculatedValue(namedMetric, Convert.ToDouble(vs.DosesDistributed), vs.Date));
                            }
                        }
                        break;
                    default:
                        break;
                }
                if (isPerCapita)
                {
                    namedMetric += "-per100K";
                }

                OutputFiles.Add(namedMetric, allValues);
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
                string namedMetric = "dailychange-";

                switch (metric)
                {
                    case Metrics.DosesAdministered:
                        namedMetric += "dosesAdmin";
                        foreach (VaccineDay vs in CoreVaccineStateData.VaccineData)
                        {
                            if (isPerCapita)
                            {
                                preProcess.Add(new CalculatedValue(namedMetric, Convert.ToDouble(vs.AdministeredPer100K), vs.Date));
                            }
                            else
                            {
                                preProcess.Add(new CalculatedValue(namedMetric, Convert.ToDouble(vs.DosesAdministered), vs.Date));
                            }
                        }
                        break;
                    case Metrics.DosesDistributed:
                        namedMetric += "dosesDist";
                        foreach (VaccineDay vs in CoreVaccineStateData.VaccineData)
                        {
                            if (isPerCapita)
                            {
                                preProcess.Add(new CalculatedValue(namedMetric, Convert.ToDouble(vs.DistributedPer100K), vs.Date));
                            }
                            else
                            {
                                preProcess.Add(new CalculatedValue(namedMetric, Convert.ToDouble(vs.DosesDistributed), vs.Date));
                            }
                        }
                        break;
                    default:
                        break;
                }
                List<CalculatedValue> averageValues = new List<CalculatedValue>();
                foreach (CalculatedValue cv in preProcess)
                {
                    var comparativeDate = cv.Date - new TimeSpan(rollingAverage, 0, 0, 0);
                    var comparativeValue = preProcess.FirstOrDefault(val => val.Date.Date == comparativeDate);
                    if (comparativeValue == null)
                    {
                        averageValues.Add(new CalculatedValue(cv.MetricName, 0, cv.Date));
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

                if (isPerCapita)
                {
                    foreach(CalculatedValue cv in preProcess)
                    {
                        processedValues.Add(new CalculatedValue(namedMetric, ((cv.Value / CoreVaccineStateData.StateBase.Population) * 100000), cv.Date));
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

        
    }
}
