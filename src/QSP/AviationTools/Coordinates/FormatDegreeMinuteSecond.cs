using static QSP.MathTools.Numbers;

namespace QSP.AviationTools.Coordinates
{
    public static class FormatDegreeMinuteSecond
    {
        // E.g. 25.073133333 -> N25° 4' 23.28"
        public static string LatToDegreeMinuteSecondFormat(double lat, string format = "F2")
        {
            string result = ToDegreeMinuteSecondFormat(lat, format);
            string ns = lat < 0.0 ? "S" : "N";
            return ns + result;
        }

        // E.g. 25.073133333, 121.216183333 ->
        // N25° 4' 23.28", E121° 12' 58.26"
        public static string LonToDegreeMinuteSecondFormat(double lon, string format = "F2")
        {
            string result = ToDegreeMinuteSecondFormat(lon, format);
            string ew = lon < 0.0 ? "W" : "E";
            return ew + result;
        }

        // E.g. 25.073133333 -> 25° 4' 23.28"
        public static string ToDegreeMinuteSecondFormat(double d, string format = "F2")
        {
            if (d < 0.0) d = -d;
            int degree = FloorInt(d);
            int minute = FloorInt((d - degree) * 60.0);
            double second = (d - degree - minute / 60.0) * 3600.0;
            string secondStr = second.ToString(format);

            return $"{degree}° {minute}' {secondStr}\"";
        }
    }
}
