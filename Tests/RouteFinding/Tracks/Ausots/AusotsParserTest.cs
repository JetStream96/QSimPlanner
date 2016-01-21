namespace Tests.RouteFinding.Tracks.Ausots
{
    // TODO: Obselete. Maybe reuse for integration test?

    //    [TestClass]
    //    public class AusotsParserTest
    //    {
    //        [TestMethod]
    //        public void ParseTest()
    //        {
    //            var htmlSource =
    //@"


    //<pre>
    //20 JANUARY 2016

    //INTERNATIONAL FLEX TRACKS GROUP A: SOUTH EAST ASIA

    //TDM TRK MY13 160119233001 
    //1601192330 1601201400 
    //JAMOR IBABI LEC CALAR OOD ARNTU LEESA 20S128E SANDY 
    //15S122E ATMAP 
    //RTS/YMML ML H164 JAMOR 
    //RMK/AUSOTS GROUP A

    //TDM TRK KS13 160120120001 
    //1601201300 1601202300 
    //SVC TRK KS13 NO TRACK - USE PUBLISHED FIXED ROUTES 
    //RMK/AUSOTS GROUP A

    //INTERNATIONAL FLEX TRACKS GROUP B: MIDDLE EAST

    //TDM TRK XB13 160120060001 
    //1601200900 1601202200 
    //DOGAR DAOVO PIBED CC 15S100E 20S106E JOLLY CAR NIPEM 
    //LST BOBET 30S132E 30S135E 30S140E IDODA AROLI CRANE 
    //RTS/CRANE Y94 PARRY Y195 GLENN BN YBBN 
    //RMK/AUSOTS GROUP B

    //DOMESTIC FLEX TRACKS GROUP E

    //TDM TRK BP13 160120093001 
    //1601200930 1601201530 
    //TAM CV PHONE 28S138E VIRUV 30S131E FRT NSM 
    //RTS/YBBN BN V179 TAM 
    //NSM Q10 HAMTN Q158 PH YPPH 
    //RMK/AUSOTS GROUP E

    //</pre>

    //";
    //            var recorder = new StatusRecorder();

    //            var parser = new AusotsParser(new AusotsMessage(htmlSource),
    //                                          recorder,
    //                                          getAirportList(new List<string> { "YBBN", "YSSY", "YPPH", "YMML" }));

    //            var result = parser.Parse();

    //            Assert.IsTrue(result.Count == 3);
    //            Assert.IsTrue(MY13Exists(result));
    //            Assert.IsTrue(XB13Exists(result));
    //            Assert.IsTrue(BP13Exists(result));
    //        }

    //        private bool BP13Exists(List<AusTrack> trks)
    //        {
    //            foreach (var i in trks)
    //            {
    //                if (i.Ident == "BP13" &&
    //                    Enumerable.SequenceEqual(i.MainRoute,
    //                                             new string[] { "TAM", "CV", "PHONE", "28S38", "VIRUV", "30S31", "FRT", "NSM" }) &&
    //                    i.TimeStart == "1601200930" &&
    //                    i.TimeEnd == "1601201530" &&
    //                    i.RouteFrom.Count == 1 &&
    //                    Enumerable.SequenceEqual(i.RouteFrom[0],
    //                                             new string[] { "BN", "V179", "TAM" }) &&
    //                    i.RouteTo.Count == 1 &&
    //                    Enumerable.SequenceEqual(i.RouteTo[0],
    //                                             new string[] { "NSM", "Q10", "HAMTN", "Q158", "PH" }) &&
    //                    i.Remarks.Contains("AUSOTS GROUP E"))
    //                {
    //                    return true;
    //                }
    //            }
    //            return false;
    //        }

    //        private bool XB13Exists(List<AusTrack> trks)
    //        {
    //            foreach (var i in trks)
    //            {
    //                if (i.Ident == "XB13" &&
    //                    Enumerable.SequenceEqual(i.MainRoute,
    //                                             new string[] {"DOGAR","DAOVO","PIBED","CC","15S00",
    //                                                           "20S06","JOLLY","CAR","NIPEM","LST","BOBET",
    //                                                           "30S32","30S35","30S40","IDODA","AROLI","CRANE" }) &&
    //                    i.TimeStart == "1601200900" &&
    //                    i.TimeEnd == "1601202200" &&
    //                    i.RouteFrom.Count == 0 &&
    //                    i.RouteTo.Count == 1 &&
    //                    Enumerable.SequenceEqual(i.RouteTo[0],
    //                                             new string[] { "CRANE", "Y94", "PARRY", "Y195", "GLENN", "BN" }) &&
    //                    i.Remarks.Contains("AUSOTS GROUP B"))
    //                {
    //                    return true;
    //                }
    //            }
    //            return false;
    //        }

    //        private bool MY13Exists(List<AusTrack> trks)
    //        {
    //            foreach (var i in trks)
    //            {
    //                if (i.Ident == "MY13" &&
    //                    Enumerable.SequenceEqual(i.MainRoute,
    //                                             new string[] {"JAMOR","IBABI","LEC","CALAR","OOD",
    //                                                           "ARNTU","LEESA","20S28","SANDY",
    //                                                           "15S22","ATMAP" }) &&
    //                    i.TimeStart == "1601192330" &&
    //                    i.TimeEnd == "1601201400" &&
    //                    i.RouteFrom.Count == 1 &&
    //                    Enumerable.SequenceEqual(i.RouteFrom[0],
    //                                             new string[] { "ML", "H164", "JAMOR" }) &&
    //                    i.RouteTo.Count == 0 &&
    //                    i.Remarks.Contains("AUSOTS GROUP A"))
    //                {
    //                    return true;
    //                }
    //            }
    //            return false;
    //        }

    //        private static AirportManager getAirportList(List<string> icao)
    //        {
    //            var db = new AirportDatabase();

    //            foreach (var i in icao)
    //            {
    //                db.Add(new Airport(i, "", 0.0, 0.0, 0, 0, 0, 0, null));
    //            }
    //            return new AirportManager(db);
    //        }
    //    }
}
