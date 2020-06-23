using System;
using System.Collections.Generic;
using System.Text;
using CovidSharp.Constants;
using CovidSharp.CovidTrack.Helpers;
using CovidSharp.Interfaces;

namespace CovidSharp.CovidTrack
{
    public static class SourceConfig
    {
        // Source: https://covidtracking.com/api

        public const string BaseUrlString = "https://covidtracking.com";
        public const string ApiStatusString = "/api/v1/status.json";

        #region US Url Constants

        public const string HistoricUsString = "/api/v1/us/daily.json";
        public const string LatestUsString = "/api/v1/us/current.json";
        private const string usWithDateBaseString = "/api/v1/us/{date}.json";

        public static string UsWithDateString(DateTime date)
        {
            return BaseUrlString + usWithDateBaseString.Replace(dateSub, CovidTrackHelpers.DateToString(date));
        }
        
        #endregion
        
        #region State Url Constants 
        
        public const string AllStateMetaString = "/api/v1/states/info.json";
        public const string HistoricStatesString = "/api/v1/states/daily.json";
        public const string LatestStatesString = "/api/v1/states/current.json";

        private const string latestStateBaseString = "/api/v1/states/{state}/current.json";
        private const string historicStateBaseString = "/api/v1/states/{state}/daily.json";
        private const string stateWithDateBaseString = "/api/v1/states/{state}/{date}.json";
        private const string stateMetaString = "/api/v1/states/{state}/info.json";

        public static string StateMeta(StateCode code)
        {
            return BaseUrlString + stateMetaString.Replace(stateSub, code.ToString().ToLower());
        }

        public static string LatestStateString(StateCode code)
        {
            return BaseUrlString + latestStateBaseString.Replace(stateSub, code.ToString().ToLower());
        }

        public static string HistoricStateString(StateCode code)
        {
            return BaseUrlString + historicStateBaseString.Replace(stateSub, code.ToString().ToLower());
        }

        public static string StateWithDateString(StateCode code, DateTime date)
        {
            return BaseUrlString + stateWithDateBaseString.Replace(stateSub, code.ToString().ToLower()).Replace(dateSub, CovidTrackHelpers.DateToString(date));
        }
        
        private const string stateSub = "{state}";

        #endregion 
        
        private const string dateSub = "{date}";
    }
}
