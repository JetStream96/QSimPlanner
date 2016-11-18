using NUnit.Framework;
using System;
using static QSP.MathTools.TableNew.Util;

namespace UnitTest.MathTools.TableNew
{
    [TestFixture]
    public class UtilTest
    {
        [Test]
        public void X0X1AreTheSameThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
            Interpolate(10.0, 10.0, 10.0, 3, 5));
        }

        [Test]
        public void X0X1AreTheSameTrivialCase()
        {
            Assert.AreEqual(5.0, Interpolate(10.0, 10.0, 10.0, 5.0, 5.0), 1E-6);
        }

        [Test]
        public void InterpolateTest()
        {
            Assert.AreEqual(-4.0, Interpolate(3.0, 5.0, 3.5, -8.0, 8.0), 1E-6);
        }
    }
}