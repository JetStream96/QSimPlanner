using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Routes
{
    public static class RouteExpandUtility
    {
        private static List<LinkedListNode<RouteNode>> findAllNodes(
            LinkedList<RouteNode> links,
            LinkedList<RouteNode> Route,
            string OriginalAirway)
        {
            var result = new List<LinkedListNode<RouteNode>>();
            var node = links.First;

            while (node != null)
            {
                var match = findNode(Route, OriginalAirway, node);

                if (node == null)
                {
                    break;
                }
                result.Add(node);
                node = node.Next;
            }
            return result;
        }

        private static LinkedListNode<RouteNode> findNode(
            LinkedList<RouteNode> Route,
            string OriginalAirway,
            LinkedListNode<RouteNode> nodeStart)
        {
            var node = nodeStart;

            while (node != null)
            {
                if (node.Value.Waypoint.Equals(Route.First.Value.Waypoint) &&
                    node.Value.AirwayToNext == OriginalAirway &&
                    node.Next.Value.Waypoint.Equals(Route.Last.Value.Waypoint))
                {
                    return node;
                }
            }
            return null;
        }

        // For example, we have a route: (P 1 Q 2 R 3 S), where P, Q, R,
        // S are waypoints, and 1, 2, 3 are airways.
        // If the given Route is (Q 4 X 5 R) while the OriginalAirway 
        // is 2, then the route will be modified to
        // (P 1 Q 4 X 5 R 3 S). i.e. All occurrences of (Q 2 R) 
        // are replaced by the given Route.       
        //  
        // Note: R must be the next node of Q.
        //
        public static void InsertRoute(
            LinkedList<RouteNode> links,
            LinkedList<RouteNode> Route,
            string OriginalAirway)
        {
            foreach (var i in findAllNodes(links, Route, OriginalAirway))
            {
                InsertRouteGivenNode(links, Route, i);
            }
        }

        private static void InsertRouteGivenNode(
            LinkedList<RouteNode> links,
            LinkedList<RouteNode> Route,
            LinkedListNode<RouteNode> node)
        {
            if (node == null)
            {
                throw new InvalidOperationException(
                    "No matching node in route to expand.");
            }

            links.First.Value = Route.First.Value;

            var nodeToAdd = Route.First.Next;

            while (nodeToAdd != Route.Last)
            {
                links.AddAfter(node, nodeToAdd.Value);
                nodeToAdd = nodeToAdd.Next;
                node = node.Next;
            }
        }
    }
}
