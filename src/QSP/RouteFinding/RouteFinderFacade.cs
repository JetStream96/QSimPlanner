using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.WindAloft;
using System.Collections.Generic;

namespace QSP.RouteFinding
{
    public class RouteFinderFacade
    {
        private WaypointList wptList;
        private AirportManager airportList;
        private string navDataLocation;
        private RouteFinder finder;

        public RouteFinderFacade(
            WaypointList wptList,
            AirportManager airportList,
            string navDataLocation = null,
            CountryCodeCollection avoidedCountry = null,
            AvgWindCalculator windCalc = null)
        {
            this.wptList = wptList;
            this.airportList = airportList;
            this.navDataLocation = navDataLocation;
            finder = new RouteFinder(wptList, avoidedCountry, windCalc);
        }

        public Route FindRoute(
            string origIcao, string origRwy, List<string> sids,
            string destIcao, string destRwy, List<string> stars)
        {
            var editor = wptList.GetEditor();
            var sidHandler = SidHandlerFactory.GetHandler(
                origIcao,
                navDataLocation,
                wptList,
                editor,
                airportList);

            var starHandler = StarHandlerFactory.GetHandler(
                destIcao,
                navDataLocation,
                wptList,
                editor,
                airportList);

            return finder.FindRoute(
                origRwy,
                sids,
                sidHandler,
                destRwy,
                stars,
                starHandler,
                editor);
        }

        public Route FindRoute(
            string origIcao, string origRwy,
            SidCollection sidCol, List<string> sids,
            string destIcao, string destRwy,
            StarCollection starCol, List<string> stars)
        {
            var editor = wptList.GetEditor();
            var sidHandler = new SidHandler(
                origIcao, sidCol, wptList, editor, airportList);

            var starHandler = new StarHandler(
                destIcao, starCol, wptList, editor, airportList);

            return finder.FindRoute(
                origRwy,
                sids,
                sidHandler,
                destRwy,
                stars,
                starHandler,
                editor);
        }

        public Route FindRoute(
            string icao, string rwy, List<string> sid, int wptIndex)
        {
            var editor = wptList.GetEditor();
            var sidHandler = SidHandlerFactory.GetHandler(
                icao,
                navDataLocation,
                wptList,
                editor,
                airportList);

            return finder.FindRoute(
                rwy,
                sid,
                sidHandler,
                wptIndex,
                editor);
        }

        public Route FindRoute(
            string icao, string rwy, SidCollection sidCol,
            List<string> sid, int wptIndex)
        {
            var editor = wptList.GetEditor();

            return finder.FindRoute(
                rwy,
                sid,
                new SidHandler(icao, sidCol, wptList, editor, airportList),
                wptIndex,
                editor);
        }

        public Route FindRoute(
            int wptIndex, string icao, string rwy, List<string> star)
        {
            var editor = wptList.GetEditor();
            var starHandler = StarHandlerFactory.GetHandler(
                icao,
                navDataLocation,
                wptList,
                editor,
                airportList);

            return finder.FindRoute(
                wptIndex,
                rwy,
                star,
                starHandler,
                editor);
        }

        public Route FindRoute(
            int wptIndex, string icao, string rwy,
            StarCollection starCol, List<string> star)
        {
            var editor = wptList.GetEditor();

            return finder.FindRoute(
                wptIndex,
                rwy,
                star,
                new StarHandler(icao, starCol, wptList, editor, airportList),
                editor);
        }
    }
}
