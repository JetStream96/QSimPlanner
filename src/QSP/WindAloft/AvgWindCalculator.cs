using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Integration;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static System.Math;
using static QSP.MathTools.Doubles;

namespace QSP.WindAloft
{
    public class AvgWindCalculator
    {
        public double Ktas { get; private set; }
        public double AltitudeFt { get; private set; }

        private IWindTableCollection windData;
        private Vector3D v1;
        private Vector3D v2;
        private double lat1;
        private double lon1;
        private double lat2;
        private double lon2;

        public AvgWindCalculator(
            IWindTableCollection windData,
            double Ktas,
            double AltitudeFt)
        {
            this.windData = windData;
            this.Ktas = Ktas;
            this.AltitudeFt = AltitudeFt;

            SetPoint1(0.0, 0.0);
            SetPoint2(0.0, 0.0);
        }

        private void SetPoint1(double lat, double lon)
        {
            lat1 = lat;
            lon1 = lon;
            v1 = LatLonToVector3D(lat1, lon1);
        }

        private void SetPoint2(double lat, double lon)
        {
            lat2 = lat;
            lon2 = lon;
            v2 = LatLonToVector3D(lat2, lon2);
        }

        // delta: in degrees
        public double GetAirDistance(
            ICoordinate point1,
            ICoordinate point2,
            double delta = 1.0)
        {
            if (point1.LatLonEquals(point2, 1E-5)) return 0.0;

            SetPoint1(point1.Lat, point1.Lon);
            SetPoint2(point2.Lat, point2.Lon);

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

        /// <summary>
        /// Given different v1 and v2, which are both unit vectors on 
        /// sphere, we can get a great circle path from v1 to v2 (choose the
        /// shortest great circle path). We walk the path by angle alpha from 
        /// v1 towards v2. This returns the point we end up with.
        /// </summary>
        private Vector3D GetV(Vector3D v1, Vector3D v2, double alpha)
        {
            double t = v1.Dot(v2);
            var M = new Matrix2by2(1.0, t, t, 1.0);
            double beta = SafeAcos(t);

            var b = new Vector2D(Cos(alpha), Cos(beta - alpha));
            var a = M.Inverse().Multiply(b);

            return v1 * a.X + v2 * a.Y;
        }

        private double GetGS(Vector3D v)
        {
            var latLon = v.ToLatLon();
            Vector3D wind = GetWind(latLon.Lat, latLon.Lon);
            double windSpd = wind.R;
            var w = GetW(v, v1, v2);

            var innerProduct = windSpd == 0.0
                ? 0.0
                : wind.Normalize().Dot(w);

            double a = 1.0;
            double b = -2.0 * windSpd * innerProduct;
            double c = windSpd * windSpd - Ktas * Ktas;
            return (-b + Sqrt(b * b - 4.0 * a * c)) / (2.0 * a);
        }
        
        private Vector3D GetW(Vector3D v, Vector3D v1, Vector3D v2)
        {
            var v3 = v1.Cross(v2);
            v3 = v3.Normalize();
            return v3.Cross(v);
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
