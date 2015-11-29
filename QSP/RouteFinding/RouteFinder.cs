using System;
using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Containers;
using QSP.Core;
using static QSP.RouteFinding.RouteFindingCore;
using QSP.LibraryExtension;

namespace QSP.RouteFinding
{

    public static class RouteFinder
    {
        private const double MAX_DIS = 99999.0;

        /// <summary>
        /// Gets a route between two aiports, from ORIG to DEST.
        /// </summary>
        public static Route FindRoute(string origIcao, string origRwy, List<string> origSid, string destIcao, string destRwy, List<string> destStar)
        {

            SidHandler sidAdder = new SidHandler(QspCore.AppSettings.NavDBLocation, origIcao);
            int origIndex = sidAdder.AddSidsToWptList(origRwy, origSid);

            StarHandler starAdder = new StarHandler(QspCore.AppSettings.NavDBLocation, destIcao);
            int destIndex = starAdder.AddStarToWptList(destStar, destRwy);

            var result = getRoute(origIndex, destIndex);
            WptList.Restore();

            result.SetNat(NatsManager);
            return result;

        }

        /// <summary>
        /// Gets a route from DEST to ALTN.
        /// </summary>
        public static Route FindRoute(string destIcao, string altnIcao, string altnRwy, List<string> altnStar)
        {
            WptList.AddWpt(Utilities.DestWpt_AltnRouteCalcHelper(destIcao, altnIcao));
            int startIndex = WptList.Count - 1;

            StarHandler starAdder = new StarHandler(QspCore.AppSettings.NavDBLocation, altnIcao);
            int endIndex = starAdder.AddStarToWptList(altnStar, altnRwy);

            var result = getRoute(startIndex, endIndex);
            WptList.Restore();

            result.SetNat(NatsManager);
            return result;
        }

        /// <summary>
        /// Gets a route from an airport to a waypoint.
        /// </summary>
        public static Route FindRoute(string icao, string rwy, List<string> sid, int wptIndex)
        {
            SidHandler sidAdder = new SidHandler(QspCore.AppSettings.NavDBLocation, icao);
            int origIndex = sidAdder.AddSidsToWptList(rwy, sid);

            var result = getRoute(origIndex, wptIndex);
            WptList.Restore();

            result.SetNat(NatsManager);
            return result;
        }

        /// <summary>
        /// Gets a route from a waypoint to an airport.
        /// </summary>
        public static Route FindRoute(int wptIndex, string icao, string rwy, List<string> star)
        {

            StarHandler starAdder = new StarHandler(QspCore.AppSettings.NavDBLocation, icao);
            int endIndex = starAdder.AddStarToWptList(star, rwy);

            var result = getRoute(wptIndex, endIndex);
            WptList.Restore();

            result.SetNat(NatsManager);
            return result;

        }

        /// <summary>
        /// Gets a route from a waypoint to a waypoint.
        /// </summary>
        public static Route FindRoute(int wptIndex1, int wptIndex2)
        {

            var result = getRoute(wptIndex1, wptIndex2);
            WptList.Restore();

            result.SetNat(NatsManager);
            return result;

        }

        private static Route extractRoute(routeFindingData FindRouteData, int startPtIndex, int endPtIndex)
        {
            Route result = new Route();
            int index = endPtIndex;

            while (index != startPtIndex)
            {
                result.Waypoints.Add(WptList.ElementAt(index).Waypoint);
                result.Via.Add(FindRouteData.FromAirway[index]);

                index = FindRouteData.FromWptIndex[index];
            }

            result.Waypoints.Add(WptList.ElementAt(startPtIndex).Waypoint);

            result.TotalDis = FindRouteData.CurrentDis[endPtIndex];
            //total distance of the entire route

            result.Via.Reverse();
            result.Waypoints.Reverse();

            return result;
        }

