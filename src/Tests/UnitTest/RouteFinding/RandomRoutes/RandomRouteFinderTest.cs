using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RandomRoutes;

namespace UnitTest.RouteFinding.RandomRoutes
{
    [TestFixture]
    public class RandomRouteFinderTest
    {
        [Test]
        public void FindTest()
        {
            var finder = FinderFactory.GetInstance();

            var result = finder.Find(
                new Waypoint("A", 10.0, 20.0),
                new Waypoint("B", 60.0, 150.0));

            for (int i = result.Count - 1; i >= 1; i--)
            {
                Assert.IsTrue(result[i].DistanceFrom(
                    result[i - 1]) <= RandomRouteFinder.MaxLegDis);
            }
        }
    }
}
