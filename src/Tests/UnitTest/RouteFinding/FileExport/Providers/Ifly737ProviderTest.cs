using QSP.LibraryExtension;
using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class Ifly737ProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT0", 0.0, 1.0), "B", -1.0,
                new Waypoint("WPT1", 0.0, 2.0), "DCT", -1.0,
                new Waypoint("WPT2", 0.0, 2.5), "C", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = Ifly737Provider.GetExportText(route);

            var expected =
@"RJBB,
RJAA,
06L,
,
,
,
,
,
,
,
-1,
,
,
,
,
0,
DIRECT,3,WPT0,0, 0.000000 1.000000,0,0, 90.00000,0,0,1,-1,0.000,0,-1000,-1000,-1,-1,-1,0,0,000.00000,0,0,,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1000,0,
B,2,WPT1,0, 0.000000 2.000000,0,0, 90.00000,0,0,1,-1,0.000,0,-1000,-1000,-1,-1,-1,0,0,000.00000,0,0,,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1000,0,
DIRECT,3,WPT2,0, 0.000000 2.500000,0,0, 90.00000,0,0,1,-1,0.000,0,-1000,-1000,-1,-1,-1,0,0,000.00000,0,0,,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1000,0,
";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextNoWaypontDoesNotThrow()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = Ifly737Provider.GetExportText(route);
        }
    }
}