using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Routes
{
    public interface IReadOnlyRoute : IReadOnlyCollection<RouteNode>
    {
        RouteNode First { get; }
        RouteNode Last { get; }
        double TotalDistance();
    }

    public static class IReadOnlyRouteExtension
    {
        public static Waypoint FirstWaypoint(this IReadOnlyRoute r)
        {
            return r.First.Waypoint;
        }

        public static Waypoint LastWaypoint(this IReadOnlyRoute r)
        {
            return r.Last.Waypoint;
        }

        public static IEnumerable<Waypoint> AllWaypoints(this IReadOnlyRoute r)
        {
            foreach (var i in r)
            {
                yield return i.Waypoint;
                foreach (var j in i.Neighbor.InnerWaypoints) yield return j;
            }
        }

        //public static IReadOnlyRoute 
    }
}
