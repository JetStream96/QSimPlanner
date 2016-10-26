using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Integration;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static System.Math;
using static QSP.MathTools.Doubles;
using static QSP.MathTools.EarthGeometry;

namespace QSP.WindAloft
{
    public class AvgWindCalculator
    {
        private static readonly Vector3D DefaultLocation =
            LatLonToVector3D(0.0, 0.0);

        public double Ktas { get; private set; }
        public double AltitudeFt { get; private set; }

        private IWindTableCollection windData;
        private Vector3D v1;
        private Vector3D v2;

        public AvgWindCalculator(
            IWindTableCollection windData,
            double Ktas,
            double AltitudeFt)
        {
            this.windData = windData;
            this.Ktas = Ktas;
            this.AltitudeFt = AltitudeFt;

            v1 = DefaultLocation;
            v2 = DefaultLocation;
        }

        // delta: in degrees
        public double GetAirDistance(
            ICoordinate point1,
            ICoordinate point2,
            double delta = 1.0)
        {
            if (point1.LatLonEquals(point2, 1E-5)) return 0.0;

            v1 = point1.ToVector3D();
            v2 = point2.ToVector3D();

            double deltaAlpha = ToRadian(delta);

            // Total distance
            double r = EarthRadiusNm * SafeAcos(v1.Dot(v2));

            // Total time required
            double time = Integrate(
                GetOneOverGS, 0.0, r, deltaAlpha * EarthRadiusNm);

            return time * Ktas;
        }

        // Returns 1.0/(ground speed in knots).
        private double GetOneOverGS(double r)
        {
            var v = GetV(v1, v2, r / EarthRadiusNm);
            return 1.0 / GetGS(v);
        }

        private double GetGS(Vector3D v)
        {
            var latLon = v.ToLatLon();
            Vector3D wind = GetWind(latLon.Lat, latLon.Lon);
            double windSpd = wind.R;
            var w = v.Equals(v2) ? -GetW(v, v1) : GetW(v, v2);

            var innerProduct = windSpd == 0.0
                ? 0.0
                : wind.Normalize().Dot(w);

            double a = 1.0;
            double b = -2.0 * windSpd * innerProduct;
            double c = windSpd * windSpd - Ktas * Ktas;
            return (-b + Sqrt(b * b - 4.0 * a * c)) / (2.0 * a);
        }

        private Vector3D GetWind(double lat, double lon)
        {
            var w = windData.GetWindUV(lat, lon, AltitudeFt);

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
