using System;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Calculations
{
    public class CrzAltProvider : ICrzAltProvider
    {
        public double ClosestAltitudeFtTo(ICoordinate current,
            ICoordinate next, double altitude)
        {
            // TODO: Westbound: Even altitudes
            // Eastbound: Odd altitudes
            // For metric system, i.e. China, it will be complicated.
            return Math.Round(altitude / 1000.0) * 1000.0;
        }

        public double ClosestAltitudeFtFrom(ICoordinate previous,
            ICoordinate current, double altitude)
        {
            return Math.Round(altitude / 1000.0) * 1000.0;
        }
    }
}
