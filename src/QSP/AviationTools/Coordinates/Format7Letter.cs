using QSP.Utilities;
using System;
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

        public static bool TryReadFrom7LetterFormat(
            string item, out LatLon result)
        {
            try
            {
                result = ReadFrom7LetterFormat(item);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static LatLon ReadFrom7LetterFormat(string item)
        {
            ConditionChecker.Ensure<ArgumentException>(item.Length == 7);

            char NS = item[2];
            char EW = item[6];

            int lat = Convert.ToInt32(item.Substring(0, 2));
            int lon = Convert.ToInt32(item.Substring(3, 3));

            ConditionChecker.Ensure<ArgumentException>(
                0 <= lat && lat <= 90 && 0 <= lon && lon <= 180);

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
