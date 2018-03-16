using System;
using NUnit.Framework;
using QSP.MathTools.Vectors;

namespace UnitTest.MathTools.Vectors
{
    [TestFixture]
    public class Vector2DTest
    {
        private const double delta = 1e-8;

        [Test]
        public void CtorTest()
        {
            var v = new Vector2D(3, 2);
            Assert.AreEqual(3, v.X, delta);
            Assert.AreEqual(2, v.Y, delta);
        }

        [Test]
        public void FromPolarTest()
        {
            var v = Vector2D.PolarCoords(2, Math.PI * 0.5);
            Assert.AreEqual(0, v.X, delta);
            Assert.AreEqual(2, v.Y, delta);
        }

        [Test]
        public void PolarTest0()
        {
            var v = Vector2D.PolarCoords(0.78, -1.25);
            Assert.AreEqual(0.78, v.R, delta);
            Assert.AreEqual(-1.25, v.Theta, delta);
        }

        [Test]
        public void PolarTest1()
        {
            var v = new Vector2D(0, 0);
            Assert.AreEqual(0, v.R, delta);
            Assert.AreEqual(0, v.Theta, delta);
        }

        [Test]
        public void EqualsTest0()
        {
            var v = new Vector2D(2, -9);
            var w = new Vector2D(2, -9);
            Assert.AreEqual(v, w);
        }

        [Test]
        public void EqualsTest1()
        {
            var v = new Vector2D(-9, 3);
            var w = new Vector2D(-9, 3 + delta * 0.5);
            Assert.IsTrue(v.Equals(w, delta));
        }

        [Test]
        public void NotEqualsTest()
        {
            var v = new Vector2D(-9, 3);
            var w = new Vector2D(-9, 2);
            Assert.AreNotEqual(v, w);
        }

        [Test]
        public void AddTest()
        {
            var v = new Vector2D(2, -9) + new Vector2D(6, 9.5);
            Assert.AreEqual(8, v.X, delta);
            Assert.AreEqual(.5, v.Y, delta);
        }

        [Test]
        public void SubtractTest()
        {
            var v = new Vector2D(2, -9) - new Vector2D(6, 9.5);
            Assert.AreEqual(-4, v.X, delta);
            Assert.AreEqual(-18.5, v.Y, delta);
        }

        [Test]
        public void MultiplyTest()
        {
            var v = new Vector2D(2, -9) * 3;
            var w = 3 * new Vector2D(2, -9);
            Assert.AreEqual(new Vector2D(6, -27), v);
            Assert.AreEqual(new Vector2D(6, -27), w);
        }

        [Test]
        public void NegateTest()
        {
            var v = -new Vector2D(2, -9);
            Assert.IsTrue(new Vector2D(-2, 9).Equals(v, delta));
        }

        [Test]
        public void NormalizeTest()
        {
            var v = new Vector2D(-4, 3).Normalize();
            Assert.IsTrue(new Vector2D(-0.8, 0.6).Equals(v, delta));
        }

        [Test]
        public void NormalizeTestInvalid()
        {
            var v = new Vector2D(0, 0);
            Assert.Throws<InvalidOperationException>(() => v.Normalize());
        }

        [Test]
        public void DotTest()
        {
            var v = new Vector2D(3, 2).Dot(new Vector2D(8, 1));
            Assert.AreEqual(26, v, delta);
        }
    }
}