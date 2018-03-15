using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.NavData.AAX
{
    public static class AtsFileLoader
    {
        /// <summary>
        /// Read all waypoints from ats.txt file. 
        /// Returns the error message if failed to parse some lines, or null.
        /// </summary>
        /// <param name="filepath">Path of ats.txt</param>
        /// <exception cref="WaypointFileReadException"></exception>
        public static string ReadFromFile(WaypointList wptList, string filepath)
        {
            try
            {
                var allLines = File.ReadLines(filepath);
                var errors = Read(wptList, allLines);
                return ReadFileErrorMsg.ErrorMsg(errors, "ats.txt");
            }
            catch (Exception e)
            {
                throw new WaypointFileReadException("Failed to read ats.txt.", e);
            }
        }

        // Reads the ATS.txt into the given WaypointList.
        // If the waypoints on a line is not found in wptList, it's added.
        // This method continues to read if an parsing error is encountered on a line.
        //
        public static IReadOnlyList<ReadFileError> Read(WaypointList wptList,
            IEnumerable<string> allLines)
        {
            var errors = new List<ReadFileError>();
            string currentAirway = "";

            allLines.ForEach((line, index) =>
            {
                var lineNum = index + 1;
                var words = line.Split(',').Select(s => s.Trim()).ToList();

                if (words.Count == 0) return;

                try
                {
                    if (words[0] == "A")
                    {
                        // This line is an airway identifier
                        currentAirway = words[1];
                    }
                    else if (words[0] == "S")
                    {
                        // This line is waypoint
                        var firstWpt = new Waypoint(
                            words[1], double.Parse(words[2]), double.Parse(words[3]));

                        var secondWpt = new Waypoint(
                            words[4], double.Parse(words[5]), double.Parse(words[6]));

                        int index1 = wptList.FindByWaypoint(firstWpt);
                        int index2 = wptList.FindByWaypoint(secondWpt);

                        // words[7] and words[8] are headings between two waypoints. 
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
                catch
                {
                    errors.Add(new ReadFileError(lineNum, line));
                }
            });

            return errors;
        }
    }
}
