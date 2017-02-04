using System.Linq;
using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.NavData.AAX;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;

namespace UnitTest.NavData.AAX
{
    [TestFixture]
    public class AtsFileLoaderTest
    {
        [Test]
        public void ReadTest()
        {
            var txt = @"
A,A1,3
S,P1,0,0,P2,0,1,0,90,60.11
S,P2,0,1,P3,0,2,270,90,60.11
";

            var w1 = new Waypoint("P1", 0.0, 0.0);
            var w2 = new Waypoint("P2", 0.0, 1.0);
            var w3 = new Waypoint("P3", 0.0, 2.0);

            var wptList = new WaypointList();
            wptList.AddWaypoint(w1);
            var err = AtsFileLoader.Read(wptList, txt.Lines());

            Assert.IsEmpty(err);
            Assert.IsTrue(wptList.AllWaypoints.SequenceEqual(w1, w2, w3));

            var n = new Neighbor("A1", 60.11);

            int i1 = wptList.FindByWaypoint(w1);
            int i2 = wptList.FindByWaypoint(w2);

            Assert.AreEqual(2, wptList.EdgeCount);
            Assert.IsTrue(wptList.GetEdge(wptList.EdgesFrom(i1).First()).Value.Equals(n));
            Assert.IsTrue(wptList.GetEdge(wptList.EdgesFrom(i2).First()).Value.Equals(n));
        }
    }
}