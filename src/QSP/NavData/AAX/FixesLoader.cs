using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static QSP.Utilities.LoggerInstance;

namespace QSP.NavData.AAX
{
    public class FixesLoader
    {
        private WaypointList wptList;
        private BiDictionary<int, string> countryCodeLookup;
        private int countryCode;

        public FixesLoader(WaypointList wptList)
        {
            this.wptList = wptList;
            countryCodeLookup = new BiDictionary<int, string>();
            countryCode = 0;
        }

        /// <summary>
        /// Loads all waypoints in waypoints.txt.
        /// </summary>
        /// <param name="filepath">Location of waypoints.txt</param>
        /// <exception cref="WaypointFileReadException"></exception>
        public BiDictionary<int, string> ReadFromFile(string filepath)
        {
            IEnumerable<string> allLines = ReadFile(filepath);
            var failedLines = new List<string>();

            foreach (var i in allLines)
            {
                try
                {
                    if (IsEmptyLine(i)) continue;
                    ReadWpt(i);
                }
                catch
                {
                    failedLines.Add(i);
                }
            }

            LogFailures(failedLines);
            return countryCodeLookup;
        }

        private static void LogFailures(List<string> failedLines)
        {
            var sb = new StringBuilder();
            sb.AppendLine("These lines in waypoints.txt cannot be parsed:");
            failedLines.ForEach(i => sb.AppendLine(i));
            WriteToLog(sb.ToString());
        }

        /// <exception cref="WaypointFileReadException"></exception>
        private static IEnumerable<string> ReadFile(string filepath)
        {
            try
            {
                return File.ReadLines(filepath);
            }
            catch (Exception ex)
            {
                throw new WaypointFileReadException(ex.Message, ex);
            }
        }

        private void ReadWpt(string i)
        {
            var words = i.Split(',');

            string id = words[0].Trim();
            double lat = double.Parse(words[1]);
            double lon = double.Parse(words[2]);
            string country = words.Length > 3 ? words[3].Trim() : "";

            int countryCode = GetCountryCode(country);

            wptList.AddWaypoint(new Waypoint(id, lat, lon, countryCode));
        }

        private int GetCountryCode(string letterCode)
        {
            if (letterCode == "")
            {
                return Waypoint.DefaultCountryCode;
            }

            int code;

            if (countryCodeLookup.TryGetBySecond(letterCode, out code))
            {
                return code;
            }

            do
            {
                countryCode++;
            } while (countryCode == Waypoint.DefaultCountryCode);

            countryCodeLookup.Add(countryCode, letterCode);
            return countryCode;
        }

        private static bool IsEmptyLine(string s)
        {
            return s.All(c => c == ' ' || c == '\t');
        }
    }
}
