using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class AerosoftAirbusProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT0", 0.0, 10.0), "B", -1.0,
                new Waypoint("WPT1", 0.0, -20.0), "DCT", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "DCT", -1.0,
                new Waypoint("WPT2", 0.0, 2.5), "C", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = AerosoftAirbusProvider.GetExportText(route);

            var expected =
@"[CoRte]
ArptDep=RJBB
ArptArr=RJAA
RwyDep=RJBB06L
RwyArr=RJAA18
DctWpt1=WPT0
DctWpt1Coordinates=0.000000,10.000000
Airway2=B
Airway2FROM=WPT0
Airway2TO=WPT1
DctWpt3=1012N2000W
DctWpt3Coordinates=10.200000,-20.000000
DctWpt4=WPT2
DctWpt4Coordinates=0.000000,2.500000
";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextNoWaypontDoesNotThrow()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = AerosoftAirbusProvider.GetExportText(route);
        }
    }
}
