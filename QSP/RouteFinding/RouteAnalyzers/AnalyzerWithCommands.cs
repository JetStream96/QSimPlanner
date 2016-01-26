using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using static QSP.RouteFinding.Tracks.Common.Utilities;

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
        private SidCollection sids;
        private StarCollection stars;

        private string origIcao;
        private string origRwy;
        private string destIcao;
        private string destRwy;
        private LinkedList<string> route;

        private Waypoint origRwyWpt;
        private Waypoint destRwyWpt;

        public AnalyzerWithCommands(string[] route,
                                     string origIcao,
                                     string origRwy,
                                     string destIcao,
                                     string destRwy,
                                     AirportManager airportList,
                                     WaypointList wptList,
                                     SidCollection sids,
                                     StarCollection stars)
        {
            this.route = new LinkedList<string>(route);
            this.origIcao = origIcao;
            this.origRwy = origRwy;
            this.destIcao = destIcao;
            this.destRwy = destRwy;
            this.airportList = airportList;
            this.wptList = wptList;
            this.sids = sids;
            this.stars = stars;
        }

        public Route Analyze()
        {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
