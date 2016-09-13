using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static QSP.MathTools.Integers;

namespace UnitTest.MathTools
{
    [TestFixture]
    public class IntergersTest
    {
        [Test]
        public void PositivePowTest()
        {
            const double delta = 1E-5;
            Assert.AreEqual(32.0, Pow(2.0, 5), delta);
            Assert.AreEqual(65.0, Pow(65.0, 1), delta);
            Assert.IsTrue(double.IsPositiveInfinity(
                Pow(double.MaxValue, 3)));
            Assert.AreEqual(0.0, Pow(double.Epsilon, 5), delta);
        }

        [Test]
        public void NonPositivePowTest()
        {
            const double delta = 1E-5;
            Assert.AreEqual(1.0, Pow(-8.0, 0), delta);
            Assert.AreEqual(1.0 / 32.0, Pow(2.0, -5), delta);
            Assert.AreEqual(1.0 / 65.0, Pow(65.0, -1), delta);
        }
    }
}
