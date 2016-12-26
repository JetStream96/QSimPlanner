using QSP.AviationTools.Coordinates;
using System.Text.RegularExpressions;

namespace QSP.RouteFinding.Tracks.Nats.Utilities
{
    public static class LatLonConverter
    {
        private static double? TryGetNumber(string s)
        {
            int i;
            double d;
            if (s.Length == 2 && int.TryParse(s, out i)) return i;

            if (s.Length > 2 &&
                int.TryParse(s.Substring(0, 2), out i) &&
                double.TryParse(s.Substring(2), out d))
            {
                return i + d / 60.0;
            }

            return null;
        }

        /// <summary>
        /// Sample input : "54/20", "5530/20", "5530/2050". 
        /// If the input is not the correct format, returns null.
        /// </summary>  
        public static LatLon TryConvertNatsCoordinate(string s)
        {
            var pattern = @"^([\d\.]+)/([\d\.]+)$";
            var match = Regex.Match(s, pattern);
            if (!match.Success) return null;
            double? lat = TryGetNumber(match.Groups[1].Value);
            double? lon = -TryGetNumber(match.Groups[2].Value);

            if (lat != null && lon != null &&
                0.0 <= lat && lat <= 90.0 &&
                -180.0 <= lon && lon <= 0.0)
            {
                return new LatLon(lat.Value, lon.Value);
            }

            return null;
        }

        public static string AutoChooseFormat(this LatLon item)
        {
            string result = Format5Letter.To5LetterFormat(item.Lat, item.Lon);
            return result ?? FormatDecimal.ToDecimalFormat(item.Lat, item.Lon);
        }
    }
}
