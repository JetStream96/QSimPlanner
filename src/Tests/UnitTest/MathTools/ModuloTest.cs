using NUnit.Framework;
using static QSP.MathTools.Modulo;

namespace UnitTest.MathTools
{
    [TestFixture]
    public class ModuloTest
    {
        [Test]
        public void ModTest()
        {
            Assert.AreEqual(0, 4.Mod(2));
            Assert.AreEqual(1, (-1).Mod(2));
            Assert.AreEqual(0, 2.Mod(2));
        }
    }
}