        private static Route getRoute(int startPtIndex, int endPtIndex)
        {

            routeFindingData FindRouteData = new routeFindingData(WptList.Count);
            routeSeachRegionPara regionPara = new routeSeachRegionPara(startPtIndex, endPtIndex, 0);
            bool routeFound = false;

            while (!routeFound && regionPara.c <= 3000)
            {
                regionPara.c += 500;
                routeFound = findRouteAttempt(regionPara, ref FindRouteData);
            }

            if (routeFound)
            {
                return extractRoute(FindRouteData, startPtIndex, endPtIndex);
            }
            else
            {
                //TODO: create new exception class
                throw new Exception();
            }

        }

        private static bool findRouteAttempt(routeSeachRegionPara regionPara, ref routeFindingData findRouteData)
        {
            findRouteData.InitializeCurrentDis(regionPara.StartPtIndex);

            var unvisited = new MinHeap<int, double>();
            unvisited.Insert(regionPara.StartPtIndex, 0.0);

            while (unvisited.Count > 0)
            {
                var current = unvisited.PopMin();

                if (current.Key == regionPara.EndPtIndex)
                {
                    return true;
                }
                updateNeighbors(current.Key, regionPara, findRouteData, unvisited, current.Value);
            }
            return false; //Route not found.            
        }

        private static void updateNeighbors(int currentWptIndex, routeSeachRegionPara regionPara,
            routeFindingData FindRouteData, MinHeap<int, double> unvisited, double currentDis)
        {
            foreach (var neighbor in WptList.ElementAt(currentWptIndex).Neighbors)
            {
                if (wptWithinRange(neighbor.Index, regionPara))
                {
                    int index = neighbor.Index;
                    double newDis = currentDis + neighbor.Distance;

                    if (FindRouteData.CurrentDis[index] == MAX_DIS && newDis < MAX_DIS)
                    {
                        unvisited.Insert(index, newDis);
                        FindRouteData.SetValue(index, currentWptIndex, neighbor.Airway, newDis);
                    }
                    else if (unvisited.ItemExists(index) && newDis < unvisited.GetElement(index).Value)
                    {
                        unvisited.ReplaceValue(index, newDis);
                        FindRouteData.SetValue(index, currentWptIndex, neighbor.Airway, newDis);
                    }
                }
            }
        }

        private static bool wptWithinRange(int wptIndex, routeSeachRegionPara regionPara)
        {
            //suppose the orig and dest rwys are already in the wptList
            if (WptList.Distance(regionPara.StartPtIndex, wptIndex) + WptList.Distance(regionPara.EndPtIndex, wptIndex) >
                2 * Math.Sqrt(regionPara.b * regionPara.b + regionPara.c * regionPara.c))
            {
                return false;
            }
            return true;
        }

        #region Helper Classes

        private class routeFindingData
        {
            public int[] FromWptIndex;
            public string[] FromAirway;
            public double[] CurrentDis;

            public routeFindingData()
            {
            }

            /// <param name="waypointCount">Total number of waypoints</param>
            public routeFindingData(int waypointCount)
            {
                FromWptIndex = new int[waypointCount];
                FromAirway = new string[waypointCount];
                CurrentDis = new double[waypointCount];
            }

            public void InitializeCurrentDis(int startPtIndex)
            {
                int len = CurrentDis.Count();
                for (int i = 0; i < len; i++)
                {
                    CurrentDis[i] = MAX_DIS;
                }

                CurrentDis[startPtIndex] = 0.0;
            }

            public void SetValue(int index, int fromIndex, string airway, double distance)
            {
                FromWptIndex[index] = fromIndex;
                FromAirway[index] = airway;
                CurrentDis[index] = distance;
            }
        }

        private class routeSeachRegionPara
        {
            public int StartPtIndex;
            public int EndPtIndex;
            public double b;
            public double c;

            public routeSeachRegionPara(int startingPtIndex, int endingPtIndex, double c)
            {
                StartPtIndex = startingPtIndex;
                EndPtIndex = endingPtIndex;
                this.c = c;
                b = 0.5 * WptList.Distance(StartPtIndex, EndPtIndex);
            }
        }

        #endregion

    }


}
