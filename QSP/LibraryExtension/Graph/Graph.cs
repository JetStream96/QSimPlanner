using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension.Graph
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

        private FixedIndexList<Node> _nodes;
        private FixedIndexList<Edge<TEdge>> _edges;

        public Graph()
        {
            _nodes = new FixedIndexList<Node>();
            _edges = new FixedIndexList<Edge<TEdge>>();
        }

        public int AddNode(TNode node)
        {
            return _nodes.Add(new Node(node));
        }

        /// <summary>
        /// Add an edge from one node to another.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public int AddEdge(int nodeFromIndex, int nodeToIndex, TEdge edge)
        {
            var edgeToAdd = new Edge<TEdge>();
            int edgeIndex = _edges.Add(edgeToAdd);

            // Add edge index to nodes
            int fromListIndex = _nodes[nodeFromIndex].next.Add(edgeIndex);
            int toListIndex = _nodes[nodeToIndex].prev.Add(edgeIndex);

            // Set correct index.
            edgeToAdd.FromIndexInList = fromListIndex;
            edgeToAdd.FromNodeIndex = nodeFromIndex;
            edgeToAdd.ToIndexInList = toListIndex;
            edgeToAdd.ToNodeIndex = nodeToIndex;
            edgeToAdd.value = edge;

            return edgeIndex;
        }

        /// <summary>
        /// Remove the node with the given index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
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

        /// <summary>
        /// Remove the edge with given index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void RemoveEdge(int edgeIndex)
        {
            var edge = _edges[edgeIndex];
            _nodes[edge.ToNodeIndex].prev.RemoveAt(edge.ToIndexInList);
            _nodes[edge.FromNodeIndex].next.RemoveAt(edge.FromIndexInList);
            _edges.RemoveAt(edgeIndex);
        }

        /// <exception cref="IndexOutOfRangeException"></exception>
        public TNode GetNode(int index)
        {
            return _nodes[index].value;
        }

        /// <exception cref="IndexOutOfRangeException"></exception>
        public IEdge<TEdge> GetEdge(int edgeIndex)
        {
            return _edges[edgeIndex];
        }

        /// <summary>
        /// The indices of edges which leaves from the given node. 
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IEnumerable<int> EdgesFrom(int index)
        {
            return _nodes[index].next;
        }

        /// <summary>
        /// The indices of edges which points to the given node. 
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IEnumerable<int> EdgesTo(int index)
        {
            return _nodes[index].prev;
        }

        public int EdgesFromCount(int index)
        {
            return _nodes[index].next.Count();
        }

        public int EdgesToCount(int index)
        {
            return _nodes[index].prev.Count();
        }

        public int NodeCount
        {
            get
            {
                return _nodes.Count;
            }
        }

        public void Clear()
        {
            _nodes.Clear();
            _edges.Clear();
        }

        public int MaxSizeNode
        {
            get
            {
                return _nodes.MaxSize;
            }
        }

    }
}
