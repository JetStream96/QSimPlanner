using QSP.Utilities;
using System;

namespace QSP.AviationTools.Coordinates
{
    // e.g. N32.665W122.1265
    public static class FormatDecimal
    {
        public static string ToDecimalFormat(double Lat, double Lon)
        {
            char NS;
            char EW;

            if (Lat < 0)
            {
                NS = 'S';
                Lat = -Lat;
            }
            else
            {
                NS = 'N';
            }

            if (Lon < 0)
            {
                EW = 'W';
                Lon = -Lon;
            }
            else
            {
                EW = 'E';
            }
            return NS + string.Format("{0:0.000000}", Lat) + EW + 
                string.Format("{0:0.000000}", Lon);
        }

        public static bool TryReadFromDecimalFormat(
            string item, out LatLon result)
        {
            try
            {
                result = ReadFromDecimalFormat(item);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static LatLon ReadFromDecimalFormat(string item)
        {
            char NS = item[0];
            int x = item.IndexOfAny(new char[] { 'E', 'W' });
            char EW = item[x];

            double lat = Convert.ToDouble(item.Substring(1, x - 1));
            double lon = Convert.ToDouble(item.Substring(x + 1));

            ExceptionHelpers.Ensure<ArgumentException>(
                0.0 <= lat && lat <= 90.0 && 0.0 <= lon && lon <= 180.0);

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
