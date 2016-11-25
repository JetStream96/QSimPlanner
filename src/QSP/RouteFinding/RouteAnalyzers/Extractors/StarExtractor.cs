using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Star;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a RouteString, Extract() returns an object 
    // containing:
    //
    // * An DestRoute containing the departure runway and STAR
    //   (if STAR exists).
    //   There are 3 cases:
    //   1. The last element of input RouteString is not a STAR.
    //   2. The first waypoint of STAR is in wptList, which
    //      may or may not be connected to an airway.
    //   3. The first waypoint of STAR is NOT in wptList.

    //   For different cases, the returning route contains:
    //   Case 1. The last enroute waypoint, then direct to the dest runway.
    //   Case 2. The first waypoint of STAR, then go to the dest runway
    //           (via STAR).
    //   Case 3. The last enroute waypoint, then direct to the first waypoint 
    //           of STAR. Then go to the dest runway (via STAR).
    //
    // * A RemainingRoute. In all cases, the last entry in RemainingRoute
    //   is guranteed to be the same as the first waypoint in the DestRoute.
    //
    // The input route should not contain the destination ICAO, and must 
    // contain one element.

    public class StarExtractor
    {
        private readonly WaypointList wptList;
        private readonly StarCollection stars;
        private readonly Waypoint rwyWpt;
        private readonly string rwy;
        private readonly LinkedList<string> route;

        public StarExtractor(
            RouteString route,
            string rwy,
            Waypoint rwyWpt,
            WaypointList wptList,
            StarCollection stars)
        {
            this.route = new LinkedList<string>(route);
            this.rwy = rwy;
            this.rwyWpt = rwyWpt;
            this.wptList = wptList;
            this.stars = stars;
        }

        public ExtractResult Extract()
        {
            var last = route.Last.Value;
            var star = TryGetStar(last);

            if (star == null)
            {
                // Case 1
                var wpt = FindWpt(last);

                var neighbor = new Neighbor("DCT", wpt.Distance(rwyWpt));
                var node1 = new RouteNode(wpt, neighbor);
                var node2 = new RouteNode(rwyWpt, null);
                var destRoute = new Route(node1, node2);

                return new ExtractResult(route.ToRouteString(), destRoute);
            }

            // Remove STAR from RouteString.
            route.RemoveLast();

            // Case 2, 3 
            var starFirstWpt = star.First();

            if (starFirstWpt.ID != route.Last.Value)
            {
                throw new ArgumentException($"{route.Last.Value} is not the"
                    + $" first waypoint of the STAR {last}.");
            }

            if (wptList.FindByWaypoint(starFirstWpt) == -1)
            {
                // Case 3

                route.RemoveLast();
                // Now the last item of route is the last enroute waypoint.

                var lastEnrouteWpt = FindWpt(route.Last.Value);
                var firstStarWpt = star.First();
                double distance1 = lastEnrouteWpt.Distance(firstStarWpt);

                var neighbor1 = new Neighbor("DCT", distance1);
                var node1 = new RouteNode(lastEnrouteWpt, neighbor1);

                double distance2 = star.TotalDistance();
                var innerWpts = star.WithoutFirstAndLast();

                var neighbor2 = new Neighbor(
                    last, distance2, innerWpts, InnerWaypointsType.Terminal);
                var node2 = new RouteNode(firstStarWpt, neighbor2);

                var node3 = new RouteNode(rwyWpt, null);
                var destRoute = new Route(node1, node2, node3);

                return new ExtractResult(route.ToRouteString(), destRoute);
            }
            else
            {
                // Case 2
                var firstStarWpt = star.First();
                double distance = star.TotalDistance();
                var innerWpts = star.WithoutFirstAndLast();

                var neighbor = new Neighbor(
                    last, distance, innerWpts, InnerWaypointsType.Terminal);
                var node1 = new RouteNode(firstStarWpt, neighbor);

                var node2 = new RouteNode(rwyWpt, null);
                var destRoute = new Route(node1, node2);

                return new ExtractResult(route.ToRouteString(), destRoute);
            }
        }

        public class ExtractResult
        {
            public RouteString RemainingRoute;
            public Route DestRoute;

            public ExtractResult(RouteString RemainingRoute, Route DestRoute)
            {
                this.RemainingRoute = RemainingRoute;
                this.DestRoute = DestRoute;
            }
        }

        private IReadOnlyList<Waypoint> TryGetStar(string starName)
        {
            try
            {
                return stars.StarWaypoints(starName, rwy, rwyWpt);
            }
            catch
            {
                // no Star in route                
                return null;
            }
        }

        private Waypoint FindWpt(string ident)
        {
            var ids = wptList.FindAllById(ident);
            if (ids.Count == 0)
            {
                throw new ArgumentException($"Cannot find waypoint {ident}.");
            }

            return ids
                .Select(i => wptList[i])
                .GetClosest(rwyWpt);
        }
    }
}
