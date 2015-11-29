using QSP.AviationTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.Containers
{

    public class WptNeighbor
    {

        private Waypoint wpt;

        private List<Neighbor> neighborList;
        public Waypoint Waypoint
        {
            get { return wpt; }
        }

        public ReadOnlyCollection<Neighbor> Neighbors
        {
            // List(Of Neighbor)
            get { return neighborList.AsReadOnly(); }
        }

        /// <summary>
        /// This method can only be called from TrackedWptList class with the correct token. Otherwise an exception will be thrown.
        /// </summary>
        /// <param name="token">The token object in TrackedWptList class.</param>
        public List<Neighbor> GetNeighborList(object token)
        {

            if (TrackedWptList.TokenMatches(token))
            {
                return neighborList;
            }
            else
            {
                throw new InvalidOperationException("This method can only be called from TrackedWptList class with the correct token.");
            }

        }

        public WptNeighbor(Waypoint Waypoint)
        {
            this.wpt = Waypoint;
            this.neighborList = new List<Neighbor>();
        }

        public WptNeighbor(Waypoint Waypoint, List<Neighbor> neighborList)
        {
            this.wpt = Waypoint;
            this.neighborList = neighborList;
        }

        /// <summary>
        /// A deep copy of the item.
        /// </summary>
        public WptNeighbor(WptNeighbor item)
        {
            this.wpt = new Waypoint(item.Waypoint);
            this.neighborList = new List<Neighbor>(item.Neighbors);
        }

        public void AddNeighbor(int index, string airway, double dis)
        {
            neighborList.Add(new Neighbor(index, airway, dis));
        }

        public LatLon LatLon()
        {
            return wpt.LatLon;
        }

        /// <summary>
        /// Value comparison of two WptNeighbors.
        /// </summary>
        public bool Equals(WptNeighbor x)
        {
            if (wpt.Equals(x.wpt) && neighborList.Count == x.neighborList.Count)
            {
                for (int i = 0; i <= neighborList.Count - 1; i++)
                {
                    if (neighborList[i].Equals(x.neighborList[i]) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private class sortWptHelper : IComparer<WptNeighbor>
        {

            public int Compare(WptNeighbor x, WptNeighbor y)
            {
                return x.wpt.WptCompare(y.wpt);
            }
        }

        private class sortIDHelper : IComparer<WptNeighbor>
        {

            public int Compare(WptNeighbor x, WptNeighbor y)
            {
                return x.wpt.ID.CompareTo(y.wpt.ID);
            }
        }

        public static IComparer<WptNeighbor> SortWpt()
        {
            return new sortWptHelper();
        }

        public static IComparer<WptNeighbor> SortID()
        {
            return new sortIDHelper();
        }

        public static LatLon LatLon(WptNeighbor item)
        {
            return item.wpt.LatLon;
        }

        public class WptNeighborEqualityComparer : IEqualityComparer<WptNeighbor>
        {

            public bool Equals(WptNeighbor x, WptNeighbor y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(WptNeighbor obj)
            {

                int hash = obj.GetHashCode();

                foreach (var i in obj.neighborList)
                {
                    hash = hash ^ i.GetHashCode();
                }

                return hash;

            }

        }

    }

}
