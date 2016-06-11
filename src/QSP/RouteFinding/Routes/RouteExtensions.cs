using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.Routes
{
    public static class RouteExtensions
    {
        public static Waypoint FirstWpt(this Route item)
        {
            return item.First.Value.Waypoint;
        }

        public static Waypoint LastWpt(this Route item)
        {
            return item.First.Value.Waypoint;
        }
    }
}
