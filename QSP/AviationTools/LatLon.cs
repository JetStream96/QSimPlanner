using System;

namespace QSP.AviationTools
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
            return Lat == other.Lat && Lon == other.Lon;
        }
    }
}