using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.IO;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.Utilities.ErrorLogger;

namespace QSP.NavData.AAX
{
    public class FixesLoader
    {
        private WaypointList wptList;

        public FixesLoader(WaypointList wptList)
        {
            this.wptList = wptList;
        }

        /// <summary>
        /// Loads all waypoints in waypoints.txt.
        /// </summary>
        /// <param name="filepath">Location of waypoints.txt</param>
        /// <exception cref="LoadWaypointFileException"></exception>
        public void ReadFromFile(string filepath)
        {
            string[] allLines = File.ReadAllLines(filepath);

            foreach (var i in allLines)
            {
                try
                {
                    if (i.Length == 0 || i[0] == ' ')
                    {
                        continue;
                    }
                    int pos = 0;

                    string id = ReadString(i, ref pos, ',');
                    double lat = ParseDouble(i, ref pos, ',');
                    double lon = ParseDouble(i, ref pos, ',');

                    wptList.AddWaypoint(new Waypoint(id, lat, lon));
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    //TODO: Write to log file. Show to user, etc.
                    throw new LoadWaypointFileException(
                        "Failed to load waypoints.txt.", ex);
                }
            }
        }
    }
}
