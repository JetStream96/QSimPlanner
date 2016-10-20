using QSP.Utilities;
using System;
using System.Text.RegularExpressions;

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
        
        public static LatLon Parse(string item)
        {
            try
            {
                return InternalParse(item);
            }
            catch
            {
                return null;
            }
        }

        private static LatLon InternalParse(string item)
        {
            var pattern = @"^([NS])(.+?)([EW])(.+)$";
            var match = Regex.Match(item, pattern);
            double lat = double.Parse(match.Groups[2].Value);
            double lon = double.Parse(match.Groups[4].Value);

            ExceptionHelpers.Ensure<ArgumentException>(
                0.0 <= lat && lat <= 90.0 && 0.0 <= lon && lon <= 180.0);

            if (match.Groups[1].Value == "S") lat = -lat;
            if (match.Groups[3].Value == "W") lon = -lon;

            return new LatLon(lat, lon);
        }
    }
}
