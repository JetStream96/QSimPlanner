using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;

namespace QSP.RouteFinding.RouteAnalyzers
{
    public static class RouteAnalyzerFacade
    {
        public static Route AnalyzeWithCommands(string route,
                                                string origIcao,
                                                string origRwy,
                                                string destIcao,
                                                string destRwy,
                                                string navDataLocation,
                                                AirportManager airportList,
                                                WaypointList wptList)
        {
            return new AnalyzerWithCommands(new CoordinateFormatter(route).Split(),
                                            origIcao,
                                            origRwy,
                                            destIcao,
                                            destRwy,
                                            airportList,
                                            wptList,
                                            SidHandlerFactory.GetHandler(origIcao, navDataLocation, wptList, airportList),
                                            StarHandlerFactory.GetHandler(destIcao, navDataLocation, wptList, airportList))
                   .Analyze();
        }

        public static Route AnalyzeStandard(string route,
                                            string origIcao,
                                            string origRwy,
                                            string destIcao,
                                            string destRwy,
                                            string navDataLocation,
                                            AirportManager airportList,
                                            WaypointList wptList)
        {
            return new StandardRouteAnalyzer(new CoordinateFormatter(route).Split(),
                                             origIcao,
                                             origRwy,
                                             destIcao,
                                             destRwy,
                                             airportList,
                                             wptList,
                                             SidHandlerFactory.GetHandler(origIcao, navDataLocation, wptList, airportList).SidCollection,
                                             StarHandlerFactory.GetHandler(destIcao, navDataLocation, wptList, airportList).StarCollection)
                  .Analyze();
        }
    }



}
