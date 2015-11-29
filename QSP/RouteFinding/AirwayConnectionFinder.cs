using System.Collections.Generic;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding
{

    //Given an waypoint and the airway it is situated, gets all intermediate waypoints connecting to the target waypoint.
    //Requires WptList.
    public class AirwayConnectionFinder
    {

        private int indexStart;
        private string airway;
        private string identEnd;

        private TrackedWptList wptList;
        public AirwayConnectionFinder(int indexStart, string airway, string identEnd) : this(indexStart, airway, identEnd, RouteFindingCore.WptList)
        {
        }

        public AirwayConnectionFinder(int indexStart, string airway, string identEnd, TrackedWptList wptList)
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
        public List<int> FindWaypointIndices()
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
            var indices = FindWaypointIndices();

            if (indices == null)
            {
                return null;
            }
            List<Waypoint> result = new List<Waypoint>();

            foreach (int i in indices)
            {
                result.Add(wptList.WaypointAt(i));
            }
            return result;
        }

        private List<int> findWptOnAirwayOneDir(FindOnAwyOption para)
        {
            List<int> result = new List<int>();
            int x = para == FindOnAwyOption.First ? 0 : 1;
            //when x hit 0, start the search

            int currentIndex = indexStart;
            int prevIndex = -1;
            WptNeighbor currentWpt = wptList.ElementAt(currentIndex);
            bool updated = true;

            while (updated)
            {
                updated = false;
                
                foreach (var i in currentWpt.Neighbors)
                {
                    if (i.Airway == airway && i.Index != prevIndex)
                    {
                        if (x == 0)
                        {
                            prevIndex = currentIndex;
                            currentIndex = i.Index;
                            currentWpt = wptList.ElementAt(currentIndex);
                            result.Add(currentIndex);

                            if (currentWpt.Waypoint.ID == identEnd)
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
                            x --;
                        }
                    }
                }
            }
            return null;
        }
    }
}


