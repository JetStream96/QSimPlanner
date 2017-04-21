using QSP.RouteFinding.Data.Interfaces;
using static System.Math;

namespace QSP.FuelCalculation.Calculations
{
    // Note: Add new class for more sophisticated implementation.
    // For metric system, i.e. China, it will be complicated.
    // It seems that the shapefile for airspaces is not publically available*. Therefore 
    // it may be impossible to detect the correct flight level system (meter vs ft) without manually 
    // define the boundaries of countries' airspaces.
    // 
    // * see https://aviation.stackexchange.com/questions/35452/where-to-get-spatial-data-for-atc-airspace
    public class BasicCrzAltProvider : ICrzAltProvider
    {
        // Westbound altitudes: 2000 4000 ... 40000 43000 47000
        // Eastbound altitudes: 3000 5000 ... 41000 45000 49000

        public double ClosestAlt(ICoordinate c, double heading, double alt)
        {
            if (0.0 <= heading && heading < 180.0)
            {
                if (alt <= 40000.0) return Round(alt / 2000.0) * 2000.0;
                if (alt >= 43000.0) return Round(alt / 4000.0) * 4000.0;
                return alt >= 41500.0 ? 43000.0 : 40000.0;
            }
            else
            {
                if (alt <= 41000.0)
                {
                    return Round((alt - 1000.0) / 2000.0) * 2000.0 + 1000.0;
                }

                return Round((alt - 1000.0) / 4000.0) * 4000.0 + 1000.0;
            }
        }

        public double ClosestAltBelow(ICoordinate c, double heading, double alt)
        {
            if (0.0 <= heading && heading < 180.0)
            {
                if (alt <= 40000.0) return FloorExclusive(alt / 2000.0) * 2000.0;
                if (alt >= 43000.0) return FloorExclusive(alt / 4000.0) * 4000.0;                  
                return 40000.0;
            }
            else
            {
                if (alt <= 41000.0)
                {
                    return FloorExclusive((alt - 1000.0) / 2000.0) * 2000.0 + 1000.0;
                }

                return FloorExclusive((alt - 1000.0) / 4000.0) * 4000.0 + 1000.0;
            }
        }

        private static double FloorExclusive(double d)
        {
            var result = Floor(d);
            if (result == d) result--;
            return result;
        }

        public bool IsValidCrzAlt(ICoordinate c, double heading, double altitude)
        {
            return Abs(ClosestAlt(c, heading, altitude) - altitude) < 1.0;
        }
    }
}
