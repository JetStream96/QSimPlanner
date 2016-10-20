using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // This class is immutable.
    public class SubRoute
    {
        public bool IsAuto { get; private set; }
        public bool IsRand { get; private set; }
        public Route Route { get; private set; }

        public static SubRoute Auto()
        {
            return new SubRoute(true, false, null);
        }

        public static SubRoute Rand()
        {
            return new SubRoute(false, true, null);
        }

        public SubRoute(Route Route) : this(false, false, Route) { }

        private SubRoute(bool IsAuto, bool IsRand, Route Route)
        {
            this.IsAuto = IsAuto;
            this.IsRand = IsRand;
            this.Route = Route;
        }
    }
}
