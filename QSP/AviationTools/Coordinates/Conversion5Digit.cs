using System;
using static QSP.AviationTools.Coordinates.Utilities;
using System.Linq;

namespace QSP.AviationTools.Coordinates
{
    public static class Conversion5Digit
    {
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
                return lat.ToString() + c + lon.ToString();
            }
        }

        private static int AlphabetPosition(string s)
        {
            if (new char[] { 'N', 'S', 'E', 'W' }.Contains(s[2]))
            {
                return 2;
            }
            return 4;
        }

        public static LatLon ReadFrom5DigitFormat(string item)
        {
            if (item.Length != 5)
            {
                return null;
            }

            int pos = AlphabetPosition(item);

            int lat = Convert.ToInt32(item.Substring(0, 2));
            int lon = pos == 2
                      ? Convert.ToInt32(item.Substring(3, 2)) + 100
                      : Convert.ToInt32(item.Substring(2, 2));


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