using QSP.AviationTools.Coordinates;
using System;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.Coordinates.Constants;
using static QSP.MathTools.Angles;

namespace QSP.MathTools
{
    public static class Utilities
    {      
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
            if (Math.Abs(deltaLon) < LatLon_TOLERENCE && Math.Abs(lat1 - lat2) < LatLon_TOLERENCE)
            {
                return 0.0;
            }

            double lat1Rad = ToRadian(lat1);
            double lat2Rad = ToRadian(lat2);
            double deltaLonRad = ToRadian(deltaLon);

            double a = Math.Sin(lat1Rad) * Math.Sin(lat2Rad) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLonRad);

            return EarthRadiusNm * Math.Acos(a);
        }
    }
}