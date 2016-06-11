using System.Collections.Generic;

namespace QSP.RouteFinding.Routes.TrackInUse
{
    public struct RouteEntry
    {
        public LinkedList<RouteNode> Route;
        public string RouteName;

        public RouteEntry(LinkedList<RouteNode> Route, string RouteName)
        {
            this.Route = Route;
            this.RouteName = RouteName;
        }
    }
}
