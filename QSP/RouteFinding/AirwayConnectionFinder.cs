using System.Collections.Generic;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding
{
    // Given an waypoint and the airway it is situated, gets all intermediate waypoints connecting to the target waypoint.
    // Requires WptList.
    public class AirwayNodeFinder
    {
        private int indexStart;
        private string airway;
        private string identEnd;
        private WaypointList wptList;
        
        public AirwayNodeFinder(int indexStart, string airway, string identEnd, WaypointList wptList)
        {
            this.indexStart = indexStart;
            this.airway = airway;
            this.identEnd = identEnd;
            this.wptList = wptList;
        }

        private enum FindOnAwyOption
        {
            First,
            Second
        }

        /// <summary>
        /// Finds the list of indices connecting the start wpt to the end wpt, via the specified airway.
        /// In the returning list, start wpt is NOT included while the end wpt is included. 
        /// Wpts appears in the same order as the way they are visited.
        /// </summary>
        public List<int> GetWaypointIndices()
        {
            var result = findWptOnAirwayOneDir(FindOnAwyOption.First);

            if (result != null)
            {
                return result;
            }
            else
            {
                return findWptOnAirwayOneDir(FindOnAwyOption.Second);
            }
        }

        /// <summary>
        /// Finds the list of waypoints connecting the start wpt to the end wpt, via the specified airway.
        /// In the returning list, start wpt is NOT included while the end wpt is included. 
        /// Wpts appears in the same order as the way they are visited.
        /// </summary>
        public List<Waypoint> FindWaypoints()
        {
            var indices = GetWaypointIndices();

            if (indices == null)
            {
                return null;
            }
            var result = new List<Waypoint>();

            foreach (int i in indices)
            {
                result.Add(wptList[i]);
            }
            return result;
        }

        private List<int> findWptOnAirwayOneDir(FindOnAwyOption para)
        {
            var result = new List<int>();
            int x =( para == FindOnAwyOption.First) ? 0 : 1;
            //when x hit 0, start the search

            int currentIndex = indexStart;
            int prevIndex = -1;
            var currentWpt = wptList[currentIndex];
            bool updated = true;

            while (updated)
            {
                updated = false;

                foreach (int i in wptList.EdgesFrom(currentIndex))
                {
                    var edge = wptList.GetEdge(i);
                    var n = edge.value;
                    int index = edge.ToNodeIndex;

                    if (n.Airway == airway && index != prevIndex)
                    {
                        if (x == 0)
                        {
                            prevIndex = currentIndex;
                            currentIndex = index;
                            currentWpt = wptList[currentIndex];
                            result.Add(currentIndex);

                            if (currentWpt.ID == identEnd)
                            {
                                return result;
                            }
                            else
                            {
                                updated = true;
                                break;
                            }
                        }
                        else
                        {
                            x--;
                        }
                    }
                }
            }
            return null;
        }
    }
}
