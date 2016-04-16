using System;

namespace QSP.AviationTools.Coordinates
{
    public class LatLon : IEquatable<LatLon>
    {
        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public LatLon(double Lat, double Lon)
        {
            this.Lat = Lat;
            this.Lon = Lon;
        }

        /// <summary>
        /// The great circle distance between two points, in nautical miles.
        /// </summary>
        public double Distance(double Lat, double Lon)
        {
            return MathTools.Utilities.GreatCircleDistance(this.Lat, this.Lon, Lat, Lon);
        }

        /// <summary>
        /// The great circle distance between two points, in nautical miles.
        /// </summary>
        public double Distance(LatLon LatLon)
        {
            return MathTools.Utilities.GreatCircleDistance(Lat, Lon, LatLon.Lat, LatLon.Lon);
        }

        public bool Equals(LatLon other)
        {
            return Math.Abs(Lat - other.Lat) < Constants.LatLon_TOLERENCE &&
                   Math.Abs(Lon - other.Lon) < Constants.LatLon_TOLERENCE;
        }

        /// <summary>
        /// Output examples: 36N170W 34N080E
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public string To7LetterFormat()
        {
            return Format7Letter.To7LetterFormat(Lat, Lon);
        }

        /// <summary>
        /// Output examples: 36N70, 3480E.
        /// Returns null if either Lat or Lon is not an integer.
        /// </summary>
        public string To5LetterFormat()
        {
            return Format5Letter.To5LetterFormat(Lat, Lon);
        }

        public string ToDecimalFormat()
        {
            return FormatDecimal.ToDecimalFormat(Lat, Lon);
        }
    }
}