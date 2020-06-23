using CovidSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidSharpUi.Models
{
    public class CalculatedValue
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string MetricName { get; set; }

        public CalculatedValue(string metricName, double value, DateTime date)
        {
            MetricName = metricName;
            Value = value;
            Date = date;
        }
    }
}
