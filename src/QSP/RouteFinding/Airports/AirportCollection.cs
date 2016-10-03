using System.Collections.Generic;
using QSP.AviationTools.Coordinates;

namespace QSP.RouteFinding.Airports
{
    public class AirportCollection
    {
        private List<Airport> airportList;

        // Uses the ICAO code to find the index in the airportList.
        private Dictionary<string, int> airportFinder;

        public AirportCollection()
        {
            airportList = new List<Airport>();
            airportFinder = new Dictionary<string, int>();
        }

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
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

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
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

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
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

        /// <summary>
        /// Returns null if the icao or rwy is not found.
        /// </summary>
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

        //public Airport this[int index]
        //{
        //    get
        //    {
        //        return airportList[index];
        //    }
        //}

        public int Count
        {
            get
            {
                return airportList.Count;
            }
        }

        // If icao already exists, an exception is thrown.
        public void Add(Airport item)
        {
            airportFinder.Add(item.Icao, airportList.Count);
            airportList.Add(item);
        }

        /// <summary>
        /// Removes the airport if icao is found. Otherwise does nothing.
        /// Returns the icao was found.
        /// </summary>
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
