using QSP.AviationTools.Coordinates;
using QSP.MathTools;
using System;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static QSP.MathTools.Integration;

namespace QSP.WindAloft
{
    public class AvgWindCalculator
    {
        private Vector3D v1;
        private Vector3D v2;
        private WxFileLoader windData;
        private double FL;
        private int tas;
        private double lat1;
        private double lon1;
        private double lat2;
        private double lon2;

        public AvgWindCalculator(
            WxFileLoader item, int trueAirspd, double flightLevel)
        {
            windData = item;
            tas = trueAirspd;
            FL = flightLevel;

            SetPoint1(0.0, 0.0);
            SetPoint2(0.0, 0.0);
        }

        public void SetPoint1(double lat, double lon)
        {
            lat1 = lat;
            lon1 = lon;
            setV1();
        }

        public void SetPoint1(LatLon item)
        {
            SetPoint1(item.Lat, item.Lon);
        }

        public void SetPoint2(double lat, double lon)
        {
            lat2 = lat;
            lon2 = lon;
            setV2();
        }

        public void SetPoint2(LatLon item)
        {
            SetPoint2(item.Lat, item.Lon);
        }

        private void setV1()
        {
            v1 = LatLonToVector3D(lat1, lon1);
        }

        private void setV2()
        {
            v2 = LatLonToVector3D(lat2, lon2);
        }

        public double GetAvgWind(double seperation)
        {
            // lat/lon 1: "from" point
            // lat/lon 2: "to" point
            // seperation: calculate a point every () degrees
            // airspd: (of the aircraft)

            double deltaAlpha = ToRadian(seperation);
            double T = 0.0;            //total time required
            double r = 0.0;            //total distance

            r = EarthRadiusNm * Math.Acos(v1.Dot(v2));            
            T = Integrate(GetOneOverGS, 0.0, r, deltaAlpha * EarthRadiusNm);

            return r / T - tas;
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
            double lat = ToDegree(v.Phi);
            double lon = SetAngleLon(ToDegree(v.Theta));
            Vector3D VWind = GetWind(lat, lon);

            var w = GetW(v, v1, v2);
            double gamma = Math.Acos((VWind.Normalize()).Dot(w));
            double a = 1;
            double b = -2 * VWind.R * Math.Cos(gamma);
            double c = VWind.R * VWind.R - tas * tas;
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
            // V_wind=V_u(-sin(theta),cos(theta),0)+V_v(-sin(phi)cos(theta),-sin(phi)sin(theta),cos(phi))
            // lat=phi, lon=theta
            // u=lon,v=lat
            
            var w = windData.GetWindUV(lat, lon, FL);
            
            lat = ToRadian(lat);
            lon = ToRadian(lon);

            double sinLat = Math.Sin(lat);
            double sinLon = Math.Sin(lon);
            double cosLon = Math.Cos(lon);

            var u1 = new Vector3D(-sinLon, cosLon, 0.0);
            var u2 = new Vector3D(-sinLat * cosLon, -sinLat * sinLon, Math.Cos(lat));

            return u1 * w.UComp + u2 * w.VComp;
        }
    }
}
