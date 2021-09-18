using CovidSharp.Models;
using CovidSharp.NytData.Models;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CovidSharp.Utils.CsvUtility;

namespace CovidSharp.NytData
{
    public class NytDataService
    {
        private JsonSerializerSettings serializeSettings { get; set; }
        public NytDataService()
        {
            serializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public async Task<List<NytCountyDay>> GetCountyCovidData(bool isLive = false)
        {
            using (var client = new HttpClient())
            {
                var rawData = new List<NytCountyDay>();

                var sourceUrl = SourceConfig.BaseUrlString;
                if (isLive) sourceUrl += SourceConfig.LiveString;
                sourceUrl += SourceConfig.CountiesString;
                
                var targetContent = await client.GetStreamAsync(sourceUrl);
                using (var reader = new StreamReader(targetContent))
                {
                    using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {

                        //date,county,state,fips,cases,deaths
                        //2021-08-11,Autauga,Alabama,01001,7854,114
                        
                        csv.Read();
                        csv.ReadHeader();
                        while (csv.Read())
                        {
                            var Cases = Int32.TryParse(csv.GetField("cases"), out int cases) ? cases : 0;
                            var Deaths = Int32.TryParse(csv.GetField("deaths"), out int deaths) ? deaths : 0;

                            var nytDatum = new NytCountyDay()
                            {
                                Cases = Cases,
                                Deaths = Deaths,
                                DateString = csv.GetField("date"),
                                State = csv.GetField("state"),
                                Fips = csv.GetField("fips"),
                                CountyName = csv.GetField("county")
                            };
                            rawData.Add(nytDatum);
                        }
                    }
                }

                return rawData;
            }
        }

        public async Task<List<CountyPop>> GetCountyPopData(Stream countyFileStream)
        {
            List<CountyPop> countyPopList = new List<CountyPop>();

            using(var reader = new StreamReader(countyFileStream))
            {
                using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var thisCounty = new CountyPop()
                        {
                            Fips = csv.GetField("FIPStxt"),
                            State = csv.GetField("State"),
                            CountyName = csv.GetField("Area_Name"),
                            PopulationString = csv.GetField("POP_ESTIMATE_2019")
                        };

                        countyPopList.Add(thisCounty);

                    }
                }
            }

            return countyPopList;
        }
    }
}
