using CovidSharp.Constants;
using CovidSharp.CovidTrack.Models;
using CovidSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CovidSharp.CovidTrack
{
    public class CovidTrackService
    {
        private JsonSerializerSettings serializeSettings { get; set; }
        public CovidTrackService() {
            serializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        #region Raw API Calls
        public async Task<ApiStatus> GetApiStatus()
        {
            using(var client = new HttpClient()) {
                var sourceUrl = SourceConfig.BaseUrlString + SourceConfig.ApiStatusString;
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<ApiStatus>(targetContent, serializeSettings);
            }
        }

        public async Task<List<UsDay>> GetHistoricUsData()
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.BaseUrlString + SourceConfig.HistoricUsString;
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<List<UsDay>>(targetContent, serializeSettings);
            }
        }

        public async Task<List<UsDay>> GetCurrentUsData()
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.BaseUrlString + SourceConfig.LatestUsString;
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<List<UsDay>>(targetContent, serializeSettings);
            }
        }

        public async Task<UsDay> GetUsDataOnDate(DateTime date)
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.UsWithDateString(date);
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<UsDay>(targetContent, serializeSettings);
            }
        }

        public async Task<List<StateMetadata>> GetAllStateMetadata()
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.BaseUrlString + SourceConfig.AllStateMetaString;
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<List<StateMetadata>>(targetContent, serializeSettings);
            }
        }

        public async Task<StateMetadata> GetStateMetadata(StateCode stateCode)
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.StateMeta(stateCode);
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<StateMetadata>(targetContent, serializeSettings);
            }
        }

        public async Task<List<StateDay>> GetHistoricStateData(StateCode stateCode = StateCode.None)
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.BaseUrlString + SourceConfig.HistoricStatesString;
                if(stateCode != StateCode.None)
                {
                    sourceUrl = SourceConfig.HistoricStateString(stateCode);
                }
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<List<StateDay>>(targetContent, serializeSettings);
            }
        }

        public async Task<List<StateDay>> GetCurrentStatesData()
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.BaseUrlString + SourceConfig.LatestStatesString;
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<List<StateDay>>(targetContent, serializeSettings);
            }
        }

        public async Task<StateDay> GetCurrentStateData(StateCode stateCode)
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.LatestStateString(stateCode);
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<StateDay>(targetContent, serializeSettings);
            }
        }
                
        public async Task<StateDay> GetStateDataByDate(StateCode stateCode, DateTime date)
        {
            using (var client = new HttpClient())
            {
                var sourceUrl = SourceConfig.StateWithDateString(stateCode, date);
                var targetContent = await client.GetStringAsync(sourceUrl);
                return JsonConvert.DeserializeObject<StateDay>(targetContent, serializeSettings);
            }
        }
        #endregion

        #region Data Object Wrappers
        public async Task<List<State>> GetAllStateData()
        {
            List<StateBase> basicStates = StatesConstants.GetStatesList();
            var returnData = new List<State>();
            var allStateData = await GetHistoricStateData();
            
            foreach(StateBase stateBase in basicStates)
            {
                var state = new State(stateBase);
                state.CovidData = allStateData.FindAll(sData => sData.State.ToLower() == stateBase.Code.ToString().ToLower());
                returnData.Add(state);
            }

            return returnData;
        }

        #endregion
    }
}
