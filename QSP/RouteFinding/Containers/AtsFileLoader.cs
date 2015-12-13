using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QSP.LibraryExtension.StringParser.Utilities;
using System.IO;
using static QSP.RouteFinding.Containers.TrackedWptList;
using static QSP.Utilities.ErrorLogger;

namespace QSP.RouteFinding.Containers
{
    public class AtsFileLoader
    {
        private TrackedWptList wptList;

        public AtsFileLoader(TrackedWptList wptList)
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
                wptList.TrackChanges = TrackChangesOption.No;

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
                        Neighbor secondWptAsNeighbor = null;

                        int index1 = wptList.FindByWaypoint(firstWpt);
                        int index2 = wptList.FindByWaypoint(secondWpt);

                        // The next two are headings between two wpts. Will be skipped.
                        pos = i.IndexOf(',', pos) + 1;
                        pos = i.IndexOf(',', pos) + 1;

                        double dis = ParseDouble(i,pos,i.Length-1);

                        //add second waypoint as required
                        if (index2 >= 0)
                        {
                            secondWptAsNeighbor = new Neighbor(index2, currentAirway, dis);
                        }
                        else
                        {
                            secondWptAsNeighbor = new Neighbor(wptList.Count, currentAirway, dis);
                            wptList.AddWpt(secondWpt);
                        }

                        //add first waypoint as required
                        if (index1 >= 0)
                        {
                            wptList.AddNeighbor(index1, secondWptAsNeighbor);
                        }
                        else
                        {
                            wptList.AddWpt(firstWpt);
                            wptList.AddNeighbor(wptList.Count - 1, secondWptAsNeighbor);
                        }
                    }
                }

                wptList.TrackChanges = TrackChangesOption.Yes;

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
