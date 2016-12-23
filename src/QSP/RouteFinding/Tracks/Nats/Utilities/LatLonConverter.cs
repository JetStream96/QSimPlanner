using QSP.AviationTools.Coordinates;
using System;
using System.Text.RegularExpressions;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.RouteFinding.Tracks.Nats.Utilities
{
    public static class LatLonConverter
    {
        /// <summary>
        /// Sample input : "54/20", "5530/20", "5530/2050". 
        /// If the input is not the correct format, an exception is thrown.
        /// </summary>        
        public static LatLon ConvertNatsCoordinate(string s)
        {
            var pattern = @"^([\d\.]+)/([\d\.]+)$";
            var match = Regex.Match(s, pattern);
            double lat = GetNumber(match.Groups[1].Value);
            double lon = -GetNumber(match.Groups[2].Value);

            Ensure<ArgumentException>(0.0 <= lat && lat <= 90.0);
            Ensure<ArgumentException>(-180.0 <= lon && lon <= 0.0);

            return new LatLon(lat, lon);
        }

        private static double GetNumber(string s)
        {
            if (s.Length == 2) return int.Parse(s);

            return int.Parse(s.Substring(0, 2)) +
                double.Parse(s.Substring(2)) / 60.0;
        }

        public static bool TryConvertNatsCoordinate(string s, out LatLon result)
        {
            try
            {
                result = ConvertNatsCoordinate(s);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static string AutoChooseFormat(this LatLon item)
        {
            string result = Format5Letter.To5LetterFormat(item.Lat, item.Lon);
            return result ?? FormatDecimal.ToDecimalFormat(item.Lat, item.Lon);
        }
    }
}
