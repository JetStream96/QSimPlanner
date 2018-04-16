using NUnit.Framework;
using QSP.NavData.AAX;
using QSP.RouteFinding.Navaids;
using static QSP.LibraryExtension.Types;

namespace UnitTest.NavData.AAX
{
    [TestFixture]
    public class NavaidsLoaderTest
    {
        [Test]
        public void LoadTest()
        {
            var lines = List(
                "1A,WILLIAMS HARBOUR,373.000,0,0,110,52.55938,-55.78192,0,CY,0,1,0",
                "1B,SABLE ISLAND,277.000,0,0,110,43.93056,-60.02278,0,CY,0,1,0",
                "1D,CHARLOTTETOWN,346.000,0,0,110,52.77541,-56.12596,0,CY,0,1,0",
                "");

            var navaids = NavaidsLoader.Load(lines);
            var x = new Navaid()
            {
                ID = "1A",
                Name = "WILLIAMS HARBOUR",
                Freq = "373.000",
                IsVOR = false,
                IsDME = false,
                RangeNm = 110,
                Lat = 52.55938,
                Lon = -55.78192,
                CountryCode = "CY"
            };

            Assert.AreEqual(3, navaids.Count);
            Assert.AreEqual(x, navaids["1A"]);
            Assert.AreEqual(43.93056, navaids["1B"].Lat);
            Assert.AreEqual("346.000", navaids["1D"].Freq);
        }
    }
}
