using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.Navaids;
using static QSP.LibraryExtension.Types;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class XplaneProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT0", 0.0, 10.0), "B", -1.0,
                new Waypoint("VOR1", 0.0, -20.0), "DCT", -1.0,
                new Waypoint("NDB2", 0.0, -30.0), "DCT", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var navaids = List(
                new Navaid() { ID = "VOR1", IsVOR = true, Lat = 0, Lon = -20 },
                new Navaid() { ID = "NDB2", IsVOR = false, Lat = 0, Lon = -30 }
            );

            var text = XplaneProvider.GetExportText(route, navaids.ToMultiMap(x => x.ID));

            var expected =
@"I
3 version
1
5
1 RJBB 0 0.000000 0.000000
11 WPT0 0 0.000000 10.000000
3 VOR1 0 0.000000 -20.000000
2 NDB2 0 0.000000 -30.000000
28 +10.200_-020.000 0 10.200000 -20.000000
1 RJAA 0 0.000000 3.000000
0 ---- 0 0.000000 0.000000
0 ---- 0 0.000000 0.000000
0 ---- 0 0.000000 0.000000
0 ---- 0 0.000000 0.000000
";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextNoWaypontDoesNotThrow()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = XplaneProvider.GetExportText(route, new MultiMap<string, Navaid>());
        }
    }
}
