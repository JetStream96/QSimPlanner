using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.RouteFinding.Routes
{
    public class RouteNode : IEquatable<RouteNode>
    {
        public Waypoint Waypoint { get; set; }
        public Neighbor Neighbor { get; set; }

        public string AirwayToNext
        {
            get
            {
                return Neighbor.Airway;
            }
        }

        public double DistanceToNext
        {
            get
            {
                return Neighbor.Distance;
            }
        }
        
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
                AirwayToNext == other.AirwayToNext &&
                Math.Abs(DistanceToNext - other.DistanceToNext) < 1E-3;
        }

        public override int GetHashCode()
        {
            return Waypoint.GetHashCode() ^
                AirwayToNext.GetHashCode() ^
                DistanceToNext.GetHashCode();
        }
    }
}
