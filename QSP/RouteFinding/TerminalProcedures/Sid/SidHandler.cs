using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidHandler
    {
        private string icao;
        private WaypointList wptList;
        private AirportManager airportList;

        public SidCollection SidCollection { get; private set; }
        
        public SidHandler(string icao, string allTxt, WaypointList wptList, AirportManager airportList)
            :this(icao,new SidReader(allTxt).Parse(), wptList, airportList)
        { }
        
        public SidHandler(string icao, SidCollection SidCollection, WaypointList wptList, AirportManager airportList)
        {
            this.icao = icao;
            this.wptList = wptList;
            this.airportList = airportList;
            this.SidCollection = SidCollection;
        }

        /// <summary>
        /// Find all SID available for the runway. Two SIDs only different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetSidList(string rwy)
        {
            return SidCollection.GetSidList(rwy);
        }

        /// <summary>
        /// Add necessary waypoints and neighbors for SID computation to WptList, and returns the index of Orig. rwy in WptList.
        /// </summary>
        public int AddSidsToWptList(string rwy, List<string> sid)
        {
            return new SidAdder(icao, SidCollection, wptList, airportList).AddSidsToWptList(rwy, sid);
        }

        /// <summary>
        /// Returns total distance of the SID and the last wpt, regardless whether the last wpt is in wptList.
        /// If there isn't any waypoint in the SID (e.g. a vector after takeoff), this returns a distance of 0.0   
        /// and the origin runway (e.g. KLAX25L).       
        /// </summary>
        /// <param name="rwy">The runway identifier. e.g. 25R </param>
        /// <param name="origRwy">The waypoint representing the origin runway.</param>
        /// <exception cref="SidNotFoundException"></exception>
        public SidInfo InfoForAnalysis(string rwy, string sid)
        {
            return SidCollection.GetSidInfo(sid, rwy, new Waypoint(icao + rwy, airportList.RwyLatLon(icao, rwy)));
        }
    }
}
