using NUnit.Framework;
using System;
using QSP.MathTools.TableNew;

namespace UnitTest.MathTools.TableNew
{
    [TestFixture]
    public class Table1DTest
    {
        [Test]
        public void ConstructorValidArray()
        {
            new Table1D(new[] { 6.0, 5.0 }, new[] { -3.0, 4.0 });
        }

        [Test]
        public void ConstructorFArrayTooSmall()
        {
            Assert.Throws<ArgumentException>(() =>
            new Table1D(new[] { 6.0, 5.0 }, new[] { -3.0 }));
        }

        [Test]
        public void ConstructorNotIncreasingNorDecreasing()
        {
            Assert.Throws<ArgumentException>(() =>
            new Table1D(new[] { 6.0, 5.0, 7.0 }, new[] { -3.0 }));
        }

        [Test]
        public void ValueAtTest()
        {
            var table = new Table1D(
                new []{ 3.0, 4.0, 5.0 },
                new [] { 8.0, -7.5, 1.35 });

            const double delta = 1E-12;
            Assert.AreEqual(23.5, table.ValueAt(2.0), delta);
            Assert.AreEqual(0.25, table.ValueAt(3.5), delta);
            Assert.AreEqual(10.2, table.ValueAt(6.0), delta);
        }
    }
}