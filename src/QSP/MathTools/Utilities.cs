using QSP.AviationTools.Coordinates;
using System;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;

namespace QSP.MathTools
{
    public static class Utilities
    {
        public static Vector3D LatLonToVector3D(double lat, double lon)
        {
            return Vector3D.GetFromSphericalCoords(
                1.0, Math.PI * 0.5 - ToRadian(lat), ToRadian(lon));
        }

        public static Vector3D LatLonToVector3D(LatLon latLon)
        {
            return LatLonToVector3D(latLon.Lat, latLon.Lon);
        }

        public static double GreatCircleDistance(
            LatLon latLon1, LatLon latLon2)
        {
            return GreatCircleDistance(
                latLon1.Lat, latLon1.Lon, latLon2.Lat, latLon2.Lon);
        }

        public static double GreatCircleDistance(
            double lat1, double lon1, double lat2, double lon2)
        {
            return GreatCircleDistance(lat1, lat2, lon1 - lon2);
        }

        public static double GreatCircleDistance(
            double lat1, double lat2, double deltaLon)
        {
            double lat1Rad = ToRadian(lat1);
            double lat2Rad = ToRadian(lat2);
            double deltaLonRad = ToRadian(deltaLon);

            double innerProduct =
                Math.Sin(lat1Rad) * Math.Sin(lat2Rad) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLonRad);

            // The inner product may be very close to -1.0 or 1.0,
            // and combined with rounding error, can actually 
            // be smaller than -1.0 or larger than 1.0, causing
            // Acos(innerProduct) to throw exception. 
            if (innerProduct > 1.0)
            {
                innerProduct = 1.0;
            }
            else if (innerProduct < -1.0)
            {
                innerProduct = -1.0;
            }

            return EarthRadiusNm * Math.Acos(innerProduct);
        }
    }
}