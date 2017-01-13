using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.LibraryExtension.Graph;
using System;
using System.Linq;

namespace UnitTest.LibraryExtension.Graph
{
    [TestFixture]
    public class GraphTest
    {
        [Test]
        public void AddThenGetNode()
        {
            var graph = new Graph<string, string>();
            var nodes = Enumerable.Range(0, 100).Select(x => x.ToString());
            var nodeIndex = nodes.Select(n => new { Node = n, Index = graph.AddNode(n) });
            Assert.IsTrue(nodeIndex.All(item => item.Node == graph.GetNode(item.Index)));
        }

        private static Graph<string, string> GetGraph()
        {
            var graph = new Graph<string, string>();
            var nodes = Enumerable.Range(0, 50).Select(i => i.ToString()).ToList();

            foreach (var node in nodes)
            {
                graph.AddNode(node);
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

        // This method tests EdgesFrom(int), and GetEdge(int).
        [Test]
        public void AddEdgeAndRead()
        {
            var graph = GetGraph();

            for (int i = 0; i < 50; i++)
            {
                var nodesTo = graph.EdgesFrom(i).Select(edge =>
                    graph.GetNode(graph.GetEdge(edge).ToNodeIndex));

                var expected = Enumerable.Range(0, 50)
                    .Except(new[] { i })
                    .Select(n => n.ToString());

                Assert.IsTrue(nodesTo.ToHashSet().SetEquals(expected));
            }
        }

        [Test]
        public void AccessNonExistingNodeShouldThrowException()
        {
            var graph = new Graph<string, string>();
            var index = graph.AddNode("0");
            Assert.That(() => graph.GetNode(index + 1), Throws.Exception);
        }

        [Test]
        public void AccessNonExistingEdgeShouldThrowException()
        {
            var graph = new Graph<string, string>();
            var node0 = graph.AddNode("0");
            var node1 = graph.AddNode("1");
            var edge = graph.AddEdge(node0, node1, "X");

            Assert.That(() => graph.GetEdge(edge + 1), Throws.Exception);
        }

        [Test]
        public void RemoveEdgeTest()
        {
            var graph = new Graph<string, string>();
            var node0 = graph.AddNode("0");
            var node1 = graph.AddNode("1");
            var edge = graph.AddEdge(node0, node1, "X");

            graph.RemoveEdge(edge);

            Assert.AreEqual(0, graph.EdgesFrom(node0).Count());
        }

        [Test]
        public void RemoveNodeCheckNodeRemoved()
        {
            var graph = new Graph<string, string>();
            var index = graph.AddNode("42");
            graph.RemoveNode(index);

            // Check the node is removed.
            Assert.That(() => graph.GetNode(index), Throws.Exception);
        }

        [Test]
        public void RemoveNodeCheckEdgesRemoved()
        {
            var graph = GetGraph();
            const int n = 39;

            graph.RemoveNode(n);

            for (int i = 0; i < 50; i++)
            {
                if (i == n) continue;
                Assert.IsTrue(graph.EdgesFrom(i).All(edge => graph.GetEdge(edge).ToNodeIndex != n));
            }
        }

    }
}
