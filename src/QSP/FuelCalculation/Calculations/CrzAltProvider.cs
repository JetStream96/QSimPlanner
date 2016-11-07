using System;
using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Calculations
{
    public class CrzAltProvider : ICrzAltProvider
    {
        public double ClosestAlt(
            ICoordinate c, double heading, double altitude)
        {
            // TODO: Westbound: Even altitudes
            // Eastbound: Odd altitudes
            // For metric system, i.e. China, it will be complicated.
            return Math.Round(altitude / 4000.0) * 4000.0;
        }

        public double ClosestAltBelow(
            ICoordinate c, double heading, double altitude)
        {
            return Math.Floor(altitude / 4000.0) * 4000.0;
        }

        public bool IsValidCrzAlt(
            ICoordinate c, double heading, double altitude)
        {
            return altitude.Mod(4000.0) < 1.0;
        }
    }
}
