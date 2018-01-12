using NUnit.Framework;
using static CommonLibrary.AviationTools.Icao;

namespace CommonLibraryTest.AviationTools
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