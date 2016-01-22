using System;
using static QSP.AviationTools.Coordinates.Utilities;
using QSP.Utilities;

namespace QSP.AviationTools.Coordinates
{
    public static class Conversion7Digit
    {
        /// <summary>
        /// Output examples: 36N170W 34N080E
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public static string To7DigitFormat(double Lat, double Lon)
        {
            int latInt = 0;
            int lonInt = 0;

            if (tryConvertInt(Lat, ref latInt) == false || tryConvertInt(Lon, ref lonInt) == false)
            {
                return null;
            }
            return To7DigitFormat(latInt, lonInt);
        }

        public static string To7DigitFormat(int lat, int lon)
        {
            char NS;

            if (lat < 0)
            {
                lat = -lat;
                NS = 'S';
            }
            else
            {
                NS = 'N';
            }

            char EW;

            if (lon < 0)
            {
                lon = -lon;
                EW = 'W';
            }
            else
            {
                EW = 'E';
            }

            return lat.ToString().PadLeft(2, '0') + NS + lon.ToString().PadLeft(3, '0') + EW;
        }

        public static bool Is7DigitFormat(string item)
        {
            if (item != null &&
                item.Length == 7 &&
                (item[2] == 'N' || item[2] == 'S') &&
                (item[6] == 'E' || item[6] == 'W'))
            {
                int lat;
                int lon;

                return int.TryParse(item.Substring(0, 2), out lat) &&
                       int.TryParse(item.Substring(3, 3), out lon) &&
                       lat >= 0 &&
                       lat <= 90 &&
                       lon >= 0 &&
                       lon <= 180;
            }
            return false;
        }

        public static LatLon ReadFrom7DigitFormat(string item)
        {
            ConditionChecker.Ensure(item.Length == 7);

            char NS = item[2];
            char EW = item[6];

            int lat = Convert.ToInt32(item.Substring(0, 2));
            int lon = Convert.ToInt32(item.Substring(3, 3));

            ConditionChecker.Ensure(lat >= 0 && lat <= 90);
            ConditionChecker.Ensure(lon >= 0 && lon <= 180);

            if (NS == 'N')
            {
                if (EW == 'E')
                {
                    return new LatLon(lat, lon);
                }
                else if (EW == 'W')
                {
                    return new LatLon(lat, -lon);
                }
            }
            else if (NS == 'S')
            {
                if (EW == 'E')
                {
                    return new LatLon(-lat, lon);
                }
                else if (EW == 'W')
                {
                    return new LatLon(-lat, -lon);
                }
            }
            throw new ArgumentException();
        }
    }
}
