using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QSP.RouteFinding.Tracks.Common
{
    // Read the track waypoints as strings, and try to find each 
    // waypoints in WptList.
    public class TrackReader<T> where T : Track
    {
        private WaypointList wptList;
        private AirportManager airportList;

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
            mainRoute = ReadMainRoute(trk.MainRoute.ToList());

            // The format of this part is rather unpredictable. 
            // For example, a route can even start with an airway:
            // RTS/CYVR V317 QQ YZT JOWEN 
            // ...
            // Since this part is not that important, we can allow it to
            // fail and still ignore it.

            List<WptPair> connectionRoutes;

            try
            {
                var from = GetRouteFrom(trk.RouteFrom);
                var to = GetRouteTo(trk.RouteTo);
                connectionRoutes = from.Union(to).ToList();
            }
            catch
            {
                connectionRoutes = new List<WptPair>();
            }

            return new TrackNodes(
                trk.Ident, trk.AirwayIdent, mainRoute, connectionRoutes);
        }

        private List<WptPair> GetExtraPairs(RouteString rteFrom, ICoordinate previous)
        {
            var result = new List<WptPair>();
            int lastIndex = -1;

            for (int index = 0; index < rteFrom.Count; index++)
            {
                if (lastIndex >= 0)
                {
                    if (IsAirway(lastIndex, rteFrom[index]))
                    {
                        lastIndex = -1;
                    }
                    else
                    {
                        int wpt = SelectWpt(rteFrom[index], previous);
                        result.Add(new WptPair(lastIndex, wpt));
                        lastIndex = wpt;
                    }
                }
                else
                {
                    lastIndex = SelectWpt(rteFrom[index], previous);
                }
            }

            return result;
        }

        private int SelectWpt(string ident, ICoordinate previous)
        {
            var candidates = wptList.FindAllById(ident);

            if (candidates.Count == 0)
            {
                throw new TrackWaypointNotFoundException(
                    "Waypoint not found.");
            }

            return candidates.MinBy(i => wptList[i].Distance(previous));
        }

        private bool IsAirway(int lastIndex, string airway)
        {
            if (airway == "UPR") return true;

            return wptList.EdgesFrom(lastIndex).Any(
                i => wptList.GetEdge(i).Value.Airway == airway);
        }

        private List<WptPair> GetRouteFrom(IEnumerable<RouteString> rteFrom)
        {
            var firstWpt = mainRoute.FirstWaypoint;
            return rteFrom
                .SelectMany(i => GetExtraPairs(i, firstWpt))
                .ToList();
        }

        private List<WptPair> GetRouteTo(IEnumerable<RouteString> rteTo)
        {
            var lastWpt = mainRoute.LastWaypoint;
            return rteTo
                .SelectMany(i => GetExtraPairs(i, lastWpt))
                .ToList();
        }

        /// <exception cref="InvalidRouteException"></exception>
        /// <exception cref="WaypointNotFoundException"></exception>
        private Route ReadMainRoute(List<string> route)
        {
            var latLon = trk.PreferredFirstLatLon;

            var analyzer = new AutoSelectAnalyzer(
                CoordinateFormatter.Split(CombineArray(route)),
                latLon,
                latLon,
                wptList);

            return analyzer.Analyze();
        }

        private string CombineArray(List<string> item)
        {
            var result = new StringBuilder();

            for (int i = 0; i < item.Count; i++)
            {
                string s = item[i];

                if ((i != 0 && i != item.Count - 1) ||
                    airportList[s] == null)
                {
                    result.Append(s + " ");
                }
            }

            return result.ToString();
        }
    }
}
