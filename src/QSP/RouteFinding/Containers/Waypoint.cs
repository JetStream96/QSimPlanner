using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.RouteFinding.Containers
{
    public class Waypoint :
        IComparable<Waypoint>, ICoordinate, IEquatable<Waypoint>
    {
        public const int DefaultCountryCode = -1;

        public string ID { get; private set; }
        public double Lat { get; private set; }
        public double Lon { get; private set; }
        public int CountryCode { get; private set; }

        public Waypoint(
            string ID,
            double Lat = 0.0,
            double Lon = 0.0,
            int CountryCode = DefaultCountryCode)
        {
            this.ID = ID;
            this.Lat = Lat;
            this.Lon = Lon;
            this.CountryCode = CountryCode;
        }

        public Waypoint(string ID, ICoordinate latLon)
            : this(ID, latLon.Lat, latLon.Lon)
        { }

        public Waypoint(Waypoint waypoint)
            : this(waypoint.ID,
                  waypoint.Lat,
                  waypoint.Lon,
                  waypoint.CountryCode)
        { }

        /// <summary>
        /// Determines whether ID, Lat, and Lon match.
        /// </summary>
        public bool Equals(Waypoint x)
        {
            return
                x != null &&
                ID == x.ID &&
                Lat == x.Lat &&
                Lon == x.Lon;
        }

        public int CompareTo(Waypoint other)
        {
            int x = ID.CompareTo(other.ID);

            if (x == 0)
            {
                int y = Lat.CompareTo(other.Lat);
                if (y == 0) return Lon.CompareTo(other.Lon);
                return y;
            }

            return x;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ Lat.GetHashCode() ^ Lon.GetHashCode();
        }
    }
}
