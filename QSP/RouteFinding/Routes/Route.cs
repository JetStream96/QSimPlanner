using QSP;
using QSP.Core;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Nats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QSP.MathTools.Utilities;

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

        public RouteNode First
        {
            get
            {
                return links.First.Value;
            }
        }

        public LinkedListNode<RouteNode> FirstNode
        {
            get
            {
                return links.First;
            }
        }

        public RouteNode Last
        {
            get
            {
                return links.Last.Value;
            }
        }

        public LinkedListNode<RouteNode> LastNode
        {
            get
            {
                return links.Last;
            }
        }

        public int Count
        {
            get
            {
                return links.Count;
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
                last.Value.AirwayToNext = viaAirway;
            }
            links.AddLast(new RouteNode(item));
        }

        public void AppendWaypoint(Waypoint item, string viaAirway, bool AutoComputeDistance)
        {
            if (AutoComputeDistance && links.Last != null)
            {
                var lastWpt = links.Last.Value.Waypoint;
                AppendWaypoint(item,
                               viaAirway,
                               GreatCircleDistance(item.Lat, item.Lon, lastWpt.Lat, lastWpt.Lon));
            }
            else
            {
                AppendWaypoint(item, viaAirway);
            }
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the airway specified.
        /// </summary>
        public void AppendWaypoint(Waypoint item, string viaAirway)
        {
            if (links.Last != null)
            {
                links.Last.Value.AirwayToNext = viaAirway;
            }
            links.AddLast(new RouteNode(item));
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by direct-to (DCT).
        /// </summary>
        public void AppendWaypoint(Waypoint item, bool AutoConnectToPrev)
        {
            if (AutoConnectToPrev)
            {
                AppendWaypoint(item, "DCT", true);
            }
            else
            {
                links.AddLast(new RouteNode(item));
            }
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// </summary>
        public void AppendWaypoint(Waypoint item)
        {
            links.AddLast(new RouteNode(item));
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
            return ToString(false, false);
        }

        public enum TracksDisplayOption
        {
            Expand,
            Collapse
        }

        /// <summary>
        /// A string represents the usual route text with the Nats display option.
        /// </summary>
        /// <exception cref="EnumNotSupportedException"></exception>
        public string ToString(bool ShowFirstWaypoint, bool ShowLastWaypoint, TracksDisplayOption para1)
        {
            switch (para1)
            {
                case TracksDisplayOption.Expand:
                    Expand();
                    break;

                case TracksDisplayOption.Collapse:
                    Collapse();
                    break;

                default:
                    throw new EnumNotSupportedException("Incorrect enum for NatsDisplayOption.");
            }
            return ToString(ShowFirstWaypoint, ShowLastWaypoint);
        }

        /// <summary>
        /// A string represents the usual route text with options.
        /// </summary>
        public string ToString(bool ShowFirstWaypoint, bool ShowLastWaypoint)
        {
            if (links.Count < 2)
            {
                throw new InvalidOperationException("Number of waypoints in the route cannot be less than 2.");
            }

            var result = new StringBuilder();
            var node = links.First;
            var last = links.Last;

            if (ShowFirstWaypoint)
            {
                result.Append(node.Value.Waypoint.ID + ' ');
            }

            while (node != last)
            {
                result.Append(node.Value.AirwayToNext + ' ');
                node = node.Next;
                result.Append(node.Value.Waypoint.ID + ' ');
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
