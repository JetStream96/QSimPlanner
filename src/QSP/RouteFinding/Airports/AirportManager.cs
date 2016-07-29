using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Data;
using System.Collections.Generic;
using static QSP.RouteFinding.Data.LatLonSearcher<
    QSP.RouteFinding.Airports.Airport>;

namespace QSP.RouteFinding.Airports
{
    // This manager class can make sure that the airportData 
    // and airportFinder matches completely.
    //
    public class AirportManager
    {
        private AirportCollection airportData;
        private LatLonSearcher<Airport> airportFinder;

        public AirportManager(AirportCollection airportData)
        {
            this.airportData = airportData;
            GenerateSearchGrids();
        }

        private void GenerateSearchGrids()
        {
            airportFinder = new LatLonSearcher<Airport>(GridSizeOption.Small);
            int count = airportData.Count;

            for (int i = 0; i < count; i++)
            {
                airportFinder.Add(airportData[i]);
            }
        }

        public Airport Find(string icao)
        {
            return airportData.Find(icao);
        }

        public List<Airport> Find(double lat, double lon, double distance)
        {
            return airportFinder.Find(lat, lon, distance);
        }

        public string[] RwyIdentList(string icao)
        {
            return airportData.RwyIdentList(icao);
        }

        public LatLon RwyLatLon(string icao, string rwy)
        {
            return airportData.RwyLatLon(icao, rwy);
        }

        public LatLon AirportLatlon(string icao)
        {
            return airportData.AirportLatlon(icao);
        }

        public int Count
        {
            get
            {
                return airportData.Count;
            }
        }

        public void Add(Airport item)
        {
            airportData.Add(item);
            airportFinder.Add(item);
        }

        public bool Remove(string icao)
        {
            var ad = airportData.Find(icao);

            if (ad == null)
            {
                return false;
            }
            else
            {
                airportData.Remove(icao);
                airportFinder.Remove(ad);
                return true;
            }
        }
    }

    public class DefaultAirportManager : AirportManager
    {
        public DefaultAirportManager() : base(new AirportCollection()) { }
    }
}
