using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using static System.Math;

namespace QSP.FuelCalculation.Calculations
{
    // TODO: This implementation is temporary.
    public class BasicCrzAltProvider : ICrzAltProvider
    {
        public double ClosestAlt(
            ICoordinate c, double heading, double altitude)
        {
            // TODO: Westbound: Even altitudes
            // Eastbound: Odd altitudes
            // For metric system, i.e. China, it will be complicated.

            bool isWestBound = 0.0 <= heading && heading < 180.0;
            var sep = Seperation(altitude);
            var altBorder = isWestBound ? 41500.0 : 41000.0;
            double adjustment;

            if (altitude <= altBorder)
            {
                adjustment = isWestBound ? 0.0 : 0.5;
            }
            else
            {
                adjustment = isWestBound ? 0.75 : 0.25;
            }

            return (Round(altitude / sep - adjustment) + adjustment) * sep;
        }

        public double ClosestAltBelow(
            ICoordinate c, double heading, double altitude)
        {
            // TODO: Wrong.
            return ClosestAlt(c, heading, altitude) - Seperation(altitude);
        }

        public bool IsValidCrzAlt(
            ICoordinate c, double heading, double altitude)
        {
            return Abs(ClosestAlt(c, heading, altitude) - altitude) < 1.0;
        }

        public double Seperation(double alt)
        {
            return alt <= 41000.0 ? 2000.0 : 4000.0;
        }
    }
}
