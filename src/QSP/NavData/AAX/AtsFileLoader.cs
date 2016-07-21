using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.IO;
using static QSP.Utilities.LoggerInstance;

namespace QSP.NavData.AAX
{
    public class AtsFileLoader
    {
        private static readonly char[] delimiters =
               new char[] { ',', ' ', '\t' };

        private WaypointList wptList;

        public AtsFileLoader(WaypointList wptList)
        {
            this.wptList = wptList;
        }

        /// <summary>
        /// Read all waypoints from ats.txt file.
        /// </summary>
        /// <param name="filepath">Path of ats.txt</param>
        /// <exception cref="WaypointFileReadException"></exception>
        public void ReadFromFile(string filepath)
        {
            try
            {
                //add error handling
                var allLines = File.ReadLines(filepath);
                string currentAirway = "";

                foreach (var i in allLines)
                {
                    var words = i.Split(
                        delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length == 0)
                    {
                        continue;
                    }

                    if (words[0] == "A")
                    {
                        // This line is an airway identifier
                        currentAirway = words[1];
                    }
                    else if (words[0] == "S")
                    {
                        // This line is waypoint
                        Waypoint firstWpt = new Waypoint(
                            words[1],
                            double.Parse(words[2]),
                            double.Parse(words[3]));

                        Waypoint secondWpt = new Waypoint(
                            words[4],
                            double.Parse(words[5]),
                            double.Parse(words[6]));

                        int index1 = wptList.FindByWaypoint(firstWpt);
                        int index2 = wptList.FindByWaypoint(secondWpt);

                        // The next two are headings between two wpts. 
                        // Will be skipped.

                        double dis = double.Parse(words[9]);

                        // Add second waypoint as required
                        if (index2 <= 0)
                        {
                            index2 = wptList.AddWaypoint(secondWpt);
                        }

                        // Add first waypoint as required
                        if (index1 < 0)
                        {
                            index1 = wptList.AddWaypoint(firstWpt);
                        }

                        // Add the connection.
                        var neighbor = new Neighbor(
                            currentAirway, AirwayType.Enroute, dis);

                        wptList.AddNeighbor(index1, index2, neighbor);

                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);

                throw new WaypointFileReadException(
                    "Failed to load ats.txt.", ex);
            }
        }
    }
}
