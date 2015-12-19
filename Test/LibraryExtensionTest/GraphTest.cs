using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Linq;

namespace Test.LibraryExtensionTest
{
    [TestClass]
    public class GraphTest
    {
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
    }
}
