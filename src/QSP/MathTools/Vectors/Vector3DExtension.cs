using QSP.AviationTools.Coordinates;
using System;
using static QSP.MathTools.Angles;

namespace QSP.MathTools.Vectors
{
    public static class Vector3DExtension
    {
        public static Vector3D LatLonToVector3D(double lat, double lon)
        {
            return Vector3D.FromSphericalCoords(
                1.0, Math.PI * 0.5 - ToRadian(lat), ToRadian(lon));
        }

        public static Vector3D ToVector3D(this LatLon latLon)
        {
            return LatLonToVector3D(latLon.Lat, latLon.Lon);
        }
    }
}
