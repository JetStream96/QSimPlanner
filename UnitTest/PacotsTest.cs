using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Pacots;
using System.IO;
using static UnitTest.Common.Utilities;

namespace UnitTest
{
    [TestClass()]
    public class PacotsTest
    {

        [TestMethod()]
        public void PacotsDownloaderTest()
        {
            File.WriteAllText("pacotsDownloadTest.html", PacotsDownloader.GetHtml());
        }

        [TestMethod()]

        public void PacotsMsgTest()
        {
            PrepareTest();

            PacotsMessage pm = new PacotsMessage(File.ReadAllText("pacotsSample.html"));
            List<PacificTrack> trks = new List<PacificTrack>();

            foreach (var i in pm.WestboundTracks)
            {
                trks.Add(new PacificTrack(i, PacotDirection.Westbound));
            }

        }

        [TestMethod()]
        public void TryFindTimeStampTest_FoundTime()
        {
            string testDtr = @"EASTBOUND PACOTS TRACKS BETWEEN JAPAN AND HAWAII,
REQUIRED FOR ACFT CROSSING 160E
BETWEEN 11061200UTC AND 11061600UTC,
TRACK 11.
 FLEX ROUTE : MORAY 34N150E 33N160E 31N170E 28N180E 25N170W CANON
JAPAN ROUTE : SMOLT OTR15 MORAY
 PHNL ROUTE : CANON BOOKE PHNL
        RMK : TRK 12 NOT AVAILABLE
              ATM CENTER TEL:81-92-608-8870. 06 NOV 10:00 2015 UNTIL 06 NOV
21:00 2015. CREATED: 05 NOV 20:30 2015
";

            var res = TrackValidPeriod.GetValidPeriod(testDtr);

            Assert.AreEqual("11061200UTC", res.Item1);
            Assert.AreEqual("11061600UTC", res.Item2);

        }

        [TestMethod()]
        public void TryFindTimeStampTest_Found1()
        {
            string testDtr = @"EASTBOUND PACOTS TRACKS BETWEEN JAPAN AND HAWAII,
REQUIRED FOR ACFT CROSSING 160E
BETWEEN 11061200UT AND 11061600UTC,
TRACK 11.
 FLEX ROUTE : MORAY 34N150E 33N160E 31N170E 28N180E 25N170W CANON
JAPAN ROUTE : SMOLT OTR15 MORAY
 PHNL ROUTE : CANON BOOKE PHNL
        RMK : TRK 12 NOT AVAILABLE
              ATM CENTER TEL:81-92-608-8870. 06 NOV 10:00 2015 UNTIL 06 NOV
21:00 2015. CREATED: 05 NOV 20:30 2015
";

            var res = TrackValidPeriod.GetValidPeriod(testDtr);

            Assert.AreEqual(string.Empty , res.Item1);
            Assert.AreEqual(string.Empty, res.Item2);
        }

        [TestMethod()]
        public void TryFindTimeStampTest_Found0()
        {
            string testDtr = @"EASTBOUND PACOTS TRACKS BETWEEN JAPAN AND HAWAII,
REQUIRED FOR ACFT CROSSING 160E
BETWEEN 11061200UT AND 1101600UTC,
TRACK 11.
 FLEX ROUTE : MORAY 34N150E 33N160E 31N170E 28N180E 25N170W CANON
JAPAN ROUTE : SMOLT OTR15 MORAY
 PHNL ROUTE : CANON BOOKE PHNL
        RMK : TRK 12 NOT AVAILABLE
              ATM CENTER TEL:81-92-608-8870. 06 NOV 10:00 2015 UNTIL 06 NOV
21:00 2015. CREATED: 05 NOV 20:30 2015
";

            var res = TrackValidPeriod.GetValidPeriod(testDtr);

            Assert.AreEqual(string.Empty, res.Item1);
            Assert.AreEqual(string.Empty, res.Item2);
        }

        [TestMethod()]
        public void EastboundTest_FoundTracks()
        {
            string testDtr = @"EASTBOUND PACOTS TRACKS BETWEEN JAPAN AND NORTH AMERICA,
TRACK 1.
 FLEX ROUTE : PUTER A590 PASRO POWAL CURVS 53N170W 52N160W 51N150W
              51N140W ORNAI
JAPAN ROUTE : PEXEL A590 PUTER
  NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU JAWBN KSEA
              ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
              ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
        RMK : ACFT LDG OTHER DEST--ORNAI SIMLU KEPKO UPR TO DEST
TRACK 2.
 FLEX ROUTE : ADGOR R591 ASPIN CHIKI 51N170W 50N160W 48N150W
              46N140W 42N130W VESPA
JAPAN ROUTE : ONION OTR5 ADNAP R591 ADGOR
  NAR ROUTE : ACFT LDG KSFO--VESPA AMAKR BGGLO KSFO
              ACFT LDG KLAX--VESPA ENI AVE FIM KLAX
TRACK 3.
 FLEX ROUTE : KALNA 44N160E 47N170E 49N180E 49N170W 48N160W 46N150W
              43N140W 39N130W DACEM
JAPAN ROUTE : ONION OTR5 KALNA
  NAR ROUTE : ACFT LDG KLAX--DACEM PAINT PIRAT AVE FIM KLAX
              ACFT LDG KSFO--DACEM PAINT PIRAT OSI KSFO
        RMK : ATM CENTER TEL:81-92-608-8870. 06 NOV 07:00 2015 UNTIL 06 NOV
21:00 2015. CREATED: 05 NOV 20:30 2015
";

            var res = EastTracksParser.SplitTrackMsg(testDtr);
            string[] idents = { "1", "2", "3" };

            string[] texts = { @"
 FLEX ROUTE : PUTER A590 PASRO POWAL CURVS 53N170W 52N160W 51N150W
              51N140W ORNAI
JAPAN ROUTE : PEXEL A590 PUTER
  NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU JAWBN KSEA
              ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
              ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
        RMK : ACFT LDG OTHER DEST--ORNAI SIMLU KEPKO UPR TO DEST
", @"
 FLEX ROUTE : ADGOR R591 ASPIN CHIKI 51N170W 50N160W 48N150W
              46N140W 42N130W VESPA
JAPAN ROUTE : ONION OTR5 ADNAP R591 ADGOR
  NAR ROUTE : ACFT LDG KSFO--VESPA AMAKR BGGLO KSFO
              ACFT LDG KLAX--VESPA ENI AVE FIM KLAX
", @"
 FLEX ROUTE : KALNA 44N160E 47N170E 49N180E 49N170W 48N160W 46N150W
              43N140W 39N130W DACEM
JAPAN ROUTE : ONION OTR5 KALNA
  NAR ROUTE : ACFT LDG KLAX--DACEM PAINT PIRAT AVE FIM KLAX
              ACFT LDG KSFO--DACEM PAINT PIRAT OSI KSFO
        RMK : ATM CENTER TEL:81-92-608-8870. 06 NOV 07:00 2015 UNTIL 06 NOV
21:00 2015. CREATED: 05 NOV 20:30 2015
" };

            Assert.AreEqual(3, res.Count);

            for (int i = 0; i <= 2; i++)
            {
                Assert.AreEqual(idents[i], res[i].Item1);
                Assert.IsTrue(texts[i] == res[i].Item2);
            }

        }

        [TestMethod()]
        public void LoadEastboundPacotsTest()
        {
            PrepareTest();

            PacotsMessage pm = new PacotsMessage(File.ReadAllText("pacotsSample.html"));
            var trksAsc = EastTracksParser.CreateEastboundTracks(pm);

        }


    }
}
