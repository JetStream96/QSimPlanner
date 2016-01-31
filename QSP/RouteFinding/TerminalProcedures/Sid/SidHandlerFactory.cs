using System;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using System.IO;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public static class SidHandlerFactory
    {
        public static SidHandler GetHandler(string icao, string navDataLocation, WaypointList wptList, AirportManager airportList)
        {
            string fileLocation = navDataLocation + "\\PROC\\" + icao + ".txt";

            try
            {
                string allTxt = File.ReadAllText(fileLocation);
                return new SidHandler(icao, allTxt, wptList, wptList.GetEditor(), airportList);
            }
            catch (Exception ex)
            {
                throw new LoadSidFileException("Failed to read " + fileLocation + ".", ex);
            }
        }
    }
}
