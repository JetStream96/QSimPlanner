using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;

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

        public StarExtractor(LinkedList<string> route,
                            string icao,
                            string rwy,
                            Waypoint rwyWpt,
                            WaypointList wptList,
                            StarCollection stars)
        {
            this.route = route;
            this.icao = icao;
            this.rwy = rwy;
            this.rwyWpt = rwyWpt;
            this.wptList = wptList;
            this.stars = stars;
        }

        public Route Extract()
        {
            destRoute = new Route();
            destRoute.AddLastWaypoint(rwyWpt);

            if (route.Count > 0)
            {
                CreateDestRoute();
            }
            return destRoute;
        }

        private void CreateDestRoute()
        {
            if (route.Last.Value == icao)
            {
                route.RemoveLast();
            }

            string starName = route.Last.Value;
            StarInfo star;

            if (TryGetStar(starName, rwyWpt, out star))
            {
                route.RemoveLast();

                // Any STAR has at least one waypoint.
                destRoute.AddFirstWaypoint(star.FirstWaypoint, starName, star.TotalDistance);

                if (route.Last.Value == star.FirstWaypoint.ID &&
                    wptList.FindAllByWaypoint(star.FirstWaypoint).Count == 0)
                {
                    route.RemoveLast();
                }
            }
        }

        private bool TryGetStar(string StarName, Waypoint rwyWpt, out StarInfo result)
        {
            try
            {
                result = stars.GetStarInfo(StarName, rwy, rwyWpt);
                return true;
            }
            catch
            {
                // no Star in route
                result = null;
                return false;
            }
        }
    }
}
