using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Calculations
{
    public class CrzAltProvider
    {
        public double ClosestAltitudeFtTo(ICoordinate current,
            ICoordinate next, double altitudeFt)
        {
            // TODO: Westbound: Even altitudes
            // Eastbound: Odd altitudes
            // For metric system, i.e. China, it will be complicated.
            return altitudeFt;
        }

        public double ClosestAltitudeFtFrom(ICoordinate previous,
            ICoordinate current, double altitudeFt)
        {
            return altitudeFt;
        }
    }
}
