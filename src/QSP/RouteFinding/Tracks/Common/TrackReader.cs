using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Airports;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace QSP.RouteFinding.Tracks.Common
{
    // Read the track waypoints as strings, and try to find each waypoints in WptList
    public class TrackReader<T> where T : Track
    {
        private WaypointList wptList;
        private AirportManager airportList;

        private List<WptPair> routeFromTo;
        private Route mainRoute;
        private T trk;

        public TrackReader(WaypointList wptList, AirportManager airportList)
        {
            this.wptList = wptList;
            this.airportList = airportList;
        }

        /// <exception cref="InvalidRouteException"></exception>
        /// <exception cref="WaypointNotFoundException"></exception>
        public TrackNodes Read(T item)
        {
            trk = item;
            mainRoute = ReadMainRoute(trk.MainRoute);

            // The format of this part is rather unpredictable. 
            // For example, a route can even start with an airway:
            // RTS/CYVR V317 QQ YZT JOWEN 
            // ...
            // Since this part is not that important, we can allow it to fail and still ignore it.
            try
            {
                routeFromTo = FindWptAllRouteFrom(trk.RouteFrom);
                routeFromTo.AddRange(FindWptAllRouteTo(trk.RouteTo));
                routeFromTo = routeFromTo.Distinct().ToList();
            }
            catch
            {
                routeFromTo = new List<WptPair>();
            }

            return new TrackNodes(trk.Ident, trk.AirwayIdent, mainRoute, routeFromTo);
        }

        #region "Method for routeFrom/To"

        private List<WptPair> GetExtraPairs(string[] rteFrom, double prevLat, double prevLon)
        {
            var result = new List<WptPair>();
            int lastIndex = -1;

            for (int index = 0; index < rteFrom.Length; index++)
            {
                if (lastIndex >= 0)
                {
                    if (IsAirway(lastIndex, rteFrom[index]))
                    {
                        lastIndex = -1;
                    }
                    else
                    {
                        int wpt = SelectWpt(prevLat, prevLon, rteFrom[index]);
                        result.Add(new WptPair(lastIndex, wpt));
                        lastIndex = wpt;
                    }
                }
                else
                {
                    lastIndex = SelectWpt(prevLat, prevLon, rteFrom[index]);
                }
            }
            return result;
        }

        private int SelectWpt(double prevLat, double prevLon, string ident)
        {
            var candidates = wptList.FindAllById(ident);

            if (candidates == null || candidates.Count == 0)
            {
                throw new TrackWaypointNotFoundException("Waypoint not found.");
            }
            return Utilities.GetClosest(prevLat, prevLon, candidates, wptList);
        }

        private bool IsAirway(int lastIndex, string airway)
        {
            if (airway == "UPR")
            {
                return true;
            }

            foreach (var i in wptList.EdgesFrom(lastIndex))
            {
                if (wptList.GetEdge(i).Value.Airway == airway)
                {
                    return true;
                }
            }
            return false;
        }

        private List<WptPair> FindWptAllRouteFrom(ReadOnlyCollection<string[]> rteFrom)
        {
            var result = new List<WptPair>();
            var firstWpt = mainRoute.FirstWaypoint;

            foreach (var i in rteFrom)
            {
                result.AddRange(GetExtraPairs(i, firstWpt.Lat, firstWpt.Lon));
            }

            return result;
        }

        private List<WptPair> FindWptAllRouteTo(ReadOnlyCollection<string[]> rteTo)
        {
            var result = new List<WptPair>();
            var lastWpt = mainRoute.LastWaypoint;

            foreach (var i in rteTo)
            {
                result.AddRange(GetExtraPairs(i, lastWpt.Lat, lastWpt.Lon));
            }

            return result;
        }

        #endregion

        #region "Method for main route"

        /// <exception cref="InvalidRouteException"></exception>
        /// <exception cref="WaypointNotFoundException"></exception>
        private Route ReadMainRoute(ReadOnlyCollection<string> rte)
        {
            LatLon latLon = trk.PreferredFirstLatLon;

            return new AutoSelectAnalyzer(
                CoordinateFormatter.Split(CombineArray(rte)),
                latLon.Lat,
                latLon.Lon,
                wptList)
                .Analyze();
        }

        private string CombineArray(ReadOnlyCollection<string> item)
        {
            var result = new StringBuilder();

            for (int i = 0; i < item.Count; i++)
            {
                string s = item[i];

                if ((i != 0 && i != item.Count - 1) ||
                    airportList.Find(s) == null)
                {
                    result.Append(s + " ");
                }
            }
            return result.ToString();
        }

        #endregion

    }
}
