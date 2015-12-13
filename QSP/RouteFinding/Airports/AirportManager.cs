using QSP.AviationTools;
using QSP.RouteFinding.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QSP.RouteFinding.Data.LatLonSearchUtility<QSP.RouteFinding.Airports.Airport>;

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
            airportDB = new FileLoader(filepath).LoadFromFile();
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
