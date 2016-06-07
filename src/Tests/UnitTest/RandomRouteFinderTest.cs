using System.Diagnostics;
using NUnit.Framework;
using QSP.RouteFinding;
using QSP.AviationTools.Coordinates;

namespace UnitTest
{

    [TestFixture]
    public class RandomRouteFinderTest
    {
        [Test]
        public void RandomRouteFinderTest1()
        {
            RandomRouteFinder x = new RandomRouteFinder(new LatLon(25.0, 120.0), new LatLon(43.0, 107.0));

            foreach (var i in x.Find())
            {
                Debug.WriteLine(i.Lat + " " + i.Lon);
            }
        }

    }
}
