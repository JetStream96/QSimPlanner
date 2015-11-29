using System.Collections.Generic;
using System.Linq;
using System.Text;
using QSP.RouteFinding.Tracks.Nats;
using QSP.Core;

namespace QSP.RouteFinding.Containers
{

    public class Route
    {

        public List<Waypoint> Waypoints { get; set; }
        public List<string> Via { get; set; }
        public double TotalDis { get; set; }

        //TODO: possibility of multiple NATs
        private bool natExpanded;
        //e.g. C (for Nat track C)
        private char natIdent;

        private Waypoint[] natWpts;
        //Includes end points. It's set to Nothing if it's not set yet, or not needed for the route.
        
        public Route()
        {
            Waypoints = new List<Waypoint>();
            Via = new List<string>();
            TotalDis = 0;
            natWpts = null;
            natExpanded = false;
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the airway specified.
        /// </summary>
        public void AppendWaypoint(Waypoint item, string viaAirway)
        {
            Waypoints.Add(item);
            Via.Add(viaAirway);
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by direct-to (DCT).
        /// </summary>

        public void AppendWaypoint(Waypoint item)
        {
            if (Waypoints.Count == 0)
            {
                Waypoints.Add(item);
            }
            else
            {
                AppendWaypoint(item, "DCT");
            }

        }

        /// <summary>
        /// Set NATs for ExpandNats/CollapseNats.
        /// </summary>
        /// <param name="handler"></param> 

        public void SetNat(NatHandler handler)
        {

            foreach (var i in Via)
            {
                if (i.Length == 4 && i[0] == 'N' && i[1] == 'A' && i[2] == 'T')
                {
                    natIdent = i[3];
                    natWpts = handler.GetTrackWaypointArray(natIdent);
                    return;

                }

            }

        }

        /// <summary>
        /// Set NATs for ExpandNats/CollapseNats.
        /// </summary>
        /// <param name="track"></param> 

        public void SetNat(NorthAtlanticTrack track)
        {
            natIdent = track.Ident;
            natWpts = new Waypoint[track.WptIndex.Count];

            for (int i = 0; i <= natWpts.Length - 1; i++)
            {
                natWpts[i] = RouteFindingCore.WptList.WaypointAt(track.WptIndex[i]);
            }
        }

        /// <summary>
        /// Collapse the NATs for the route, if not done already.  
        /// </summary>
        /// <remarks></remarks>

        public void CollapseNats()
        {
            if (natWpts == null || natExpanded == false)
            {
                return;
            }

            int numWptToRemove = natWpts.Count() - 2;
            int firstWptIndex = findWptIndex(natWpts[0].ID);

            Waypoints.RemoveRange(firstWptIndex + 1, numWptToRemove);
            Via.RemoveRange(firstWptIndex + 1, numWptToRemove);
            Via[firstWptIndex] = "NAT" + natIdent;

            natExpanded = false;

        }

        private int findWptIndex(string wptName)
        {

            for (int i = 0; i <= Waypoints.Count - 1; i++)
            {
                if (Waypoints[i].ID == wptName)
                {
                    return i;
                }
            }

            return -1;

        }

        private int findViaIndex(string airwayName)
        {

            for (int i = 0; i <= Via.Count - 1; i++)
            {
                if (Via[i] == airwayName)
                {
                    return i;
                }
            }

            return -1;

        }

        /// <summary>
        /// Expand the NATs for the route, if not already expanded. Must call SetNats before otherwise nothing will be done.
        /// </summary>

        public void ExpandNats()
        {
            if (natWpts == null || natExpanded)
            {
                return;
            }

            int currentIndex = findViaIndex("NAT" + natIdent);
            Via[currentIndex] = "DCT";
            currentIndex ++;


            if (natWpts.Count() > 2)
            {

                for (int j = 1; j <= natWpts.Count() - 2; j++)
                {
                    Waypoints.Insert(currentIndex, natWpts[j]);
                    Via.Insert(currentIndex, "DCT");
                    currentIndex ++;

                }

            }

            natExpanded = true;

        }

        /// <summary>
        /// A string represents the usual route text.
        /// </summary>
        public override string ToString()
        {
            return ToString(RouteDisplayOption.WaypointToWaypoint);
        }


        private void appendRoute(StringBuilder item)
        {
            item.Append(Via[0] + " ");


            for (int i = 1; i <= Via.Count - 1; i++)
            {

                if (Via[i] == "DCT" || Via[i] != Via[i - 1])
                {
                    item.Append(Waypoints[i].ID + " " + Via[i] + " ");

                }

            }

        }

        public enum NatsDisplayOption
        {
            Expand,
            Collapse
        }

        public enum RouteDisplayOption
        {

            AirportToAirport,
            AirportToWaypoint,
            WaypointToAirport,
            WaypointToWaypoint

        }

        /// <summary>
        /// A string represents the usual route text with the Nats display option.
        /// </summary>
        public string ToString(NatsDisplayOption para1, RouteDisplayOption para2)
        {

            switch (para1)
            {

                case NatsDisplayOption.Expand:
                    this.ExpandNats();

                    break;
                case NatsDisplayOption.Collapse:
                    this.CollapseNats();

                    break;
                default:

                    throw new InvalidAircraftDatabaseException("Incorrect enum for NatsDisplayOption.");
            }

            return this.ToString(para2);

        }

        /// <summary>
        /// A string represents the usual route text with options.
        /// </summary>
        /// <param name="para">AirportToAirport: Both first and last waypoint will not be shown.
        /// AirportToWaypoint: First waypoint will not be shown.
        /// WaypointToAirport: Last waypoint will not be shown.
        /// WaypointToWaypoint: Both first and last waypoint are shown.</param>
        public string ToString(RouteDisplayOption para)
        {

            StringBuilder result = new StringBuilder();

            if (para == RouteDisplayOption.WaypointToAirport || para == RouteDisplayOption.WaypointToWaypoint)
            {
                result.Append(Waypoints[0].ID + " ");
            }

            appendRoute(result);

            if (para == RouteDisplayOption.WaypointToWaypoint || para == RouteDisplayOption.AirportToWaypoint)
            {
                result.Append(Waypoints.Last().ID + " ");
            }

            return result.ToString();

        }

    }

}
