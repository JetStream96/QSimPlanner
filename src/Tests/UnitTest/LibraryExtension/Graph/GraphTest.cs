using System;
using NUnit.Framework;
using QSP.LibraryExtension.Graph;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.LibraryExtension.Graph
{
    [TestFixture]
    public class GraphTest
    {
        // In writing this test, the mechanism of FixedIndexList is sometimes assumed. 
        // i.e. When adding N elements to a new FixedIndexList, their indices should 
        // be 0, 1, ... ,N-1.

        [Test]
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

        private Graph<int, string> CreateGraph0()
        {
            var graph = new Graph<int, string>();

            for (int i = 0; i < 100; i++)
            {
                graph.AddNode(-i);
            }
            return graph;
        }

        private Graph<int, string> CreateGraph1()
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

        private List<int> CreateList(int NUM, int index)
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

        [Test]
        // This method tests EdgesFrom(int), and GetEdge(int).
        public void AddEdge_ReadWithForEach()
        {
            var graph = CreateGraph1();

            for (int i = 0; i < 50; i++)
            {
                var x = new List<int>();

                foreach (var k in graph.EdgesFrom(i)) // k is the index of edge
                {
                    x.Add(graph.GetNode(graph.GetEdge(k).ToNodeIndex));
                }

                x.Sort();
                x.Reverse();

                Assert.IsTrue(Enumerable.SequenceEqual(CreateList(50, i), x));
            }
        }

        [Test]
        public void AccessNonExistingNode_Exception()
        {
            var graph = CreateGraph0();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            graph.GetNode(100));
        }

        [Test]
        public void AccessNonExistingEdge_Exception()
        {
            var graph = CreateGraph0();

            Assert.Throws<IndexOutOfRangeException>(() => graph.GetEdge(0));
        }

        [Test]
        public void RemoveEdgeTest()
        {
            var tuple = CreateGraph2();
            var graph = tuple.Item2;
            var edgeIndex = tuple.Item1;

            graph.RemoveEdge(edgeIndex);

            foreach (var i in graph.EdgesFrom(27))
            {
                Assert.AreNotEqual(edgeIndex, i);
            }
        }

        private Tuple<int, Graph<int, string>> CreateGraph2()
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

        [Test]
        public void RemoveNode_CheckNodeRemoved()
        {
            var graph = CreateGraph1();
            int N = 39;

            graph.RemoveNode(N);

            // Check the node is removed.
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            graph.GetNode(N));
        }

        [Test]
        public void RemoveNode_CheckEdgesRemoved()
        {
            var graph = CreateGraph1();
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
