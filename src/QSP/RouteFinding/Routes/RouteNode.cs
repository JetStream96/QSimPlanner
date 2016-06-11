using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.RouteFinding.Routes
{
    public class RouteNode : IEquatable<RouteNode>, ICoordinate
    {
        public Waypoint Waypoint { get; private set; }
        public string AirwayToNext { get; set; }
        public double DistanceToNext { get; set; }

        public double Lat
        {
            get
            {
                return Waypoint.Lat;
            }
        }

        public double Lon
        {
            get
            {
                return Waypoint.Lon;
            }
        }

        public RouteNode(Waypoint Waypoint)
        {
            this.Waypoint = Waypoint;
        }

        public RouteNode(Waypoint Waypoint, string AirwayToNext, double DistanceToNext)
        {
            this.Waypoint = Waypoint;
            this.AirwayToNext = AirwayToNext;
            this.DistanceToNext = DistanceToNext;
        }

        public bool Equals(RouteNode other)
        {
            return (Waypoint.Equals(other.Waypoint) &&
                    AirwayToNext == other.AirwayToNext &&
                    Math.Abs(DistanceToNext - other.DistanceToNext) < 1E-3);
        }
    }
}
