using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.Constants
{
    public static class StatePopulation
    {
        public static Dictionary<StateCode, int> PopulationByState()
        {
            Dictionary<StateCode, int> pop = new Dictionary<StateCode, int>();

            pop[StateCode.AK] = 731545;
            pop[StateCode.AL] = 4903185;
            pop[StateCode.AR] = 3017825;
            pop[StateCode.AZ] = 7278717;
            pop[StateCode.CA] = 39512223;
            pop[StateCode.CO] = 5758736;
            pop[StateCode.CT] = 3565287;
            pop[StateCode.DC] = 705749;
            pop[StateCode.DE] = 973764;
            pop[StateCode.FL] = 21477737;
            pop[StateCode.GA] = 10617423;
            pop[StateCode.HI] = 1415872;
            pop[StateCode.IA] = 3155070;
            pop[StateCode.ID] = 1792065;
            pop[StateCode.IL] = 12671821;
            pop[StateCode.IN] = 6732219;
            pop[StateCode.KS] = 2913314;
            pop[StateCode.KY] = 4467673;
            pop[StateCode.LA] = 4648794;
            pop[StateCode.MA] = 6949503;
            pop[StateCode.MD] = 6045680;
            pop[StateCode.ME] = 1344212;
            pop[StateCode.MI] = 9986857;
            pop[StateCode.MN] = 5639632;
            pop[StateCode.MO] = 6137428;
            pop[StateCode.MS] = 2976149;
            pop[StateCode.MT] = 1068778;
            pop[StateCode.NC] = 10488084;
            pop[StateCode.ND] = 762062;
            pop[StateCode.NE] = 1934408;
            pop[StateCode.NH] = 1359711;
            pop[StateCode.NJ] = 8882190;
            pop[StateCode.NM] = 2096829;
            pop[StateCode.NV] = 3080156;
            pop[StateCode.NY] = 19453561;
            pop[StateCode.OH] = 11689100;
            pop[StateCode.OK] = 3956971;
            pop[StateCode.OR] = 4217737;
            pop[StateCode.PA] = 12801989;
            pop[StateCode.PR] = 3193694;
            pop[StateCode.RI] = 1059361;
            pop[StateCode.SC] = 5148714;
            pop[StateCode.SD] = 884659;
            pop[StateCode.TN] = 6833174;
            pop[StateCode.TX] = 28995881;
            pop[StateCode.UT] = 3205958;
            pop[StateCode.VA] = 8535519;
            pop[StateCode.VT] = 623989;
            pop[StateCode.WA] = 7614893;
            pop[StateCode.WI] = 5822434;
            pop[StateCode.WV] = 1787147;
            pop[StateCode.WY] = 578759;

            pop[StateCode.MP] = 53883;
            pop[StateCode.AS] = 55212;
            pop[StateCode.VI] = 104449;
            pop[StateCode.GU] = 162742;

            return pop;
        }
    }
}
