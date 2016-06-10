using System.Collections.Generic;

namespace QSP.RouteFinding.Routes
{
    public static class RouteCollapseUtility
    {
        private static LinkedListNode<RouteNode> nodesMatch(
            LinkedList<RouteNode> route, LinkedListNode<RouteNode> nodeStart)
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
                    if (node.Value.Waypoint.Equals(
                        nodeCompare.Value.Waypoint))
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        private static List<NodePair> findNode(
            LinkedList<RouteNode> links, LinkedList<RouteNode> route)
        {
            var result = new List<NodePair>();

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
                    result.Add(new NodePair(node, end));
                    node = end.Next;
                }
            }

            return result;
        }

        public static void Collapse(
            LinkedList<RouteNode> links,
            LinkedList<RouteNode> route,
            string airway)
        {
            foreach (var i in findNode(links, route))
            {
                var node = i.Start;
                var end = i.End;

                node.Value.AirwayToNext = airway;

                while (node.Next != end)
                {
                    links.Remove(node.Next);
                }
            }
        }

        private struct NodePair
        {
            public LinkedListNode<RouteNode> Start { get; private set; }
            public LinkedListNode<RouteNode> End { get; private set; }

            public NodePair(
                LinkedListNode<RouteNode> Start,
                LinkedListNode<RouteNode> End)
            {
                this.Start = Start;
                this.End = End;
            }
        }
    }
}
