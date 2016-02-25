using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LandingPerfCalculation.Boeing.PerfData
{
    public enum SurfaceCondition
    {
        Dry = 0,
        Good = 1,
        Medium = 2,
        Poor = 3
    }

    public enum DataColumn
    {
        RefDis = 0,
        WtAdjustAbove = 1,
        WtAdjustBelow = 2,
        AltAdjust = 3,
        AltAdjustHigh = 4,
        HeadwindCorr = 5,
        TailwindCorr = 6,
        DownhillCorr = 7,
        UphillCorr = 8,
        TempAboveISA = 9,
        TempBelowISA = 10,
        AppSpdAdjust = 11,
        OneRev = 12,
        NoRev = 13
    }
}
