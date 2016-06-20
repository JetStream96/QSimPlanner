using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;

namespace QSP.RouteFinding.RouteAnalyzers
{
    public static class RouteAnalyzerFacade
    {
        public static Route AnalyzeWithCommands(
            string route,
            string origIcao,
            string origRwy,
            string destIcao,
            string destRwy,
            string navDataLocation,
            AirportManager airportList,
            WaypointList wptList)
        {
            return new AnalyzerWithCommands(
                new CoordinateFormatter(route).Split(),
                origIcao,
                origRwy,
                destIcao,
                destRwy,
                airportList,
                wptList,
                SidHandlerFactory.GetHandler(
                    origIcao,
                    navDataLocation,
                    wptList,
                    wptList.GetEditor(),
                    airportList)
                    .SidCollection,
                StarHandlerFactory.GetHandler(
                    destIcao,
                    navDataLocation,
                    wptList,
                    wptList.GetEditor(),
                    airportList)
                    .StarCollection)
               .Analyze();
        }

        public static Route AnalyzeStandard(
            string route,
            string origIcao,
            string origRwy,
            string destIcao,
            string destRwy,
            string navDataLocation,
            AirportManager airportList,
            WaypointList wptList)
        {
            return new StandardRouteAnalyzer(
                new CoordinateFormatter(route).Split(),
                origIcao,
                origRwy,
                destIcao,
                destRwy,
                airportList,
                wptList,
                SidHandlerFactory.GetHandler(
                    origIcao, navDataLocation, wptList,
                    wptList.GetEditor(), airportList).SidCollection,
                StarHandlerFactory.GetHandler(
                    destIcao, navDataLocation, wptList,
                    wptList.GetEditor(), airportList).StarCollection)
                  .Analyze();
        }

        public static Route AnalyzeAutoSelect(
            string route,
            double preferredLat,
            double preferredLon,
            WaypointList wptList)
        {
            return new AutoSelectAnalyzer(
                new CoordinateFormatter(route).Split(),
                    preferredLat,
                    preferredLon,
                    wptList)
                   .Analyze();
        }
    }
}