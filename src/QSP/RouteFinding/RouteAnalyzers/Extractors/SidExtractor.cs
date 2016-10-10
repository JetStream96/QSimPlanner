using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a LinkedList<string>, Extract() returns an object 
    // containing:
    //
    // * RemainingRoute, which represents a route which contains all 
    //   entries of the one given in constructor except:
    //   (1) Origin ICAO
    //   (2) SID name
    //   (3) The last waypoint of SID, if the waypoint is not in wptList.
    //
    // * A boolean which indicates whether SID exists.
    // * A route containing the departure runway and SID (if SID exists).

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
            IEnumerable<string> route,
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
