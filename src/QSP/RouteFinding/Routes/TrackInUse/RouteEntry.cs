using System.Collections.Generic;

namespace QSP.RouteFinding.Routes.TrackInUse
{
    public struct RouteEntry
    {
        public LinkedList<RouteNode> Route { get; }
        public string RouteName { get; }

        public RouteEntry(LinkedList<RouteNode> Route, string RouteName)
        {
            this.Route = Route;
            this.RouteName = RouteName;
        }
    }
}
