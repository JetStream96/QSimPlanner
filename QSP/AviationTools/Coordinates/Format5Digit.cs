using System;
using QSP.Utilities;
using System.Linq;
using static QSP.AviationTools.Coordinates.Utilities;

namespace QSP.AviationTools.Coordinates
{
    public static class Format5Digit
    {
        private static char[] NSEW = new char[] { 'N', 'S', 'E', 'W' };

        /// <summary>
        /// Output examples: 36N70, 3480E.
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public static string To5DigitFormat(double Lat, double Lon)
        {
            int latInt = 0;
            int lonInt = 0;

            if (tryConvertInt(Lat, ref latInt) == false || tryConvertInt(Lon, ref lonInt) == false)
            {
                return null;
            }
            return To5DigitFormat(latInt, lonInt);
        }

        private static char selectChar(int lat, int lon)
        {
            if (lat >= 0)
            {
                if (lon >= 0)
                {
                    return 'E';
                }
                return 'N';
            }
            else
            {
                if (lon >= 0)
                {
                    return 'S';
                }
                return 'W';
            }
        }

        public static string To5DigitFormat(int lat, int lon)
        {
            char c = selectChar(lat, lon);

            lat = Math.Abs(lat);
            lon = Math.Abs(lon);

            if (lon < 100)
            {
                return lat.ToString() + lon.ToString() + c;
            }
            else
            {
                return lat.ToString() + c + (lon - 100).ToString();
            }
        }

        private static int AlphabetPosition(string s)
        {
            if (NSEW.Contains(s[2]))
            {
                return 2;
            }
            return 4;
        }

        public static bool TryReadFrom5DigitFormat(string item, out LatLon result)
        {
            try
            {
                result = ReadFrom5DigitFormat(item);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static LatLon ReadFrom5DigitFormat(string item)
        {
            ConditionChecker.Ensure(item.Length == 5);

            int pos = AlphabetPosition(item);

            int lat = Convert.ToInt32(item.Substring(0, 2));
            int lon = pos == 2
                      ? Convert.ToInt32(item.Substring(3, 2)) + 100
                      : Convert.ToInt32(item.Substring(2, 2));

            ConditionChecker.Ensure(lat >= 0 && lat <= 90);
            ConditionChecker.Ensure(lon >= 0 && lon <= 180);

            switch (item[pos])
            {
                case 'E':
                    return new LatLon(lat, lon);

                case 'S':
                    return new LatLon(-lat, lon);

                case 'W':
                    return new LatLon(-lat, -lon);

                case 'N':
                    return new LatLon(lat, -lon);

                default:
                    throw new ArgumentException();
            }
        }
    }
}