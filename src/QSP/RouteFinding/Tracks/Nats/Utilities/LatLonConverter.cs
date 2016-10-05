using QSP.AviationTools.Coordinates;
using System;
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
            int x = s.IndexOf('/');
            Ensure<ArgumentException>(x >= 0);
            
            double Lat = double.Parse(s.Substring(0, 2));
            double Lon = double.Parse(s.Substring(x + 1, 2));

            if (x > 2)
            {
                double addLat = double.Parse(s.Substring(2, x - 2));
                Ensure<ArgumentException>(0.0 <= addLat && addLat <= 60.0);
                Lat += addLat / 60;
            }

            if (s.Length - x - 1 > 2)
            {
                double addLon = double.Parse(s.Substring(x + 3));
                Ensure<ArgumentException>(0.0 <= addLon && addLon <= 60.0);
                Lon += addLon / 60;
            }

            Ensure<ArgumentException>(0.0 <= Lat && Lat <= 90.0);
            Ensure<ArgumentException>(0.0 <= Lon && Lon <= 180.0);

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
