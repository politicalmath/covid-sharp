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

        public VaccineDay(VaccineStateDay vsd)
        {
            Date = vsd.DataDate;
            DosesDistributed = vsd.DosesDistributed;
            DosesAdministered = vsd.DosesAdministered;
            DistributedPer100K = vsd.DistributedPer100K;
            AdministeredPer100K = vsd.AdministeredPer100K;
        }
    }
}
