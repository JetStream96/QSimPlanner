using QSP.RouteFinding.Containers;
using System;

namespace QSP.RouteFinding.Routes
{
    public class RouteNode : IEquatable<RouteNode>
    {
        public Waypoint Waypoint { get; private set; }
        public Neighbor Neighbor { get; private set; }
        
        public RouteNode(Waypoint Waypoint, Neighbor Neighbor)
        {
            this.Waypoint = Waypoint;
            this.Neighbor = Neighbor;
        }

        public bool Equals(RouteNode other)
        {
            return
                other != null &&
                Waypoint.Equals(other.Waypoint) &&
                Neighbor.Equals(other.Neighbor);
        }

        public override int GetHashCode()
        {
            return Waypoint.GetHashCode() ^
                Neighbor.GetHashCode();
        }
    }
}
