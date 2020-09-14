using CovidSharp.Constants;
using CovidSharp.CovidTrack.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.Models
{
    public class StateBase
    {
        public StateCode Code;
        public StateName Name;
        public int Population;

        public StateBase(StateName name, StateCode code, int statePop)
        {
            Code = code;
            Name = name;
            Population = statePop;
        }

    }

    public class State
    {
        public StateBase StateBase { get; set; }
        public List<StateDay> CovidData { get; set; }
        public List<BaseCovidDay> BaseCovidData { get; set; }

        public State(StateBase sBase)
        {
            StateBase = sBase;
        }
    }

   
}
