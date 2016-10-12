using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.RouteFinding.Routes
{
    public class RouteNode : IEquatable<RouteNode>, ICoordinate
    {
        public Waypoint Waypoint { get; private set; }
        public Neighbor Neighbor { get; private set; }

        public string AirwayToNext
        {
            get
            {
                return Neighbor.Airway;
            }
            set
            {
                Neighbor = new Neighbor(
                    value, Neighbor.Distance, Neighbor.InnerWaypoints);
            }
        }

        public double DistanceToNext
        {
            get
            {
                return Neighbor.Distance;
            }
            set
            {
                Neighbor = new Neighbor(
                   Neighbor.Airway, value, Neighbor.InnerWaypoints);
            }
        }

        public double Lat { get { return Waypoint.Lat; } }
        public double Lon { get { return Waypoint.Lon; } }

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
