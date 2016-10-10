using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    public class StarExtractor
    {
        private WaypointList wptList;
        private StarCollection stars;
        private Waypoint rwyWpt;
        private string icao;
        private string rwy;

        private LinkedList<string> route;
        private Route destRoute;

        public StarExtractor(
            IEnumerable<string> route,
            string icao,
            string rwy,
            Waypoint rwyWpt,
            WaypointList wptList,
            StarCollection stars)
        {
            this.route = new LinkedList<string>(route);
            this.icao = icao;
            this.rwy = rwy;
            this.rwyWpt = rwyWpt;
            this.wptList = wptList;
            this.stars = stars;
        }

        public ExtractResult Extract()
        {
            destRoute = new Route();
            destRoute.AddLastWaypoint(rwyWpt);

            if (route.Count > 0) CreateDestRoute();

            return new ExtractResult
            { RemainingRoute = route, Star = destRoute };
        }

        public class ExtractResult
        {
            public IEnumerable<string> RemainingRoute;
            public Route Star;
        }

        private void CreateDestRoute()
        {
            if (route.Last.Value == icao)
            {
                route.RemoveLast();
            }

            string starName = route.Last.Value;
            var star = TryGetStar(starName, rwyWpt);

            if (star != null)
            {
                route.RemoveLast();

                // Any STAR has at least one waypoint.
                destRoute.AddFirstWaypoint(
                    star.FirstWaypoint, starName, star.TotalDistance);

                if (route.Last.Value == star.FirstWaypoint.ID &&
                    wptList.FindByWaypoint(star.FirstWaypoint) == -1)
                {
                    route.RemoveLast();
                }
            }
        }

        private StarInfo TryGetStar(string StarName, Waypoint rwyWpt)
        {
            try
            {
                return stars.GetStarInfo(StarName, rwy, rwyWpt);
            }
            catch
            {
                // no Star in route                
                return null;
            }
        }
    }
}
