using QSP.LibraryExtension.Graph;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.RouteFinding.AirwayStructure
{
    public static class EdgeExtension
    {
        // Since the neighbor distance is loaded directly from file and is only
        // accurate to 2 digits. We recompute for better precision.
        public static Neighbor RecomputeDistance(this Edge<Neighbor> item, WaypointList wptList)
        {
            var n = item.Value;
            if (n.InnerWaypoints.Count != 0) return n;

            var from = wptList[item.FromNodeIndex];
            var to = wptList[item.ToNodeIndex];
            var dis = from.Distance(to);
            return new Neighbor(n.Airway, dis, n.InnerWaypoints, n.Type);
        }
    }
}