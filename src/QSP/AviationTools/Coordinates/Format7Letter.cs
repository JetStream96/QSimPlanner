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
            if (item.Length != 7) return null;

            char NS = item[2];
            char EW = item[6];

            int lat, lon;

            if (!int.TryParse(item.Substring(0, 2), out lat) ||
                !int.TryParse(item.Substring(3, 3), out lon) ||
                lat < 0 || lat > 90 || lon < 0 || lon > 180)
            {
                return null;
            }

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

            return null;
        }
    }
}
