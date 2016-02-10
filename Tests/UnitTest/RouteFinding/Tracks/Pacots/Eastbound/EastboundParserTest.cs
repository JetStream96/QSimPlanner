using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Airports;
using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Pacots.Eastbound;

namespace UnitTest.RouteFinding.Tracks.Pacots.Eastbound
{
    [TestClass]
    public class EastboundParserTest
    {
        [TestMethod]
        public void ParseTest()
        {
            // Arrange
            var msg = @"EASTBOUND PACOTS TRACKS BETWEEN JAPAN AND NORTH AMERICA,
TRACK 1.
 FLEX ROUTE : EMRON 40N160E 41N170E 43N180E 45N170W 47N160W 48N150W
              49N140W PRETY
JAPAN ROUTE : ONION OTR5 ADNAP OTR7 EMRON
  NAR ROUTE : ACFT LDG KSEA--PRETY TAMRU TOU MARNR KSEA
              ACFT LDG KPDX--PRETY TAMRU TOU KEIKO KPDX
        RMK : ACFT LDG CYVR--48N150W 50N140W ORNAI SIMLU KEPKO YAZ
              FOCHE CYVR
              ACFT LDG OTHER DEST--PRETY TAMRU SEFIX UPR TO DEST
TRACK 2.
 FLEX ROUTE : LEPKI 39N160E 40N170E 41N180E 41N170W 41N160W 41N150W
              42N140W 41N130W TRYSH
JAPAN ROUTE : AVBET OTR11 LEPKI
  NAR ROUTE : ACFT LDG KSFO--TRYSH AMAKR BGGLO KSFO
              ACFT LDG KLAX--TRYSH ENI AVE FIM KLAX
TRACK 3.
 FLEX ROUTE : SEALS 36N150E 38N160E 39N170E 39N180E 39N170W 40N160W
              40N150W 40N140W 39N130W DACEM
JAPAN ROUTE : VACKY OTR13 SEALS
  NAR ROUTE : ACFT LDG KLAX--DACEM PAINT PIRAT AVE FIM KLAX
              ACFT LDG KSFO--DACEM PAINT PIRAT OSI KSFO
        RMK : ATM CENTER TEL:81-92-608-8870. 08 FEB 07:00 2016 UNTIL 08 FEB
21:00 2016. CREATED: 07 FEB 19:09 2016";

            var airports = new AirportCollection();
            airports.Add(new Airport("KSEA", "", 0.0, 0.0, 0, 0, 0, 0, null));
            airports.Add(new Airport("KPDX", "", 0.0, 0.0, 0, 0, 0, 0, null));
            airports.Add(new Airport("KLAX", "", 0.0, 0.0, 0, 0, 0, 0, null));

            // Act
            var trks = new EastboundParser(new AirportManager(airports))
                     .Parse(new PacotsMessage(new List<string>(),
                                              new List<string>() { msg },
                                              "",
                                              ""));
            // Assert
            Assert.AreEqual(3, trks.Count);

            var trk1 = getTrack(trks, "1");
            Assert.AreEqual(PacotDirection.Eastbound, trk1.Direction);

            Assert.IsTrue(Enumerable.SequenceEqual(trk1.MainRoute,
                new string[] { "EMRON", "40N160E", "41N170E", "43N180E", "45N170W", "47N160W", "48N150W",
              "49N140W", "PRETY"}));

            Assert.AreEqual(1, trk1.RouteFrom.Count);

            Assert.IsTrue(Enumerable.SequenceEqual(trk1.RouteFrom[0],
               new string[] { "ONION", "OTR5", "ADNAP", "OTR7", "EMRON" }));

            Assert.AreEqual(2, trk1.RouteTo.Count);

            Assert.IsTrue(Enumerable.SequenceEqual(trk1.RouteTo[0],
               new string[] { "PRETY", "TAMRU", "TOU", "MARNR" }));

            Assert.IsTrue(Enumerable.SequenceEqual(trk1.RouteTo[1],
               new string[] { "PRETY", "TAMRU", "TOU", "KEIKO" }));

            Assert.IsTrue(trk1.Remarks == @" ACFT LDG CYVR--48N150W 50N140W ORNAI SIMLU KEPKO YAZ
              FOCHE CYVR
              ACFT LDG OTHER DEST--PRETY TAMRU SEFIX UPR TO DEST
");
        }

        private PacificTrack getTrack(IEnumerable<PacificTrack> trks, string ID)
        {
            foreach (var i in trks)
            {
                if (i.Ident == ID)
                {
                    return i;
                }
            }
            return null;
        }
    }
}
