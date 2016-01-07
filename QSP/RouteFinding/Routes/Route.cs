using System.Collections.Generic;
using System.Linq;
using System.Text;
using QSP.RouteFinding.Tracks.Nats;
using QSP.Core;
using QSP.RouteFinding.Containers;
using static QSP.MathTools.Utilities;
using System;
using System.Collections;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Routes
{
    public class Route : IEnumerable<RouteNode>, IEnumerable
    {
        private LinkedList<RouteNode> links;
        private RouteToggler toggler;

        public double TotalDistance
        {
            get
            {
                if (links.Count == 0)
                {
                    throw new InvalidOperationException("The route is empty.");
                }

                double totalDis = 0.0;
                var first = links.First;
                var node = first;
                var next = node.Next;

                while (next != first)
                {
                    totalDis += node.Value.DistanceToNext;
                    node = next;
                    next = node.Next;
                }

                return totalDis;
            }
        }

        public Route()
        {
            links = new LinkedList<RouteNode>();
            toggler = new RouteToggler(links);
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the airway specified, with the given distance.       
        /// </summary>
        /// <param name="viaAirway">Airway or SID/STAR name.</param>
        public void AppendWaypoint(Waypoint item, string viaAirway, double distanceFromPrev)
        {
            var last = links.Last;

            if (last != null)  // Route is non-empty.
            {
                last.Value.DistanceToNext = distanceFromPrev;
            }
            links.AddLast(new RouteNode(item, viaAirway, 0.0));
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the airway specified.
        /// </summary>
        public void AppendWaypoint(Waypoint item, string viaAirway)
        {
            var last = links.Last;
            double distance;

            if (last != null)  // Route is non-empty.
            {
                var lastWpt = last.Value.Waypoint;
                distance = GreatCircleDistance(item.Lat, item.Lon, lastWpt.Lat, lastWpt.Lon);
            }
            else
            {
                distance = 0.0;
            }

            AppendWaypoint(item,
                           viaAirway,
                           distance);
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by direct-to (DCT).
        /// </summary>
        public void AppendWaypoint(Waypoint item)
        {
            AppendWaypoint(item, "DCT");
        }

        //public void AddAfter(LinkedListNode<RouteNode> node, RouteNode nodeToAdd)
        //{
        //    links.AddAfter(node, nodeToAdd);
        //}
        
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

            for (int i = 0; i < natWpts.Length; i++)
            {
                natWpts[i] = RouteFindingCore.WptList[track.WptIndex[i]];
            }
        }

        /// <summary>
        /// Collapse the tracks for the route, if not done already.  
        /// </summary>
        public void Collapse()
        {
            toggler.Collapse();
        }
        
        /// <summary>
        /// Expand the Tracks for the route, if not already expanded. 
        /// </summary>
        public void Expand()
        {
            toggler.Expand();
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

            for (int i = 1; i < Via.Count; i++)
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
        /// <exception cref="InvalidAircraftDatabaseException"></exception>
        public string ToString(NatsDisplayOption para1, RouteDisplayOption para2)
        {
            switch (para1)
            {

                case NatsDisplayOption.Expand:
                    this.Expand();

                    break;
                case NatsDisplayOption.Collapse:
                    this.Collapse();

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

        public IEnumerator<RouteNode> GetEnumerator()
        {
            return links.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
