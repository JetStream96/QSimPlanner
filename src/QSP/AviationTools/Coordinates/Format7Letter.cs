using System.Text.RegularExpressions;
using static QSP.MathTools.Numbers;

namespace QSP.AviationTools.Coordinates
{
    public static class Format7Letter
    {
        /// <summary>
        /// Output examples: 36N170W 34N080E
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public static string ToString(double Lat, double Lon)
        {
            if (IsInteger(Lat, Constants.LatLonTolerance) &&
                IsInteger(Lon, Constants.LatLonTolerance))
            {
                return ToString(RoundToInt(Lat), RoundToInt(Lon));
            }

            return null;
        }

        public static string ToString(int lat, int lon)
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

            return lat.ToString().PadLeft(2, '0') + NS +
                lon.ToString().PadLeft(3, '0') + EW;
        }

        /// <summary>
        /// Returns null if the format is incorrect.
        /// </summary>
        public static LatLon Parse(string item)
        {
            var pattern = @"(\d\d)([NS])(\d\d\d)([EW])";
            var match = Regex.Match(item, pattern);

            if (match.Success &&
                int.TryParse(match.Groups[1].Value, out int lat) &&
                int.TryParse(match.Groups[3].Value, out int lon) &&
                0 <= lat && lat <= 90 && 0 <= lon && lon <= 180)
            {
                if (match.Groups[2].Value == "S") lat = -lat;
                if (match.Groups[4].Value == "W") lon = -lon;
                return new LatLon(lat, lon);
            }

            return null;
        }        
    }
}
