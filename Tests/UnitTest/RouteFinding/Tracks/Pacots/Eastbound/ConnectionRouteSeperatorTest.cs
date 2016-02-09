using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Pacots.Eastbound;
using System.Collections;
using System.Linq;

namespace UnitTest.RouteFinding.Tracks.Pacots.Eastbound
{
    [TestClass]
    public class ConnectionRouteSeperatorTest
    {
        [TestMethod]
        public void SeperateTest()
        {
            var text = @"JAPAN ROUTE : ONION OTR5 KALNA
  NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU MARNR KSEA
              ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
              ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
        ";

            var sep = new ConnectionRouteSeperator(text);
            var result = sep.Seperate();

            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result[0] == " ONION OTR5 KALNA");
            Assert.IsTrue(result[1] == "ORNAI SIMLU KEPKO TOU MARNR KSEA");
            Assert.IsTrue(result[2] == "ORNAI SIMLU KEPKO TOU KEIKO KPDX");
            Assert.IsTrue(result[3] == "ORNAI SIMLU KEPKO YAZ FOCHE CYVR        ");
        }

        [TestMethod]
        public void SeperateMultiLineTest()
        {
            var text = @"JAPAN ROUTE : ONION OTR5 KALNA
  NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU                MARNR KSEA
              ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX";

            var sep = new ConnectionRouteSeperator(text);
            var result = sep.Seperate();

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result[0] == " ONION OTR5 KALNA");
            Assert.IsTrue(result[1] == "ORNAI SIMLU KEPKO TOU                MARNR KSEA");
            Assert.IsTrue(result[2] == "ORNAI SIMLU KEPKO TOU KEIKO KPDX");
        }
    }
}
