using System.Linq;
using NUnit.Framework;
using QSP.RouteFinding.Airports;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFinding.Airports
{
    [TestFixture]
    public class AirportManagerTest
    {
        [Test]
        public void FindRemoveTest()
        {
            var airport = TestAirport();            
            var col = new AirportManager();
            col.Add(airport);

            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col["ABCD"].Equals(airport));

            Assert.IsFalse(col.Remove("XYZ"));
            Assert.AreEqual(1, col.Count);

            Assert.IsTrue(col.Remove("ABCD"));
            Assert.AreEqual(0, col.Count);
            Assert.IsNull(col["ABCD"]);
        }

        [Test]
        public void FindTest()
        {
            var airport = TestAirport();
            var col = new AirportManager();
            col.Add(airport);

            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col["ABCD"].Equals(airport));
        }

        [Test]
        public void FindRwysTest()
        {
            var col = TestCollection();
            var rwys = col.RwyIdents("ABCD").ToList();

            Assert.AreEqual(2, rwys.Count);
            Assert.IsTrue(rwys.Contains("01") && rwys.Contains("19"));
        }

        [Test]
        public void FindRwyTest()
        {
            var col = TestCollection();
            var latLon = col.FindRwy("ABCD", "01");

            Assert.AreEqual(1.0, latLon.Lat, 1E-8);
            Assert.AreEqual(1.0, latLon.Lon, 1E-8);

            Assert.IsNull(col.FindRwy("ABCD", "05"));
        }

        [Test]
        public void WhenCannotFindRwysShouldReturnNull()
        {
            var col = new AirportManager();

            Assert.IsNull(col["ABCD"]);
            Assert.IsNull(col.FindRwy("ABCD", "01"));
            Assert.IsNull(col.RwyIdents("ABCD"));
        }
        
        private AirportManager TestCollection()
        {
            var col = new AirportManager();
            col.Add(TestAirport());
            return col;
        }

        private Airport TestAirport()
        {
            return GetAirport("ABCD",
                GetRwyData("01", 1.0, 1.0),
                GetRwyData("19", 1.0, 1.0));
        }
    }
}
