using NUnit.Framework;
using System;
using static QSP.MathTools.Interpolation.Interpolate1D;

namespace UnitTest.MathTools.Interpolation
{
    [TestFixture]
    public class Interpolate1DTest
    {
        [Test]
        public void X0X1AreTheSameThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
            Interpolate(10.0, 10.0, 3, 5, 10.0));
        }

        [Test]
        public void X0X1AreTheSameTrivialCase()
        {
            Assert.AreEqual(5.0, Interpolate(10.0, 10.0, 5.0, 5.0, 10.0), 1E-6);
        }

        [Test]
        public void InterpolateTest1()
        {
            Assert.AreEqual(-4.0, Interpolate(3.0, 5.0, -8.0, 8.0, 3.5), 1E-6);
        }

        [Test]
        public void InterpolateTest2()
        {
            Assert.AreEqual(12.2,
                            Interpolate(new double[] { 3.0, 5.0, 7.0, 9.0 },
                                        new double[] { -9.0, -1.0, 12.0, 14.0 },
                                        7.2),
                            1E-6);
        }

        [Test]
        public void InterpolateTest3()
        {
            Assert.AreEqual(12.2,
                            Interpolate(new double[] { 3.0, 5.0, 7.0, 9.0 },
                                        new double[] { -9.0, -1.0, 12.0, 14.0 },
                                        2,
                                        7.2),
                            1E-6);
        }

    }
}
