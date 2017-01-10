using NUnit.Framework;
using QSP.MathTools;

namespace UnitTest.MathTools
{
    [TestFixture]
    public class NumbersTest
    {
        [Test]
        public void PositivePowTest()
        {
            const double delta = 1E-5;
            Assert.AreEqual(32.0, Numbers.Pow(2.0, 5), delta);
            Assert.AreEqual(65.0, Numbers.Pow(65.0, 1), delta);
            Assert.IsTrue(double.IsPositiveInfinity(Numbers.Pow(double.MaxValue, 3)));
            Assert.AreEqual(0.0, Numbers.Pow(double.Epsilon, 5), delta);
        }

        [Test]
        public void NonPositivePowTest()
        {
            const double delta = 1E-5;
            Assert.AreEqual(1.0, Numbers.Pow(-8.0, 0), delta);
            Assert.AreEqual(1.0 / 32.0, Numbers.Pow(2.0, -5), delta);
            Assert.AreEqual(1.0 / 65.0, Numbers.Pow(65.0, -1), delta);
        }
    }
}
