using QSP.MathTools.Vectors;
using static QSP.MathTools.Angles;
using static QSP.MathTools.EarthGeometry;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static System.Math;

namespace QSP.WindAloft
{
    public static class GroundSpeedCalculation
    {
        /// <summary>
        /// Gets the ground speed (in knots) at v, where the route is from v1
        /// to v2. The v, v1, and v2 are unit vectors on the sphere. v1 must be
        /// different from v2 but v can be the same as v1 or v2.
        /// </summary>
        public static double GetGS(
            IWindTableCollection windData,
            double altitudeFt,
            double ktas,
            Vector3D v1,
            Vector3D v2,
            Vector3D v)
        {
            var latLon = v.ToLatLon();
            Vector3D wind = GetWind(
                windData, altitudeFt, latLon.Lat, latLon.Lon);
            double windSpd = wind.R;
            var w = v.Equals(v2) ? -GetW(v, v1) : GetW(v, v2);

            var innerProduct = windSpd == 0.0
                ? 0.0
                : wind.Normalize().Dot(w);

            double a = 1.0;
            double b = -2.0 * windSpd * innerProduct;
            double c = windSpd * windSpd - ktas * ktas;
            return (-b + Sqrt(b * b - 4.0 * a * c)) / (2.0 * a);
        }

        /// <summary>
        /// Gets the wind vector in earth's coordinate at the given location. 
        /// </summary>
        public static Vector3D GetWind(
            IWindTableCollection windData,
            double altitudeFt,
            double lat, 
            double lon)
        {
            var w = windData.GetWindUV(lat, lon, altitudeFt);

            lat = ToRadian(lat);
            lon = ToRadian(lon);

            double sinLat = Sin(lat);
            double sinLon = Sin(lon);
            double cosLon = Cos(lon);

            var u1 = new Vector3D(-sinLon, cosLon, 0.0);
            var u2 = new Vector3D(
                -sinLat * cosLon, -sinLat * sinLon, Cos(lat));

            return u1 * w.UComp + u2 * w.VComp;
        }
    }
}
