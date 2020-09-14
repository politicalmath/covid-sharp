using CovidSharp.owiData.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CovidSharp.owiData
{
    public class OwidService
    {
        private JsonSerializerSettings serializeSettings { get; set; }
        public OwidService()
        {
            serializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public async Task<Dictionary<string, OwidCountry>> GetRawOwidData()
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = OwidConfig.SourceUrl;
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<Dictionary<string, OwidCountry>>(targetContent, serializeSettings);
            }
        }

        public async Task<List<OwidCountry>> GetAllWorldData() {

            List<OwidCountry> returnData = new List<OwidCountry>();
            var rawOwidData = await GetRawOwidData();
            foreach (KeyValuePair<string, OwidCountry> kvp in rawOwidData){
                var country = kvp.Value;
                country.CountryCode = kvp.Key;
                returnData.Add(country);
            }

            return returnData;
        }
    }
}
