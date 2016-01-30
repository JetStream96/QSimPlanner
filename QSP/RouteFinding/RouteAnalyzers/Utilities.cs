using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.RouteAnalyzers
{
    public static class Utilities
    {
        /// <summary>
        /// Merge the two route by appending RouteToMerge to Original.
        /// The first
        /// </summary>
        public static void MergeWith(this Route item, Route RouteToMerge)
        {
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
                item.AppendRoute(RouteToMerge, "DCT");
            }
        }
    }
}
