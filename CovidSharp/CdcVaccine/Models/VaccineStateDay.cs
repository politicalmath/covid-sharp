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

        [JsonProperty("Doses_Distributed")]
        public int DosesDistributed { get; set; }

        [JsonProperty("Doses_Administered")]
        public int DosesAdministered { get; set; }

        [JsonProperty("Dist_Per_100K")]
        public int DistributedPer100K { get; set; }

        [JsonProperty("Admin_Per_100K")]
        public int AdministeredPer100K { get; set; }

        [JsonProperty("Census2019")]
        public int StatePopulation { get; set; }

        [JsonProperty("Administered_Moderna")]
        public int? DosesAdministeredModerna { get; set; }

        [JsonProperty("Administered_Pfizer")]
        public int? DosesAdministeredPfizer { get; set; }

        [JsonProperty("Administered_Unk_Manuf")]
        public int? DosesAdministeredUnknown { get; set; }

        [JsonProperty("Ratio_Admin_Dist")]
        public double? DistributedAdminRatio { get; set; }
    }
}
