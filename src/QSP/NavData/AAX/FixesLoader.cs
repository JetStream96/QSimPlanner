using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.LibraryExtension;
using System;
using System.IO;
using static QSP.LibraryExtension.StringParser.Utilities;

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
        /// <exception cref="WaypointFileParseException"></exception>
        public BiDictionary<int, string> ReadFromFile(string filepath)
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

            return countryCodeLookup;
        }

        private void ReadWpt(string i)
        {
            int pos = 0;

            string id = ReadString(i, ref pos, ',');
            double lat = ParseDouble(i, ref pos, ',');
            double lon = ParseDouble(i, ref pos, ',');

            string country = ReadToNextDelimeter(i, DelimiterWords, ref pos);
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
    }
}
