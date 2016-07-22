using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using System;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Integration;
using static QSP.MathTools.Vectors.Vector3DExtension;

namespace QSP.WindAloft
{
    public class AvgWindCalculator
    {
        public double TrueAirspeed { get; set; }
        public double AltitudeFt { get; set; }

        private IWindTableCollection windData;
        private Vector3D v1;
        private Vector3D v2;
        private double lat1;
        private double lon1;
        private double lat2;
        private double lon2;

        public AvgWindCalculator(
            IWindTableCollection windData, 
            double trueAirspeed, 
            double altitudeFt)
        {
            this.windData = windData;
            this.TrueAirspeed = trueAirspeed;
            this.AltitudeFt = altitudeFt;

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

        public CalcResult GetAvgWind(
            ICoordinate point1,
            ICoordinate point2,
            double delta = 1.0)
        {
            // from point1 to point2
            // delta: in degrees
            // airspd: (of the aircraft)

            SetPoint1(point1.Lat, point1.Lon);
            SetPoint2(point2.Lat, point2.Lon);

            double deltaAlpha = ToRadian(delta);

            //total distance
            double r = EarthRadiusNm * Math.Acos(v1.Dot(v2));

            // Total time required
            double time = Integrate(
                GetOneOverGS, 0.0, r, deltaAlpha * EarthRadiusNm);

            return new CalcResult()
            {
                AvgTailWind = r / time - TrueAirspeed,
                AirDis = time * TrueAirspeed
            };
        }

        public struct CalcResult
        {
            public double AvgTailWind; public double AirDis;
        }

        private double GetOneOverGS(double r)
        {
            var v = GetV(v1, v2, r / EarthRadiusNm);
            return 1.0 / GetGS(v);
        }

        private Vector3D GetV(Vector3D v1, Vector3D v2, double alpha)
        {
            double t = v1.Dot(v2);
            var M = new Matrix2by2(1.0, t, t, 1.0);
            double beta = Math.Acos(t);

            var b = new Vector2D(Math.Cos(alpha), Math.Cos(beta - alpha));
            var a = M.Inverse().Multiply(b);

            return v1 * a.X + v2 * a.Y;
        }

        private double GetGS(Vector3D v)
        {
            double lat = ToDegree(0.5 * Math.PI - v.Phi);
            double lon = SetAngleLon(ToDegree(v.Theta));
            Vector3D VWind = GetWind(lat, lon);

            var w = GetW(v, v1, v2);
            double gamma = Math.Acos((VWind.Normalize()).Dot(w));
            double a = 1;
            double b = -2 * VWind.R * Math.Cos(gamma);
            double c = VWind.R * VWind.R - TrueAirspeed * TrueAirspeed;
            return (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        }

        private double SetAngleLon(double a)
        {
            return (a + 180).Mod(360) - 180;
        }

        private Vector3D GetW(Vector3D v, Vector3D v1, Vector3D v2)
        {
            var v3 = v1.Cross(v2);
            v3 = v3.Normalize();
            return v3.Cross(v);
        }

        private Vector3D GetWind(double lat, double lon)
        {
            // Given u-comp, V_u, and v-comp, V_v, then we have
            // V_wind = V_u(-sin(theta),cos(theta),0) +
            //          V_v(-sin(phi)cos(theta),-sin(phi)sin(theta),cos(phi))
            // 
            // lat=phi, lon=theta
            // u=lon, v=lat

            var w = windData.GetWindUV(lat, lon, AltitudeFt);

            lat = ToRadian(lat);
            lon = ToRadian(lon);

            double sinLat = Math.Sin(lat);
            double sinLon = Math.Sin(lon);
            double cosLon = Math.Cos(lon);

            var u1 = new Vector3D(-sinLon, cosLon, 0.0);
            var u2 = new Vector3D(
                -sinLat * cosLon, -sinLat * sinLon, Math.Cos(lat));

            return u1 * w.UComp + u2 * w.VComp;
        }
    }
}
