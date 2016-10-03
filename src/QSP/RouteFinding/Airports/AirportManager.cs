using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Data;
using System.Collections.Generic;
using static QSP.RouteFinding.Data.LatLonSearcher<
    QSP.RouteFinding.Airports.Airport>;
using System.Linq;

namespace QSP.RouteFinding.Airports
{
    // This manager class can make sure that the airportData 
    // and airportFinder matches completely.
    //
    public class AirportManager
    {
        private Dictionary<string, Airport> airportData;
        private LatLonSearcher<Airport> airportFinder;

        public AirportManager() : this(new Airport[0]) { }

        public AirportManager(IEnumerable<Airport> airportData)
        {
            this.airportData = airportData.ToDictionary(a => a.Icao);
            GenerateSearchGrids();
        }

        private void GenerateSearchGrids()
        {
            airportFinder = new LatLonSearcher<Airport>(GridSizeOption.Small);
            foreach (var i in airportData.Values) airportFinder.Add(i);
        }

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
        public Airport this[string icao]
        {
            get
            {
                Airport airport;
                if (airportData.TryGetValue(icao, out airport))
                {
                    return airport;
                }

                return null;
            }
        }

        public List<Airport> Find(double lat, double lon, double distance)
        {
            return airportFinder.Find(lat, lon, distance);
        }

        /// <summary>
        /// Returns null if the icao is not found.
        /// </summary>
        public IEnumerable<string> RwyIdentList(string icao)
        {
            return this[icao]?.Rwys.Select(r => r.RwyIdent);
        }

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
        public RwyData FindRwy(string icao, string rwy)
        {
            return this[icao]?
                .Rwys
                .FirstOrDefault(r => r.RwyIdent == rwy);
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
            airportData.Add(item.Icao, item);
            airportFinder.Add(item);
        }

        /// <summary>
        /// Removes the airport if icao is found. Otherwise does nothing.
        /// Returns the icao was removed.
        /// </summary>
        public bool Remove(string icao)
        {
            var ad = this[icao];
            if (ad == null) return false;

            airportData.Remove(icao);
            airportFinder.Remove(ad);
            return true;
        }
    }

    public class DefaultAirportManager : AirportManager
    {
        public DefaultAirportManager() : base() { }
    }
}
