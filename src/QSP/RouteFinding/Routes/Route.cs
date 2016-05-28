using QSP.RouteFinding.Containers;
using QSP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static QSP.MathTools.GCDis;

namespace QSP.RouteFinding.Routes
{
    public class Route : IEnumerable<RouteNode>, IEnumerable
    {
        protected LinkedList<RouteNode> links;

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

                while (node != links.Last)
                {
                    totalDis += node.Value.DistanceToNext;
                    node = node.Next;
                }

                return totalDis;
            }
        }

        public RouteNode First
        {
            get
            {
                return links.First?.Value;
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
                return links.Last?.Value;
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
        }

        public Route(Route item)
        {
            links = new LinkedList<RouteNode>(item);
        }

        /// <summary>
        /// Add the specified waypoint as the first in the route. 
        /// This waypoint is connected to the next one by the airway specified, with the given distance.       
        /// </summary>
        public void AddFirstWaypoint(Waypoint item, string AirwayToNext, double distanceToNext)
        {
            links.AddFirst(new RouteNode(item, AirwayToNext, distanceToNext));
        }

        public void AddFirstWaypoint(Waypoint item, string viaAirway, bool AutoComputeDistance)
        {
            if (AutoComputeDistance && links.First != null)
            {
                var firstWpt = links.First.Value.Waypoint;
                AddFirstWaypoint(item,
                                 viaAirway,
                                 Distance(item.Lat, item.Lon, firstWpt.Lat, firstWpt.Lon));
            }
            else
            {
                links.AddLast(new RouteNode(item, viaAirway, 0.0));
            }
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the airway specified, with the given distance.       
        /// </summary>
        /// <param name="viaAirway">Airway or SID/STAR name.</param>
        public void AddLastWaypoint(Waypoint item, string viaAirway, double distanceFromPrev)
        {
            var last = links.Last;

            if (last != null)  // Route is non-empty.
            {
                last.Value.DistanceToNext = distanceFromPrev;
                last.Value.AirwayToNext = viaAirway;
            }
            links.AddLast(new RouteNode(item));
        }

        public void AddLastWaypoint(Waypoint item, string viaAirway, bool AutoComputeDistance)
        {
            if (AutoComputeDistance && links.Last != null)
            {
                var lastWpt = links.Last.Value.Waypoint;
                AddLastWaypoint(item,
                                viaAirway,
                                Distance(item.Lat, item.Lon, lastWpt.Lat, lastWpt.Lon));
            }
            else
            {
                if (links.Last != null)
                {
                    links.Last.Value.AirwayToNext = viaAirway;
                }
                links.AddLast(new RouteNode(item));
            }
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// </summary>
        public void AddLastWaypoint(Waypoint item)
        {
            links.AddLast(new RouteNode(item));
        }

        /// <summary>
        /// Append the given route at the end of the current one.
        /// </summary>
        public void AppendRoute(Route item, string airway)
        {
            var lastWpt = Last.Waypoint;
            var firstWpt = item.First.Waypoint;

            AppendRoute(item, airway, Distance(lastWpt.Lat, lastWpt.Lon, firstWpt.Lat, firstWpt.Lon));
        }

        /// <summary>
        /// Append the given route at the end of the current one.
        /// </summary>
        public void AppendRoute(Route item, string airway, double distance)
        {
            if (item.Count == 0)
            {
                return;
            }

            links.Last.Value.AirwayToNext = airway;
            links.Last.Value.DistanceToNext = distance;

            for (var node = item.FirstNode;
                 node != null;
                 node = node.Next)
            {
                links.AddLast(node.Value);
            }
        }

        /// <summary>
        /// Connect the given route at the end of the current one.
        /// The last waypoint of current route must be the same as the first one in item.
        /// </summary>
        public void ConnectRoute(Route item)
        {
            if (item.Count == 0)
            {
                return;
            }

            ConditionChecker.Ensure<ArgumentException>(Last.Waypoint.Equals(item.First.Waypoint));
            links.RemoveLast();

            for (var node = item.FirstNode;
                 node != null;
                 node = node.Next)
            {
                links.AddLast(node.Value);
            }
        }

        /// <summary>
        /// A string represents the usual route text.
        /// </summary>
        public override string ToString()
        {
            return ToString(false, false);
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

            while (node.Next != last)
            {
                if (node.Value.AirwayToNext != node.Next.Value.AirwayToNext)
                {
                    result.Append(node.Value.AirwayToNext + ' ');
                    node = node.Next;
                    result.Append(node.Value.Waypoint.ID + ' ');
                }
                else
                {
                    node = node.Next;
                }
            }
            result.Append(node.Value.AirwayToNext + ' ');

            if (ShowLastWaypoint)
            {
                result.Append(last.Value.Waypoint.ID);
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
