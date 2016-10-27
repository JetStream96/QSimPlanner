using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.AviationTools.Coordinates
{
    public class LatLon : IEquatable<LatLon>, ICoordinate
    {
        public double Lat { get; }
        public double Lon { get; }

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
            return GCDis.Distance(this.Lat, this.Lon, Lat, Lon);
        }

        /// <summary>
        /// The great circle distance between two points, in nautical miles.
        /// </summary>
        public double Distance(LatLon LatLon)
        {
            return Distance(LatLon.Lat, LatLon.Lon);
        }

        public bool Equals(LatLon other)
        {
            return other != null &&
                Math.Abs(Lat - other.Lat) < Constants.LatLonTolerance &&
                Math.Abs(Lon - other.Lon) < Constants.LatLonTolerance;
        }

        public override int GetHashCode()
        {
            return Lat.GetHashCode() ^ Lon.GetHashCode();
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