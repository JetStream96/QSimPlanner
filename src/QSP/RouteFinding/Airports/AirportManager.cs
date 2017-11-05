using QSP.RouteFinding.Data;
using System.Collections.Generic;
using System.Linq;
using static QSP.RouteFinding.Data.LatLonSearcher<QSP.RouteFinding.Airports.IAirport>;

namespace QSP.RouteFinding.Airports
{
    // This manager class can make sure that the airportData 
    // and airportFinder matches completely.
    //
    public class AirportManager
    {
        private Dictionary<string, IAirport> airportData;
        private LatLonSearcher<IAirport> airportFinder;

        public AirportManager() : this(new Airport[0]) { }

        public AirportManager(IEnumerable<IAirport> airportData)
        {
            this.airportData = airportData.ToDictionary(a => a.Icao);
            GenerateSearchGrids();
        }

        private void GenerateSearchGrids()
        {
            airportFinder = new LatLonSearcher<IAirport>(GridSizeOption.Small);
            foreach (var i in airportData.Values) airportFinder.Add(i);
        }

        /// <summary>
        /// Returns null if the icao is not found.
        /// </summary>
        public IAirport this[string icao]
        {
            get
            {
                if (airportData.TryGetValue(icao, out var airport)) return airport;
                return null;
            }
        }

        public List<IAirport> Find(double lat, double lon, double distance)
        {
            return airportFinder.Find(lat, lon, distance);
        }

        /// <summary>
        /// Returns null if the icao is not found.
        /// </summary>
        public IEnumerable<string> RwyIdents(string icao)
        {
            return this[icao]?.Rwys.Select(r => r.RwyIdent);
        }

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
        public IRwyData FindRwy(string icao, string rwy)
        {
            return this[icao]?.Rwys.FirstOrDefault(r => r.RwyIdent == rwy);
        }

        public int Count => airportData.Count;

        public void Add(IAirport item)
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

    public sealed class DefaultAirportManager : AirportManager
    {
        public DefaultAirportManager() : base() { }
    }
}
