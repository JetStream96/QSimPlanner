using System;
using QSP.AviationTools;

namespace QSP.MathTools
{
    public static class Utilities
    {
        private static double PIOver180 = Math.PI / 180.0;
        private static double _180OverPI = 180.0 / Math.PI;

        public static double ToRadian(double t)
        {
            return t * PIOver180;
        }

        public static double ToDegree(double t)
        {
            return t * _180OverPI;
        }

        public static Vector3D LatLonToVector3D(double lat, double lon)
        {
            return Vector3D.GetFromSphericalCoords(1.0, Math.PI / 2 - ToRadian(lat), ToRadian(lon));
        }

        public static Vector3D LatLonToVector3D(LatLon latLon)
        {
            return LatLonToVector3D(latLon.Lat, latLon.Lon);
        }

        public static double GreatCircleDistance(LatLon latLon1, LatLon latLon2)
        {
            return GreatCircleDistance(latLon1.Lat, latLon1.Lon, latLon2.Lat, latLon2.Lon);
        }

        public static double GreatCircleDistance(Tuple<double, double> latLon1, Tuple<double, double> latLon2)
        {
            return GreatCircleDistance(latLon1.Item1, latLon1.Item2, latLon2.Item1, latLon2.Item2);
        }

        public static double GreatCircleDistance(double lat1, double lon1, double lat2, double lon2)
        {
            return GreatCircleDistance(lat1, lat2, lon1 - lon2);
        }

        public static double GreatCircleDistance(double lat1, double lat2, double deltaLon)
        {
            if (Math.Abs(deltaLon) < 1E-08 && Math.Abs(lat1 - lat2) < 1E-08)
            {
                return 0.0;
            }

            double lat1Rad = ToRadian(lat1);
            double lat2Rad = ToRadian(lat2);
            double deltaLonRad = ToRadian(deltaLon);

            double a = Math.Sin(lat1Rad) * Math.Sin(lat2Rad) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLonRad);

            return AviationConstants.RADIUS_EARTH_NM * Math.Acos(a);
        }

        public static int Mod(this int item, int x)
        {
            return (item % x + x) % x;
        }

        public static double Mod(this double item, int x)
        {
            return (item % x + x) % x;
        }

        public static double Mod(this double item, double x)
        {
            return (item % x + x) % x;
        }

    }
}