using QSP.RouteFinding.Navaids;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.NavData.AAX
{
    /// <summary>
    /// Contains VOR, NDB, DME.
    /// </summary>
    public static class NavaidsLoader
    {
        /// <summary>
        /// Loads all navaids.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, Navaid> LoadFromFile(string filePath)
        {
            return Load(File.ReadAllLines(filePath));
        }

        /// <summary>
        /// Loads all navaids.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, Navaid> Load(IEnumerable<string> lines)
        {
            return lines.Select(line => DefaultIfThrows(() => ParseLine(line), null))
                        .Where(x => x != null)
                        .ToDictionary(x => x.ID);
        }

        private static Navaid ParseLine(string line)
        {
            var x = line.Split(',');
            return new Navaid()
            {
                ID = x[0],
                Name = x[1],
                Freq = x[2],
                IsVOR = x[3] == "1",
                IsDME = x[4] == "1",
                RangeNm = int.Parse(x[5]),
                Lat = double.Parse(x[6]),
                Lon = double.Parse(x[7]),
                ElevationFt = int.Parse(x[8]),
                CountryCode = x[9]
            };
        }
    }
}
