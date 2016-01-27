using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding;
using QSP.AviationTools.Coordinates;

namespace Tests
{

    [TestClass()]
    public class RandomRouteFinderTest
    {
        [TestMethod()]
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
