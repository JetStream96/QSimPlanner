using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data;
using System.Collections.Generic;

namespace UnitTest.RouteFinding.Data
{
    [TestClass]
    public class LatLonSearcherTest
    {
        [TestMethod]
        public void FindTest()
        {
            var searcher = new LatLonSearcher<Waypoint>(5, 10);
            var allPts = new List<Waypoint>();

            for (int i = -90; i <= 90; i++)
            {
                for (int j = -180; j <= 180; j++)
                {
                    var wpt = new Waypoint("", i, j);
                    allPts.Add(wpt);
                }
            }

            foreach (var k in allPts)
            {
                searcher.Add(k);
            }

            var result = searcher.Find(75.0, 120.0, 1000.0);
            var expectation = expected(allPts);

            Assert.AreEqual(expectation.Count, result.Count);

            foreach (var m in result)
            {
                Assert.IsTrue(expectation.Contains(m));
            }
        }

        private HashSet<Waypoint> expected(List<Waypoint> items)
        {
            var result = new HashSet<Waypoint>();
            var target = new Waypoint("", 75.0, 120.0);

            foreach (var i in items)
            {
                if (i.DistanceFrom(target) <= 1000.0)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        [TestMethod]
        public void RemoveTest()
        {
            var searcher = new LatLonSearcher<Waypoint>(5, 10);
            var wptToRemove = new Waypoint("", -55.0, 100.0);

            for (int i = -90; i <= 90; i++)
            {
                for (int j = -180; j <= 180; j++)
                {
                    if (i == -55 && j == 100)
                    {
                        searcher.Add(wptToRemove);
                    }
                    else
                    {
                        searcher.Add(new Waypoint("", i, j));
                    }
                }
            }

            searcher.Remove(wptToRemove);

            var result = searcher.Find(-55.0, 100.0, 300.0);

            Assert.IsFalse(result.Contains(wptToRemove));
        }
    }
}
