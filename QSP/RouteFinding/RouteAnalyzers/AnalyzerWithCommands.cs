using QSP.AviationTools.Coordinates;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // The accepted formats are similar to those of StandardRouteAnalyzer, except:
    //
    // (1) The first (or the one right after Origin), last (or the one right before Dest.), 
    //     or any entry between two waypoints, can be "AUTO" or "RAND".
    //
    // (2) "AUTO" finds the shortest route between the specified waypoints.
    //     If it's the first entry, then a route between departure runway and first 
    //     waypoint is found. The case for last entry is similar.
    //
    // (3) Similarly, "RAND" finds a random route.
    //

    public class AnalyzerWithCommands
    {
        private WaypointList wptList;
        private AirportManager airportList;
        private SidHandler sidHandler;
        private StarHandler starHandler;

        private string origIcao;
        private string origRwy;
        private string destIcao;
        private string destRwy;
        private string[] route;

        private Waypoint origRwyWpt;
        private Waypoint destRwyWpt;

        public AnalyzerWithCommands(string[] route,
                                     string origIcao,
                                     string origRwy,
                                     string destIcao,
                                     string destRwy,
                                     AirportManager airportList,
                                     WaypointList wptList,
                                     SidHandler sidHandler,
                                     StarHandler starHandler)
        {
            this.route = route;
            this.origIcao = origIcao;
            this.origRwy = origRwy;
            this.destIcao = destIcao;
            this.destRwy = destRwy;
            this.airportList = airportList;
            this.wptList = wptList;
            this.sidHandler = sidHandler;
            this.starHandler = starHandler;
        }

        public Route Analyze()
        {
            setRwyWpts();
            var subRoutes = splitEntries(route);
            var analyzed = computeRoutes(subRoutes);
            fillCommands(subRoutes, analyzed);
            return connectAll(analyzed);
        }

        private static List<List<string>> splitEntries(string[] route)
        {
            var subRoutes = new List<List<string>>();
            var tmp = new List<string>();

            foreach (var i in route)
            {
                if (i == "AUTO" || i == "RAND")
                {
                    addIfNonEmpty(subRoutes, ref tmp);
                    subRoutes.Add(new List<string> { i });
                }
                else
                {
                    tmp.Add(i);
                }
            }
            addIfNonEmpty(subRoutes, ref tmp);

            return subRoutes;
        }

        private static void addIfNonEmpty(List<List<string>> subRoutes, ref List<string> tmp)
        {
            if (tmp.Count > 0)
            {
                subRoutes.Add(tmp);
                tmp = new List<string>();
            }
        }

        private void setRwyWpts()
        {
            origRwyWpt = new Waypoint(origIcao + origRwy, airportList.RwyLatLon(origIcao, origRwy));
            destRwyWpt = new Waypoint(destIcao + destRwy, airportList.RwyLatLon(destIcao, destRwy));
        }

        private List<Route> computeRoutes(List<List<string>> subRoutes)
        {
            var result = new List<Route>();

            for (int i = 0; i < subRoutes.Count; i++)
            {
                var route = new LinkedList<string>(subRoutes[i]);

                if (route.Count == 1 &&
                    (route.First.Value == "AUTO" || route.First.Value == "RAND"))
                {
                    result.Add(null);
                }
                else
                {
                    Route origRoute = null;
                    Route destRoute = null;

                    if (i == 0)
                    {
                        origRoute = new SidExtractor(route, origIcao, origRwy, origRwyWpt, wptList, sidHandler.SidCollection)
                                    .Extract();
                    }

                    if (i == subRoutes.Count - 1)
                    {
                        destRoute = new StarExtractor(route, destIcao, destRwy, destRwyWpt, wptList, starHandler.StarCollection)
                                    .Extract();
                    }

                    var mainRoute = new AutoSelectAnalyzer(route.ToArray(), origRwyWpt.Lat, origRwyWpt.Lon, wptList)
                                    .Analyze();

                    result.Add(appendRoute(origRoute, appendRoute(mainRoute, destRoute)));
                }
            }
            return result;
        }

        private static Route appendRoute(Route original, Route routeToAppend)
        {
            if (original == null)
            {
                return routeToAppend;
            }

            if (routeToAppend == null)
            {
                return original;
            }

            original.MergeWith(routeToAppend);
            return original;
        }

        private void fillCommands(List<List<string>> subRoutes, List<Route> analyzed)
        {
            for (int i = 0; i < subRoutes.Count; i++)
            {
                if (analyzed[i] == null)
                {
                    var startEnd = getStartEndWpts(analyzed, i);

                    if (subRoutes[i][0] == "AUTO")
                    {
                        analyzed[i] = findRoute(analyzed, i);
                    }
                    else
                    {
                        // RAND
                        var randRoute = randRouteToRoute(new RandomRouteFinder(startEnd.Item1.LatLon,
                                                                             startEnd.Item2.LatLon)
                                                       .Find());
                        randRouteAddOrigDest(randRoute, analyzed, i);
                        analyzed[i] = randRoute;
                    }
                }
            }
        }

        private Route findRoute(List<Route> analyzed, int index)
        {
            var routeFinder = new RouteFinder(wptList, airportList);

            if (index == 0)
            {
                if (index == analyzed.Count - 1)
                {
                    return routeFinder
                          .FindRoute(origIcao, origRwy, sidHandler.SidCollection.GetSidList(origRwy), sidHandler,
                                     destIcao, destRwy, starHandler.StarCollection.GetStarList(destRwy), starHandler);
                }
                else
                {
                    int wptTo = wptList.FindByWaypoint(analyzed[index + 1].First.Waypoint);

                    return routeFinder
                           .FindRoute(origIcao, origRwy, sidHandler.SidCollection.GetSidList(origRwy), sidHandler,
                                      wptTo);
                }
            }
            else
            {
                if (index == analyzed.Count - 1)
                {
                    int wptFrom = wptList.FindByWaypoint(analyzed[index - 1].Last.Waypoint);

                    return routeFinder
                          .FindRoute(wptFrom,
                                     destIcao, destRwy, starHandler.StarCollection.GetStarList(destRwy), starHandler);
                }
                else
                {
                    int wptFrom = wptList.FindByWaypoint(analyzed[index - 1].Last.Waypoint);
                    int wptTo = wptList.FindByWaypoint(analyzed[index + 1].First.Waypoint);

                    return routeFinder.FindRoute(wptFrom, wptTo);
                }
            }
        }

        private void randRouteAddOrigDest(Route route, List<Route> analyzed, int index)
        {
            if (index == 0)
            {
                route.AddFirstWaypoint(origRwyWpt, "DCT", true);
            }

            if (index == analyzed.Count - 1)
            {
                route.AddLastWaypoint(destRwyWpt, "DCT", true);
            }
        }

        private static Route randRouteToRoute(List<LatLon> randRoute)
        {
            var result = new Route();

            if (randRoute.Count > 2)
            {
                for (int i = 1; i < randRoute.Count - 1; i++)
                {
                    double lat = randRoute[i].Lat;
                    double lon = randRoute[i].Lon;

                    result.AddLastWaypoint(new Waypoint(Format5Letter.To5LetterFormat(lat, lon), lat, lon), "DCT", true);
                }
            }
            return result;
        }

        private Pair<Waypoint, Waypoint> getStartEndWpts(List<Route> subRoutes, int index)
        {
            var start = index == 0
                ? origRwyWpt
                : subRoutes[index - 1].Last.Waypoint;

            var end = index == subRoutes.Count - 1
                ? destRwyWpt
                : subRoutes[index + 1].First.Waypoint;

            return new Pair<Waypoint, Waypoint>(start, end);
        }

        private static Route connectAll(List<Route> subRoutes)
        {
            var route = subRoutes[0];

            for (int i = 1; i < subRoutes.Count; i++)
            {
                route.MergeWith(subRoutes[i]);
            }
            return route;
        }
    }
}
