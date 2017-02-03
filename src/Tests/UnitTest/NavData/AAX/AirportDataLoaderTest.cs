using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using System.Linq;

namespace UnitTest.NavData.AAX
{
    [TestFixture]
    public class AirportDataLoaderTest
    {
        [Test]
        public void LoadTest()
        {
            var txt = @"
A,ABCD,NAME1,1.0,2.0,100,18000,18000,3000,0
R,09,95,3000,150,0,0.0,0,1.0,2.0,100,3.00,50,1,0
R,27,275,3000,150,0,0.0,0,1.0,2.0,100,3.00,50,1,0

A,EFGH,NAME2,1.0,2.0,100,3000,5000,2500,0
R,36,003,2500,100,0,0.0,0,1.0,2.0,100,3.00,50,1,0
R,18,183,2500,100,0,0.0,0,1.0,2.0,100,3.00,50,1,0
";

            var airports = AirportDataLoader.Load(txt.Lines());

            Assert.AreEqual(2, airports.Count);

            var ap1 = airports["ABCD"];
            var rwy09 = new RwyData("09",  "95", 3000, 150, true, false, "0.0", "0", 1.0, 2.0, 
                100, 3.0, 50, AirportDataLoader.SurfTypes[1], 0);
            var rwy27 = new RwyData("27", "275", 3000, 150, true, false, "0.0", "0", 1.0, 2.0, 
                100, 3.0, 50, AirportDataLoader.SurfTypes[1], 0);

            var expected1 = new Airport("ABCD", "NAME1", 1.0, 2.0, 100, true, 18000, 18000, 3000, 
                new [] {rwy09, rwy27});

            Assert.IsTrue(expected1.Equals(ap1));

            var ap2 = airports["EFGH"];
            Assert.AreEqual("NAME2",ap2.Name);
            Assert.IsTrue(ap2.Rwys.Select(r => r.RwyIdent).SetEquals("18", "36"));
        }
    }
}