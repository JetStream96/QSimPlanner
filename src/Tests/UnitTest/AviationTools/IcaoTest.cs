using NUnit.Framework;
using static QSP.AviationTools.Icao;

namespace UnitTest.AviationTools
{
    [TestFixture]
    public class IcaoTest
    {
        [Test]
        public void TrimIcaoTest()
        {
            Assert.AreEqual("KLAS", TrimIcao("klas"));
            Assert.AreEqual("KLAS", TrimIcao("  klas "));
            Assert.AreEqual("KLAS", TrimIcao("  KLas    "));
        }
    }
}