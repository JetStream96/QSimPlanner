using System;
using System.Linq;
using static QSP.MathTools.Numbers;

namespace QSP.AviationTools.Coordinates
{
    public static class Format5Letter
    {
        private static char[] NSEW = { 'N', 'S', 'E', 'W' };

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

        private static char SelectChar(int lat, int lon)
        {
            if (lat >= 0)
            {
                return lon >= 0 ? 'E' : 'N';
            }
            else
            {
                return lon >= 0 ? 'S' : 'W';
            }
        }

        public static string To5LetterFormat(int lat, int lon)
        {
            char c = SelectChar(lat, lon);

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

        private static int AlphabetPosition(string s)
        {
            if (NSEW.Contains(s[2])) return 2;
            if (NSEW.Contains(s[4])) return 4;
            return -1;
        }

        /// <summary>
        /// Returns null if the format is incorrect.
        /// </summary>
        public static LatLon Parse(string item)
        {
            if (item.Length != 5) return null;

            int pos = AlphabetPosition(item);
            if (pos == -1) return null;
            int lat, lon;

            if (!int.TryParse(item.Substring(0, 2), out lat)) return null;

            if (pos == 2)
            {
                if (!int.TryParse(item.Substring(3, 2), out lon)) return null;
                lon += 100;
            }
            else
            {
                if (!int.TryParse(item.Substring(2, 2), out lon)) return null;
            }

            if (lat < 0 || lat > 90 || lon < 0 || lon > 180) return null;

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
                    return null;
            }
        }
    }
}