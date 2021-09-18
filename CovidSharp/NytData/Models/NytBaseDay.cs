using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.NytData.Models
{
    public class NytBaseDay
    {
        public string DateString { get; set; }
        public DateTime Date { get
            {
                return DateConvert(DateString);
            }
        }
        public int Cases { get; set; }
        public int Deaths { get; set; }

        public static DateTime DateConvert(string dateString) {
            //"2021-08-11"
            var dateSplit = dateString.Split('-');
            int year = Convert.ToInt32(dateSplit[0]);
            int month = Convert.ToInt32(dateSplit[1]);
            int day = Convert.ToInt32(dateSplit[2]);
            return new DateTime(year, month, day);
        }

    }

    public class NytStateDay : NytBaseDay
    {
        public string State { get; set; }
        public string Fips { get; set; }
    }

    public class NytCountyDay : NytStateDay
    {
        public string CountyName { get; set; }
        // calculated values
        public double Population { get; set; }
        public double Cases7Avg { get; set; }
        public double Deaths7Avg { get; set; }
        public double Cases7AvgPer100K { get; set; }
        public double Deaths7AvgPer100K { get; set; }
        public NytCountyDay PreviousWeekday { get; set; }

        public NytCountyDay()
        {
        }

        public void ProcessValues()
        {
            if(PreviousWeekday != null) {
                var caseDiff = Convert.ToDouble(Cases - PreviousWeekday.Cases);
                var deathsDiff = Convert.ToDouble(Deaths - PreviousWeekday.Deaths);

                Cases7Avg = caseDiff / 7;
                Deaths7Avg = deathsDiff / 7;

                if (Cases7Avg < 0) Cases7Avg = 0;
                if (Deaths7Avg < 0) Deaths7Avg = 0;

                Cases7AvgPer100K = (Cases7Avg / Population) * 100000;
                Deaths7AvgPer100K = (Deaths7Avg / Population) * 100000;
            }
        }
    }

}
