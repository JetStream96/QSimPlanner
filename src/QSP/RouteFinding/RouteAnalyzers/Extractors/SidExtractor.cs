using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a LinkedList<string>, Extract() returns an object 
    // containing:
    //
    // * RemainingRoute, which represents a route which contains all 
    //   entries of the one given in constructor except:
    //   (1) origin ICAO
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

            if (route.Count > 0)
            {
                CreateOrigRoute();
            }

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
            if (route.First.Value == icao)
            {
                route.RemoveFirst();
            }

            string sidName = route.First.Value;
            var sid = TryGetSid(sidName, rwyWpt);

            if (sid != null)
            {
                route.RemoveFirst();
                var last = origRoute.Last.Value;
                last.AirwayToNext = sidName;
                sidExists = true;

                if (Math.Abs(sid.TotalDistance) > 1E-8)
                {
                    // SID has at least one waypoint.                    
                    last.DistanceToNext = sid.TotalDistance;
                    origRoute.AddLastWaypoint(sid.LastWaypoint);

                    if (route.First.Value == sid.LastWaypoint.ID &&
                        wptList.FindByWaypoint(sid.LastWaypoint) == -1)
                    {
                        route.RemoveFirst();
                    }
                }
            }
        }

        private SidInfo TryGetSid(string sidName, Waypoint rwyWpt)
        {
            try
            {
                return sids.GetSidInfo(sidName, rwy, rwyWpt);
            }
            catch
            {
                // no SID in route
                return null;
            }
        }
    }
}
