using MinMaxHeap;
using QSP.LibraryExtension.Graph;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.WindAloft;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static System.Math;

namespace QSP.RouteFinding
{
    // Effect of wind is not be applied to SID, STAR legs.
    // 
    public class RouteFinder
    {
        private WaypointList wptList;
        private CountryCodeCollection avoidedCountry;
        private AvgWindCalculator windCalc;

        public RouteFinder(
            WaypointList wptList,
            CountryCodeCollection avoidedCountry = null,
            AvgWindCalculator windCalc = null)
        {
            this.wptList = wptList;
            this.avoidedCountry =
                avoidedCountry ?? new CountryCodeCollection();
            this.windCalc = windCalc;
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
        /// <exception cref="RouteNotFoundException"></exception>
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
        public Route FindRoute(
            string rwy,
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
        public Route FindRoute(
            int wptIndex,
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
            RouteFindingData FindRouteData, int startPtIndex, int endPtIndex)
        {
            var route = new Route();
            var wptData = FindRouteData.WaypointData;
            var edge = wptData[endPtIndex].FromEdge;

            while (true)
            {
                int from = edge.FromNodeIndex;
                int to = edge.ToNodeIndex;
                var wptFrom = wptList[from];
                var neighbor = edge.Value;
                var airway = neighbor.Airway;

                if (to == endPtIndex)
                {
                    route.AddFirstWaypoint(wptList[to], "");
                }

                var n = new RouteNode(wptFrom, neighbor);
                route.Nodes.AddFirst(n);

                if (from == startPtIndex) return route;
                edge = wptData[from].FromEdge;
            }
        }

        /// <summary>
        /// Finds a route from the waypoint in wptList with 
        /// index startPtIndex, to endPtIndex.
        /// </summary>
        /// <exception cref="RouteNotFoundException"></exception>
        private Route GetRoute(int startPtIndex, int endPtIndex)
        {
            var FindRouteData = new RouteFindingData(
                wptList.NodeIndexUpperBound + 1);

            var region = new RouteSeachRegion(
                wptList, startPtIndex, endPtIndex);

            region.MaxDistanceSum = region.DirectDistance * 1.25;
            bool routeFound = false;

            while (routeFound == false &&
                region.MaxDistanceSum <= region.MaxPossibleDistanceSum)
            {
                routeFound = FindRouteAttempt(region, FindRouteData);
                region.MaxDistanceSum *= 1.5;
            }

            if (routeFound)
            {
                return ExtractRoute(FindRouteData, startPtIndex, endPtIndex);
            }

            throw new RouteNotFoundException(
                $"No route exists between {wptList[startPtIndex].ID} " +
                $"and {wptList[endPtIndex].ID}.");
        }

        private bool FindRouteAttempt(
            RouteSeachRegion regionPara,
            RouteFindingData findRouteData)
        {
            findRouteData.Init(regionPara.StartPtIndex);

            var unvisited = new MinHeap<int, double>();
            unvisited.Add(regionPara.StartPtIndex, 0.0);

            while (unvisited.Count > 0)
            {
                var current = unvisited.ExtractMin();
                if (current.Key == regionPara.EndPtIndex) return true;

                UpdateNeighbors(
                    current.Key,
                    regionPara,
                    findRouteData,
                    unvisited,
                    current.Value);
            }

            return false; //Route not found.            
        }

        private double GetEdgeDistance(Edge<Neighbor> edge)
        {
            if (windCalc == null) return edge.Value.Distance;

            var neighbor = edge.Value;
            var from = wptList[edge.FromNodeIndex];
            var to = wptList[edge.ToNodeIndex];

            if (neighbor.InnerWaypoints.Count == 0)
            {
                return windCalc.GetAirDistance(from, to);
            }

            var waypoints = new Waypoint[] { from }
            .Concat(neighbor.InnerWaypoints)
            .Concat(new Waypoint[] { to });

            return windCalc.GetAirDistance(waypoints);
        }
        
        private void UpdateNeighbors(
            int currentWptIndex,
            RouteSeachRegion regionPara,
            RouteFindingData findRouteData,
            MinHeap<int, double> unvisited,
            double currentDis)
        {
            foreach (var edgeIndex in wptList.EdgesFrom(currentWptIndex))
            {
                var wptData = findRouteData.WaypointData;
                var edge = wptList.GetEdge(edgeIndex);
                int index = edge.ToNodeIndex;
                var countryCode = wptList[index].CountryCode;

                if (WptWithinRange(findRouteData, index, regionPara) &&
                    avoidedCountry.Contains(countryCode) == false)
                {
                    double newDis = currentDis + GetEdgeDistance(edge);

                    if (wptData[index].CurrentDistance ==
                        double.PositiveInfinity)
                    {
                        // The node was never touched.
                        unvisited.Add(index, newDis);
                        wptData[index] = new RouteFindingData.WaypointStatus(
                            edge,
                            newDis,
                            InRange.Yes);
                    }
                    else if (unvisited.ContainsKey(index) &&
                        newDis < unvisited[index].Value)
                    {
                        unvisited.ChangeValue(index, newDis);
                        wptData[index] = new RouteFindingData.WaypointStatus(
                            edge,
                            newDis,
                            InRange.Yes);
                    }
                }
            }
        }

        private bool WptWithinRange(RouteFindingData findRouteData,
            int wptIndex, RouteSeachRegion region)
        {
            var data = findRouteData.WaypointData;

            if (data[wptIndex].WithInRange == InRange.Unknown)
            {
                // Suppose the orig and dest rwys are already in the wptList
                var p = region;

                bool inRange =
                    wptList.Distance(p.StartPtIndex, wptIndex) +
                    wptList.Distance(p.EndPtIndex, wptIndex) <
                    p.MaxDistanceSum;

                data[wptIndex].WithInRange =
                    inRange ? InRange.Yes : InRange.No;
            }

            return (int)data[wptIndex].WithInRange == 1;
        }

        #region Helper Classes

        public enum InRange
        {
            Unknown = 0,
            Yes = 1,
            No = 2
        }

        private class RouteFindingData
        {
            public WaypointStatus[] WaypointData { get; private set; }

            public RouteFindingData() { }

            // Count: Total number of waypoints
            public RouteFindingData(int Count)
            {
                WaypointData = new WaypointStatus[Count];
            }

            public void Init(int startPtIndex)
            {
                int len = WaypointData.Length;

                for (int i = 0; i < len; i++)
                {
                    // Initial distance
                    WaypointData[i] = new WaypointStatus(
                        null, double.PositiveInfinity, InRange.Unknown);
                }

                WaypointData[startPtIndex] = new WaypointStatus(
                        null, 0.0, InRange.Unknown);
            }

            public struct WaypointStatus
            {
                public Edge<Neighbor> FromEdge { get; set; }
                public double CurrentDistance { get; set; }
                public InRange WithInRange { get; set; }

                public WaypointStatus(
                    Edge<Neighbor> FromEdge,
                    double CurrentDistance,
                    InRange WithInRange)
                {
                    this.FromEdge = FromEdge;
                    this.CurrentDistance = CurrentDistance;
                    this.WithInRange = WithInRange;
                }
            }
        }

        private class RouteSeachRegion
        {
            public readonly double MaxPossibleDistanceSum = PI * EarthRadiusNm;

            public int StartPtIndex { get; private set; }
            public int EndPtIndex { get; private set; }
            public double DirectDistance { get; private set; }
            public double MaxDistanceSum { get; set; }

            public RouteSeachRegion(
                WaypointList wptList,
                int StartPtIndex,
                int EndPtIndex,
                double MaxDistanceSum = 0.0)
            {
                this.StartPtIndex = StartPtIndex;
                this.EndPtIndex = EndPtIndex;
                this.MaxDistanceSum = MaxDistanceSum;
                DirectDistance = wptList.Distance(StartPtIndex, EndPtIndex);
            }
        }

        #endregion

    }
}
