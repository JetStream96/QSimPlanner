using NUnit.Framework;
using QSP.RouteFinding.Data.Interfaces;
using System;

namespace UnitTest.RouteFinding.Data.Interfaces
{
    [TestFixture]    
    public class ICoordinateExtensionTest
    {
        private const double delta = 1E-8;

        [Test]
        public void LatLonEqualsTest()
        {
            var p1 = new pt(2.0, 80.0);
            var p2 = new pt(2.05, 80.0);

            Assert.IsFalse(p1.LatLonEquals(p2));
            Assert.IsFalse(p1.LatLonEquals(p2, 0.03));
            Assert.IsTrue(p1.LatLonEquals(p2, 0.06));
        }

        [Test]
        public void GetClosestEmptyCollectionShouldThrow()
        {
            var items = new pt[0];

            Assert.Throws<InvalidOperationException>(() =>
            items.GetClosest(0.0, 0.0));
        }

        [Test]
        public void GetClosestTest()
        {
            var items = new pt[] 
            {
                new pt(10.0, 20.0),
                new pt(20.0, 20.0),
                new pt(30.0, 20.0),
                new pt(40.0, 20.0),
                new pt(50.0, 20.0),
                new pt(60.0, 20.0),
                new pt(70.0, 20.0),
            };

            var p = items.GetClosest(42.0, 20.0);
            Assert.AreEqual(items[3], p);
        }

        [Test]
        public void TotalDistance3Pts()
        {
            var pts = new pt[]
            {
                new pt(0.0,3.5),
                new pt(-15.0,31.5),
                new pt(85.5,160.0)
            };

            var expected = pts[0].Distance(pts[1]) + pts[1].Distance(pts[2]);

            Assert.AreEqual(expected, pts.TotalDistance(), delta);
        }

        [Test]
        public void TotalDistance2Pts()
        {
            var pts = new pt[]
            {
                new pt(0.0,3.5),
                new pt(-15.0,31.5)
            };

            var expected = pts[0].Distance(pts[1]);

            Assert.AreEqual(expected, pts.TotalDistance(), delta);
        }

        [Test]
        public void TotalDistance1Pt()
        {
            var pts = new pt[]
            {
                new pt(0.0,3.5)
            };

            Assert.AreEqual(0.0, pts.TotalDistance(), delta);
        }

        [Test]
        public void TotalDistance0Pt()
        {
            var pts = new pt[] { };
            Assert.AreEqual(0.0, pts.TotalDistance(), delta);
        }

        [Test]
        public void ToRouteTest()
        {
            pt[] pts =
            {
                new pt(5.0, 10.0),
                new pt(15.0, 20.5)
            };

            var route = pts.ToRoute();

            Assert.AreEqual(2, route.Count);

            var pt0 = route.FirstWaypoint;
            Assert.IsTrue(pt0.LatLonEquals(pts[0]));
            Assert.IsTrue(route.FirstNode.Value.AirwayToNext == "DCT");

            var pt1 = route.FirstNode.Next.Value.Waypoint;
            Assert.IsTrue(pt1.LatLonEquals(pts[1]));
        }

        private class pt : ICoordinate
        {
            public double Lat { get; private set; }
            public double Lon { get; private set; }

            public pt(double Lat, double Lon)
            {
                this.Lat = Lat;
                this.Lon = Lon;
            }
        }
    }
}
