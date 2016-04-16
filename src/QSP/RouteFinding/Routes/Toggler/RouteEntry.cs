using System.Collections.Generic;

namespace QSP.RouteFinding.Routes.Toggler
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
