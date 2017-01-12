using NUnit.Framework;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class DoubleArrayCompareTest
    {
        private const double delta = 1E-10;

        [Test]
        public void OneDimArrayCompareTest()
        {
            double[] a = { 1.0, 2.0, 3.0 };
            double[] b = { 1.0, 2.0, 3.0 };
            double[] c = { 3.0, 2.0, 1.0 };
            double[] d = { 1.0 + 5E-11, 2.0, 3.0 };
            double[] e = null;
            double[] f = { 1.0, 2.0 };

            Assert.IsTrue(DoubleArrayCompare.Equals(a, b, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, c, delta));
            Assert.IsTrue(DoubleArrayCompare.Equals(a, d, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, e, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, f, delta));
        }

        [Test]
        public void TwoDimArrayCompareTest()
        {
            double[][] a =
            {
                new[] { 1.0, 2.0, 3.0 },
                new[] { 8.0, -5.0, 1.5 }
            };

            double[][] b =
            {
                new[] { 1.0, 2.0, 3.0 },
                new[] { 8.0, -5.0, 1.5 }
            };

            double[][] c =
            {
                new[] { 10.0, 12.0, 13.0 },
                new[] { 8.0, -5.0, 1.5 }
            };

            Assert.IsTrue(DoubleArrayCompare.Equals(a, b, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, c, delta));
        }

        [Test]
        public void ThreeDimArrayCompareTest()
        {
            var a = new[]
            {
                new[]
                {
                    new[] { 1.0, 2.0 },
                    new[] { 8.0, -5.0 }
                },
                new[]
                {
                    new[] { 5.0, 6.0 },
                    new[] { 7.0, 8.0 }
                }
           };

            var b = new[]
            {
                new[]
                {
                    new[] { 1.0, 2.0 },
                    new[] { 8.0, -5.0 }
                },
                new[]
                {
                    new[] { 5.0, 6.0 },
                    new[] { 7.0, 8.0 }
                }
            };

            var c = new[]
            {
                new[]
                {
                    new[] { 1.0, 2.0 },
                    new[] { 8.0, -5.0 }
                },
                new[]
                {
                    new[] { 5.0, 6.0 },
                    new[] { 0.0, 0.0 }
                }
            };

            Assert.IsTrue(DoubleArrayCompare.Equals(a, b, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, c, delta));
        }
    }
}
