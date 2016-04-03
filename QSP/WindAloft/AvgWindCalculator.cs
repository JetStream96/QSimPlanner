using QSP.AviationTools.Coordinates;
using QSP.MathTools;
using System;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Utilities;
using static QSP.MathTools.Angles;

namespace QSP.WindAloft
{

    public class AvgWindCalculator
    {
        private Vector3D v1 = new Vector3D();
        private Vector3D v2 = new Vector3D();
        private WxFileLoader windData;
        private double FL;
        private int tas;
        private double lat1;
        private double lon1;
        private double lat2;
        private double lon2;

        public AvgWindCalculator(WxFileLoader item, int trueAirspd, double flightLevel)
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
            //lat/lon 1: "from" point
            //lat/lon 2: "to" point
            //seperation: calculate a point every () degrees
            //airspd: (of the aircraft)

            double delta_alpha = ToRadian(seperation);
            double T = 0.0;
            //total time required
            double r = 0.0;
            //total distance

            r = EarthRadiusNm * Math.Acos(v1.InnerProductWith(v2));

            var g = new RealValuedFunction(GetOneOverGS);

            T = g.Integrate(0, r, delta_alpha * EarthRadiusNm);
            return r / T - tas;
        }

        private double GetOneOverGS(double r)
        {
            Vector3D v = Get_v(v1, v2, r / EarthRadiusNm);
            return 1.0 / GetGS(v);
        }

        private Vector3D Get_v(Vector3D v1, Vector3D v2, double alpha)
        {
            Matrix2by2 A = new Matrix2by2();
            A.a11 = 1.0;
            A.a22 = 1.0;
            A.a12 = v1.InnerProductWith(v2);
            A.a21 = v1.InnerProductWith(v2);

            double beta = Math.Acos(v1.InnerProductWith(v2));
            var b1_b2 = new Vector2D(Math.Cos(alpha), Math.Cos(beta - alpha));
            var a1_a2 = A.InverseMatrix().MultiplyVector2D(b1_b2);

            return v1 * a1_a2.x + v2 * a1_a2.y;
        }

        private double GetGS(Vector3D v)
        {
            double lat = ToDegree(v.phi);
            double lon = SetAngleLon(ToDegree(v.theta));
            Vector3D V_wind = GetWind(lat, lon);

            Vector3D w = Get_w(v, v1, v2);
            double gamma = Math.Acos((V_wind.Normalize()).InnerProductWith(w));
            double a = 1;
            double b = -2 * V_wind.r * Math.Cos(gamma);
            double c = V_wind.r * V_wind.r - tas * tas;
            return (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        }

        private double SetAngleLon(double a)
        {
            return (a + 180).Mod(360) - 180;
        }

        private Vector3D Get_w(Vector3D v, Vector3D v1, Vector3D v2)
        {
            Vector3D v3 = v1.CrossProductWith(v2);
            v3 = v3.Normalize();
            return v3.CrossProductWith(v);
        }

        private Vector3D GetWind(double lat, double lon)
        {
            //Given u-comp, V_u, and v-comp, V_v, then we have
            //V_wind=V_u(-sin(theta),cos(theta),0)+V_v(-sin(phi)cos(theta),-sin(phi)sin(theta),cos(phi))
            //lat=phi, lon=theta
            //u=lon,v=lat

            Vector3D WindVec = new Vector3D();

            Tuple<double, double> t = windData.GetWindUV(lat, lon, FL);

            double u_comp = t.Item1;
            double v_comp = t.Item2;

            lat = ToRadian(lat);
            lon = ToRadian(lon);

            double sinLat = Math.Sin(lat);
            double sinLon = Math.Sin(lon);
            double cosLon = Math.Cos(lon);

            var u1 = new Vector3D(-sinLon, cosLon, 0.0);
            var u2 = new Vector3D(-sinLat * cosLon, -sinLat * sinLon, Math.Cos(lat));

            return u1 * u_comp + u2 * v_comp;
        }
    }
}
