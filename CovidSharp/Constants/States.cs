using CovidSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.Constants
{
    public static class StatesConstants
    {
        public static List<StateBase> GetStatesList()
        {
            var populationDict = StatePopulation.PopulationByState();
            List<StateBase> stateInfo = new List<StateBase>();
            stateInfo.Add(new StateBase(StateName.Alaska, StateCode.AK, populationDict[StateCode.AK]));
            stateInfo.Add(new StateBase(StateName.Alabama, StateCode.AL, populationDict[StateCode.AL]));
            stateInfo.Add(new StateBase(StateName.Arkansas, StateCode.AR, populationDict[StateCode.AR]));
            stateInfo.Add(new StateBase(StateName.AmericanSamoa, StateCode.AS, populationDict[StateCode.AS]));
            stateInfo.Add(new StateBase(StateName.Arizona, StateCode.AZ, populationDict[StateCode.AZ]));
            stateInfo.Add(new StateBase(StateName.California, StateCode.CA, populationDict[StateCode.CA]));
            stateInfo.Add(new StateBase(StateName.Colorado, StateCode.CO, populationDict[StateCode.CO]));
            stateInfo.Add(new StateBase(StateName.Connecticut, StateCode.CT, populationDict[StateCode.CT]));
            stateInfo.Add(new StateBase(StateName.WashingtonDC, StateCode.DC, populationDict[StateCode.DC]));
            stateInfo.Add(new StateBase(StateName.Delaware, StateCode.DE, populationDict[StateCode.DE]));
            stateInfo.Add(new StateBase(StateName.Florida, StateCode.FL, populationDict[StateCode.FL]));
            stateInfo.Add(new StateBase(StateName.Georgia, StateCode.GA, populationDict[StateCode.GA]));
            stateInfo.Add(new StateBase(StateName.Guam, StateCode.GU, populationDict[StateCode.GU]));
            stateInfo.Add(new StateBase(StateName.Hawaii, StateCode.HI, populationDict[StateCode.HI]));
            stateInfo.Add(new StateBase(StateName.Iowa, StateCode.IA, populationDict[StateCode.IA]));
            stateInfo.Add(new StateBase(StateName.Idaho, StateCode.ID, populationDict[StateCode.ID]));
            stateInfo.Add(new StateBase(StateName.Illinois, StateCode.IL, populationDict[StateCode.IL]));
            stateInfo.Add(new StateBase(StateName.Indiana, StateCode.IN, populationDict[StateCode.IN]));
            stateInfo.Add(new StateBase(StateName.Kansas, StateCode.KS, populationDict[StateCode.KS]));
            stateInfo.Add(new StateBase(StateName.Kentucky, StateCode.KY, populationDict[StateCode.KY]));
            stateInfo.Add(new StateBase(StateName.Louisiana, StateCode.LA, populationDict[StateCode.LA]));
            stateInfo.Add(new StateBase(StateName.Massachusetts, StateCode.MA, populationDict[StateCode.MA]));
            stateInfo.Add(new StateBase(StateName.Maryland, StateCode.MD, populationDict[StateCode.MD]));
            stateInfo.Add(new StateBase(StateName.Maine, StateCode.ME, populationDict[StateCode.ME]));
            stateInfo.Add(new StateBase(StateName.Michigan, StateCode.MI, populationDict[StateCode.MI]));
            stateInfo.Add(new StateBase(StateName.Minnesota, StateCode.MN, populationDict[StateCode.MN]));
            stateInfo.Add(new StateBase(StateName.Missouri, StateCode.MO, populationDict[StateCode.MO]));
            stateInfo.Add(new StateBase(StateName.MarianaIslands, StateCode.MP, populationDict[StateCode.MP]));
            stateInfo.Add(new StateBase(StateName.Mississippi, StateCode.MS, populationDict[StateCode.MS]));
            stateInfo.Add(new StateBase(StateName.Montana, StateCode.MT, populationDict[StateCode.MT]));
            stateInfo.Add(new StateBase(StateName.NorthCarolina, StateCode.NC, populationDict[StateCode.NC]));
            stateInfo.Add(new StateBase(StateName.NorthDakota, StateCode.ND, populationDict[StateCode.ND]));
            stateInfo.Add(new StateBase(StateName.Nebraska, StateCode.NE, populationDict[StateCode.NE]));
            stateInfo.Add(new StateBase(StateName.NewHampshire, StateCode.NH, populationDict[StateCode.NH]));
            stateInfo.Add(new StateBase(StateName.NewJersey, StateCode.NJ, populationDict[StateCode.NJ]));
            stateInfo.Add(new StateBase(StateName.NewMexico, StateCode.NM, populationDict[StateCode.NM]));
            stateInfo.Add(new StateBase(StateName.Nevada, StateCode.NV, populationDict[StateCode.NV]));
            stateInfo.Add(new StateBase(StateName.NewYork, StateCode.NY, populationDict[StateCode.NY]));
            stateInfo.Add(new StateBase(StateName.Ohio, StateCode.OH, populationDict[StateCode.OH]));
            stateInfo.Add(new StateBase(StateName.Oklahoma, StateCode.OK, populationDict[StateCode.OK]));
            stateInfo.Add(new StateBase(StateName.Oregon, StateCode.OR, populationDict[StateCode.OR]));
            stateInfo.Add(new StateBase(StateName.Pennsylvania, StateCode.PA, populationDict[StateCode.PA]));
            stateInfo.Add(new StateBase(StateName.PuertoRico, StateCode.PR, populationDict[StateCode.PR]));
            stateInfo.Add(new StateBase(StateName.RhodeIsland, StateCode.RI, populationDict[StateCode.RI]));
            stateInfo.Add(new StateBase(StateName.SouthCarolina, StateCode.SC, populationDict[StateCode.SC]));
            stateInfo.Add(new StateBase(StateName.SouthDakota, StateCode.SD, populationDict[StateCode.SD]));
            stateInfo.Add(new StateBase(StateName.Tennessee, StateCode.TN, populationDict[StateCode.TN]));
            stateInfo.Add(new StateBase(StateName.Texas, StateCode.TX, populationDict[StateCode.TX]));
            stateInfo.Add(new StateBase(StateName.Utah, StateCode.UT, populationDict[StateCode.UT]));
            stateInfo.Add(new StateBase(StateName.Virginia, StateCode.VA, populationDict[StateCode.VA]));
            stateInfo.Add(new StateBase(StateName.VirginIslands, StateCode.VI, populationDict[StateCode.VI]));
            stateInfo.Add(new StateBase(StateName.Vermont, StateCode.VT, populationDict[StateCode.VT]));
            stateInfo.Add(new StateBase(StateName.Washington, StateCode.WA, populationDict[StateCode.WA]));
            stateInfo.Add(new StateBase(StateName.Wisconsin, StateCode.WI, populationDict[StateCode.WI]));
            stateInfo.Add(new StateBase(StateName.WestVirginia, StateCode.WV, populationDict[StateCode.WV]));
            stateInfo.Add(new StateBase(StateName.Wyoming, StateCode.WY, populationDict[StateCode.WY]));

            return stateInfo;
        }

    }
}
