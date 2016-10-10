using QSP.LibraryExtension;
using System.Collections.Generic;

namespace QSP.RouteFinding.Routes
{
    public static class RouteExtensions
    {
        // E.g. 
        // items = '1 A 2 B 3'
        // route = '2 C 4 D 3'
        // airway = 'B'
        // 
        // (Alphabets are aiways, numbers are waypoints.)
        //
        // By route and airway, we search for '2 B 3' in items and then
        // replace it with route.
        // So result is '1 A 2 C 4 D 3'
        //
        public static void InsertRoute(
            this LinkedList<RouteNode> items,
            LinkedList<RouteNode> route,
            string airway)
        {
            // This finds '2'.
            var matches = items.FindAll((n) =>
            {
                return n.Value.Waypoint.Equals(route.First.Value.Waypoint) &&
                n.Value.AirwayToNext == airway &&
                n.Next != null &&
                n.Next.Value.Waypoint.Equals(route.Last.Value.Waypoint);
            });

            foreach (var i in matches)
            {
                var j = i.Next;
                items.AddAfter(i, route);
                items.Remove(i);
                items.Remove(j.Previous);
            }
        }

        /// <summary>
        /// If first waypoint of other and the last one in item are:
        /// (1) The same: The two routes are connected.
        /// (2) Different: other is appended after item, with airway "DCT".
        /// </summary>
        public static void Merge(
            this Route item, Route other, bool useLastAirway = false)
        {
            if (other.Count == 0) return;

            if (item.Count == 0)
            {
                item.Nodes.AddLast(other);
                return;
            }

            if (item.LastWaypoint.Equals(other.FirstWaypoint))
            {
                item.ConnectRoute(other);
            }
            else
            {
                var airway = useLastAirway 
                    ? item.Last.Value.AirwayToNext 
                    : "DCT";

                item.AddLast(other, airway);
            }
        }
    }
}
