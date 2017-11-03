using QSP.AviationTools;

namespace QSP.RouteFinding.Airports
{
    public static class IRwyDataExtension
    {
        public static double LengthMeter(this IRwyData r) =>
            r.ElevationFt * Constants.FtMeterRatio;
    }
}
