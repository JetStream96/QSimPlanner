using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.IO;

namespace QSP.NavData.AAX
{
    /// <summary>
    /// Read from file and reads the airport.txt into 
    /// an instance of AirportDatabase.
    /// </summary>
    public class AirportDataLoader
    {
        private string filepath;

        public AirportDataLoader(string filepath)
        {
            this.filepath = filepath;
        }

        /// <exception cref="RwyDataFormatException"></exception>
        /// <exception cref="ReadAirportFileException"></exception>
        public AirportCollection LoadFromFile()
        {
            var airportList = new AirportCollection();
            IEnumerable<string> allLines = null;

            try
            {
                allLines = File.ReadLines(filepath);
            }
            catch (Exception ex)
            {
                throw new ReadAirportFileException(
                    "Unable to read from " + filepath + ".", ex);
            }

            Airport airport = null;
            List<RwyData> rwys = null;

            foreach (var i in allLines)
            {
                try
                {
                    var words = i.Split(',');

                    if (words.Length == 0)
                    {
                        continue;
                    }

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
            airportList.Add(airport);

            return airportList;            
        }

        private static string[] surfTypes = new string[]
        {
            "Concrete",
            "Asphalt or Bitumen",
            "Gravel, Coral Or Ice",
            "Other"
        };

        private static RwyData ReadRwy(string[] words)
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
                surfTypes[SurfaceType],
                RwyStatus);
        }
    }
}
