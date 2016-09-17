using NUnit.Framework;
using QSP.WindAloft;
using static System.Math;

namespace UnitTest.WindAloft
{
    [TestFixture]
    public class WindTest
    {
        private const double delta = 1E-6;

        [Test]
        public void FromUVNoWind()
        {
            var w = Wind.FromUV(0.0, 0.0);
            Assert.AreEqual(0.0, w.Speed, delta);
            Assert.IsTrue(0.0 < w.Direction && w.Direction <= 360.0);
        }

        [Test]
        public void FromUVTest()
        {
            Wind w;

            w = Wind.FromUV(10.0, 0.0);
            Assert.AreEqual(10.0, w.Speed, delta);
            Assert.AreEqual(270.0, w.Direction, delta);

            w = Wind.FromUV(0.0, -10.0);
            Assert.AreEqual(10.0, w.Speed, delta);
            Assert.AreEqual(360.0, w.Direction, delta);

            w = Wind.FromUV(10.0, 10.0);
            Assert.AreEqual(10.0 * Sqrt(2.0), w.Speed, delta);
            Assert.AreEqual(225.0, w.Direction, delta);

            w = Wind.FromUV(-10.0, -10.0);
            Assert.AreEqual(10.0 * Sqrt(2.0), w.Speed, delta);
            Assert.AreEqual(45.0, w.Direction, delta);
        }
    }
}
