using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class PmdgProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var abcd = Common.GetAirport("ABCD");
            var efgh = Common.GetAirport("EFGH");
            var manager = Common.GetAirportManager(abcd, efgh);

            var route = Common.GetRoute(
                new Waypoint("ABCD02", 0.0, 0.0), "SID", -1.0,
                new Waypoint("WPT1", 0.0, 1.0), "AWY", -1.0,
                new Waypoint("WPT2", 0.0, 2.0), "STAR", -1.0,
                new Waypoint("EFGH18", 0.0, 3.0));

            var text = PmdgProvider.GetExportText(route, manager);
            var expected =
@"Flight plan is built by QSimPlanner.

4

ABCD
1
DIRECT
1 S 0 W 0 0
-----
1
0

1
0
-
-1000000
-1000000

WPT1
5
AWY
1 S 0 E 1 0
0
0
0

WPT2
5
DIRECT
1 S 0 E 2 0
0
0
0

EFGH
1
-
1 S 0 W 0 0
-----
0
0

1
0
-
-1000000
-1000000

";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextCoordinateFormatIsCorrect()
        {
            var abcd = Common.GetAirport("ABCD");
            var efgh = Common.GetAirport("EFGH");
            var manager = Common.GetAirportManager(abcd, efgh);

            var route = Common.GetRoute(
                new Waypoint("ABCD02", 0.0, 0.0), "SID", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "STAR", -1.0,
                new Waypoint("EFGH18", 0.0, 3.0));

            var text = PmdgProvider.GetExportText(route, manager);
            var expected =
                @"Flight plan is built by QSimPlanner.

4

ABCD
1
DIRECT
1 S 0 W 0 0
-----
1
0

1
0
-
-1000000
-1000000

1012N2000W
5
DIRECT
1 S 0 E 2 0
0
0
0

EFGH
1
-
1 S 0 W 0 0
-----
0
0

1
0
-
-1000000
-1000000

";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }
    }
}
