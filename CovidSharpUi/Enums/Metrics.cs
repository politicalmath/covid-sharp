using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidSharpUi.Enums
{
    public enum Metrics
    {
        Deaths,
        Cases, 
        Tests, 
        PercentPositive,
        HospitalCurrent,
        HospitalNew, 
        CaseJerk,
        DosesDistributed,
        DosesAdministered,
        Dose1Adults,
        Dose1Seniors,
        Does1AdultPopPct,
        Dose1SeniorPopPct,
        SeriesCompleteAdults,
        SeriesCompleteSeniors,
        SeriesCompleteAdultPct,
        SeriesCompleteSeniorPct
    }
}
