using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.RouteAnalyzers
{
    public static class Utilities
    {
        /// <summary>
        /// Merge the two route by appending RouteToMerge.
        /// The first waypoint of RouteToMerge and the last one in the original route
        /// can be the same or different.
        /// </summary>
        public static void MergeWith(this Route item, Route RouteToMerge)
        {
            if (item.Count == 0)
            {
                item = RouteToMerge;
                return;
            }

            if (RouteToMerge.Count == 0)
            {
                return;
            }

            if (item.Last.Waypoint.Equals(RouteToMerge.First.Waypoint))
            {
                item.ConnectRoute(RouteToMerge);
            }
            else
            {
                item.AppendRoute(RouteToMerge, item.Last.AirwayToNext ?? "DCT");
            }
        }
    }
}
