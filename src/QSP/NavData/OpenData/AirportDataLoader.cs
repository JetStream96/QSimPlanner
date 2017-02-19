using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.MathTools.Numbers;

namespace QSP.NavData.OpenData
{
    /// <summary>
    /// Read from file and reads the airports.csv and runways.csv
    /// into an instance of AirportDatabase.
    /// </summary>
    public class AirportDataLoader
    {
        private string folderpath;

        public AirportDataLoader(string folderpath)
        {
            this.folderpath = folderpath;
        }

        private MultiMap<string, RwyData> LoadRwys()
        {
            string rwyFile = folderpath + @"\runways.csv";
            var lines = File.ReadAllLines(rwyFile);

            var rwys = new MultiMap<string, RwyData>();
            var surfTypes = new Dictionary<string, string>();

            // Ignore first line.
            for (int i = 1; i < lines.Length; i++)
            {
                try
                {
                    var words = lines[i].Split(',');
                    var icao = words[2].Trim('"');
                    var id = words[8].Trim('"');

                    // Reduce memory usage by sharing these strings.
                    var surface = words[5].Trim('"', ' ');
                    if (surfTypes.ContainsKey(surface))
                    {
                        surface = surfTypes[surface];
                    }
                    else
                    {
                        surfTypes[surface] = surface;
                    }

                    if (!double.TryParse(words[3], out var len) ||
                        !double.TryParse(words[4], out var width) ||
                        !double.TryParse(words[9], out var lat) ||
                        !double.TryParse(words[10], out var lon) ||
                        !double.TryParse(words[11], out var elev) ||
                        !double.TryParse(words[13], out var heading))
                    {
                        continue;
                    }

                    var r = new RwyData(id, RoundToInt(heading).ToString(), RoundToInt(len),
                        RoundToInt(width), false, false, "", "", lat, lon, RoundToInt(elev), 0.0,
                            0, surface, -1);

                    rwys.Add(icao, r);
                    id = words[15].Trim('"');

                    if (!double.TryParse(words[16], out lat) ||
                       !double.TryParse(words[17], out lon) ||
                       !double.TryParse(words[18], out elev) ||
                       !double.TryParse(words[20], out heading))
                    {
                        continue;
                    }

                    r = new RwyData(id, RoundToInt(heading).ToString(), RoundToInt(len),
                        RoundToInt(width), false, false, "", "", lat, lon, RoundToInt(elev), 0.0,
                            0, surface, -1);

                    rwys.Add(icao, r);
                }
                catch
                { }
            }

            return rwys;
        }

        /// <exception cref="ReadAirportFileException"></exception>
        public AirportManager LoadFromFile()
        {
            var airportDB = new AirportManager();
            string[] allLines = null;
            var path = folderpath + @"\airports.csv";

            try
            {
                allLines = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                throw new ReadAirportFileException("Unable to read from " + path + ".", ex);
            }

            MultiMap<string, RwyData> rwyList;
            try
            {
                rwyList = LoadRwys();
            }
            catch (Exception ex)
            {
                throw new ReadAirportFileException("Unable to read runways.csv.", ex);
            }

            for (int i = 1; i < allLines.Length; i++)
            {
                try
                {
                    var words = allLines[i].Split(',');

                    string icao = words[1].Trim('"');
                    string name = words[3].Trim('"');

                    var rwys = rwyList.FindAll(icao);

                    if (rwys.Count == 0 ||
                        !double.TryParse(words[4], out var lat) ||
                        !double.TryParse(words[5], out var lon) ||
                        !double.TryParse(words[6], out var elevation))
                    {
                        continue;
                    }

                    int longestRwyLength = rwys.Max(x => x.LengthFt);

                    airportDB.Add(new Airport(icao, name, lat, lon, RoundToInt(elevation),
                        false, 0, 0, longestRwyLength, rwys));
                }
                catch { }
            }

            return airportDB;
        }
    }
}
