using QSP.RouteFinding.Containers;
using System;

namespace QSP.RouteFinding.Routes
{
    public class RouteNode : IEquatable<RouteNode>
    {
        /// <summary>
        /// The current waypoint.
        /// </summary>
        public Waypoint Waypoint { get; }

        /// <summary>
        /// The neighbor (edge) to next waypoint.
        /// </summary>
        public Neighbor AirwayToNext { get; }
        
        public RouteNode(Waypoint Waypoint, Neighbor airwayToNext)
        {
            this.Waypoint = Waypoint;
            this.AirwayToNext = airwayToNext;
        }

        public bool Equals(RouteNode other)
        {
            return
                other != null &&
                Waypoint.Equals(other.Waypoint) &&
                AirwayToNext.Equals(other.AirwayToNext);
        }

        public override int GetHashCode()
        {
            return Waypoint.GetHashCode() ^
                AirwayToNext.GetHashCode();
        }
    }
}
