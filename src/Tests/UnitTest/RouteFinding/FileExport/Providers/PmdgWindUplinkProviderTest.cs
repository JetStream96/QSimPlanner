using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;
using QSP.WindAloft;
using System.Collections.Generic;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class PmdgWindUplinkProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT0", 0.0, 10.0), "B", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = PmdgWindUplinkProvider.GetExportText(route, new WxTablesStub());

            var expected =
@"RJBB	275@27(8)	275@27(8)	275@27(8)	275@27(8)	275@27(8)
	275@27(8)	275@27(8)	275@27(8)	275@27(8)	275@27(8)
WPT0	263@41(3)	263@41(3)	263@41(3)	263@41(3)	263@41(3)
	263@41(3)	263@41(3)	263@41(3)	263@41(3)	263@41(3)
RJAA	278@26(6)	278@26(6)	278@26(6)	278@26(6)	278@26(6)
	278@26(6)	278@26(6)	278@26(6)	278@26(6)	278@26(6)";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextCoordinateFormatIsCorrect()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "A", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "B", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = PmdgWindUplinkProvider.GetExportText(route, new WxTablesStub());

            var expected =
                @"RJBB	275@27(8)	275@27(8)	275@27(8)	275@27(8)	275@27(8)
	275@27(8)	275@27(8)	275@27(8)	275@27(8)	275@27(8)
1012N2000W	263@41(3)	263@41(3)	263@41(3)	263@41(3)	263@41(3)
	263@41(3)	263@41(3)	263@41(3)	263@41(3)	263@41(3)
RJAA	278@26(6)	278@26(6)	278@26(6)	278@26(6)	278@26(6)
	278@26(6)	278@26(6)	278@26(6)	278@26(6)	278@26(6)";

            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }

        [Test]
        public void GetExportTextNoWaypontDoesNotThrow()
        {
            var route = Common.GetRoute(
                new Waypoint("RJBB06L", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("RJAA18", 0.0, 3.0));

            var text = PmdgWindUplinkProvider.GetExportText(route, new WxTablesStub());
        }

        private class WxTablesStub : IWxTableCollection
        {
            public double GetTemp(double lat, double lon, double altitudeFt)
            {
                if (lon == 0) return 8;
                if (lon == 10) return 3;
                return 6;
            }

            public WindUV GetWindUV(double lat, double lon, double altitudeFt)
            {
                if (lon == 0) return new Wind(275, 27).ToWindUV();
                if (lon == 10) return new Wind(263, 41).ToWindUV();
                return new Wind(278, 26).ToWindUV();
            }
        }
    }
}
