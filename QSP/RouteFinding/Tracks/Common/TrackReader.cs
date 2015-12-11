using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using QSP.AviationTools;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.Tracks.Common
{

    public class TrackReader
    {

        //Read the track waypoints as strings, and try to find each waypoints in WptList

        private List<WptPair> routeFromTo;

        private Route _mainRoute;

        private ITrack trk;

        public TrackReader(ITrack item)
        {
            trk = item;
            _mainRoute = readMainRoute(trk.MainRoute);

            //The format of this part is rather unpredictable. For example, a route can even start with an airway:
            //RTS/CYVR V317 QQ YZT JOWEN 
            //KSEA TOU FINGS JOWEN 
            //...
            //Since this part is not that important, we can allow it to fail and still ignore it.
            try
            {
                routeFromTo = findWptAllRouteFrom(trk.RouteFrom);
                routeFromTo.AddRange(findWptAllRouteTo(trk.RouteTo));
                routeFromTo = routeFromTo.Distinct().ToList();
            }
            catch 
            {
                routeFromTo = new List<WptPair>();
            }

        }

        public Route MainRoute
        {
            get { return _mainRoute; }
        }

        public ReadOnlyCollection<WptPair> PairsToAdd
        {
            get { return routeFromTo.AsReadOnly(); }
        }

        #region "Method for routeFrom/To"

        private List<WptPair> getExtraPairs(string[] rteFrom, double prevLat, double prevLon)
        {

            List<WptPair> result = new List<WptPair>();
            int lastIndex = -1;


            for (int index = 0; index <= rteFrom.Count() - 1; index++)
            {

                if (lastIndex >= 0)
                {
                    if (isAirway(lastIndex, rteFrom[index]))
                    {
                        lastIndex = -1;

                    }
                    else
                    {
                        int wpt = selectWpt(prevLat, prevLon, rteFrom[index]);
                        result.Add(new WptPair(lastIndex, wpt));
                        lastIndex = wpt;

                    }


                }
                else
                {
                    lastIndex = selectWpt(prevLat, prevLon, rteFrom[index]);

                }

            }

            return result;

        }

        private int selectWpt(double prevLat, double prevLon, string ident)
        {

            var candidates = RouteFindingCore.WptList.FindAllByID(ident);

            if (candidates == null || candidates.Count == 0)
            {
                throw new TrackWaypointNotFoundException("Waypoint not found.");
            }

            return Utilities.ChooseSubsequentWpt(prevLat, prevLon, candidates);

        }

        private bool isAirway(int lastIndex, string airway)
        {


            foreach (var i in RouteFindingCore.WptList[lastIndex].Neighbors)
            {
                if (i.Airway == airway)
                {
                    return true;
                }

            }

            return false;

        }

        private List<WptPair> findWptAllRouteFrom(ReadOnlyCollection<string[]> rteFrom)
        {

            List<WptPair> result = new List<WptPair>();
            var firstWpt = _mainRoute.Waypoints.First();

            foreach (var i in rteFrom)
            {
                result.AddRange(getExtraPairs(i, firstWpt.Lat, firstWpt.Lon));
            }

            return result;

        }

        private List<WptPair> findWptAllRouteTo(ReadOnlyCollection<string[]> rteTo)
        {

            List<WptPair> result = new List<WptPair>();
            var lastWpt = _mainRoute.Waypoints.Last();

            foreach (var i in rteTo)
            {
                result.AddRange(getExtraPairs(i, lastWpt.Lat, lastWpt.Lon));
            }

            return result;

        }

        #endregion

        #region "Method for main route"

        private Route readMainRoute(ReadOnlyCollection<string> rte)
        {

            LatLon latLon = trk.PreferredFirstLatLon;
            return new SimpleRouteAnalyzer(combineArr(rte), latLon.Lat, latLon.Lon).Parse();

        }

        private string combineArr(ReadOnlyCollection<string> item)
        {

            StringBuilder result = new StringBuilder();

            foreach (var i in item)
            {
                result.Append(i + " ");
            }

            return result.ToString();

        }

        #endregion

    }

}
