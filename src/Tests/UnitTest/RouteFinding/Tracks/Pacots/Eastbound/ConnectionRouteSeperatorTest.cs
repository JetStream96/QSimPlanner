using NUnit.Framework;
using QSP.RouteFinding.Tracks.Pacots.Eastbound;

namespace UnitTest.RouteFinding.Tracks.Pacots.Eastbound
{
    [TestFixture]
    public class ConnectionRouteSeperatorTest
    {
        [Test]
        public void SeperateTest()
        {
            var text = @"JAPAN ROUTE : ONION OTR5 KALNA
  NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU MARNR KSEA
              ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
              ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
        ";

            var result = ConnectionRouteSeperator.Seperate(text);

            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result[0] == " ONION OTR5 KALNA");
            Assert.IsTrue(result[1] == "ORNAI SIMLU KEPKO TOU MARNR KSEA");
            Assert.IsTrue(result[2] == "ORNAI SIMLU KEPKO TOU KEIKO KPDX");
            Assert.IsTrue(result[3] == "ORNAI SIMLU KEPKO YAZ FOCHE CYVR        ");
        }

        [Test]
        public void SeperateMultiLineTest()
        {
            var text =
@"RCTP/VHHH ROUTE : MOLKA M750 BUNGU Y81 SYOYU Y809 KAGIS A590 PABBA
                  OTR5 ADNAP OTR7 EMRON
      NAR ROUTE : ACFT LDG KSFO--CEPAS PIRAT OSI KSFO
                  ACFT LDG KLAX--CEPAS PIRAT AVE FIM KLAX";

            var result = ConnectionRouteSeperator.Seperate(text);

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result[0] == @" MOLKA M750 BUNGU Y81 SYOYU Y809 KAGIS A590 PABBA                  OTR5 ADNAP OTR7 EMRON");
            Assert.IsTrue(result[1] == "CEPAS PIRAT OSI KSFO");
            Assert.IsTrue(result[2] == "CEPAS PIRAT AVE FIM KLAX");
        }
    }
}
