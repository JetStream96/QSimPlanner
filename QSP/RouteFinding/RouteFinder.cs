using QSP.Core;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System;
using System.Collections.Generic;
using static QSP.RouteFinding.Constants;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding
{
    public class RouteFinder
    {
        private string navDBLoation;
        private WaypointList wptList;
        private AirportManager airportList;

        public RouteFinder() : this(QspCore.AppSettings.NavDBLocation, WptList, AirportList) { }

        public RouteFinder(string navDBLoation, WaypointList wptList, AirportManager airportList)
        {
            this.navDBLoation = navDBLoation;
            this.wptList = wptList;
            this.airportList = airportList;
        }

        /// <summary>
        /// Add SID to wptList and returns the index of origin rwy.
        /// </summary>
        private int addSid(string icao, string rwy, List<string> sid)
        {
            return new SidHandler(icao, navDBLoation, wptList, airportList).AddSidsToWptList(rwy, sid);
        }

        /// <summary>
        /// Add STAR to wptList and returns the index of destination rwy.
        /// </summary>
        private int addStar(string icao, string rwy, List<string> star)
        {
            var starAdder = new StarHandler(icao, navDBLoation, wptList, airportList);
            return starAdder.AddStarsToWptList(rwy, star);
        }

        /// <summary>
        /// Gets a route between two aiports, from ORIG to DEST.
        /// </summary>
        public ManagedRoute FindRoute(string origIcao, string origRwy, List<string> origSid, string destIcao, string destRwy, List<string> destStar)
        {
            int origIndex = addSid(origIcao, origRwy, origSid);
            int destIndex = addStar(destIcao, destRwy, destStar);

            var result = getRoute(origIndex, destIndex);
            wptList.Restore();
            return result;
        }

        /// <summary>
        /// Gets a route from an airport to a waypoint.
        /// </summary>
        public ManagedRoute FindRoute(string icao, string rwy, List<string> sid, int wptIndex)
        {
            int origIndex = addSid(icao, rwy, sid);

            var result = getRoute(origIndex, wptIndex);
            wptList.Restore();
            return result;
        }

        /// <summary>
        /// Gets a route from a waypoint to an airport.
        /// </summary>
        public ManagedRoute FindRoute(int wptIndex, string icao, string rwy, List<string> star)
        {
            int endIndex = addStar(icao, rwy, star);
            var result = getRoute(wptIndex, endIndex);
            wptList.Restore();
            return result;
        }

        /// <summary>
        /// Gets a route from a waypoint to a waypoint.
        /// </summary>
        public ManagedRoute FindRoute(int wptIndex1, int wptIndex2)
        {
            var result = getRoute(wptIndex1, wptIndex2);
            wptList.Restore();
            return result;
        }

        private ManagedRoute extractRoute(routeFindingData FindRouteData, int startPtIndex, int endPtIndex)
        {
            var result = new ManagedRoute(TracksInUse);

            var waypoints = new List<Waypoint>();
            var airways = new List<string>();
            var totalDistances = new List<double>();

            int index = endPtIndex;

            while (index != startPtIndex)
            {
                waypoints.Add(wptList[index]);
                airways.Add(FindRouteData.WaypointData[index].FromAirway);
                totalDistances.Add(FindRouteData.WaypointData[index].CurrentDistance);

                index = FindRouteData.WaypointData[index].FromWptIndex;
            }

            waypoints.Add(wptList[startPtIndex]);
            ConvertToNeighborDistance(totalDistances);

            return buildRoute(waypoints, airways, totalDistances);
        }

        private static void ConvertToNeighborDistance(List<double> totalDistances)
        {
            for (int i = 0; i < totalDistances.Count - 1; i++)
            {
                totalDistances[i] -= totalDistances[i + 1];
            }
        }

        private static ManagedRoute buildRoute(List<Waypoint> waypoints, List<string> airways, List<double> totalDistances)
        {
            var result = new ManagedRoute(TracksInUse);
            int edgeCount = airways.Count;

            result.AppendWaypoint(waypoints[edgeCount]);

            for (int i = edgeCount - 1; i >= 0; i--)
            {
                result.AppendWaypoint(waypoints[i],
                                      airways[i],
                                      totalDistances[i]);
            }
            return result;
        }

        /// <summary>
        /// Finds a route from the waypoint in wptList with index startPtIndex, to endPtIndex.
        /// </summary>
        /// <exception cref="RouteNotFoundException"></exception>
        private ManagedRoute getRoute(int startPtIndex, int endPtIndex)
        {
            var FindRouteData = new routeFindingData(wptList.MaxSize);
            var regionPara = new routeSeachRegionPara(startPtIndex, endPtIndex, 0.0, wptList);
            bool routeFound = false;

            while (routeFound == false && regionPara.c <= 3000.0)
            {
                regionPara.c += 500.0;
                routeFound = findRouteAttempt(regionPara, FindRouteData);
            }

            if (routeFound)
            {
                return extractRoute(FindRouteData, startPtIndex, endPtIndex);
            }
            else
            {
                throw new RouteNotFoundException();
            }
        }

        private bool findRouteAttempt(routeSeachRegionPara regionPara, routeFindingData findRouteData)
        {
            findRouteData.InitializeDistance(regionPara.StartPtIndex);

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

        private void updateNeighbors(int currentWptIndex, routeSeachRegionPara regionPara, routeFindingData FindRouteData,
                                     MinHeap<int, double> unvisited, double currentDis)
        {
            foreach (var edgeIndex in wptList.EdgesFrom(currentWptIndex))
            {
                var wptData = FindRouteData.WaypointData;
                var edge = wptList.GetEdge(edgeIndex);
                int index = edge.ToNodeIndex;

                if (wptWithinRange(index, regionPara))
                {
                    double newDis = currentDis + edge.value.Distance;

                    if (Math.Abs(wptData[index].CurrentDistance - MAX_DIS) < 1E-3 && newDis < MAX_DIS)
                    {
                        unvisited.Insert(index, newDis);
                        wptData[index] = new routeFindingData.WaypointStatus(currentWptIndex, edge.value.Airway, newDis);
                    }
                    else if (unvisited.ItemExists(index) && newDis < unvisited.GetElement(index).Value)
                    {
                        unvisited.ReplaceValue(index, newDis);
                        wptData[index] = new routeFindingData.WaypointStatus(currentWptIndex, edge.value.Airway, newDis);
                    }
                }
            }
        }

        private bool wptWithinRange(int wptIndex, routeSeachRegionPara regionPara)
        {
            //suppose the orig and dest rwys are already in the wptList
            return (wptList.Distance(regionPara.StartPtIndex, wptIndex) + wptList.Distance(regionPara.EndPtIndex, wptIndex) <
                    2 * Math.Sqrt(regionPara.b * regionPara.b + regionPara.c * regionPara.c));
        }

        #region Helper Classes

        private class routeFindingData
        {
            public WaypointStatus[] WaypointData { get; private set; }

            public routeFindingData()
            {
            }

            /// <param name="Count">Total number of waypoints</param>
            public routeFindingData(int Count)
            {
                WaypointData = new WaypointStatus[Count];
            }

            public void InitializeDistance(int startPtIndex)
            {
                int len = WaypointData.Length;

                for (int i = 0; i < len; i++)
                {
                    WaypointData[i].CurrentDistance = MAX_DIS;
                }
                WaypointData[startPtIndex].CurrentDistance = 0.0;
            }

            public struct WaypointStatus
            {
                public int FromWptIndex { get; set; }
                public string FromAirway { get; set; }
                public double CurrentDistance { get; set; }

                public WaypointStatus(int FromWptIndex, string FromAirway, double CurrentDistance)
                {
                    this.FromWptIndex = FromWptIndex;
                    this.FromAirway = FromAirway;
                    this.CurrentDistance = CurrentDistance;
                }
            }
        }

        private class routeSeachRegionPara
        {
            public int StartPtIndex;
            public int EndPtIndex;
            public double b;
            public double c;

            public routeSeachRegionPara(int StartPtIndex, int EndPtIndex, double c, WaypointList wptList)
            {
                this.StartPtIndex = StartPtIndex;
                this.EndPtIndex = EndPtIndex;
                this.c = c;
                b = 0.5 * wptList.Distance(this.StartPtIndex, this.EndPtIndex);
            }
        }

        #endregion

    }
}
