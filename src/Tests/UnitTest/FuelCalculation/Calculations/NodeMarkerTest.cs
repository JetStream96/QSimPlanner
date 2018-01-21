using FakeItEasy;
using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.FuelCalculation.Calculations;
using QSP.FuelCalculation.Results.Nodes;
using System.Collections.Generic;
using System.Linq;
using static CommonLibrary.LibraryExtension.IEnumerables;
using static QSP.FuelCalculation.Calculations.NodeMarker;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class NodeMarkerTest
    {
        [Test]
        public void TocIndexTest0()
        {
            var nodes = TransformNode(50.0, 1000.0, 3000.0);
            Assert.AreEqual(2, TocIndex(nodes));
        }

        [Test]
        public void TocIndexTest1()
        {
            var nodes = TransformNode(50.0, 10000.0, 10000.0001, 10000.0, 3000.0);
            Assert.AreEqual(1, TocIndex(nodes));
        }

        [Test]
        public void TocIndexTest2()
        {
            var nodes = TransformNode(50.0, 10000.0, 3000.0);
            Assert.AreEqual(1, TocIndex(nodes));
        }

        [Test]
        public void TodIndexTest0()
        {
            var nodes = TransformNode(3500.0, 3250.0, 3000.0);
            Assert.AreEqual(0, TodIndex(nodes));
        }

        [Test]
        public void TodIndexTest1()
        {
            var nodes = TransformNode(50.0, 10000.0, 10000.0001, 10000.0, 3000.0);
            Assert.AreEqual(3, TodIndex(nodes));
        }

        [Test]
        public void TodIndexTest2()
        {
            var nodes = TransformNode(50.0, 10000.0, 3000.0);
            Assert.AreEqual(1, TodIndex(nodes));
        }

        [Test]
        public void ScIndiciesTest()
        {
            var nodes = TransformNode(50.0, 10000.0, 10000.0001, 10000.0, 10800.0,
                12000.0, 12000.0001, 14000.0, 3000.0);

            Assert.IsTrue(ScIndices(nodes).SequenceEqual(3, 6));
        }

        private static IReadOnlyList<IPlanNode> TransformNode(params double[] alts)
        {
            return alts.Select((alt, index) => GetNode(alt, index, index)).ToList();
        }

        private static IPlanNode GetNode(double alt, double lat, double lon)
        {
            var val = new IntermediateNode(new LatLon(lat, lon));
            var node = A.Fake<IPlanNode>();
            A.CallTo(() => node.NodeValue).Returns(val);
            A.CallTo(() => node.Alt).Returns(alt);
            return node;
        }
    }
}