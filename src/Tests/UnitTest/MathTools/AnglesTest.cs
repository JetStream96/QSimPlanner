using NUnit.Framework;
using System;
using static QSP.MathTools.Angles;

namespace UnitTest.MathTools
{
    [TestFixture]
    public class AnglesTest
    {
        [Test]
        public void ToDegreeTest()
        {
            Assert.AreEqual(180.0, ToDegree(Math.PI), 1e-8);
        }

        [Test]
        public void ToRadianTest()
        {
            Assert.AreEqual(2 * Math.PI, ToRadian(360.0), 1e-8);
        }
    }
}