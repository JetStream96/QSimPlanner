using System;

namespace QSP.AviationTools.Coordinates
{
    public static class Utilities
    {
        public static bool tryConvertInt(double d, ref int result)
        {
            double x = Math.Round(d);

            if (Math.Abs(x - d) < Constants.LatLon_TOLERENCE)
            {
                result = (int)x;
                return true;
            }
            return false;
        }
    }
}
