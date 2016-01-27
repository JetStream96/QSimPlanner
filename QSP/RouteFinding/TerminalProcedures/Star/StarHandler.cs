using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures.Star
{

    public class StarHandler
    {
        private string icao;
        private WaypointList wptList;
        private AirportManager airportList;

        public StarCollection StarCollection { get; private set; }
        
        public StarHandler(string icao, string allTxt, WaypointList wptList, AirportManager airportList)
        {
            this.icao = icao;
            this.wptList = wptList;
            this.airportList = airportList;

            StarCollection = new StarReader(allTxt).Parse();
        }
        
        /// <summary>
        /// Find all STARs available for the runway. Two STARs only different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetStarList(string rwy)
        {
            return StarCollection.GetStarList(rwy);
        }

        /// <summary>
        /// Add necessary waypoints and neighbors for STAR computation to WptList, and returns the index of Dest. rwy in WptList.
        /// </summary>
        public int AddStarsToWptList(string rwy, List<string> star)
        {
            return new StarAdder(icao, StarCollection).AddStarsToWptList(rwy, star);
        }

        /// <summary>
        /// Returns total distance of the STAR and the last wpt, regardless whether the last wpt is in wptList.
        /// If there isn't any waypoint in the STAR (e.g. a vector after takeoff), this returns a distance of 0.0   
        /// and the origin runway (e.g. KLAX25L).       
        /// </summary>
        /// <param name="rwy">The runway identifier. e.g. 25R </param>
        /// <param name="origRwy">The waypoint representing the origin runway.</param>
        /// <exception cref="StarNotFoundException"></exception>
        public StarInfo InfoForAnalysis(string rwy, string star)
        {
            return StarCollection.GetStarInfo(star, rwy, new Waypoint(icao + rwy, airportList.RwyLatLon(icao, rwy)));
        }
    }

}
