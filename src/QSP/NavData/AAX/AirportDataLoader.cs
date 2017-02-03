using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// TODO: Add unit test for this namespace.
namespace QSP.NavData.AAX
{
    public static class AirportDataLoader
    {
        /// <summary>
        /// Read from  airport.txt file and return an AirportDatabase.
        /// </summary>
        /// <exception cref="RwyDataFormatException"></exception>
        /// <exception cref="ReadAirportFileException"></exception>
        public static AirportManager LoadFromFile(string filePath)
        {
            IEnumerable<string> allLines = null;

            try
            {
                allLines = File.ReadLines(filePath);
            }
            catch (Exception ex)
            {
                throw new ReadAirportFileException($"Unable to read from {filePath}.", ex);
            }

            return Load(allLines);
        }
        // TODO: Improve error handling.
        /// <exception cref="RwyDataFormatException"></exception>
        public static AirportManager Load(IEnumerable<string> allLines)
        {
            var airportList = new AirportManager();

            Airport airport = null;
            List<RwyData> rwys = null;

            foreach (var i in allLines)
            {
                try
                {
                    var words = i.Split(',').Select(s => s.Trim()).ToList();
                    if (words.Count == 0) continue;

                    if (words[0] == "A")
                    {
                        if (airport != null)
                        {
                            // Add the previously read airport.
                            airportList.Add(airport);
                        }

                        var icao = words[1];
                        var name = words[2];
                        var lat = double.Parse(words[3]);
                        var lon = double.Parse(words[4]);
                        var elevation = int.Parse(words[5]);
                        var transAlt = int.Parse(words[6]);
                        var transLvl = int.Parse(words[7]);
                        var longestRwyLength = int.Parse(words[8]);
                        rwys = new List<RwyData>();

                        // Will be added later
                        airport = new Airport(
                            icao,
                            name,
                            lat,
                            lon,
                            elevation,
                            true,
                            transAlt,
                            transLvl,
                            longestRwyLength,
                            rwys);
                    }
                    else if (words[0] == "R")
                    {
                        rwys.Add(ReadRwy(words));
                    }
                }
                catch (Exception ex)
                {
                    throw new RwyDataFormatException(
                        "Incorrect format in runway database is found.", ex);
                }
            }

            // Add the last airport.
            if (airport != null) airportList.Add(airport);

            return airportList;
        }

        public static readonly IReadOnlyList<string> SurfTypes = new[]
        {
            "Concrete",
            "Asphalt or Bitumen",
            "Gravel, Coral Or Ice",
            "Other"
        };

        private static RwyData ReadRwy(List<string> words)
        {
            string RwyIdent = words[1];
            string Heading = words[2];
            int Length = int.Parse(words[3]);
            int Width = int.Parse(words[4]);
            bool IlsAvail = int.Parse(words[5]) == 1;
            string IlsFreq = words[6];
            string IlsHeading = words[7];
            double Lat = double.Parse(words[8]);
            double Lon = double.Parse(words[9]);
            int Elevation = int.Parse(words[10]);
            double GlideslopeAngle = double.Parse(words[11]);
            int ThresholdOverflyHeight = int.Parse(words[12]);
            int SurfaceType = int.Parse(words[13]);
            int RwyStatus = int.Parse(words[14]);

            return new RwyData(
                RwyIdent,
                Heading,
                Length,
                Width,
                true,
                IlsAvail,
                IlsFreq,
                IlsHeading,
                Lat,
                Lon,
                Elevation,
                GlideslopeAngle,
                ThresholdOverflyHeight,
                SurfTypes[SurfaceType],
                RwyStatus);
        }
    }
}
