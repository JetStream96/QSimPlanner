using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Linq;
using RouteString = System.Collections.Generic.IReadOnlyList<string>;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a RouteString, Extract() returns an object 
    // containing:
    //
    // * An DestRoute containing the departure runway and STAR
    //   (if STAR exists).
    //   There are 3 cases:
    //   1. The last element of input RouteString is not a STAR.
    //   2. The first waypoint of STAR is in wptList, which
    //      may or may not be connected to an airway.
    //   3. The first waypoint of STAR is NOT in wptList.

    //   For different cases, the returning route contains:
    //   Case 1. The last enroute waypoint, then direct to the dest runway.
    //   Case 2. The first waypoint of STAR, then go to the dest runway
    //           (via STAR).
    //   Case 3. The last enroute waypoint, then direct to the first waypoint 
    //           of STAR. Then go to the dest runway (via STAR).
    //
    // * A RemainingRoute. In all cases, the last entry in RemainingRoute
    //   is guranteed to be the same as the first waypoint in the DestRoute.
    //
    // The input route should not contain the origin ICAO, and must contain 
    // one element.

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
            { RemainingRoute = route.ToList(), DestRoute = destRoute };
        }

        public class ExtractResult
        {
            public RouteString RemainingRoute;
            public Route DestRoute;
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
