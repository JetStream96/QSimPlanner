using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.FuelCalculation.Calculations;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class BasicCrzAltProviderTest
    {
        [Test]
        public void ClosestAltTest()
        {
            var p = new BasicCrzAltProvider();
            var c = new LatLon(0.0, 0.0);
            var west = 100.0;
            var east = 320.0;

            Assert.AreEqual(25000.0, p.ClosestAlt(c, east, 25000.0));
            Assert.AreEqual(25000.0, p.ClosestAlt(c, east, 25900.0));
            Assert.AreEqual(26000.0, p.ClosestAlt(c, west, 25001.0));
            Assert.AreEqual(41000.0, p.ClosestAlt(c, east, 40900.0));
            Assert.AreEqual(41000.0, p.ClosestAlt(c, east, 41900.0));
            Assert.AreEqual(40000.0, p.ClosestAlt(c, west, 41400.0));
            Assert.AreEqual(45000.0, p.ClosestAlt(c, east, 43600.0));
        }

        [Test]
        public void ClosestAltBelowTest()
        {
            var p = new BasicCrzAltProvider();
            var c = new LatLon(0.0, 0.0);
            var west = 100.0;
            var east = 320.0;

            Assert.AreEqual(23000.0, p.ClosestAltBelow(c, east, 25000.0));
            Assert.AreEqual(24000.0, p.ClosestAltBelow(c, west, 25000.0));
            Assert.AreEqual(39000.0, p.ClosestAltBelow(c, east, 41000.0));
            Assert.AreEqual(40000.0, p.ClosestAltBelow(c, west, 43000.0));
            Assert.AreEqual(41000.0, p.ClosestAltBelow(c, east, 45000.0));
        }
    }
}