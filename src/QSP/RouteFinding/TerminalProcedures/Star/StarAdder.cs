using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;
using static QSP.RouteFinding.WaypointAirwayConnector;
using static QSP.Utilities.LoggerInstance;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // Adds necessary waypoints and neighbors to wptList for automatic route 
    // finder or route analyzer, and returns the index of origin rwy 
    // in wptList.
    // 
    // There are 4 cases:
    // 1. There's no STAR at all. 
    // 2. The first wpt in STAR is NOT connected to an airway but in wptList.
    // 3. The first wpt in STAR is connected to an airway.
    // 4. The first wpt in STAR is NOT connected to an airway and NOT 
    //    in wptList.
    //  
    public class StarAdder
    {
        private string icao;
        private WaypointList wptList;
        private WaypointListEditor editor;
        private AirportManager airportList;
        private StarCollection stars;
        private WptSearchOption option;

        public StarAdder(
            string icao,
            StarCollection stars,
            WaypointList wptList,
            WaypointListEditor editor,
            AirportManager airportList)
            : this(
                  icao,
                  stars,
                  wptList,
                  editor,
                  airportList,
                  new WptSearchOption())
        { }

        public StarAdder(
            string icao,
            StarCollection stars,
            WaypointList wptList,
            WaypointListEditor editor,
            AirportManager airportList,
            WptSearchOption option)
        {
            this.icao = icao;
            this.stars = stars;
            this.wptList = wptList;
            this.editor = editor;
            this.airportList = airportList;
            this.option = option;
        }

        // The 4 different cases are treated seperately.
        // The corresponding actions for each case are:
        //
        // (In all cases, destination runway is added to wptList.)
        //
        // Case 1. Adds destination runway as neighbors of some waypoints* 
        //         (use DCT as airway).      
        // Case 2. Finds neighbors of the first waypoint in STAR, and add 
        //         first waypoint in STAR as their neighbors 
        //         (use STAR name as airway).
        // Case 3. Adds the destination runway as a neighbor of first 
        //         waypoint in STAR (use STAR name as airway).
        // Case 4. Same as case 2. But also adds the first waypoint to wptList.
        //
        // * Using WptSearchOption.
        //

        /// <summary>
        /// Add necessary waypoints and neighbors for STAR computation 
        /// to WptList, and returns the index of Dest. rwy in WptList.
        /// </summary>
        public int AddStarsToWptList(string rwy, List<string> starsToAdd)
        {
            // Case 1
            if (starsToAdd.Count == 0) return ProcessCase1(rwy);

            // Case 2, 3, 4
            var latLon = airportList.FindRwy(icao, rwy);
            var wpt = new Waypoint(icao + rwy, latLon);
            int index = editor.AddWaypoint(wpt);

            foreach (var i in starsToAdd)
            {
                try
                {
                    // this is where case 2, 3, 4 are handled.
                    AddToWptList(index, rwy, i);
                }
                catch (WaypointNotFoundException ex)
                {
                    WriteToLog(ex);
                }
            }

            return index;
        }

        private int ProcessCase1(string rwy)
        {
            var rwyLatLon = airportList.FindRwy(icao, rwy);
            var nearbyWpts = AirwayConnections(rwyLatLon);

            int index = editor.AddWaypoint(new Waypoint(icao + rwy, rwyLatLon));

            foreach (var i in nearbyWpts)
            {
                var neighbor = new Neighbor("DCT", i.Distance);
                editor.AddNeighbor(i.Index, index, neighbor);
            }

            return index;
        }

        /// <exception cref="WaypointNotFoundException"></exception>
        private void AddToWptList(int rwyIndex, string rwy, string star)
        {
            var starWpts = stars.StarWaypoints(star, rwy, wptList[rwyIndex]);
            var firstWpt = starWpts[0];
            int firstWptIndex = wptList.FindByWaypoint(firstWpt);

            if (firstWptIndex < 0)
            {
                // Case 4
                firstWptIndex = editor.AddWaypoint(firstWpt);
            }

            if (wptList.EdgesToCount(firstWptIndex) == 0)
            {
                // Case 2                                 
                foreach (var k in AirwayConnections(firstWpt))
                {
                    var n = new Neighbor("DCT", k.Distance);
                    editor.AddNeighbor(k.Index, firstWptIndex, n);
                }
            }
            // For case 2, 3 and 4
            var neighbor = new Neighbor(
                star, 
                starWpts.TotalDistance(), 
                starWpts.WithoutFirstAndLast());

            editor.AddNeighbor(firstWptIndex, rwyIndex, neighbor);
        }

        private List<IndexDistancePair> AirwayConnections(ICoordinate item)
        {
            return FromAirway(item.Lat, item.Lon, wptList, option);
        }

    }
}
