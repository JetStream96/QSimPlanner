using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class Ifly747v2ProviderTest
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

            var text = Ifly747v2Provider.GetExportText(route);

            var expected =
@"[RTE]
ORIGIN_AIRPORT=RJBB
DEST_AIRPORT=RJAA

[RTE.0]
RouteName=
Name=WPT0
Latitude=0.000000
Longitude=1.000000
CrossThisPoint=0
Heading=0
Speed=0
Altitude=0
Frequency=
FrequencyID=

[RTE.1]
RouteName=B
Name=WPT1
Latitude=0.000000
Longitude=2.000000
CrossThisPoint=0
Heading=0
Speed=0
Altitude=0
Frequency=
FrequencyID=

[RTE.2]
RouteName=
Name=WPT2
Latitude=0.000000
Longitude=2.500000
CrossThisPoint=0
Heading=0
Speed=0
Altitude=0
Frequency=
FrequencyID=

[CDU]
CRZ_ALT=
COST_INDEX=
";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextCoordinateFormatIsCorrect()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "B", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = Ifly747v2Provider.GetExportText(route);

            var expected =
                @"[RTE]
ORIGIN_AIRPORT=RJBB
DEST_AIRPORT=RJAA

[RTE.0]
RouteName=
Name=1012N2000W
Latitude=10.200000
Longitude=-20.000000
CrossThisPoint=0
Heading=0
Speed=0
Altitude=0
Frequency=
FrequencyID=

[CDU]
CRZ_ALT=
COST_INDEX=
";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextNoWaypontDoesNotThrow()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = Ifly747v2Provider.GetExportText(route);
        }
    }
}
