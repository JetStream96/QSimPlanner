using System.Collections.Generic;
using QSP.AviationTools;

namespace QSP.RouteFinding.Airports
{
    public class AirportDatabase
    {
        private List<Airport> airportList;
        private Dictionary<string, int> airportFinder;   // Uses the ICAO code to find the index in the airportList.

        public AirportDatabase()
        {
            airportList = new List<Airport>();
            airportFinder = new Dictionary<string, int>();
        }

        public Airport Find(string icao)
        {
            int index;
            bool keyFound = airportFinder.TryGetValue(icao, out index);

            if (keyFound)
            {
                return airportList[index];
            }
            else
            {
                return null;
            }
        }

        public string[] RwyIdentList(string icao)
        {
            int index;
            bool keyFound = airportFinder.TryGetValue(icao, out index);

            if (keyFound == false)
            {
                return null;
            }
            else
            {
                var rwy = airportList[index].Rwys;
                string[] result = new string[rwy.Count];

                for (int i = 0; i < rwy.Count; i++)
                {
                    result[i] = rwy[i].RwyIdent;
                }
                return result;
            }
        }

        public LatLon RwyLatLon(string icao, string rwy)
        {
            int index;
            bool keyFound = airportFinder.TryGetValue(icao, out index);

            if (keyFound == false)
            {
                return null;
            }
            else
            {
                var runway = airportList[index].Rwys;

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
            int index;
            bool keyFound = airportFinder.TryGetValue(icao, out index);

            if (keyFound == false)
            {
                return null;
            }
            else
            {
                return new LatLon(airportList[index].Lat, airportList[index].Lon);
            }
        }
              
        public Airport this[int index]
        {
            get
            {
                return airportList[index];
            }
        }

        public int Count
        {
            get
            {
                return airportList.Count;
            }
        }

        public void Add(Airport item)
        {
            airportFinder.Add(item.Icao, airportList.Count);
            airportList.Add(item);
        }

        public bool Remove(string icao)
        {
            int index;
            bool keyFound = airportFinder.TryGetValue(icao, out index);

            if (keyFound)
            {
                airportList.RemoveAt(index);
                airportFinder.Remove(icao);
            }
            return keyFound;
        }
    }
}
