using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding;
using QSP;
using QSP.AviationTools;

namespace Test
{

    [TestClass()]
    public class RandomRouteFinderTest
    {
        [TestMethod()]
        public void RandomRouteFinderTest1()
        {
            RandomRouteFinder x = new RandomRouteFinder(new LatLon(34.53, -118.72), new LatLon(31.5, 123.5));

            foreach (var i in x.Find())
            {
                Debug.WriteLine(i.Lat  + " " + i.Lon);
            }
        }

    }
}
