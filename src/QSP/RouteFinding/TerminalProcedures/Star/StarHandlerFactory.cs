using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using System;
using System.IO;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    public static class StarHandlerFactory
    {
        /// <exception cref="LoadStarFileException"></exception>
        public static StarHandler GetHandler(
            string icao,
            string navDataLocation,
            WaypointList wptList,
            WaypointListEditor editor,
            AirportManager airportList)
        {
            string fileLocation = navDataLocation + "\\PROC\\" + icao + ".txt";

            try
            {
                string allTxt = File.ReadAllText(fileLocation);
                return new StarHandler(icao, allTxt, wptList, editor, airportList);
            }
            catch (Exception ex)
            {
                throw new LoadStarFileException(
                    "Failed to read " + fileLocation + ".", ex);
            }
        }
    }
}
