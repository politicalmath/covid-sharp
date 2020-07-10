using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CovidSharp.Utils.CsvUtility;

namespace CovidSharp.NytData
{
    public class NytDataService
    {
        public NytDataService()
        {

        }

        public async Task GetCountyData(bool isLive = false)
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.BaseUrlString;
                if (isLive) sourceUrl += SourceConfig.LiveString;
                sourceUrl += SourceConfig.CountiesString;

                var targetContent = await client.GetStreamAsync(sourceUrl);
                using (CsvFileReader dataReader = new CsvFileReader(targetContent))
                {

                }
            }
        }
    }
}
