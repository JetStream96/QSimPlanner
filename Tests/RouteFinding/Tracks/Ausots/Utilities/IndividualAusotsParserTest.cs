using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Ausots.Utilities;
using QSP.RouteFinding.Airports;
using System.Linq;
using QSP.AviationTools.Coordinates;

namespace Tests.RouteFinding.Tracks.Ausots.Utilities
{
    [TestClass]
    public class IndividualAusotsParserTest
    {
        [TestMethod]
        public void WhenTrackNotAvailReturnNull()
        {
            var parser = new IndividualAusotsParser(
@"TDM TRK MY16 151113233001 
1511132330 1511141400 
SVC TRK MY16 NO TRACK - USE PUBLISHED FIXED ROUTES 
RMK/AUSOTS GROUP A",
                new AirportManager(new AirportDatabase()));

            Assert.IsTrue(parser.Parse() == null);
        }

        [TestMethod]
        public void RtsRmkExistShouldReturnCorrectAusTrack()
        {
            var parser = new IndividualAusotsParser(
@"TDM TRK SY16 151114030001 
1511140300 1511141900 
PKS WILLY BHI 30S135E 27S129E 24S124E PD OSOTO DOMOM SAPDA 
RTS/YSSY TESAT H44 KAT A576 PKS 
RMK/AUSOTS GROUP A",
getAirportList(new List<string> { "YSSY" }));

            var trk = parser.Parse();

            Assert.AreEqual("SY16", trk.Ident);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.MainRoute,
                                                   new string[] { "PKS", "WILLY", "BHI", "30S35", "27S29", "24S24",
                                                                    "PD", "OSOTO", "DOMOM", "SAPDA" }));
            Assert.AreEqual(1, trk.RouteFrom.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.RouteFrom[0],
                                                   new string[] { "TESAT", "H44", "KAT", "A576", "PKS" }));
            Assert.AreEqual(0, trk.RouteTo.Count);
            Assert.AreEqual("AUSOTS GROUP A", trk.Remarks);

            Assert.AreEqual("1511140300", trk.TimeStart);
            Assert.AreEqual("1511141900", trk.TimeEnd);
            Assert.IsTrue(trk.PreferredFirstLatLon.Equals(new LatLon(-25.0, 133.0)));
        }

        private static AirportManager getAirportList(List<string> icao)
        {
            var db = new AirportDatabase();

            foreach (var i in icao)
            {
                db.Add(new Airport(i, "", 0.0, 0.0, 0, 0, 0, 0, null));
            }
            return new AirportManager(db);
        }

        [TestMethod]
        public void RtsDoesNotExist()
        {
            var parser = new IndividualAusotsParser(
@"TDM TRK SY16 151114030001 
1511140300 1511141900 
PKS WILLY BHI 30S135E 27S129E 24S124E PD OSOTO DOMOM SAPDA 
RMK/AUSOTS GROUP A",
null);

            var trk = parser.Parse();

            Assert.AreEqual("SY16", trk.Ident);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.MainRoute,
                                                   new string[] { "PKS", "WILLY", "BHI", "30S35", "27S29", "24S24",
                                                                    "PD", "OSOTO", "DOMOM", "SAPDA" }));
            Assert.AreEqual(0, trk.RouteFrom.Count);
            Assert.AreEqual(0, trk.RouteTo.Count);
            Assert.AreEqual("AUSOTS GROUP A", trk.Remarks);
        }

        [TestMethod]
        public void RmkDoesNotExistRemarkShouldBeEmptyString()
        {
            var parser = new IndividualAusotsParser(
@"TDM TRK SY16 151114030001 
1511140300 1511141900 
PKS WILLY BHI 30S135E 27S129E 24S124E PD OSOTO DOMOM SAPDA 
RTS/YSSY TESAT H44 KAT A576 PKS 
",
getAirportList(new List<string> { "YSSY" }));

            var trk = parser.Parse();

            Assert.AreEqual("SY16", trk.Ident);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.MainRoute,
                                                   new string[] { "PKS", "WILLY", "BHI", "30S35", "27S29", "24S24",
                                                                    "PD", "OSOTO", "DOMOM", "SAPDA" }));
            Assert.AreEqual(1, trk.RouteFrom.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.RouteFrom[0],
                                                   new string[] { "TESAT", "H44", "KAT", "A576", "PKS" }));
            Assert.AreEqual(0, trk.RouteTo.Count);
            Assert.AreEqual("", trk.Remarks);
        }

        [TestMethod]
        public void NeitherRtsRmkExist()
        {
            var parser = new IndividualAusotsParser(
@"TDM TRK SY16 151114030001 
1511140300 1511141900 
PKS WILLY BHI 30S135E 27S129E 24S124E PD OSOTO DOMOM SAPDA 
",
null);

            var trk = parser.Parse();

            Assert.AreEqual("SY16", trk.Ident);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.MainRoute,
                                                   new string[] { "PKS", "WILLY", "BHI", "30S35", "27S29", "24S24",
                                                                    "PD", "OSOTO", "DOMOM", "SAPDA" }));
            Assert.AreEqual(0, trk.RouteFrom.Count);
            Assert.AreEqual(0, trk.RouteTo.Count);
            Assert.AreEqual("", trk.Remarks);
        }

        [TestMethod]
        public void MutlipleConnectionRoutes()
        {
            var parser = new IndividualAusotsParser(
@"TDM TRK PB13 160120143001 
1601201430 1601202200 
NSM CAG 32S130E WR EVIEC IDODA AROLI CRANE 
RTS/YPPH PH H18 BURGU H18 NSM 
CRANE Y94 PARRY Y195 GLENN BN YBBN 
RMK/AUSOTS GROUP E",
getAirportList(new List<string> { "YBBN", "YPPH" }));

            var trk = parser.Parse();

            Assert.AreEqual("PB13", trk.Ident);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.MainRoute,
                                                   new string[] { "NSM", "CAG", "32S30", "WR", "EVIEC", "IDODA", "AROLI", "CRANE" }));
            Assert.AreEqual(1, trk.RouteFrom.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.RouteFrom[0],
                                                   new string[] { "PH", "H18", "BURGU", "H18", "NSM" }));
            Assert.AreEqual(1, trk.RouteTo.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(trk.RouteTo[0],
                                                   new string[] { "CRANE", "Y94", "PARRY", "Y195", "GLENN", "BN" }));
            Assert.AreEqual("AUSOTS GROUP E", trk.Remarks);
        }
    }
}
