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
        HalfRev = 12,
        NoRev = 13
    }

    public enum ReverserOption
    {
        Both = 0,

        // One reverser for twin-engined aircraft, 
        // two reversers for four-engined aircraft.
        HalfRev = 1,

        NoRev = 2
    }
}
