using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Containers
{
    public class WaypointContainer
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
        private FlexibleList <WptData> content;

        #endregion

        public WaypointContainer()
        {
            content = new FlexibleList <WptData>();
            searchHelper = new HashMap<string, int>();
        }
        
        public void AddWpt(string ID, double Lat, double Lon)
        {
            searchHelper.Add(ID, content.Count);
            content.Add(new WptData(ID, Lat, Lon, 0));
        }

        public void AddWpt(Waypoint item)
        {
            searchHelper.Add(item.ID, content.Count);
            content.Add(new WptData(item, 0));
        }

        public void AddWpt(WptNeighbor item)
        {
            searchHelper.Add(item.ID, content.Count);
            content.Add(new WptData(item, 0));
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
        
    }
}
