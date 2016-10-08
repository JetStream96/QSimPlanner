using System;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;

namespace QSP.MathTools
{
    public static class GCDis
    {
        /// <summary>
        /// Great circle distance between two points.
        /// </summary>
        public static double Distance(
            double lat1, double lon1, double lat2, double lon2)
        {
            return Distance(lat1, lat2, lon1 - lon2);
        }

        /// <summary>
        /// Great circle distance between two points.
        /// </summary>
        public static double Distance(
            double lat1, double lat2, double deltaLon)
        {
            double lat1Rad = ToRadian(lat1);
            double lat2Rad = ToRadian(lat2);
            double deltaLonRad = ToRadian(deltaLon);

            double sinLat1 = Math.Sin(lat1Rad);
            double cosLat1 = Math.Sqrt(1 - sinLat1 * sinLat1);
            double sinLat2 = Math.Sin(lat2Rad);
            double cosLat2 = Math.Sqrt(1 - sinLat2 * sinLat2);

            double innerProduct = sinLat1 * sinLat2 +
                cosLat1 * cosLat2 * Math.Cos(deltaLonRad);

            // The inner product may be very close to -1.0 or 1.0,
            // and combined with rounding error, can actually 
            // be smaller than -1.0 or larger than 1.0, causing
            // Acos(innerProduct) to throw exception. 

            return EarthRadiusNm * Doubles.SafeAcos(innerProduct);
        }
    }
}