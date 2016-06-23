using MinMaxHeap;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding
{
    // The distance computed by RouteFinder is based on the the 
    // values of edges in graph, which directly comes from the 
    // text file and is only accurate to 2 decimal places.
    // Therefore it may not be completely accurate. 
    // 
    public class RouteFinder
    {
        private WaypointList wptList;

        public RouteFinder(WaypointList wptList)
        {
            this.wptList = wptList;
        }

        /// <summary>
        /// Add SID to wptList and returns the index of origin rwy.
        /// </summary>
        private int AddSid(
            string rwy, List<string> sid, SidHandler sidHandler)
        {
            return sidHandler.AddSidsToWptList(rwy, sid);
        }

        /// <summary>
        /// Add STAR to wptList and returns the index of destination rwy.
        /// </summary>
        private int AddStar(
            string rwy, List<string> star, StarHandler starHandler)
        {
            return starHandler.AddStarsToWptList(rwy, star);
        }

        /// <summary>
        /// Gets a route between two aiports, from ORIG to DEST.
        /// </summary>
        public Route FindRoute(
            string origRwy, List<string> origSid, SidHandler sidHandler,
            string destRwy, List<string> destStar, StarHandler starHandler,
            WaypointListEditor editor)
        {
            int origIndex = AddSid(origRwy, origSid, sidHandler);
            int destIndex = AddStar(destRwy, destStar, starHandler);

            var result = GetRoute(origIndex, destIndex);
            editor.Undo();
            return result;
        }

        /// <summary>
        /// Gets a route from an airport to a waypoint.
        /// </summary>
        public Route FindRoute(string rwy,
                               List<string> sid,
                               SidHandler sidHandler,
                               int wptIndex,
                               WaypointListEditor editor)
        {
            int origIndex = AddSid(rwy, sid, sidHandler);

            var result = GetRoute(origIndex, wptIndex);
            editor.Undo();
            return result;
        }

        /// <summary>
        /// Gets a route from a waypoint to an airport.
        /// </summary>
        public Route FindRoute(int wptIndex,
                               string rwy,
                               List<string> star,
                               StarHandler starHandler,
                               WaypointListEditor editor)
        {
            int endIndex = AddStar(rwy, star, starHandler);
            var result = GetRoute(wptIndex, endIndex);
            editor.Undo();
            return result;
        }

        /// <summary>
        /// Gets a route from a waypoint to a waypoint.
        /// </summary>
        public Route FindRoute(int wptIndex1, int wptIndex2)
        {
            var result = GetRoute(wptIndex1, wptIndex2);
            return result;
        }

        private Route ExtractRoute(
            routeFindingData FindRouteData, int startPtIndex, int endPtIndex)
        {
            var waypoints = new List<Waypoint>();
            var airways = new List<string>();
            var totalDistances = new List<double>();

            int index = endPtIndex;

            while (index != startPtIndex)
            {
                var finderData = FindRouteData.WaypointData[index];

                waypoints.Add(wptList[index]);
                airways.Add(finderData.FromAirway);
                totalDistances.Add(finderData.CurrentDistance);

                index = FindRouteData.WaypointData[index].FromWptIndex;
            }

            waypoints.Add(wptList[startPtIndex]);
            ConvertToNeighborDistance(totalDistances);

            return BuildRoute(waypoints, airways, totalDistances);
        }

        private static void ConvertToNeighborDistance(
            List<double> totalDistances)
        {
            for (int i = 0; i < totalDistances.Count - 1; i++)
            {
                totalDistances[i] -= totalDistances[i + 1];
            }
        }

        private static Route BuildRoute(
            List<Waypoint> waypoints,
            List<string> airways,
            List<double> totalDistances)
        {
            var result = new Route();
            int edgeCount = airways.Count;

            result.AddLastWaypoint(waypoints[edgeCount]);

            for (int i = edgeCount - 1; i >= 0; i--)
            {
                result.AddLastWaypoint(
                    waypoints[i],
                    airways[i],
                    totalDistances[i]);
            }
            return result;
        }

        /// <summary>
        /// Finds a route from the waypoint in wptList with 
        /// index startPtIndex, to endPtIndex.
        /// </summary>
        /// <exception cref="RouteNotFoundException"></exception>
        private Route GetRoute(int startPtIndex, int endPtIndex)
        {
            var FindRouteData = new routeFindingData(wptList.MaxSize);
            var regionPara = new routeSeachRegion(
                startPtIndex, endPtIndex, 0.0, wptList);

            bool routeFound = false;

            while (routeFound == false && regionPara.c <= 3000.0)
            {
                regionPara.c += 500.0;
                routeFound = FindRouteAttempt(regionPara, FindRouteData);
            }

            if (routeFound)
            {
                return ExtractRoute(FindRouteData, startPtIndex, endPtIndex);
            }
            else
            {
                throw new RouteNotFoundException();
            }
        }

        private bool FindRouteAttempt(
            routeSeachRegion regionPara, routeFindingData findRouteData)
        {
            findRouteData.InitializeDistance(regionPara.StartPtIndex);

            var unvisited = new MinHeap<int, double>();
            unvisited.Add(regionPara.StartPtIndex, 0.0);

            while (unvisited.Count > 0)
            {
                var current = unvisited.ExtractMin();

                if (current.Key == regionPara.EndPtIndex)
                {
                    return true;
                }

                UpdateNeighbors(
                    current.Key,
                    regionPara,
                    findRouteData,
                    unvisited,
                    current.Value);
            }

            return false; //Route not found.            
        }

        private void UpdateNeighbors(
            int currentWptIndex,
            routeSeachRegion regionPara,
            routeFindingData FindRouteData,
            MinHeap<int, double> unvisited,
            double currentDis)
        {
            foreach (var edgeIndex in wptList.EdgesFrom(currentWptIndex))
            {
                var wptData = FindRouteData.WaypointData;
                var edge = wptList.GetEdge(edgeIndex);
                int index = edge.ToNodeIndex;

                if (WptWithinRange(index, regionPara))
                {
                    double newDis = currentDis + edge.Value.Distance;

                    if (wptData[index].CurrentDistance == double.PositiveInfinity)
                    {
                        // The node was never touched.
                        unvisited.Add(index, newDis);
                        wptData[index] = new routeFindingData.WaypointStatus(
                            currentWptIndex, edge.Value.Airway, newDis);
                    }
                    else if (
                        unvisited.ContainsKey(index) &&
                        newDis < unvisited[index].Value)
                    {
                        unvisited.ChangeValue(index, newDis);
                        wptData[index] = new routeFindingData.WaypointStatus(
                            currentWptIndex, edge.Value.Airway, newDis);
                    }
                }
            }
        }

        private bool WptWithinRange(
            int wptIndex, routeSeachRegion regionPara)
        {
            //suppose the orig and dest rwys are already in the wptList
            var p = regionPara;

            return
                wptList.Distance(p.StartPtIndex, wptIndex) +
                wptList.Distance(p.EndPtIndex, wptIndex) <
                2 * Math.Sqrt(p.b * p.b + p.c * p.c);
        }

        #region Helper Classes

        private class routeFindingData
        {
            public WaypointStatus[] WaypointData { get; private set; }

            public routeFindingData() { }

            // Count: Total number of waypoints
            public routeFindingData(int Count)
            {
                WaypointData = new WaypointStatus[Count];
            }

            public void InitializeDistance(int startPtIndex)
            {
                int len = WaypointData.Length;

                for (int i = 0; i < len; i++)
                {
                    // Initial distance
                    WaypointData[i].CurrentDistance = double.PositiveInfinity;
                }

                WaypointData[startPtIndex].CurrentDistance = 0.0;
            }

            public struct WaypointStatus
            {
                public int FromWptIndex { get; set; }
                public string FromAirway { get; set; }
                public double CurrentDistance { get; set; }

                public WaypointStatus(
                    int FromWptIndex,
                    string FromAirway,
                    double CurrentDistance)
                {
                    this.FromWptIndex = FromWptIndex;
                    this.FromAirway = FromAirway;
                    this.CurrentDistance = CurrentDistance;
                }
            }
        }

        private class routeSeachRegion
        {
            public int StartPtIndex;
            public int EndPtIndex;
            public double b;
            public double c;

            public routeSeachRegion(
                int StartPtIndex,
                int EndPtIndex,
                double c,
                WaypointList wptList)
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
