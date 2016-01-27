using System;
using System.Collections.Generic;
using System.IO;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.Airports
{
    /// <summary>
    /// Read from file and reads the airport.txt into an instance of AirportDatabase.
    /// </summary>
    public class FileLoader
    {
        private string filepath;

        public FileLoader(string filepath)
        {
            this.filepath = filepath;
        }

        /// <exception cref="RwyDatabaseFormatException"></exception>
        /// <exception cref="ReadAirportDBFileException"></exception>
        public AirportCollection LoadFromFile()
        {
            var airportDB = new AirportCollection();
            string[] allLines = null;

            try
            {
                allLines = File.ReadAllLines(filepath);
            }
            catch (Exception ex)
            {
                throw new ReadAirportDBFileException("Unable to read from " + filepath + ".", ex);
            }

            bool isFirst = true;

            // Data for an airport
            string icao = null;
            string name = null;
            double lat = 0.0;
            double lon = 0.0;
            int elevation = 0;
            int transAlt = 0;
            int transLvl = 0;
            int longestRwyLength = 0;
            List<RwyData> rwys = new List<RwyData>();

            foreach (var i in allLines)
            {
                try
                {
                    if (i.Length == 0)
                    {
                        continue;
                    }

                    if (i[0] == 'A')
                    {
                        if (isFirst == false)
                        {
                            // Add the previously read airport.
                            airportDB.Add(new Airport(icao, name, lat, lon, elevation, transAlt, transLvl, longestRwyLength, rwys));

                            // Create a new list of runways.
                            rwys = new List<RwyData>();
                        }
                        else
                        {
                            isFirst = false;
                        }

                        int pos = i.IndexOf(',') + 1;

                        icao = ReadString(i, ref pos, ',');
                        name = ReadString(i, ref pos, ',');
                        lat = ParseDouble(i, ref pos, ',');
                        lon = ParseDouble(i, ref pos, ',');
                        elevation = ParseInt(i, ref pos, ',');
                        transAlt = ParseInt(i, ref pos, ',');
                        transLvl = ParseInt(i, ref pos, ',');
                        longestRwyLength = ParseInt(i, ref pos, ',');
                        rwys = new List<RwyData>();
                    }
                    else if (i[0] == 'R')
                    {
                        rwys.Add(readRwy(rwys, i));
                    }
                }
                catch (Exception ex)
                {
                    throw new RwyDatabaseFormatException("Incorrect format in runway database is found.", ex);
                }
            }
            // Add the last airport.
            airportDB.Add(new Airport(icao, name, lat, lon, elevation, transAlt, transLvl, longestRwyLength, rwys));

            return airportDB;
        }

        private static RwyData readRwy(List<RwyData> rwys, string i)
        {
            int pos = i.IndexOf(',') + 1;
            string RwyIdent = ReadString(i, ref pos, ',');
            string Heading = ReadString(i, ref pos, ',');
            int Length = ParseInt(i, ref pos, ',');
            int Width = ParseInt(i, ref pos, ',');
            bool IlsAvail = ParseInt(i, ref pos, ',') == 1;
            string IlsFreq = ReadString(i, ref pos, ',');
            string IlsHeading = ReadString(i, ref pos, ',');
            double Lat = ParseDouble(i, ref pos, ',');
            double Lon = ParseDouble(i, ref pos, ',');
            int Elevation = ParseInt(i, ref pos, ',');
            double GlideslopeAngle = ParseDouble(i, ref pos, ',');
            int ThresholdOverflyHeight = ParseInt(i, ref pos, ',');
            int SurfaceType = ParseInt(i, ref pos, ',');
            int RwyStatus = ParseInt(i, ref pos, ',');

            return new RwyData(RwyIdent, Heading, Length, Width, IlsAvail, IlsFreq, IlsHeading, Lat, Lon, Elevation,
                                  GlideslopeAngle, ThresholdOverflyHeight, SurfaceType, RwyStatus);
        }
    }
}
