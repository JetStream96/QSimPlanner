using QSP.Utilities;
using System;
using System.Linq;
using static QSP.MathTools.Doubles;

namespace QSP.AviationTools.Coordinates
{
    public static class Format5Letter
    {
        private static char[] NSEW = new char[] { 'N', 'S', 'E', 'W' };

        /// <summary>
        /// Output examples: 36N70, 3480E.
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public static string To5LetterFormat(double Lat, double Lon)
        {
            if (IsInteger(Lat, Constants.LatLonTolerance) &&
                IsInteger(Lon, Constants.LatLonTolerance))
            {
                return To5LetterFormat(RoundToInt(Lat), RoundToInt(Lon));
            }

            return null;
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

        public static string To5LetterFormat(int lat, int lon)
        {
            char c = selectChar(lat, lon);

            lat = Math.Abs(lat);
            lon = Math.Abs(lon);

            if (lon < 100)
            {
                return lat.ToString().PadLeft(2, '0') +
                    lon.ToString().PadLeft(2, '0') + c;
            }
            else
            {
                return lat.ToString().PadLeft(2, '0') + c +
                    (lon - 100).ToString().PadLeft(2, '0');
            }
        }

        private static int alphabetPosition(string s)
        {
            if (NSEW.Contains(s[2]))
            {
                return 2;
            }
            else if (NSEW.Contains(s[4]))
            {
                return 4;
            }

            throw new ArgumentException();
        }

        public static bool TryReadFrom5LetterFormat(
            string item, out LatLon result)
        {
            try
            {
                result = ReadFrom5LetterFormat(item);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static LatLon ReadFrom5LetterFormat(string item)
        {
            ConditionChecker.Ensure<ArgumentException>(item.Length == 5);

            int pos = alphabetPosition(item);

            int lat = int.Parse(item.Substring(0, 2));
            int lon = pos == 2
                      ? int.Parse(item.Substring(3, 2)) + 100
                      : int.Parse(item.Substring(2, 2));

            ConditionChecker.Ensure<ArgumentException>(0 <= lat && lat <= 90);
            ConditionChecker.Ensure<ArgumentException>(0 <= lon && lon <= 180);

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