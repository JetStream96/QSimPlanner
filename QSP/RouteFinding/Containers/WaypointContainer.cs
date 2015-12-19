using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace QSP.RouteFinding.Containers
{
    public class WaypointContainer : IEnumerable<WptNeighbor>, IEnumerable
    {
        #region Container

        private class WptData : WptNeighbor
        {
            public int NumNodeFrom { get; set; }  //indicates how many other waypoints have this wpt as neighbor

            public List<Neighbor> NeighborList
            {
                get
                {
                    return _neighborList;
                }
                set
                {
                    _neighborList = value;
                }
            }

            public WptData(string ID, double Lat, double Lon, int NumNodeFrom) : base(ID, Lat, Lon)
            {
                this.NumNodeFrom = NumNodeFrom;
            }

            public WptData(Waypoint waypoint, int NumNodeFrom) : base(waypoint)
            {
                this.NumNodeFrom = NumNodeFrom;
            }

            public WptData(WptNeighbor WptNeighbor, int NumNodeFrom) : base(WptNeighbor)
            {
                this.NumNodeFrom = NumNodeFrom;
            }
        }

        #endregion

        #region Fields

        private HashMap<string, int> searchHelper;
        private FixedIndexList<WptData> content;

        #endregion

        public WaypointContainer()
        {
            content = new FixedIndexList<WptData>();
            searchHelper = new HashMap<string, int>();
        }

        public int AddWpt(string ID, double Lat, double Lon)
        {
            searchHelper.Add(ID, content.Count);
            return content.Add(new WptData(ID, Lat, Lon, 0));
        }

        public int AddWpt(Waypoint item)
        {
            searchHelper.Add(item.ID, content.Count);
            return content.Add(new WptData(item, 0));
        }

        public int AddWpt(WptNeighbor item)
        {
            searchHelper.Add(item.ID, content.Count);
            return content.Add(new WptData(item, 0));
        }

        public void AddNeighbor(int index, Neighbor item)
        {
            content[index].NeighborList.Add(item);
            content[item.Index].NumNodeFrom++;
        }

        public void Clear()
        {
            content.Clear();
            searchHelper.Clear();
        }

        public WptNeighbor this[int index]
        {
            get
            {
                return content[index];
            }
        }

        public int NumberOfNodeFrom(int index)
        {
            return content[index].NumNodeFrom;
        }

        public int Count
        {
            get { return content.Count; }
        }

        /// <summary>
        /// The upper bound of indices of elements plus one. 
        /// </summary>
        public int MaxSize
        {
            get
            {
                return content.MaxSize;
            }
        }

        /// <summary>
        /// Find the index of WptNeighbor by ident of a waypoint.
        /// </summary>
        public int FindByID(string ident)
        {
            return searchHelper[ident];
        }

        /// <summary>
        /// Find all WptNeighbors by ident of a waypoint.
        /// </summary> 
        public List<int> FindAllByID(string ident)
        {
            return searchHelper.AllMatches(ident);
        }

        /// <summary>
        /// Find the index of WptNeighbor matching the waypoint.
        /// </summary>
        public int FindByWaypoint(string ident, double lat, double lon)
        {
            return FindByWaypoint(new Waypoint(ident, lat, lon));
        }

        /// <summary>
        /// Find the index of WptNeighbor matching the waypoint.
        /// </summary>
        public int FindByWaypoint(Waypoint wpt)
        {
            var candidates = searchHelper.AllMatches(wpt.ID);

            if (candidates != null)
            {
                foreach (int i in candidates)
                {
                    if (this[i].Equals(wpt))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Find all occurences of WptNeighbor matching the waypoint.
        /// </summary>
        public List<int> FindAllByWaypoint(Waypoint wpt)
        {
            var candidates = searchHelper.AllMatches(wpt.ID);
            var results = new List<int>();

            foreach (int i in candidates)
            {
                if (content[i].Equals(wpt))
                {
                    results.Add(i);
                }
            }
            return results;
        }

        public void RemoveAt(int index)
        {
            content.RemoveAt(index);
            searchHelper.Remove(content[index].ID, index, HashMap<string, int>.RemoveParameter.RemoveFirst);
        }

        public void RemoveNeighbor(int wptIndex, Neighbor neighbor)
        {
            content[neighbor.Index].NumNodeFrom--;
            content[wptIndex].NeighborList.Remove(neighbor);
        }

        public IEnumerator<WptNeighbor> GetEnumerator()
        {
            return content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return content.GetEnumerator();
        }
    }
}
