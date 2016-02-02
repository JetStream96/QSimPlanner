using System.Collections.Generic;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;

namespace QSP.RouteFinding
{
    public class RouteFinderFacade
    {
        private WaypointList wptList;
        private AirportManager airportList;
        private string navDataLocation;
        private RouteFinder finder;

        public RouteFinderFacade(WaypointList wptList, AirportManager airportList, string navDataLocation)
        {
            this.wptList = wptList;
            this.airportList = airportList;
            this.navDataLocation = navDataLocation;
            finder = new RouteFinder(wptList, airportList);
        }

        public Route FindRoute(string origIcao, string origRwy, List<string> sids,
                               string destIcao, string destRwy, List<string> stars)
        {
            return finder.FindRoute(
                origRwy, 
                sids, 
                SidHandlerFactory.GetHandler(origIcao, navDataLocation, wptList, airportList),
                destRwy, 
                stars, 
                StarHandlerFactory.GetHandler(destIcao, navDataLocation, wptList, airportList));
        }

        public Route FindRoute(string icao, string rwy, List<string> sid, int wptIndex)
        {
            return finder.FindRoute(
                rwy,
                sid, 
                SidHandlerFactory.GetHandler(icao, navDataLocation, wptList, airportList), 
                wptIndex);
        }

        public Route FindRoute(int wptIndex, string icao, string rwy, List<string> star)
        {
            return finder.FindRoute(
                wptIndex,
                rwy, 
                star, 
                StarHandlerFactory.GetHandler(icao, navDataLocation, wptList, airportList));
        }
    }
}
