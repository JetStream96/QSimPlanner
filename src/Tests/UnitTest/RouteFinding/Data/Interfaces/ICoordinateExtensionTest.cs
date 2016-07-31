using NUnit.Framework;
using QSP.RouteFinding.Data.Interfaces;
using System;
using static QSP.MathTools.GCDis;

namespace UnitTest.RouteFinding.Data.Interfaces
{
    [TestFixture]    
    public class ICoordinateExtensionTest
    {
        private const double delta = 1E-8;

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

            var expected =
                Distance(pts[0].Lat, pts[0].Lon, pts[1].Lat, pts[1].Lon) +
                Distance(pts[1].Lat, pts[1].Lon, pts[2].Lat, pts[2].Lon);

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

            var expected =
                Distance(pts[0].Lat, pts[0].Lon, pts[1].Lat, pts[1].Lon);

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
