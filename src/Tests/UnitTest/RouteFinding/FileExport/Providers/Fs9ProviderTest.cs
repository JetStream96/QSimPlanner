using QSP.LibraryExtension;
using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class Fs9ProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var abcd = Common.GetAirport("ABCD");
            var efgh = Common.GetAirport("EFGH");
            var manager = Common.GetAirportManager(abcd, efgh);

            var route = Common.GetRoute(
               new Waypoint("ABCD02"), "A", -1.0,
               new Waypoint("WPT", 0.0, 1.0), "B", -1.0,
               new Waypoint("EFGH18"));

            var text = Fs9Provider.GetExportText(new ExportInput()
            {
                Route = route,
                Airports = manager
            });

            var expected =
@"[flightplan]
title=ABCD to EFGH
description=ABCD, EFGH
type=IFR
routetype=3
cruising_altitude=10000
departure_id=ABCD, N0* 0.00', E0* 0.00', +000000.00
destination_id=EFGH, N0* 0.00', E0* 0.00', +000000.00
departure_name=
destination_name=
waypoint.0=ABCD, A, N0* 0.00', E0* 0.00', +000000.00, 
waypoint.1=WPT, I, N0* 0.00', E1* 0.00', +000000.00, 
waypoint.2=EFGH, A, N0* 0.00', E0* 0.00', +000000.00, 
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
                new Waypoint("ABCD02"), "A", -1.0,
                new Waypoint("N10.2W20.0", 10.2, -20.0), "B", -1.0,
                new Waypoint("EFGH18"));

            var text = Fs9Provider.GetExportText(new ExportInput()
            {
                Route = route,
                Airports = manager
            });

            var expected =
@"[flightplan]
title=ABCD to EFGH
description=ABCD, EFGH
type=IFR
routetype=3
cruising_altitude=10000
departure_id=ABCD, N0* 0.00', E0* 0.00', +000000.00
destination_id=EFGH, N0* 0.00', E0* 0.00', +000000.00
departure_name=
destination_name=
waypoint.0=ABCD, A, N0* 0.00', E0* 0.00', +000000.00, 
waypoint.1=1012N2000W, I, N10* 12.00', W20* 0.00', +000000.00, 
waypoint.2=EFGH, A, N0* 0.00', E0* 0.00', +000000.00, 
";
            Assert.IsTrue(expected.EqualsIgnoreNewlineStyle(text));
        }
    }
}
