using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data;
using System.Collections.Generic;
using static QSP.RouteFinding.WaypointAirwayConnector;
using static QSP.Utilities.LoggerInstance;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // Adds necessary waypoints and neighbors to wptList for automatic 
    // route finder or route analyzer, and returns the index of origin 
    // rwy in wptList.
    // 
    // This breaks down into 5 cases:
    // 1. There's no SID at all. 
    // 2. The SID ends with a vector.
    // 3. The SID ends with a waypoint. The waypoint is in wptList but 
    //    not connected to an airway.
    // 4. The SID ends with a waypoint which is connected to an airway. 
    // 5. The SID ends with a waypoint. The waypoint is NOT in wptList.
    //  

    public class SidAdder
    {
        private string icao;
        private WaypointList wptList;
        private WaypointListEditor editor;
        private AirportManager airportList;
        private SidCollection sids;
        private WptSearchOption option;

        public SidAdder(
            string icao,
            SidCollection sids,
            WaypointList wptList,
            WaypointListEditor editor,
            AirportManager airportList)
            : this(
                  icao,
                  sids,
                  wptList,
                  editor,
                  airportList,
                  new WptSearchOption())
        { }

        public SidAdder(
            string icao,
            SidCollection sids,
            WaypointList wptList,
            WaypointListEditor editor,
            AirportManager airportList,
            WptSearchOption option)
        {
            this.icao = icao;
            this.sids = sids;
            this.wptList = wptList;
            this.editor = editor;
            this.airportList = airportList;
            this.option = option;
        }

        // The 5 different cases are treated seperately.
        // The corresponding actions for each case are:
        //
        // (In all cases, origin runway is added to wptList.)
        //
        // Case 1. Adds neighbors* (use DCT as airway) to the origin runway.      
        // Case 2. Finds neighbors of the last waypoint in SID and add them 
        //         as neighbors of the origin runway (use SID name as airway).
        // Case 3. Same as case 4. But also finds neighbors of the last 
        //         waypoint, and add them as neighbors (use DCT as airway).
        // Case 4. Adds the last waypoint as a neighbor of the origin runway
        //         (use SID name as airway).
        // Case 5. Same as case 3. But also adds the last waypoint to wptList.
        //
        // * Using WptSearchOption.
        //

        /// <summary>
        /// Add necessary waypoints and neighbors for SID computation to 
        /// WptList, and returns the index of Orig. rwy in WptList.
        /// </summary>
        public int AddSidsToWptList(string rwy, List<string> sidsToAdd)
        {
            if (sidsToAdd.Count == 0)
            {
                // Case 1
                return ProcessCase1(rwy);
            }
            else
            {
                // Case 2, 3, 4, 5
                var latLon = airportList.RwyLatLon(icao, rwy);
                var wpt = new Waypoint(icao + rwy, latLon);
                int index = editor.AddWaypoint(wpt);

                foreach (var i in sidsToAdd)
                {
                    try
                    {
                        // this is where case 2, 3, 4, 5 are handled.  
                        AddToWptList(index, rwy, i);
                    }
                    catch (WaypointNotFoundException ex)
                    {
                        WriteToLog(ex.ToString());
                    }
                }
                return index;
            }
        }

        private int ProcessCase1(string rwy)
        {
            var rwyLatLon = airportList.RwyLatLon(icao, rwy);
            var nearbyWpts = AirwayConnections(rwyLatLon.Lat, rwyLatLon.Lon);

            int index = editor.AddWaypoint(new Waypoint(icao + rwy, rwyLatLon));

            foreach (var i in nearbyWpts)
            {
                editor.AddNeighbor(index, i.Index, new Neighbor("DCT", i.Distance));
            }

            return index;
        }

        /// <exception cref="WaypointNotFoundException"></exception>
        private void AddToWptList(int rwyIndex, string rwy, string sid)
        {
            var sidInfo = sids.GetSidInfo(sid, rwy, wptList[rwyIndex]);
            var lastWpt = sidInfo.LastWaypoint;

            if (sidInfo.EndsWithVector)
            {
                // Case 2 
                ProcessCase2(rwyIndex, sid, lastWpt, sidInfo.TotalDistance);
            }
            else
            {
                // Case 3, 4, 5
                int lastWptIndex = wptList.FindByWaypoint(lastWpt);

                if (lastWptIndex < 0)
                {
                    // Case 5
                    lastWptIndex = editor.AddWaypoint(lastWpt);
                }

                if (wptList.EdgesFromCount(lastWptIndex) == 0)
                {
                    // Case 3                                  
                    foreach (var k in AirwayConnections(lastWpt.Lat, lastWpt.Lon))
                    {
                        editor.AddNeighbor(
                            lastWptIndex,
                            k.Index,
                            new Neighbor("DCT", k.Distance));
                    }
                }
                // Forcase 3, 4 and 5
                editor.AddNeighbor(
                    rwyIndex,
                    lastWptIndex,
                    new Neighbor(sid, sidInfo.TotalDistance));
            }
        }

        private void ProcessCase2(
            int rwyIndex, string sid, Waypoint lastWpt, double disAdd)
        {
            var endPoints = AirwayConnections(lastWpt.Lat, lastWpt.Lon);

            foreach (var i in endPoints)
            {
                editor.AddNeighbor(
                    rwyIndex, i.Index, new Neighbor(sid, i.Distance + disAdd));
            }
        }

        private List<IndexDistancePair> AirwayConnections(
            double lat, double lon)
        {
            return ToAirway(lat, lon, wptList, option);
        }

    }
}
