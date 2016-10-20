using System.Text.RegularExpressions;
using static QSP.MathTools.Doubles;

namespace QSP.AviationTools.Coordinates
{
    public static class Format7Letter
    {
        /// <summary>
        /// Output examples: 36N170W 34N080E
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public static string To7LetterFormat(double Lat, double Lon)
        {
            if (IsInteger(Lat, Constants.LatLonTolerance) &&
                IsInteger(Lon, Constants.LatLonTolerance))
            {
                return To7LetterFormat(RoundToInt(Lat), RoundToInt(Lon));
            }

            return null;
        }

        public static string To7LetterFormat(int lat, int lon)
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

            int lat, lon;
            if (match.Success &&
                int.TryParse(match.Groups[1].Value, out lat) &&
                int.TryParse(match.Groups[3].Value, out lon) &&
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
