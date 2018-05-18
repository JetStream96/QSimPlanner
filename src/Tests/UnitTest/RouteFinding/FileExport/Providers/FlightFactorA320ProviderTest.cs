using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class FlightFactorA320ProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT0", 0.0, 10.0), "B", -1.0,
                new Waypoint("WPT1", 0.0, -20.0), "DCT", -1.0,
                new Waypoint("WPT2", 0.0, 2.5), "C", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = FlightFactorA320Provider.GetExportText(route);

            var expected = "RTE RJBBRJAA01 RJBB DCT WPT0 B WPT1 DCT WPT2 DCT RJAA";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextCoordinateFormatIsCorrect()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = FlightFactorA320Provider.GetExportText(route);

            var expected = "RTE RJBBRJAA01 RJBB DCT 1012N2000W DCT RJAA";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextNoWaypont()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = FlightFactorA320Provider.GetExportText(route);

            var expected = "RTE RJBBRJAA01 RJBB RJAA";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }
    }
}
