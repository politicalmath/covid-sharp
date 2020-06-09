using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CovidSharp.CovidTrack.Helpers
{
    public static class CovidTrackHelpers
    {
        public static string DateToString(DateTime date)
        {
            return date.ToString("yyyyMMdd", CultureInfo.InvariantCulture).ToLower();
        }

    }
}
