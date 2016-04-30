using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.MathTools.Doubles;
using static QSP.AviationTools.Constants;

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

        private MultiMap<string, RwyData> loadRwys()
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
                    var len = double.Parse(words[3]) * FtMeterRatio;
                    var width = double.Parse(words[4]) * FtMeterRatio;

                    // Reduce memory usage by sharing these strings.
                    var surface = words[5].Trim('"');
                    if (surfTypes.ContainsKey(surface))
                    {
                        surface = surfTypes[surface];
                    }

                    var id = words[8].Trim('"');
                    double lat = double.Parse(words[9]);
                    double lon = double.Parse(words[10]);
                    double elev = double.Parse(words[11]);
                    double heading = double.Parse(words[12]);//TODO:

                    rwys.Add(
                        icao,
                        new RwyData(
                            id,
                            RoundToInt(heading).ToString(),
                            RoundToInt(len),
                            RoundToInt(width),
                            false,
                            false,
                            "",
                            "",
                            lat,
                            lon,
                            RoundToInt(elev),
                            0.0,
                            0,
                            surface,
                            -1));

                    id = words[14].Trim('"');
                    lat = double.Parse(words[15]);
                    lon = double.Parse(words[16]);
                    elev = double.Parse(words[17]);
                    heading = double.Parse(words[18]);//TODO:

                    rwys.Add(
                        icao,
                        new RwyData(
                            id,
                            RoundToInt(heading).ToString(),
                            RoundToInt(len),
                            RoundToInt(width),
                            false,
                            false,
                            "",
                            "",
                            lat,
                            lon,
                            RoundToInt(elev),
                            0.0,
                            0,
                            surface,
                            -1));
                }
                catch
                { }
            }

            return rwys;
        }

        /// <exception cref="ReadAirportFileException"></exception>
        public AirportCollection LoadFromFile()
        {
            var airportDB = new AirportCollection();
            string[] allLines = null;
            var path = folderpath + @"\airports.csv";

            try
            {
                allLines = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                throw new ReadAirportFileException(
                    "Unable to read from " + path + ".", ex);
            }

            MultiMap<string, RwyData> rwyList;
            try
            {
                rwyList = loadRwys();
            }
            catch (Exception ex)
            {
                throw new ReadAirportFileException(
                    "Unable to read runways.csv.", ex);
            }

            for (int i = 1; i < allLines.Length; i++)
            {
                try
                {
                    var words = allLines[i].Split(',');

                    string icao = words[1].Trim('"');
                    string name = words[3].Trim('"');
                    double lat = double.Parse(words[4]);
                    double lon = double.Parse(words[5]);
                    double elevation = double.Parse(words[6]);
                    var rwys = rwyList.FindAll(icao);

                    if (rwys.Count == 0)
                    {
                        continue;
                    }

                    int longestRwyLength = rwys.Max(x => x.Length);

                    airportDB.Add(
                        new Airport(
                                    icao,
                                    name,
                                    lat,
                                    lon,
                                    RoundToInt(elevation),
                                    false,
                                    0,
                                    0,
                                    longestRwyLength,
                                    rwys));
                }
                catch { }
            }
            return airportDB;
        }
    }
}
