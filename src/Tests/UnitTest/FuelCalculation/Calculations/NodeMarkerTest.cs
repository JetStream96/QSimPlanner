using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using static QSP.FuelCalculation.Calculations.NodeMarker;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class NodeMarkerTest
    {
        [Test]
        public void TocIndexTest1()
        {
            var nodes = TransformNode(
                new[] { 50.0, 10000.0, 10000.0001, 10000.0, 3000.0 });

            Assert.AreEqual(1, TocIndex(nodes));
        }

        [Test]
        public void TocIndexTest2()
        {
            var nodes = TransformNode(new[] { 50.0, 10000.0, 3000.0 });
            Assert.AreEqual(1, TocIndex(nodes));
        }

        [Test]
        public void TodIndexTest1()
        {
            var nodes = TransformNode(
                new[] { 50.0, 10000.0, 10000.0001, 10000.0, 3000.0 });

            Assert.AreEqual(3, TodIndex(nodes));
        }

        [Test]
        public void TodIndexTest2()
        {
            var nodes = TransformNode(new[] { 50.0, 10000.0, 3000.0 });
            Assert.AreEqual(1, TodIndex(nodes));
        }

        [Test]
        public void ScIndiciesTest()
        {
            var alts = new[] {50.0, 10000.0, 10000.0001, 10000.0, 10800.0,
                12000.0, 12000.0001, 14000.0, 3000.0 };

            var nodes = TransformNode(alts);

            Assert.IsTrue(Enumerable.SequenceEqual(
               new[] { 3, 6 }, ScIndices(nodes)));
        }

        private static IPlanNode[] TransformNode(double[] alts)
        {
            var result = new IPlanNode[alts.Length];

            for (int i = 0; i < alts.Length; i++)
            {
                result[i] = new PlanNodeStub(alts[i], i, i);
            }

            return result;
        }

        private class PlanNodeStub : IPlanNode
        {
            public object NodeValue { get; }
            public IWindTableCollection WindTable { get; }
            public LinkedListNode<RouteNode> NextRouteNode { get; }
            public ICoordinate NextPlanNodeCoordinate { get; }
            public double Alt { get; }
            public double GrossWt { get; }
            public double FuelOnBoard { get; }
            public double TimeRemaining { get; }
            public double Kias { get; }
            public double Ktas { get; }
            public double Gs { get; }
            public Waypoint PrevWaypoint { get; }
            public LinkedListNode<RouteNode> PrevRouteNode { get; }
            public double Lat { get; }
            public double Lon { get; }

            public PlanNodeStub(double Alt, double Lat, double Lon)
            {
                this.Alt = Alt;
                this.Lat = Lat;
                this.Lon = Lon;
            }
        }
    }
}