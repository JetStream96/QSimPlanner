using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QSP.RouteFinding.Data.LatLonSearchUtility<QSP.RouteFinding.Airports.Airport>;
using System.IO;
using QSP.AviationTools;
using QSP.RouteFinding.Data;

namespace QSP.RouteFinding.Airports
{
    public class AirportManager
    {
        // This manager class can make sure that the airportDB and airportFinder matches completely.

        private AirportDatabase airportDB;
        private LatLonSearchUtility<Airport> airportFinder;

        public AirportManager(AirportDatabase airportDB)
        {
            this.airportDB = airportDB;
            generateSearchGrids();
        }

        public AirportManager(string filepath)
        {
            airportDB = new AirportDatabase();

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            LoadFromFile(filepath);
            sw.Stop();
            //System.Windows.Forms.MessageBox.Show (string.Format("Took {0} ms.", sw.ElapsedMilliseconds));

            generateSearchGrids();
        }

        private void generateSearchGrids()
        {
            airportFinder = new LatLonSearchUtility<Airport>(GridSizeOption.Small);
            int count = airportDB.Count;

            for (int i = 0; i < count; i++)
            {
                airportFinder.Add(airportDB[i]);
            }
        }

        /// <exception cref="RwyDatabaseFormatException"></exception>
        /// <exception cref="ReadAirportDBFileException"></exception>
        private void LoadFromFile(string filepath)
        {
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
            string[] line = null;

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
                    line = i.Split(',');

                    if (line.Length <= 1)
                    {
                        continue;
                    }

                    if (line[0] == "A")
                    {
                        if (isFirst == false)
                        {
                            airportDB.Add(new Airport(icao, name, lat, lon, elevation, transAlt, transLvl, longestRwyLength, rwys));
                            rwys = new List<RwyData>();
                        }
                        else
                        {
                            isFirst = false;
                        }

                        icao = line[1];
                        name = line[2];
                        lat = Convert.ToDouble(line[3]);
                        lon = Convert.ToDouble(line[4]);
                        elevation = Convert.ToInt32(line[5]);
                        transAlt = transLvl = Convert.ToInt32(line[6]);
                        longestRwyLength = Convert.ToInt32(line[7]);
                        rwys = new List<RwyData>();
                    }
                    else if (line[0] == "R")
                    {
                        rwys.Add(new RwyData(line[1], line[2].PadLeft(3, '0'), Convert.ToInt32(line[3]), Convert.ToInt32(line[4]),
                                 line[5][0] == '1' ? true : false, line[6], line[7], Convert.ToDouble(line[8]), Convert.ToDouble(line[9]),
                                 Convert.ToInt32(line[10]), Convert.ToDouble(line[11]), Convert.ToInt32(line[12]), Convert.ToInt32(line[13]),
                                 Convert.ToInt32(line[14])));
                    }
                }
                catch (Exception ex)
                {
                    throw new RwyDatabaseFormatException("Incorrect format in runway database is found.", ex);
                }
            }
        }

        public Airport Find(string icao)
        {
            return airportDB.Find(icao);
        }

        public List<Airport> Find(double lat, double lon, double distance)
        {
            return airportFinder.Find(lat, lon, distance);
        }

        public string[] RwyIdentList(string icao)
        {
            return airportDB.RwyIdentList(icao);
        }

        public LatLon RwyLatLon(string icao, string rwy)
        {
            return airportDB.RwyLatLon(icao, rwy);
        }

        public LatLon AirportLatlon(string icao)
        {
            return airportDB.AirportLatlon(icao);
        }
        
        public int Count
        {
            get
            {
                return airportDB.Count;
            }
        }

        public void Add(Airport item)
        {
            airportDB.Add(item);
            airportFinder.Add(item);
        }

        public bool Remove(string icao)
        {
            var ad = airportDB.Find(icao);

            if (ad == null)
            {
                return false;

            }
            else {
                airportDB.Remove(icao);
                airportFinder.Remove(ad);
                return true;
            }
        }

    }
}
