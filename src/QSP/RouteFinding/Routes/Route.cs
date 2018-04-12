using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.RouteFinding.Routes
{
    public class Route : IReadOnlyCollection<RouteNode>
    {
        public LinkedList<RouteNode> Nodes { get; }

        /// <exception cref="NullReferenceException"></exception>
        public Waypoint FirstWaypoint => First.Value.Waypoint;

        /// <exception cref="NullReferenceException"></exception>
        public Waypoint LastWaypoint => Last.Value.Waypoint;

        public LinkedListNode<RouteNode> First => Nodes.First;
        public LinkedListNode<RouteNode> Last => Nodes.Last;
        public int Count => Nodes.Count;

        public Route()
        {
            Nodes = new LinkedList<RouteNode>();
        }

        public Route(Route item)
        {
            Nodes = new LinkedList<RouteNode>(item);
        }

        public Route(params RouteNode[] item)
        {
            Nodes = new LinkedList<RouteNode>(item);
        }

        public Route(IEnumerable<RouteNode> item)
        {
            Nodes = new LinkedList<RouteNode>(item);
        }

        public double TotalDistance()
        {
            if (Nodes.Count == 0)
            {
                throw new InvalidOperationException("Route is empty.");
            }

            double dis = 0.0;
            var node = First;

            while (node != Last)
            {
                dis += node.Value.AirwayToNext.Distance;
                node = node.Next;
            }

            return dis;
        }

        /// <summary>
        /// Add the specified waypoint as the first in the route. 
        /// This waypoint is connected to the next one by the 
        /// airway specified, with the given distance.       
        /// </summary>
        public void AddFirstWaypoint(Waypoint item, string viaAirway, double distanceToNext)
        {
            var node = new RouteNode(item, new Neighbor(viaAirway, distanceToNext));
            Nodes.AddFirst(node);
        }

        public void AddFirstWaypoint(Waypoint item, string viaAirway)
        {
            var first = Nodes.First;
            double distance = first == null ? 0.0 : item.Distance(first.Value.Waypoint);
            AddFirstWaypoint(item, viaAirway, distance);
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the 
        /// airway specified, with the given distance.       
        /// </summary>
        /// <param name="viaAirway">Airway or SID/STAR name.</param>
        public void AddLastWaypoint(Waypoint item, string viaAirway, double distanceFromPrev)
        {
            var last = Nodes.Last;

            if (last != null)  // Route is non-empty.
            {
                var n = new Neighbor(viaAirway, distanceFromPrev);
                last.Value = new RouteNode(last.Value.Waypoint, n);
            }

            Nodes.AddLast(new RouteNode(item, null));
        }

        public void AddLastWaypoint(Waypoint item, string viaAirway)
        {
            var last = Nodes.Last;
            double distance = Last == null ? 0.0 : item.Distance(last.Value.Waypoint);
            AddLastWaypoint(item, viaAirway, distance);
        }

        /// <summary>
        /// Add the waypoint to the end of the route.
        /// </summary>
        public void AddLastWaypoint(Waypoint item)
        {
            Nodes.AddLast(new RouteNode(item, null));
        }

        /// <summary>
        /// Connect the given route at the end of the current one.
        /// The last waypoint of current route must be the same 
        /// as the first one in item.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Connect(Route item)
        {
            if (item.Count == 0) return;

            if (Last != null)
            {
                // This route is non-empty.
                Ensure<ArgumentException>(LastWaypoint.Equals(item.FirstWaypoint));
                Nodes.RemoveLast();
            }

            Nodes.AddLast(item.Nodes);
        }

        /// <summary>
        /// A string represents the usual route text.
        /// </summary>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// A string represents the usual route text with options.
        /// If the route contains only a "DCT", even if ShowDct is false, this will return "DCT".
        /// </summary>
        public string ToString(bool ShowDct)
        {
            if (Nodes.Count < 2)
            {
                throw new InvalidOperationException(
                    "Number of waypoints in the route is less than 2.");
            }

            var result = new StringBuilder();
            var node = Nodes.First;
            var last = Nodes.Last;

            while (node.Next != last)
            {
                if (node.Value.AirwayToNext.Airway == "DCT")
                {
                    if (ShowDct)
                    {
                        result.Append(node.Value.AirwayToNext.Airway + ' ');
                    }

                    node = node.Next;
                    result.Append(node.Value.Waypoint.ID + ' ');
                }
                else if (node.Value.AirwayToNext.Airway != node.Next.Value.AirwayToNext.Airway)
                {
                    result.Append(node.Value.AirwayToNext.Airway + ' ');
                    node = node.Next;
                    result.Append(node.Value.Waypoint.ID + ' ');
                }
                else
                {
                    node = node.Next;
                }
            }

            if (node.Value.AirwayToNext.Airway != "DCT" || ShowDct)
            {
                result.Append(node.Value.AirwayToNext.Airway);
            }

            return result.Length == 0 ? "DCT" : result.ToString();
        }

        public IEnumerator<RouteNode> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(Route other)
        {
            return this.Reverse().Skip(1).SequenceEqual(other.Reverse().Skip(1)) &&
                LastWaypoint.Equals(other.LastWaypoint);
        }
    }
}
