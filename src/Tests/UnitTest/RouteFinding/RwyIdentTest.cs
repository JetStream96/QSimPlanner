using NUnit.Framework;
using QSP.RouteFinding;

namespace UnitTest.RouteFinding
{
    [TestFixture]
    public class RwyIdentTest
    {
        [Test]
        public void RwyIdentResult()
        {
            Assert.IsTrue(RwyIdent.IsRwyIdent("18"));
            Assert.IsTrue(RwyIdent.IsRwyIdent("36"));
            Assert.IsTrue(RwyIdent.IsRwyIdent("01"));
            Assert.IsFalse(RwyIdent.IsRwyIdent("45"));

            Assert.IsTrue(RwyIdent.IsRwyIdent("18R"));
            Assert.IsTrue(RwyIdent.IsRwyIdent("27C"));
            Assert.IsTrue(RwyIdent.IsRwyIdent("09R"));
            Assert.IsFalse(RwyIdent.IsRwyIdent("00C"));
            Assert.IsFalse(RwyIdent.IsRwyIdent("XYR"));
        }
    }
}