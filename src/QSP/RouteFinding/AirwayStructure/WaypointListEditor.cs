using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.AirwayStructure
{
    public class WaypointListEditor
    {
        private WaypointList wptList;
        private TrackerItem tracker;

        public WaypointListEditor(WaypointList wptList)
        {
            this.wptList = wptList;
            tracker = new TrackerItem();
        }

        public int AddWaypoint(Waypoint item)
        {
            int index = wptList.AddWaypoint(item);
            tracker.AddWaypointRecord(index);
            return index;
        }

        public int AddNeighbor(int from, int to, Neighbor item)
        {
            int index = wptList.AddNeighbor(from, to, item);
            tracker.AddNeighborRecord(index);
            return index;
        }

        public void Undo()
        {
            //remove neighbors first
            foreach (var j in tracker.AddedNeighbor)
            {
                wptList.RemoveNeighbor(j);
            }

            // Remove all wpts.
            foreach (var k in tracker.AddedWaypoint)
            {
                wptList.RemoveAt(k);
            }

            // Clear the tracker
            tracker = new TrackerItem();
        }
    }
}
