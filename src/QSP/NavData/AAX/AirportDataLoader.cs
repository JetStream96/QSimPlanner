using CommonLibrary.LibraryExtension;
using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.NavData.AAX
{
    public static class AirportDataLoader
    {
        /// <summary>
        /// Read from airport.txt file and return an AirportManager.
        /// </summary>
        /// <exception cref="ReadAirportFileException"></exception>
        public static LoadResult LoadFromFile(string filePath)
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
        
        public static LoadResult Load(IEnumerable<string> allLines)
        {
            var errors = new List<ReadFileError>();
            var airportList = new AirportManager();

            Airport airport = null;
            List<RwyData> rwys = null;

            allLines.ForEach((line, index) =>
            {
                var lineNum = index + 1;
                var words = line.Split(',').Select(s => s.Trim()).ToList();

                if (words.Count > 0)
                {
                    if (words[0] == "A")
                    {
                        // Add the previously read airport.
                        if (airport != null) airportList.Add(airport);

                        var a = ReadAirport(words);
                        if (a != null)
                        {
                            airport = a;
                            rwys = (List<RwyData>)a.Rwys;
                        }
                        else
                        {
                            errors.Add(new ReadFileError(lineNum, line));
                        }
                    }
                    else if (words[0] == "R")
                    {
                        var r = ReadRwy(words);
                        if (r != null)
                        {
                            rwys.Add(ReadRwy(words));
                        }
                        else
                        {
                            errors.Add(new ReadFileError(lineNum, line));
                        }
                    }
                }
            });

            // Add the last airport.
            if (airport != null) airportList.Add(airport);

            return new LoadResult() {Airports = airportList, Errors = errors};
        }

        public struct LoadResult
        {
            public AirportManager Airports; public IReadOnlyList<ReadFileError> Errors;
        }
        
        public static readonly IReadOnlyList<string> SurfTypes = new[]
        {
            "Concrete",
            "Asphalt or Bitumen",
            "Gravel, Coral Or Ice",
            "Other"
        };

        private static Airport ReadAirport(List<string> words)
        {
            try
            {
                var icao = words[1];
                var name = words[2];
                var lat = double.Parse(words[3]);
                var lon = double.Parse(words[4]);
                var elevation = int.Parse(words[5]);
                var transAlt = int.Parse(words[6]);
                var transLvl = int.Parse(words[7]);
                var longestRwyLength = int.Parse(words[8]);

                // Runways Will be added later
                return new Airport(icao, name, lat, lon, elevation, true, transAlt,
                    transLvl, longestRwyLength, new List<RwyData>());
            }
            catch
            {
                return null;
            }
        }

        private static RwyData ReadRwy(List<string> words)
        {
            try
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

                return new RwyData(RwyIdent, Heading, Length, Width, true, IlsAvail, IlsFreq,
                    IlsHeading, Lat, Lon, Elevation, GlideslopeAngle, ThresholdOverflyHeight,
                    SurfTypes[SurfaceType], RwyStatus);
            }
            catch
            {
                return null;
            }
        }
    }
}
