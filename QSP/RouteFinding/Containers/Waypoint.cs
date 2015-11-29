using QSP.AviationTools;
using System;
using System.Collections.Generic;
namespace QSP
{
    public class Waypoint : IComparable<Waypoint>
    {

        private string ident;
        private double latitude;

        private double longitude;
        public string ID
        {
            get { return ident; }
        }

        public double Lat
        {
            get { return latitude; }
        }

        public double Lon
        {
            get { return longitude; }
        }

        public Waypoint(string ID) : this(ID, 0.0, 0.0)
        {
        }

        public Waypoint(string ID, double Lat, double Lon)
        {
            ident = ID;
            latitude = Lat;
            longitude = Lon;
        }

        public Waypoint(string ID, LatLon latLon)
        {
            ident = ID;
            latitude = latLon.Lat;
            longitude = latLon.Lon;
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

            if (ID == x.ID && Lat == x.Lat && Lon == x.Lon)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int WptCompare(Waypoint x)
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

        public int CompareTo(Waypoint x)
        {
            return WptCompare(x);
        }

        private class sortIDHelper : IComparer<Waypoint>
        {

            public int Compare(Waypoint x, Waypoint y)
            {
                return x.ID.CompareTo(y.ID);
            }
        }

        public static IComparer<Waypoint> SortID()
        {
            return new sortIDHelper();
        }

    }
}
