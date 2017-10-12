using CommonLibrary.LibraryExtension;
using NUnit.Framework;
using QSP.NavData.AAX;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System.Linq;

namespace UnitTest.NavData.AAX
{
    [TestFixture]
    public class FixesLoaderTest
    {
        [Test]
        public void ReadTest()
        {
            var txt = @"
XY000,50.0,10.0,AA
XY001,50.0,10.0,
XY002,50.0,10.0,BB";

            var wptList = new WaypointList();
            var codes = new FixesLoader(wptList).Read(txt.Lines());

            Assert.IsTrue(wptList.AllWaypoints.SequenceEqual(
                new Waypoint("XY000", 50.0, 10.0, 1),
                new Waypoint("XY001", 50.0, 10.0, Waypoint.DefaultCountryCode),
                new Waypoint("XY002", 50.0, 10.0, 2)));
        }

        [Test]
        public void ReadShouldContinueIfALineIsInvalid()
        {
            var txt = @"
XY000,50.0,10.0,AA
XY001,50.0,abc,
XY002,50.0,10.0,BB";

            var wptList = new WaypointList();
            var codes = new FixesLoader(wptList).Read(txt.Lines());

            Assert.AreEqual(2, codes.FirstToSecond.Keys.Count());
        }
    }
}