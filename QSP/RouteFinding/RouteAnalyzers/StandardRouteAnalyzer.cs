using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Airports;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;
using static QSP.MathTools.Utilities;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Utilizes BasicRouteAnalyzer, with the additional functionality of reading airports and SIDs/STARs.
    //
    // 1. Input: The string array, consisting of airport icao code (ICAO), airway (AWY), 
    //    and/or waypoint (WPT) sysbols.
    //           
    // 2. All characters should be capital.
    //
    // 3. Format: {ICAO, SID, WPT, AWY, WPT, ... , WPT, STAR, ICAO}
    //    (1) First ICAO must be identical to origin icao code. Last ICAO must be identical to dest icao code.
    //    (2) Both ICAO can be omitted.
    //    (3) If an airway is DCT, it should be omitted. The route will be a direct between the two waypoints.
    //    (4) SID/STAR can be omitted. The route will be a direct from/to airport.
    //
    // 4. All cases of SID/STAR are handled. These cases are handled by SidHandler and StarHandler.
    //
    // 5. If the format is wrong, an InvalidIdentifierException will be thrown with an message describing 
    //    the place where the problem occurs.
    //
    // 6. It's not allowed to direct from one waypoint to another which is more than 500 nm away.
    //    Otherwise an WaypointTooFarException will be thrown.
    //

    public class StandardRouteAnalyzer
    {
        private AirportManager airportList;
        private SidCollection sids;
        private StarCollection stars;

        private string origIcao;
        private string origRwy;
        private string destIcao;
        private string destRwy;
        private string[] route;

        private Route origPart;
        private Route destPart;

        public StandardRouteAnalyzer(string[] route,
                                     string origIcao,
                                     string origRwy,
                                     string destIcao,
                                     string destRwy,
                                     AirportManager airportList,
                                     SidCollection sids,
                                     StarCollection stars)
        {
            this.route = route;
            this.origIcao = origIcao;
            this.origRwy = origRwy;
            this.destIcao = destIcao;
            this.destRwy = destRwy;
            this.airportList = airportList;
            this.sids = sids;
            this.stars = stars;
        }

        public Route Analyze()
        {

        }

    }
}
