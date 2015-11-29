using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QSP.AviationTools;

namespace QSP.RouteFinding
{

    public class AirportDatabase
    {
        private List<AirportData> airport;

        public AirportDatabase()
        {
            airport = new List<AirportData>();
        }

        public void LoadFromFile(string filepath)
        {
            string[] allLines = null;

            try
            {
                allLines = File.ReadAllLines(filepath);
            }
            catch (Exception ex)
            {
                throw new IOException("Unable to read from " + filepath + ".", ex);
            }

            string[] line = null;


            foreach (var i in allLines)
            {
                try
                {
                    line = i.Split(',');

                    if (line.Length <= 1)
                    {
                        continue;
                    }


                    if (line[0] == "A")
                    {
                        airport.Add(new AirportData(line[1], line[2], Convert.ToDouble(line[3]), Convert.ToDouble(line[4]), Convert.ToInt32(line[5]), Convert.ToInt32(line[6]), Convert.ToInt32(line[7])));


                    }
                    else if (line[0] == "R")
                    {
                        airport.Last().Rwys.Add(new RwyData(line[1], line[2].PadLeft(3, '0'), Convert.ToInt32(line[3]), Convert.ToInt32(line[4]), line[5][0] == '1' ? true : false, line[6], line[7], Convert.ToDouble(line[8]), Convert.ToDouble(line[9]), Convert.ToInt32(line[10]),
                        Convert.ToDouble(line[11]), Convert.ToInt32(line[12]), Convert.ToInt32(line[13]), Convert.ToInt32(line[14])));

                    }


                }
                catch (Exception ex)
                {
                    throw new RwyDatabaseFormatException("Incorrect format in runway database is found.", ex);

                }

            }

            computeMaxRwyLength();

        }


        private void computeMaxRwyLength()
        {
            int max = 0;

            foreach (var i in airport)
            {
                max = 0;

                foreach (var j in i.Rwys)
                {
                    if (j.Length > max)
                    {
                        max = j.Length;
                    }
                }
                i.LongestRwyLength = max;
            }
        }

        private int getIndex(string icao)
        {
            return airport.BinarySearch(new AirportData(icao), AirportData.CompareIcao());
        }

        public AirportData Find(string icao)
        {

            int index = getIndex(icao);

            if (index >= 0)
            {
                return new AirportData(airport[index]);
            }
            else
            {
                return null;
            }

        }

        public string[] RwyIdentList(string icao)
        {
            int index = getIndex(icao);

            if (index < 0)
            {
                return null;
            }
            else
            {
                var rwy = airport[index].Rwys;
                string[] result = new string[rwy.Count];

                for (int i = 0; i <= rwy.Count - 1; i++)
                {
                    result[i] = rwy[i].RwyIdent;
                }
                return result;
            }
        }

        public LatLon RwyLatLon(string icao, string rwy)
        {
            int index = getIndex(icao);

            if (index < 0)
            {
                return null;
            }
            else
            {
                var runway = airport[index].Rwys;

                foreach (var i in runway)
                {
                    if (i.RwyIdent == rwy)
                    {
                        return new LatLon(i.Lat, i.Lon);
                    }
                }
            }
            return null;
        }
        
        public LatLon AirportLatlon(string icao)
        {
            int index = getIndex(icao);

            if (index < 0)
            {
                return null;
            }
            else
            {
                return new LatLon(airport[index].Lat, airport[index].Lon);
            }
        }

        public Vector3D AirportLatLonAltIcao(string icao)
        {
            int index = getIndex(icao);

            if (index < 0)
            {
                return null;
            }
            else
            {
                return new Vector3D(airport[index].Lat, airport[index].Lon, airport[index].Elevation);
            }
        }

        public LatLonSearchUtility<AirportData> GenerateSearchGrids()
        {
            var searchGrid = new LatLonSearchUtility<AirportData>(LatLonSearchUtility<AirportData>.GridSizeOption.Small, AirportData.LatLon);

            foreach (var i in airport)
            {
                searchGrid.Add(i);
            }

            return searchGrid;
        }

    }

}

