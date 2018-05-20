using static QSP.MathTools.Numbers;

namespace QSP.AviationTools.Coordinates
{
    public static class FormatDegreeMinuteSecond
    {
        // E.g. 25.073133333 -> N25° 4' 23.28"
        public static string LatToString(double lat, string format = "F2")
        {
            string result = ToString(lat, format);
            string ns = lat < 0.0 ? "S" : "N";
            return ns + result;
        }

        // E.g. 25.073133333, 121.216183333 ->
        // N25° 4' 23.28", E121° 12' 58.26"
        public static string LonToString(double lon, string format = "F2")
        {
            string result = ToString(lon, format);
            string ew = lon < 0.0 ? "W" : "E";
            return ew + result;
        }

        // E.g. 25.073133333 -> 25° 4' 23.28"
        public static string ToString(double d, string format = "F2")
        {
            if (d < 0.0) d = -d;
            int degree = FloorInt(d);
            int minute = FloorInt((d - degree) * 60.0);
            double second = (d - degree - minute / 60.0) * 3600.0;
            string secondStr = second.ToString(format);

            // Due to rounding errors, it's possible that second is 60.
            // We handle this condition.
            if (secondStr.Substring(0, 2) == "60")
            {
                secondStr = 0.ToString(format);
                minute += 1;
            }

            if (minute == 60)
            {
                minute = 0;
                degree++;
            }

            return $"{degree}° {minute}' {secondStr}\"";
        }
    }
}
