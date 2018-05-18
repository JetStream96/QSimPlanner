using static QSP.MathTools.Numbers;

namespace QSP.AviationTools.Coordinates
{
    public static class FormatDegreeMinute
    {
        // E.g. 25.073133333 -> N25° 4.39'
        public static string LatToString(double lat, string format = "F2")
        {
            string result = ToString(lat, format);
            string ns = lat < 0.0 ? "S" : "N";
            return ns + result;
        }

        // E.g. 25.073133333, 121.216183333 ->
        // N25° 4.39', E121° 12.97'
        public static string LonToString(double lon, string format = "F2")
        {
            string result = ToString(lon, format);
            string ew = lon < 0.0 ? "W" : "E";
            return ew + result;
        }

        // E.g. 25.073133333 -> 25° 4.39'
        public static string ToString(double d, string format = "F2")
        {
            if (d < 0.0) d = -d;
            int degree = FloorInt(d);
            double minute = (d - degree) * 60.0;
            string minuteStr = minute.ToString(format);

            return $"{degree}° {minuteStr}'";
        }
    }
}
