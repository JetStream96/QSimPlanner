using QSP.AviationTools;
using System;
using System.Collections.Generic;
using QSP.RouteFinding.Data;
using static QSP.RouteFinding.Constants;

namespace QSP.RouteFinding.Containers
{
    public class Waypoint : IComparable<Waypoint>, ICoordinate, IEquatable<Waypoint>
    {
        public string ID { get; private set; }
        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public Waypoint(string ID) : this(ID, 0.0, 0.0)
        {
        }

        public Waypoint(string ID, double Lat, double Lon)
        {
            this.ID = ID;
            this.Lat = Lat;
            this.Lon = Lon;
        }

        public Waypoint(string ID, LatLon latLon) : this(ID, latLon.Lat, latLon.Lon)
        {
        }

        public Waypoint(Waypoint waypoint) : this(waypoint.ID, waypoint.Lat, waypoint.Lon)
        {
        }

        public LatLon LatLon
        {
            get { return new LatLon(Lat, Lon); }
        }

        /// <summary>
        /// Determines whether ID, Lat, and Lon match.
        /// </summary>
        public bool Equals(Waypoint x)
        {
            return (ID == x.ID && Math.Abs(Lat - x.Lat) < LATLON_TOLERANCE && Math.Abs(Lon - x.Lon) < LATLON_TOLERANCE);
        }
        
        public int CompareTo(Waypoint x)
        {
            int i = ID.CompareTo(x.ID);

            if (i == 0)
            {
                int j = Lat.CompareTo(x.Lat);

                if (j == 0)
                {
                    return Lon.CompareTo(x.Lon);
                }
                else
                {
                    return j;
                }
            }
            else
            {
                return i;
            }
        }

        private class sortIDHelper : Comparer<Waypoint>
        {
            public override int Compare(Waypoint x, Waypoint y)
            {
                return x.ID.CompareTo(y.ID);
            }
        }

        public static Comparer<Waypoint> SortID()
        {
            return new sortIDHelper();
        }

    }
}
