using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.IO;
using static QSP.LibraryExtension.StringParser.Utilities;

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
        /// <exception cref="WaypointFileReadException"></exception>
        /// <exception cref="WaypointFileParseException"></exception>
        public void ReadFromFile(string filepath)
        {
            string[] allLines = null;

            try
            {
                allLines = File.ReadAllLines(filepath);
            }
            catch (Exception ex)
            {
                throw new WaypointFileReadException("", ex);
            }

            foreach (var i in allLines)
            {
                try
                {
                    if (i.Length == 0 || i[0] == ' ')
                    {
                        continue;
                    }

                    ReadWpt(i);
                }
                catch
                {
                    throw new WaypointFileParseException(
                       "This line in waypoints.txt cannot be parsed:\n" +
                       i + "\n(Reason: Wrong format)");
                }
            }
        }

        private void ReadWpt(string i)
        {
            int pos = 0;

            string id = ReadString(i, ref pos, ',');
            double lat = ParseDouble(i, ref pos, ',');
            double lon = ParseDouble(i, ref pos, ',');

            wptList.AddWaypoint(new Waypoint(id, lat, lon));
        }
    }
}
