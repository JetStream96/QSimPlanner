using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using static QSP.Core.QspCore;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding.TerminalProcedures.Star
{

    public class StarHandler
    {
        private string icao;
        private WaypointList wptList;
        private AirportManager airportList;
        private StarCollection starCollection;

        //TODO: remove this
        public StarHandler(string icao) : this(icao, AppSettings.NavDBLocation, WptList, AirportList)
        {
        }

        public StarHandler(string icao, string navDBLocation, WaypointList wptList, AirportManager airportList)
        {
            this.icao = icao;
            this.wptList = wptList;
            this.airportList = airportList;
            ReadFromFile(navDBLocation);
        }

        /// <param name="navDBLocation">The file path, which is e.g., PROC\RCTP.txt\</param>
        /// <exception cref="LoadStarFileException"></exception>
        private void ReadFromFile(string navDBLocation)
        {
            string fileLocation = navDBLocation + "\\PROC\\" + icao + ".txt";
            string allTxt = null;

            try
            {
                allTxt = File.ReadAllText(fileLocation);
            }
            catch (Exception ex)
            {
                throw new LoadStarFileException("Failed to read " + fileLocation + ".", ex);
            }

            starCollection = new StarReader(allTxt).Parse();
        }

        /// <summary>
        /// Find all STARs available for the runway. Two STARs only different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetStarList(string rwy)
        {
            return starCollection.GetStarList(rwy);
        }

        /// <summary>
        /// Add necessary waypoints and neighbors for STAR computation to WptList, and returns the index of Dest. rwy in WptList.
        /// </summary>
        public int AddStarsToWptList(string rwy, List<string> star)
        {
            return new StarAdder(icao, starCollection).AddStarsToWptList(rwy, star);
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
            return starCollection.GetStarInfo(star, rwy, new Waypoint(icao + rwy, airportList.RwyLatLon(icao, rwy)));
        }
    }

}
