using CovidSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.CdcVaccine.Models
{
    public class VaccineState
    {
        public StateBase StateBase { get; set; }
        public List<VaccineDay> VaccineData { get; set; }

        public VaccineState(StateBase stateBase)
        {
            StateBase = stateBase;
            VaccineData = new List<VaccineDay>();
        }
    }

    public class VaccineDay
    {
        public DateTime Date { get; set; }
        public int DosesDistributed { get; set; }
        public int DosesAdministered { get; set; }
        public int DistributedPer100K { get; set; }
        public int AdministeredPer100K { get; set; }
        public double PercentAdultVaccinated { get; set; }
        public double PercentSeniorsVaccinated { get; set; }


        public VaccineDay(VaccineStateDay vsd)
        {
            Date = vsd.DataDate;
            DosesDistributed = vsd.DosesDistributed;
            DosesAdministered = vsd.DosesAdministered;
            DistributedPer100K = vsd.DistributedPer100K;
            AdministeredPer100K = vsd.AdministeredPer100K;
            PercentAdultVaccinated = 0;
            PercentSeniorsVaccinated = 0;
            if(vsd.Dose1Admin18PlusPct.HasValue)
                PercentAdultVaccinated = vsd.Dose1Admin18PlusPct.Value;
            if(vsd.Dose1Admin65PlusPct.HasValue)
                PercentSeniorsVaccinated = vsd.Dose1Admin65PlusPct.Value; 
        }
    }
}
