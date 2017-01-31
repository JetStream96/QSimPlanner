using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.IO;
using System.Linq;
using static QSP.Utilities.LoggerInstance;

namespace QSP.NavData.AAX
{
    public static class AtsFileLoader
    {
        /// <summary>
        /// Read all waypoints from ats.txt file.
        /// </summary>
        /// <param name="filepath">Path of ats.txt</param>
        /// <exception cref="WaypointFileReadException"></exception>
        public static void ReadFromFile(WaypointList wptList, string filepath)
        {
            try
            {
                //add error handling
                var allLines = File.ReadLines(filepath);
                string currentAirway = "";

                foreach (var i in allLines)
                {
                    var words = i.Split(',').Select(s => s.Trim()).ToList();

                    if (words.Count == 0) continue;

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
                        if (index2 <= 0) index2 = wptList.AddWaypoint(secondWpt);
                        
                        // Add first waypoint as required
                        if (index1 < 0) index1 = wptList.AddWaypoint(firstWpt);

                        // Add the connection.
                        var neighbor = new Neighbor(currentAirway, dis);

                        wptList.AddNeighbor(index1, index2, neighbor);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                throw new WaypointFileReadException("Failed to load ats.txt.", ex);
            }
        }
    }
}
