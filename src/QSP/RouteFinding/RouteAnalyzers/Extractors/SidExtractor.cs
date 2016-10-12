using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using System.Linq;
using RouteString = System.Collections.Generic.IReadOnlyList<string>;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a RouteString, Extract() returns an object 
    // containing:
    //
    // * A boolean which indicates whether SID exists.
    //
    // * An OriginRoute containing the departure runway and SID 
    //   (if SID exists).
    //   There are 4 cases:
    //   1. The first element of input RouteString is not a SID.
    //   2. The SID ends with a vector.
    //   3. The SID ends with a waypoint. The waypoint is in wptList, which
    //      may or may not be connected to an airway.
    //   4. The SID ends with a waypoint. The waypoint is NOT in wptList.

    //   For different cases, the returning route contains:
    //   Case 1. The origin runway, then direct to the first enroute waypoint.
    //   Case 2. The origin runway, then go to the first enroute waypoint
    //           via SID.
    //   Case 3. The origin runway, then go to the last waypoint of SID (via 
    //           SID).
    //   Case 4. The origin runway, then go to the last waypoint of SID (via 
    //           SID), then direct to the first enroute waypoint.
    //
    // * A RemainingRoute. In all cases, the first entry in RemainingRoute
    //   must be the same as the last waypoint in the OriginRoute.
    //
    // The input route should not contain the origin ICAO, and must contain 
    // one element.

    public class SidExtractor
    {
        private WaypointList wptList;
        private SidCollection sids;
        private Waypoint rwyWpt;
        private string icao;
        private string rwy;
        private bool sidExists;

        private LinkedList<string> route;
        private Route origRoute;

        public SidExtractor(
            RouteString route,
            string icao,
            string rwy,
            Waypoint rwyWpt,
            WaypointList wptList,
            SidCollection sids)
        {
            this.route = new LinkedList<string>(route);
            this.icao = icao;
            this.rwy = rwy;
            this.rwyWpt = rwyWpt;
            this.wptList = wptList;
            this.sids = sids;
            sidExists = false;
        }

        public ExtractResult Extract()
        {
            origRoute = new Route();
            origRoute.AddLastWaypoint(rwyWpt);

            if (route.Count > 0) CreateOrigRoute();

            return new ExtractResult
            {
                RemainingRoute = route,
                Sid = origRoute,
                SidExists = sidExists
            };
        }

        public class ExtractResult
        {
            public IEnumerable<string> RemainingRoute;
            public bool SidExists;
            public Route Sid;
        }

        private void CreateOrigRoute()
        {
            if (route.First.Value == icao) route.RemoveFirst();

            string sidName = route.First.Value;
            var sid = TryGetSid(sidName, rwyWpt);

            if (sid != null)
            {
                var waypoints = sid.Waypoints;
                route.RemoveFirst();
                var last = origRoute.Last.Value;
                last.AirwayToNext = sidName;
                sidExists = true;

                if (sid.Waypoints.Count >= 2)
                {
                    // SID has at least one waypoint other than the runway.
                    last.DistanceToNext = waypoints.TotalDistance();
                    var lastWpt = waypoints.Last();
                    origRoute.AddLastWaypoint(lastWpt);

                    if (route.First.Value == lastWpt.ID &&
                        wptList.FindByWaypoint(lastWpt) == -1)
                    {
                        route.RemoveFirst();
                    }
                }
            }
        }

        private SidWaypoints TryGetSid(string sidName, Waypoint rwyWpt)
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
