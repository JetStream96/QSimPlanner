using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QSP.Utilities;

namespace QSP.NavData.AAX
{
    public class FixesLoader
    {
        private readonly WaypointList wptList;
        private readonly ILogger logger;
        private readonly CountryCodeGenerator generator = new CountryCodeGenerator();

        public FixesLoader(WaypointList wptList) : this(wptList, EmptyLogger.Instance) { }

        public FixesLoader(WaypointList wptList, ILogger logger)
        {
            this.wptList = wptList;
            this.logger = logger;
        }

        /// <summary>
        /// Loads all waypoints in waypoints.txt.
        /// </summary>
        /// <param name="filePath">Path of waypoints.txt</param>
        /// <exception cref="WaypointFileReadException"></exception>
        public IReadOnlyBiDictionary<int, string> ReadFromFile(string filePath)
        {
            var allLines = ReadFile(filePath);
            return Read(allLines);
        }

        public IReadOnlyBiDictionary<int, string> Read(IEnumerable<string> allLines)
        {
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
            return generator.CountryCodeLookup;
        }

        private void LogFailures(List<string> failedLines)
        {
            if (failedLines.Count == 0) return;
            var sb = new StringBuilder();
            sb.AppendLine("These lines in waypoints.txt cannot be parsed:");
            failedLines.ForEach(i => sb.AppendLine(i));
            logger.Log(sb.ToString());
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
                throw new WaypointFileReadException("Failed to read waypoints.txt.", ex);
            }
        }

        private void ReadWpt(string i)
        {
            var words = i.Split(',');

            string id = words[0].Trim();
            double lat = double.Parse(words[1]);
            double lon = double.Parse(words[2]);
            string country = words.Length > 3 ? words[3].Trim() : "";

            int countryCode = generator.GetCountryCode(country);

            wptList.AddWaypoint(new Waypoint(id, lat, lon, countryCode));
        }

        private static bool IsEmptyLine(string s)
        {
            return s.All(c => c == ' ' || c == '\t');
        }
    }
}
