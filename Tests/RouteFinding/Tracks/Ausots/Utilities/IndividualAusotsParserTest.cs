using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Ausots.Utilities;
using QSP.RouteFinding.Airports;
using System.Linq;
using QSP.AviationTools;

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

    }
}
