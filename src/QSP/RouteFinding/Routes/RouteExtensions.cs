using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.RouteAnalyzers;
using System.Collections.Generic;
using System.Linq;

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

        public static SubRoute ToSubRoute(this Route r)
        {
            return new SubRoute(r);
        }

        public static Route Connect(this IEnumerable<Route> routes)
        {
            return routes.Aggregate((a, b) => { a.Connect(b); return a; });
        }

        public static IEnumerable<Waypoint> AllWaypoints(this Route r)
        {
            foreach (var i in r)
            {
                yield return i.Waypoint;
                foreach (var j in i.Neighbor.InnerWaypoints) yield return j;
            }
        }        
    }
}
