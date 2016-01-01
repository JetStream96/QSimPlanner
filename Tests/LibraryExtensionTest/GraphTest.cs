using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension.Graph;
using System.Collections.Generic;
using System.Linq;

namespace Tests.LibraryExtensionTest
{
    [TestClass]
    public class GraphTest
    {
        // In writing this test, the mechanism of FixedIndexList is sometimes assumed. 
        // i.e. When adding N elements to a new FixedIndexList, their indices should be 0, 1, ... ,N-1.

        [TestMethod]
        public void AddThenGetNode()
        {
            var graph = new Graph<int, string>();
            var indices = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                indices.Add(graph.AddNode(-i));
            }

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(-i, graph.GetNode(indices[i]));
            }
        }

        private Graph<int, string> createGraph0()
        {
            var graph = new Graph<int, string>();

            for (int i = 0; i < 100; i++)
            {
                graph.AddNode(-i);
            }
            return graph;
        }

        private Graph<int, string> createGraph1()
        {
            var graph = new Graph<int, string>();

            for (int i = 0; i < 50; i++)
            {
                graph.AddNode(-i);
            }

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (i != j)
                    {
                        graph.AddEdge(i, j, (i * j).ToString());
                    }
                }
            }
            return graph;
        }

        private List<int> createList(int NUM, int index)
        {
            var x = new List<int>();

            for (int i = 0; i < NUM; i++)
            {
                if (i != index)
                {
                    x.Add(-i);
                }
            }
            return x;
        }

        [TestMethod]
        // This method tests EdgesFrom(int), and GetEdge(int).
        public void AddEdge_ReadWithForEach()
        {
            var graph = createGraph1();

            for (int i = 0; i < 50; i++)
            {
                var x = new List<int>();

                foreach (var k in graph.EdgesFrom(i)) // k is the index of edge
                {
                    x.Add(graph.GetNode(graph.GetEdge(k).ToNodeIndex));
                }

                x.Sort();
                x.Reverse();

                Assert.IsTrue(Enumerable.SequenceEqual(createList(50, i), x));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AccessNonExistingNode_Exception()
        {
            var graph = createGraph0();
            graph.GetNode(100);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AccessNonExistingEdge_Exception()
        {
            var graph = createGraph0();
            graph.GetEdge(0);
        }

        [TestMethod]
        public void RemoveEdgeTest()
        {
            var tuple = createGraph2();
            var graph = tuple.Item2;
            var edgeIndex = tuple.Item1;

            graph.RemoveEdge(edgeIndex);

            foreach (var i in graph.EdgesFrom(27))
            {
                Assert.AreNotEqual(edgeIndex, i);
            }
        }

        private Tuple<int, Graph<int, string>> createGraph2()
        {
            const int N = 27;
            const int M = 36;

            var graph = new Graph<int, string>();

            for (int i = 0; i < 50; i++)
            {
                graph.AddNode(-i);
            }

            for (int i = 0; i < 50; i++)
            {
                if (i != N && i != M)
                {
                    graph.AddEdge(N, i, (N * i).ToString());
                }
            }

            return new Tuple<int, Graph<int, string>>(graph.AddEdge(N, M, (N * M).ToString()), graph);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveNode_CheckNodeRemoved()
        {
            var graph = createGraph1();
            int N = 39;

            graph.RemoveNode(N);

            // Check the node is removed.
            graph.GetNode(N);
        }

        [TestMethod]
        public void RemoveNode_CheckEdgesRemoved()
        {
            var graph = createGraph1();
            int N = 39;

            graph.RemoveNode(N);

            for (int i = 0; i < 50; i++)
            {
                if (i != N)
                {
                    foreach (var j in graph.EdgesFrom(i))
                    {
                        Assert.AreNotEqual(N, graph.GetEdge(j).ToNodeIndex);
                    }
                }
            }
        }

    }
}
