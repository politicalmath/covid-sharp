using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CdcVaccine
{
    public class VaccineStateDay
    {
        [JsonProperty("Date")]
        public string DateString { get; set; }

        public DateTime DataDate
        {
            get
            {
                try
                {
                    int y = Convert.ToInt32(DateString.Substring(0, 4));
                    int m = Convert.ToInt32(DateString.Substring(5, 2));
                    int d = Convert.ToInt32(DateString.Substring(8, 2));
                    return new DateTime(y, m, d);
                } catch
                {
                    int y = Convert.ToInt32(DateString.Substring(6, 4));
                    int m = Convert.ToInt32(DateString.Substring(0, 2));
                    int d = Convert.ToInt32(DateString.Substring(3, 2));
                    return new DateTime(y, m, d);
                }
            }
        }

        [JsonProperty("MMWR_week")]
        public int MmwrWeek { get; set; }

        [JsonProperty("Location")]
        public string StateCode { get; set; }

        [JsonProperty("ShortName")]
        public string ShortName { get; set; }

        [JsonProperty("LongName")]
        public string StateName { get; set; }

        [JsonProperty("Census2019")]
        public int StatePopulation { get; set; }

        [JsonProperty("Doses_Distributed")]
        public int DosesDistributed { get; set; }

        [JsonProperty("Doses_Administered")]
        public int DosesAdministered { get; set; }

        [JsonProperty("Dist_Per_100K")]
        public int DistributedPer100K { get; set; }

        [JsonProperty("Admin_Per_100K")]
        public int AdministeredPer100K { get; set; }

        // The CDC started tracking dose numbers on the 12th
        [JsonProperty("Administered_Dose1")]
        public int? Dose1Administered { get; set; }

        [JsonProperty("Administered_Dose1_Per_100K")]
        public int? Dose1AdministeredPer100K { get; set; }

        [JsonProperty("Administered_Dose2")]
        public int? Dose2Administered { get; set; }

        [JsonProperty("Administered_Dose2_Per_100K")]
        public int? Dose2AdministeredPer100K { get; set; }

        // The CDC Tracked manifacturing for about 5 days
        [JsonProperty("Administered_Moderna")]
        public int? DosesAdministeredModerna { get; set; }
        
        [JsonProperty("Administered_Pfizer")]
        public int? DosesAdministeredPfizer { get; set; }

        [JsonProperty("Administered_Janssen")]
        public int? DosesAdministeredJanssen { get; set; }

        [JsonProperty("Administered_Unk_Manuf")]
        public int? DosesAdministeredUnknown { get; set; }

        [JsonProperty("Ratio_Admin_Dist")]
        public double? DistributedAdminRatio { get; set; }

        [JsonProperty("Administered_Dose1_Pop_Pct")]
        public double? Dose1AdminPopulationPct { get; set; }
        
        [JsonProperty("Administered_Dose2_Pop_Pct")]
        public double? Dose2AdminPopulationPct { get; set; }
        
        [JsonProperty("Administered_Dose1_Recip_18Plus")]
        public int? Dose1Admin18Plus { get; set; }
        
        [JsonProperty("Administered_Dose1_Recip_18PlusPop_Pct")]
        public double? Dose1Admin18PlusPct { get; set; }
        
        [JsonProperty("Administered_18Plus")]
        public int? Admin18Plus { get; set; }
        
        [JsonProperty("Administered_Dose1_Recip_65Plus")]
        public int? Dose1Admin65Plus { get; set; }
        
        [JsonProperty("Administered_Dose1_Recip_65PlusPop_Pct")]
        public double? Dose1Admin65PlusPct { get; set; }
        
        [JsonProperty("Administered_65Plus")]
        public int? Admin65Plus { get; set; }
        
        [JsonProperty("Administered_Dose2_Recip")]
        public int? Dose2Admin { get; set; }
        
        [JsonProperty("Administered_Dose2_Recip_18Plus")]
        public int? Dose2Admin18Plus { get; set; }
        
        [JsonProperty("Administered_Dose2_Recip_18PlusPop_Pct")]
        public double? Dose2Admin18PlusPct { get; set; }
        
        [JsonProperty("Series_Complete_Pop_Pct")]
        public double? SeriesCompletePopPct { get; set; }
        
        [JsonProperty("Series_Complete_18Plus")]
        public int? SeriesComplete18Plus { get; set; }
        
        [JsonProperty("Series_Complete_18PlusPop_Pct")]
        public double? SeriesComplete18PlusPct { get; set; }
        
        [JsonProperty("Series_Complete_65Plus")]
        public int? SeriesComplete65Plus { get; set; }

        [JsonProperty("Series_Complete_65PlusPop_Pct")]
        public double? SeriesComplete65PlusPct { get; set; }
    }
}
