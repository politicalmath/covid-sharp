using CovidSharp.Constants;
using CovidSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CovidSharp.Utils
{
    public static class StateMapHelper {
        public static StateBase StringToStateMap(string stateString)
        {
            var listOfStates = StatesConstants.GetStatesList();
            switch (stateString.ToLower())
            {
                case "alabama":
                case "al":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.AL);
                case "alaska":
                case "ak":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.AK);
                case "american samoa":
                case "americansamoa":
                case "as":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.AS);
                case "arizona":
                case "az":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.AZ);
                case "arkansas":
                case "ar":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.AR);
                case "california":
                case "ca":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.CA);
                case "colorado":
                case "co":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.CO);
                case "connecticut":
                case "ct":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.CT);
                case "delaware":
                case "de":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.DE);
                case "districtofcolumbia":
                case "district of columbia":
                case "washingtondc":
                case "dc":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.DC);
                case "florida":
                case "fl":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.FL);
                case "georgia":
                case "ga":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.GA);
                case "guam":
                case "gu":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.GU);
                case "hawaii":
                case "hi":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.HI);
                case "idaho":
                case "id":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.ID);
                case "illinois":
                case "il":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.IL);
                case "indiana":
                case "in":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.IN);
                case "iowa":
                case "ia":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.IA);
                case "kansas":
                case "ks":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.KS);
                case "kentucky":
                case "ky":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.KY);
                case "louisiana":
                case "la":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.LA);
                case "maine":
                case "me":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.ME);
                case "maryland":
                case "md":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MD);
                case "massachusetts":
                case "ma":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MA);
                case "michigan":
                case "mi":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MI);
                case "minnesota":
                case "mn":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MN);
                case "mississippi":
                case "ms":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MS);
                case "missouri":
                case "mo":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MO);
                case "montana":
                case "mt":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.MT);
                case "nebraska":
                case "ne":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NE);
                case "nevada":
                case "nv":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NV);
                case "new hampshire":
                case "newhampshire":
                case "nh":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NH);
                case "new jersey":
                case "newjersey":
                case "nj":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NJ);
                case "new mexico":
                case "newmexico":
                case "nm":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NM);
                case "new york":
                case "newyork":
                case "ny":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NY);
                case "north carolina":
                case "northcarolina":
                case "nc":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.NC);
                case "north dakota":
                case "northdakota":
                case "nd":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.ND);
                case "ohio":
                case "oh":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.OH);
                case "oklahoma":
                case "ok":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.OK);
                case "oregon":
                case "or":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.OR);
                case "pennsylvania":
                case "pa":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.PA);
                case "puerto rico":
                case "puertorico":
                case "pr":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.PR);
                case "rhode island":
                case "rhodeisland":
                case "ri":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.RI);
                case "south carolina":
                case "southcarolina":
                case "sc":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.SC);
                case "south dakota":
                case "southdakota":
                case "sd":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.SD);
                case "tennessee":
                case "tn":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.TN);
                case "texas":
                case "tx":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.TX);
                case "utah":
                case "ut":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.UT);
                case "vermont":
                case "vt":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.VT);
                case "virgin islands":
                case "virginislands":
                case "vi":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.VI);
                case "virginia":
                case "va":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.VA);
                case "washington":
                case "wa":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.WA);
                case "west virginia":
                case "westvirginia":
                case "wv":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.WV);
                case "wisconsin":
                case "wi":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.WI);
                case "wyoming":
                case "wy":
                    return listOfStates.FirstOrDefault(s => s.Code == Constants.StateCode.WY);
                default:
                    Debug.WriteLine("Bad State - " + stateString.ToLower());
                    return null;
            }
        }

        public static bool LooseStateStringMatch(string stateString, string aliasString)
        {
            var l1 = stateString.ToLower();
            var l2 = stateString.ToLower();
            if (stateString.ToLower() == aliasString.ToLower())
                return true;
            else
            {
                if ((l1 == "american samoa" || l1 == "americansamoa") && (l2 == "american samoa" || l2 == "americansamoa")) return true;

                if ((l1 == "districtofcolumbia" || l1 == "district of columbia" || l1 == "washingtondc") && 
                    (l2 == "districtofcolumbia" || l2 == "district of columbia" || l1 == "washingtondc")) return true;

                if ((l1 == "west virginia" || l1 == "westvirginia") &&
                    (l2 == "west virginia" || l2 == "westvirginia")) return true;

                if ((l1 == "virgin islands" || l1 == "virginislands") &&
                    (l2 == "virgin islands" || l2 == "virginislands")) return true;

                if ((l1 == "puerto rico" || l1 == "puertorico") &&
                    (l2 == "puerto rico" || l2 == "puertorico")) return true;

                if ((l1 == "rhode island" || l1 == "rhodeisland") &&
                    (l2 == "rhode island" || l2 == "rhodeisland")) return true;

                if ((l1 == "south carolina" || l1 == "southcarolina") &&
                    (l2 == "south carolina" || l2 == "southcarolina")) return true;

                if ((l1 == "south dakota" || l1 == "southdakota") &&
                    (l2 == "south dakota" || l2 == "southdakota")) return true;

                if ((l1 == "new hampshire" || l1 == "newhampshire") &&
                    (l2 == "new hampshire" || l2 == "newhampshire")) return true;

                if ((l1 == "new jersey" || l1 == "newjersey") &&
                    (l2 == "new jersey" || l2 == "newjersey")) return true;

                if ((l1 == "new mexico" || l1 == "newmexico") &&
                    (l2 == "new mexico" || l2 == "newmexico")) return true;

                if ((l1 == "new york" || l1 == "newyork") &&
                    (l2 == "new york" || l2 == "newyork")) return true;

                if ((l1 == "north carolina" || l1 == "northcarolina") &&
                    (l2 == "north carolina" || l2 == "northcarolina")) return true;

                if ((l1 == "north dakota" || l1 == "northdakota") &&
                    (l2 == "north dakota" || l2 == "northdakota")) return true;
                
                return false;
            }
        }
    
        public static string StateBaseToString(StateName sn)
        {
            switch (sn)
            {
                case StateName.AmericanSamoa:
                    return "American Samoa";
                case StateName.MarianaIslands:
                    return "Norther Mariana Islands"; 
                case StateName.WashingtonDC:
                    return "District of Columbia";
                case StateName.NewHampshire:
                    return "New Hampshire";
                case StateName.NewJersey:
                    return "New Jersey";
                case StateName.NewMexico:
                    return "New Mexico";
                case StateName.NewYork:
                    return "New York";
                case StateName.NorthCarolina:
                    return "North Carolina";
                case StateName.NorthDakota:
                    return "North Dakota";
                case StateName.RhodeIsland:
                    return "Rhode Island";
                case StateName.SouthCarolina:
                    return "South Carolina";
                case StateName.SouthDakota:
                    return "South Dakota";
                case StateName.WestVirginia:
                    return "West Virginia";
                default:
                    return sn.ToString();
            }
        }
    }
}
