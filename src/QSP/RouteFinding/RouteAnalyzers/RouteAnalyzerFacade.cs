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
            var sidHandler = SidHandlerFactory.GetHandler(
                origIcao,
                navDataLocation,
                wptList,
                wptList.GetEditor(),
                airportList);

            var starHandler = StarHandlerFactory.GetHandler(
                destIcao,
                navDataLocation,
                wptList,
                wptList.GetEditor(),
                airportList);

            return new AnalyzerWithCommands(
                CoordinateFormatter.Split(route),
                origIcao,
                origRwy,
                destIcao,
                destRwy,
                airportList,
                wptList,
                wptList.GetEditor(),
                sidHandler.SidCollection,
                starHandler.StarCollection).Analyze();
        }
    }
}