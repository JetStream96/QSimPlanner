using QSP.AviationTools.Coordinates;
using QSP.LibraryExtension.Graph;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data;
using System.Collections.Generic;
using static QSP.MathTools.Utilities;

namespace QSP.RouteFinding.AirwayStructure
{
    /// <summary>
    /// Representation of the airways and waypoints. 
    /// This is implemented with a hash table, so searching by 
    /// waypoint ident is O(1).
    /// This class is NOT thread safe.
    /// </summary>
    public class WaypointList
    {
        private WaypointContainer _content;
        private LatLonSearcher<WptSeachWrapper> _finder;

        public WaypointList()
        {
            _content = new WaypointContainer();
            _finder = new LatLonSearcher<WptSeachWrapper>(1, 5);
        }
        
        public int AddWaypoint(Waypoint item)
        {
            int index = _content.AddWpt(item);
            _finder.Add(new WptSeachWrapper(index, item.Lat, item.Lon));
            return index;
        }

        public int AddNeighbor(int indexFrom, int indexTo, Neighbor item)
        {
            return _content.AddNeighbor(indexFrom, indexTo, item);
        }

        public Waypoint this[int index]
        {
            get
            {
                return _content[index];
            }
        }

        public bool WaypointExists(int WaypointIndex)
        {
            return _content.ItemExists(WaypointIndex);
        }

        public LatLon LatLonAt(int index)
        {
            return this[index].LatLon;
        }

        public int EdgesFromCount(int index)
        {
            return _content.EdgesFromCount(index);
        }

        public int EdgesToCount(int index)
        {
            return _content.EdgesToCount(index);
        }

        public int Count
        {
            get { return _content.Count; }
        }

        /// <summary>
        /// The upper bound of indices of elements plus one. 
        /// </summary>
        public int MaxSize
        {
            get
            {
                return _content.MaxSize;
            }
        }

        /// <summary>
        /// Find the index of WptNeighbor by ident of a waypoint.
        /// Returns -1 if not found.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public int FindByID(string ident)
        {
            return _content.FindByID(ident);
        }

        /// <summary>
        /// Find all WptNeighbors by ident of a waypoint.
        /// </summary> 
        public List<int> FindAllByID(string ident)
        {
            return _content.FindAllByID(ident);
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
        /// Returns -1 if no match is found.
        /// </summary>
        public int FindByWaypoint(Waypoint wpt)
        {
            return _content.FindByWaypoint(wpt);
        }

        /// <summary>
        /// Find all occurences of WptNeighbor matching the waypoint.
        /// </summary>
        public List<int> FindAllByWaypoint(Waypoint wpt)
        {
            return _content.FindAllByWaypoint(wpt);
        }

        public List<WptSeachWrapper> Find(
            double lat, double lon, double distance)
        {
            return _finder.Find(lat, lon, distance);
        }

        public WaypointListEditor GetEditor()
        {
            return new WaypointListEditor(this);
        }

        public double Distance(int index1, int index2)
        {
            return GreatCircleDistance(this[index1].Lat, this[index1].Lon,
                                       this[index2].Lat, this[index2].Lon);
        }

        public void RemoveAt(int index)
        {
            _finder.Remove(
                new WptSeachWrapper(
                    index, _content[index].Lat, _content[index].Lon));

            _content.RemoveAt(index);
        }

        public void RemoveNeighbor(int edgeIndex)
        {
            _content.RemoveNeighbor(edgeIndex);
        }

        public IEnumerable<int> EdgesFrom(int index)
        {
            return _content.EdgesFrom(index);
        }

        public IEnumerable<int> EdgesTo(int index)
        {
            return _content.EdgesTo(index);
        }

        public Edge<Neighbor> GetEdge(int edgeIndex)
        {
            return _content.GetEdge(edgeIndex);
        }

    }
}