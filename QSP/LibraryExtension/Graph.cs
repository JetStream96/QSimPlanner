using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    public class Graph<TNode, TEdge>
    {
        private class Node
        {
            public TNode value;
            public FixedIndexList<int> prev;
            public FixedIndexList<int> next;

            public Node(TNode value)
            {
                this.value = value;
                prev = new FixedIndexList<int>();
                next = new FixedIndexList<int>();
            }
        }

        private class Edge
        {
            public TEdge value;
            public int FromNodeIndex;
            public int FromIndexInList;
            public int ToNodeIndex;
            public int ToIndexInList;

            public Edge() { }

        }

        private FixedIndexList<Node> _nodes;
        private FixedIndexList<Edge> _edges;

        public Graph()
        {
            _nodes = new FixedIndexList<Node>();
            _edges = new FixedIndexList<Edge>();
        }

        public int AddNode(TNode node)
        {
            return _nodes.Add(new Node(node));
        }

        public void AddEdge(int nodeFromIndex, int nodeToIndex, TEdge edge)
        {
            var edgeToAdd = new Edge();
            int edgeIndex = _edges.Add(edgeToAdd);

            // Add edge index to nodes
            int fromListIndex = _nodes[nodeFromIndex].next.Add(edgeIndex);
            int toListIndex = _nodes[nodeToIndex].prev.Add(edgeIndex);

            // Set correct index.
            edgeToAdd.FromIndexInList = fromListIndex;
            edgeToAdd.FromNodeIndex = nodeFromIndex;
            edgeToAdd.ToIndexInList = toListIndex;
            edgeToAdd.ToIndexInList = nodeToIndex;
        }

        public void RemoveNode(int index)
        {
            var node = _nodes[index];

            foreach (var i in node.prev)
            {
                RemoveEdge(i);
            }

            foreach (var j in node.next)
            {
                RemoveEdge(j);
            }

            _nodes.RemoveAt(index);
        }

        public void RemoveEdge(int edgeIndex)
        {
            var edge = _edges[edgeIndex];
            _nodes[edge.ToNodeIndex].prev.RemoveAt(edge.ToIndexInList);
            _nodes[edge.FromNodeIndex].next.RemoveAt(edge.FromIndexInList);
            _edges.RemoveAt(edgeIndex);
        }

        public TNode GetNode(int index)
        {
            return _nodes[index].value;
        }

        public TEdge GetEdge(int edgeIndex)
        {
            return _edges[edgeIndex].value;
        }

        /// <summary>
        /// The indices of edges which the given node connects with. 
        /// </summary>
        public IEnumerable <int> NextNodes(int index)
        {
            return _nodes[index].next;
        }       

    }
}
