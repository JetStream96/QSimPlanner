using NUnit.Framework;
using QSP.MathTools.Vectors;
using System;

namespace UnitTest.MathTools.Vectors
{
    [TestFixture]
    public class Vector3DTest
    {
        private const double delta = 1e-8;

        [Test]
        public void CtorTest()
        {
            var v = new Vector3D(3, 2, 1);
            Assert.AreEqual(3, v.X, delta);
            Assert.AreEqual(2, v.Y, delta);
            Assert.AreEqual(1, v.Z, delta);
        }

        [Test]
        public void FromSphericalTest()
        {
            var v = Vector3D.FromSphericalCoords(2, Math.PI * 0.5, Math.PI * 0.5);
            Assert.AreEqual(0, v.X, delta);
            Assert.AreEqual(2, v.Y, delta);
            Assert.AreEqual(0, v.Z, delta);
        }

        [Test]
        public void SphericalTest0()
        {
            var v = Vector3D.FromSphericalCoords(2, 0.78, -1.25);
            Assert.AreEqual(2, v.R, delta);
            Assert.AreEqual(0.78, v.Phi, delta);
            Assert.AreEqual(-1.25, v.Theta, delta);
        }

        [Test]
        public void SphericalTest1()
        {
            var v = new Vector3D(0, 0, 0);
            Assert.AreEqual(0, v.R, delta);
            Assert.AreEqual(0, v.Theta, delta);
            Assert.AreEqual(0, v.Phi, delta);
        }

        [Test]
        public void EqualsTest0()
        {
            var v = new Vector3D(2, -9, 3);
            var w = new Vector3D(2, -9, 3);
            Assert.AreEqual(v, w);
        }

        [Test]
        public void EqualsTest1()
        {
            var v = new Vector3D(2, -9, 3);
            var w = new Vector3D(2, -9, 3 + delta * 0.5);
            Assert.IsTrue(v.Equals(w, delta));
        }

        [Test]
        public void NotEqualsTest()
        {
            var v = new Vector3D(2, -9, 3);
            var w = new Vector3D(2, -9, 2);
            Assert.AreNotEqual(v, w);
        }

        [Test]
        public void AddTest()
        {
            var v = new Vector3D(2, -9, 3) + new Vector3D(6, 9.5, 4);
            Assert.AreEqual(8, v.X, delta);
            Assert.AreEqual(.5, v.Y, delta);
            Assert.AreEqual(7, v.Z, delta);
        }

        [Test]
        public void SubtractTest()
        {
            var v = new Vector3D(2, -9, 3) - new Vector3D(6, 9.5, 4);
            Assert.AreEqual(-4, v.X, delta);
            Assert.AreEqual(-18.5, v.Y, delta);
            Assert.AreEqual(-1, v.Z, delta);
        }

        [Test]
        public void MultiplyTest()
        {
            var v = new Vector3D(2, -9, 3) * 3;
            var w = 3 * new Vector3D(2, -9, 3);
            Assert.AreEqual(new Vector3D(6, -27, 9), v);
            Assert.AreEqual(new Vector3D(6, -27, 9), w);
        }

        [Test]
        public void NegateTest()
        {
            var v = -new Vector3D(2, -9, 3);
            Assert.IsTrue(new Vector3D(-2, 9, -3).Equals(v, delta));
        }

        [Test]
        public void NormalizeTest()
        {
            var v = new Vector3D(0, -4, 3).Normalize();
            Assert.IsTrue(new Vector3D(0, -0.8, 0.6).Equals(v, delta));
        }

        [Test]
        public void NormalizeTestInvalid()
        {
            var v = new Vector3D(0, 0, 0);
            Assert.Throws<InvalidOperationException>(() => v.Normalize());
        }

        [Test]
        public void DotTest()
        {
            var v = new Vector3D(3, 2, 9).Dot(new Vector3D(8, 1, 0));
            Assert.AreEqual(26, v, delta);
        }

        [Test]
        public void CrossTest()
        {
            var v = new Vector3D(3, 2, 9).Cross(new Vector3D(8, 1, 0));
            Assert.IsTrue(new Vector3D(-9, 72, -13).Equals(v, delta));
        }
    }
}