using QSP.LibraryExtension;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Containers;
using QSP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QSP.RouteFinding.Routes
{
    public class Route : IEnumerable<RouteNode>, IEnumerable
    {
        protected LinkedList<RouteNode> route;

        public double TotalDistance
        {
            get
            {
                if (route.Count == 0)
                {
                    throw new InvalidOperationException("Route is empty.");
                }

                return route.TotalDistance();
            }
        }

        /// <exception cref="NullReferenceException"></exception>
        public Waypoint FirstWaypoint
        {
            get
            {
                return First.Value.Waypoint;
            }
        }

        /// <exception cref="NullReferenceException"></exception>
        public Waypoint LastWaypoint
        {
            get
            {
                return Last.Value.Waypoint;
            }
        }
        
        public LinkedListNode<RouteNode> First
        {
            get
            {
                return route.First;
            }
        }
        
        public LinkedListNode<RouteNode> Last
        {
            get
            {
                return route.Last;
            }
        }

        public int Count
        {
            get
            {
                return route.Count;
            }
        }

        public Route()
        {
            route = new LinkedList<RouteNode>();
        }

        public Route(Route item)
        {
            route = new LinkedList<RouteNode>(item);
        }

        /// <summary>
        /// Add the specified waypoint as the first in the route. 
        /// This waypoint is connected to the next one by the 
        /// airway specified, with the given distance.       
        /// </summary>
        public void AddFirstWaypoint(
            Waypoint item, string viaAirway, double distanceToNext)
        {
            var node = new RouteNode(item, viaAirway, distanceToNext);
            route.AddFirst(node);
        }

        public void AddFirstWaypoint(Waypoint item, string viaAirway)
        {
            var first = route.First;

            double distance =
                first == null ?
                0.0 :
                item.DistanceFrom(first.Value.Waypoint);

            AddFirstWaypoint(item, viaAirway, distance);
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// This waypoint is connected from the previous one by the 
        /// airway specified, with the given distance.       
        /// </summary>
        /// <param name="viaAirway">Airway or SID/STAR name.</param>
        public void AddLastWaypoint(
            Waypoint item, string viaAirway, double distanceFromPrev)
        {
            var last = route.Last;

            if (last != null)  // Route is non-empty.
            {
                last.Value.DistanceToNext = distanceFromPrev;
                last.Value.AirwayToNext = viaAirway;
            }

            route.AddLast(new RouteNode(item));
        }

        public void AddLastWaypoint(Waypoint item, string viaAirway)
        {
            var last = route.Last;

            double distance =
               Last == null ?
               0.0 :
               item.DistanceFrom(last.Value.Waypoint);

            AddLastWaypoint(item, viaAirway, distance);
        }

        /// <summary>
        /// Append the specified waypoint to the end of the route. 
        /// </summary>
        public void AddLastWaypoint(Waypoint item)
        {
            route.AddLast(new RouteNode(item));
        }

        // TODO: Does it work when Last is null?
        /// <summary>
        /// Append the given route at the end of the current one.
        /// </summary>
        public void AppendRoute(Route item, string airway)
        {
            var lastWpt = LastWaypoint;
            var firstWpt = item.FirstWaypoint;

            AppendRoute(item, airway, lastWpt.DistanceFrom(firstWpt));
        }

        /// <summary>
        /// Append the given route at the end of the current one.
        /// </summary>
        public void AppendRoute(Route item, string airway, double distance)
        {
            route.Last.Value.AirwayToNext = airway;
            route.Last.Value.DistanceToNext = distance;
            route.AddLast(item.route);
        }

        /// <summary>
        /// Connect the given route at the end of the current one.
        /// The last waypoint of current route must be the same 
        /// as the first one in item.
        /// </summary>
        public void ConnectRoute(Route item)
        {
            if (item.Count == 0)
            {
                return;
            }

            ConditionChecker.Ensure<ArgumentException>(
                LastWaypoint.Equals(item.FirstWaypoint));
            route.RemoveLast();
            route.AddLast(item.route);
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
            if (route.Count < 2)
            {
                throw new InvalidOperationException(
                    "Number of waypoints in the route is less than 2.");
            }

            var result = new StringBuilder();
            var node = route.First;
            var last = route.Last;

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
            return route.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
