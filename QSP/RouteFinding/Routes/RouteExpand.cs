using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Routes
{
    public static class RouteExpand
    {
        private static List<LinkedListNode<RouteNode>> findAllNodes(LinkedList<RouteNode> links, LinkedList<RouteNode> Route,
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

        private static LinkedListNode<RouteNode> findNode(LinkedList<RouteNode> Route, string OriginalAirway,
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

        // For example, we have a route: (P A1 Q A2 R A3 S), where P, Q, R, S are waypoints, and A1, A2, A3 are airways.
        // If the given Route is (Q B1 X B2 Y R) while the OriginalAirway is A2, then the route will be modified to
        // (P A1 Q B1 X B2 Y R A3 S). i.e. All occurrences of (Q A2 R) are replaced by the given Route.       
        //  
        // Note: R must be the next node of R.
        //
        public static void InsertRoute(LinkedList<RouteNode> links, LinkedList<RouteNode> Route, string OriginalAirway)
        {
            foreach (var i in findAllNodes(links, Route, OriginalAirway))
            {
                InsertRouteGivenNode(links, Route, i);
            }
        }

        private static void InsertRouteGivenNode(LinkedList<RouteNode> links, LinkedList<RouteNode> Route, LinkedListNode<RouteNode> node)
        {
            if (node == null)
            {
                throw new InvalidOperationException("No matching node in route to expand.");
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
