using QSP.AviationTools.Coordinates;
using QSP.Utilities;
using System;

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
            int x = s.IndexOf('/');
            ConditionChecker.ThrowWhenNegative<ArgumentException>(x);

            double Lat = Convert.ToDouble(s.Substring(0, 2));
            double Lon = Convert.ToDouble(s.Substring(x + 1, 2));

            if (x > 2)
            {
                double addLat = Convert.ToDouble(s.Substring(2, x - 2));
                ConditionChecker.Ensure<ArgumentException>(addLat >= 0.0 && addLat <= 60.0);
                Lat += addLat / 60;
            }

            if (s.Length - x - 1 > 2)
            {
                double addLon = Convert.ToDouble(s.Substring(x + 3));
                ConditionChecker.Ensure<ArgumentException>(addLon >= 0.0 && addLon <= 60.0);
                Lon += addLon / 60;
            }

            ConditionChecker.Ensure<ArgumentException>(Lat >= 0.0 && Lat <= 90.0);
            ConditionChecker.Ensure<ArgumentException>(Lon >= 0 && Lon <= 180);

            return new LatLon(Lat, -Lon);
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

            if (result != null)
            {
                return result;
            }
            return FormatDecimal.ToDecimalFormat(item.Lat, item.Lon);
        }
    }
}
