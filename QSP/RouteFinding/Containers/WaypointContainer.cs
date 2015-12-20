using QSP.LibraryExtension;
using QSP.LibraryExtension.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static QSP.LibraryExtension.HashMap<string, int>;

namespace QSP.RouteFinding.Containers
{
    public class WaypointContainer
    {
        #region Fields

        private Graph<Waypoint, Neighbor> graph;
        private HashMap<string, int> searchHelper;

        #endregion

        public WaypointContainer()
        {
            graph = new Graph<Waypoint, Neighbor>();
            searchHelper = new HashMap<string, int>();
        }

        public int AddWpt(Waypoint item)
        {
            int index = graph.AddNode(item);
            searchHelper.Add(item.ID, index);
            return index;
        }

        public int AddNeighbor(int indexFrom, int indexTo, Neighbor item)
        {
            return graph.AddEdge(indexFrom, indexTo, item);
        }

        public void Clear()
        {
            graph.Clear();
            searchHelper.Clear();
        }

        public Waypoint this[int index]
        {
            get
            {
                return graph.GetNode(index);
            }
        }

        public int EdgesFromCount(int index)
        {
            return graph.EdgesFromCount(index);
        }

        public int Count
        {
            get
            {
                return graph.NodeCount;
            }
        }

        /// <summary>
        /// The upper bound of indices of elements plus one. 
        /// </summary>
        public int MaxSize
        {
            get
            {
                return graph.MaxSizeNode;
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
        /// Find the index of WptNeighbor matching the waypoint. Returns -1 if no match is found.
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
                if (graph.GetNode(i).Equals(wpt))
                {
                    results.Add(i);
                }
            }
            return results;
        }

        public void RemoveAt(int index)
        {
            searchHelper.Remove(graph.GetNode(index).ID, index, RemoveParameter.RemoveFirst);
            graph.RemoveNode(index);
        }

        public void RemoveNeighbor(int edgeIndex)
        {
            graph.RemoveEdge(edgeIndex);
        }

        public IEnumerable<int> EdgesFrom(int index)
        {
            return graph.EdgesFrom(index);
        }

        public IEdge<Neighbor> GetEdge(int edgeIndex)
        {
            return graph.GetEdge(edgeIndex);
        }

    }
}
