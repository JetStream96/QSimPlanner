using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Routes
{
    public static class RouteCollapse
    {
        private static LinkedListNode<RouteNode> nodesMatch(LinkedList<RouteNode> route, LinkedListNode<RouteNode> nodeStart)
        {
            var node = nodeStart;
            var nodeCompare = route.First;

            while (node != null && nodeCompare != null)
            {
                if (nodeCompare != route.Last)
                {
                    if (node.Value.Equals(nodeCompare.Value))
                    {
                        node = node.Next;
                        nodeCompare = nodeCompare.Next;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (node.Value.Waypoint.Equals(nodeCompare.Value.Waypoint))
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        private static List<Tuple<LinkedListNode<RouteNode>, LinkedListNode<RouteNode>>>
                    findNode(LinkedList<RouteNode> links, LinkedList<RouteNode> route)
        {
            var result = new List<Tuple<LinkedListNode<RouteNode>, LinkedListNode<RouteNode>>>();

            var node = links.First;

            while (node != null)
            {
                var end = nodesMatch(route, node);

                if (end == null)
                {
                    node = node.Next;
                }
                else
                {
                    result.Add(new Tuple<LinkedListNode<RouteNode>, LinkedListNode<RouteNode>>(node, end));
                    node = end.Next;
                }
            }
            return result;
        }

        public static void Collapse(LinkedList<RouteNode> links, LinkedList<RouteNode> route, string airway)
        {
            foreach (var i in findNode(links, route))
            {
                var node = i.Item1;
                var end = i.Item2;

                node.Value.AirwayToNext = airway;

                while (node.Next != end)
                {
                    links.Remove(node.Next);
                }
            }
        }
    }
}
