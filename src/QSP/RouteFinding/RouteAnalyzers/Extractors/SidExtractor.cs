using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a RouteString, Extract() returns an object containing:
    //
    // * An OriginRoute containing the departure runway and SID 
    //   (if SID exists).
    //   There are 4 cases:
    //   1. The first element of input RouteString is not a SID.
    //   2. The SID ends with a vector.
    //   3. The SID ends with a waypoint. The waypoint is in wptList, which
    //      may or may not be connected to an airway.
    //   4. The SID ends with a waypoint. The waypoint is NOT in wptList.
    //
    //   For different cases, the returning route contains:
    //   Case 1. The origin runway, then direct to the first enroute waypoint.
    //   Case 2. The origin runway, then go to the first enroute waypoint
    //           via SID.
    //   Case 3. The origin runway, then go to the last waypoint of SID (via SID).
    //   Case 4. The origin runway, then go to the last waypoint of SID (via SID), 
    //           then direct to the first enroute waypoint.
    //          
    // * A RemainingRoute. In all cases, the first entry in RemainingRoute
    //   is guranteed to be the same as the last waypoint in the OriginRoute.
    //
    // The input route should not contain the origin ICAO, and must contain 
    // one element.

    public class SidExtractor
    {
        private readonly WaypointList wptList;
        private readonly SidCollection sids;
        private readonly Waypoint rwyWpt;
        private readonly string rwy;
        private readonly LinkedList<string> route;

        public SidExtractor(
            RouteString route,
            string rwy,
            Waypoint rwyWpt,
            WaypointList wptList,
            SidCollection sids)
        {
            this.route = new LinkedList<string>(route);
            this.rwy = rwy;
            this.rwyWpt = rwyWpt;
            this.wptList = wptList;
            this.sids = sids;
        }

        // @Throws
        // May throw exception if input is not valid.
        public ExtractResult Extract()
        {
            var first = route.First();
            var sid = TryGetSid(first);

            if (sid == null)
            {
                // Case 1
                var wpt = FindWpt(first);

                var neighbor = new Neighbor("DCT", rwyWpt.Distance(wpt));
                var node1 = new RouteNode(rwyWpt, neighbor);
                var node2 = new RouteNode(wpt, null);
                var origRoute = new Route(node1, node2);

                return new ExtractResult(route.ToRouteString(), origRoute);
            }

            // Remove SID from RouteString.
            route.RemoveFirst();

            if (sid.EndsWithVector)
            {
                // Case 2
                var wpt = FindWpt(route.First());
                double distance = sid.Waypoints.Concat(wpt).TotalDistance();
                var innerWpts = sid.Waypoints.Skip(1).ToList();

                var neighbor = new Neighbor(
                    first, distance, innerWpts, InnerWaypointsType.Terminal);
                var node1 = new RouteNode(rwyWpt, neighbor);
                var node2 = new RouteNode(wpt, null);
                var origRoute = new Route(node1, node2);

                return new ExtractResult(route.ToRouteString(), origRoute);
            }

            // Case 3, 4            
            var sidLastWpt = sid.Waypoints.Last();

            if (sidLastWpt.ID != route.First())
            {
                throw new ArgumentException($"{route.First()} is not the last"
                    + $" waypoint of the SID {first}.");
            }

            if (wptList.FindByWaypoint(sidLastWpt) == -1)
            {
                // Case 4

                route.RemoveFirst();
                // Now the first item of route is the first enroute waypoint.

                double distance1 = sid.Waypoints.TotalDistance();
                var innerWpts = sid.Waypoints.WithoutFirstAndLast();

                var neighbor1 = new Neighbor(
                    first, distance1, innerWpts, InnerWaypointsType.Terminal);
                var node1 = new RouteNode(rwyWpt, neighbor1);

                var lastSidWpt = sid.Waypoints.Last();
                var firstEnrouteWpt = FindWpt(route.First());
                double distance2 = lastSidWpt.Distance(firstEnrouteWpt);
                var neighbor2 = new Neighbor("DCT", distance2);
                var node2 = new RouteNode(lastSidWpt, neighbor2);

                var node3 = new RouteNode(firstEnrouteWpt, null);
                var origRoute = new Route(node1, node2, node3);

                return new ExtractResult(route.ToRouteString(), origRoute);
            }
            else
            {
                // Case 3
                double distance1 = sid.Waypoints.TotalDistance();
                var innerWpts = sid.Waypoints.WithoutFirstAndLast();

                var neighbor1 = new Neighbor(
                    first, distance1, innerWpts, InnerWaypointsType.Terminal);
                var node1 = new RouteNode(rwyWpt, neighbor1);
                var node2 = new RouteNode(sid.Waypoints.Last(), null);
                var origRoute = new Route(node1, node2);

                return new ExtractResult(route.ToRouteString(), origRoute);
            }
        }

        private Waypoint FindWpt(string ident)
        {
            var ids = wptList.FindAllById(ident);
            if (ids.Count == 0)
            {
                throw new ArgumentException($"Cannot find waypoint {ident}.");
            }

            return ids.Select(i => wptList[i]).GetClosest(rwyWpt);
        }

        public class ExtractResult
        {
            public RouteString RemainingRoute;
            public Route OrigRoute;

            public ExtractResult(RouteString RemainingRoute, Route OrigRoute)
            {
                this.RemainingRoute = RemainingRoute;
                this.OrigRoute = OrigRoute;
            }
        }

        private SidWaypoints TryGetSid(string sidName)
        {
            try
            {
                return sids.SidWaypoints(sidName, rwy, rwyWpt);
            }
            catch
            {
                // no SID in route
                return null;
            }
        }
    }
}
