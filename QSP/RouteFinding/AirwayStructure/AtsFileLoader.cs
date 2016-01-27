using QSP.RouteFinding.Containers;
using System;
using System.IO;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.Utilities.ErrorLogger;

namespace QSP.RouteFinding.AirwayStructure
{
    public class AtsFileLoader
    {
        private WaypointList wptList;

        public AtsFileLoader(WaypointList wptList)
        {
            this.wptList = wptList;
        }

        /// <summary>
        /// Read all waypoints from ats.txt file.
        /// </summary>
        /// <param name="filepath">Path of ats.txt</param>
        /// <exception cref="LoadWaypointFileException"></exception>
        public void ReadAtsFromFile(string filepath)
        {
            try
            {
                wptList.CurrentlyTracked = TrackChangesOption.No;

                //add error handling
                string[] allLines = File.ReadAllLines(filepath);
                string currentAirway = "";

                foreach (var i in allLines)
                {
                    if (i.Length == 0)
                    {
                        continue;
                    }

                    int pos = i.IndexOf(',') + 1;

                    if (i[0] == 'A')
                    {
                        //this line is an airway identifier
                        currentAirway = ReadString(i, ref pos, ',');
                    }
                    else if (i[0] == 'S')
                    {
                        //this line is waypoint
                        Waypoint firstWpt = getWpt(i, ref pos);
                        Waypoint secondWpt = getWpt(i, ref pos);

                        int index1 = wptList.FindByWaypoint(firstWpt);
                        int index2 = wptList.FindByWaypoint(secondWpt);

                        // The next two are headings between two wpts. Will be skipped.
                        pos = i.IndexOf(',', pos) + 1;
                        pos = i.IndexOf(',', pos) + 1;

                        double dis = ParseDouble(i, pos, i.Length - 1);

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
                        wptList.AddNeighbor(index1, index2, new Neighbor(currentAirway, dis));
                    }
                }

                wptList.CurrentlyTracked = TrackChangesOption.Yes;

            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                throw new LoadWaypointFileException("Failed to load ats.txt.", ex);  //TODO: show to the user
            }
        }

        private Waypoint getWpt(string i, ref int pos)
        {
            string id = ReadString(i, ref pos, ',');
            double lat = ParseDouble(i, ref pos, ',');
            double lon = ParseDouble(i, ref pos, ',');
            return new Waypoint(id, lat, lon);
        }

    }
}
