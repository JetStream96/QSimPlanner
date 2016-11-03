using System;
using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.WindAloft;
using UnitTest.FuelCalculation.FuelData;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class InitialPlanCreatorTest
    {
        [Test]
        public void CreateTest()
        {
            var abcd = GetAirport("ABCD", 0.0);
            var efgh = GetAirport("EFGH", 3000.0);
            var route = GetRoute(
                new Waypoint("ABCD12R", 0.0, 0.0), "SID", -1.0,
                new Waypoint("A", 1.0, 0.0), "1", -1.0,
                new Waypoint("B", 1.0, 2.0), "STAR", -1.0,
                new Waypoint("EFGH", 2.0, 3.0));

            var creator = new InitialPlanCreator(
                GetAirportManager(abcd, efgh),
                new CrzAltProviderStub(),
                new WindCollectionStub(),
                route,
                FuelDataItemTest.GetItem(),
                55000.0,
                5000.0,
                41000.0);

            var nodes = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = nodes[0];
            Assert.AreEqual(37000.0, first.Alt, 0.1);
            Assert.AreEqual(6084.4, first.FuelOnBoard, 1.0);
            Assert.AreEqual(61084.4, first.GrossWt, 1.0);
            Assert.AreEqual(37.85, first.TimeRemaining, 1.0);
        }

        private static Airport GetAirport(string icao, double alt)
        {
            return new Airport(icao, null, 0.0, 0.0, (int)alt, 
                false, 0, 0, 0, null);
        }

        public class WindCollectionStub : IWindTableCollection
        {
            public WindUV GetWindUV(double lat, double lon, double altitudeFt)
            {
                return new WindUV(0.0, 0.0);
            }
        }

        public class CrzAltProviderStub : ICrzAltProvider
        {
            public double ClosestAltitudeFtFrom(ICoordinate previous, 
                ICoordinate current, double altitude)
            {
                return Math.Round(altitude / 1000.0) * 1000.0;
            }

            public double ClosestAltitudeFtTo(ICoordinate current, 
                ICoordinate next, double altitude)
            {
                return Math.Round(altitude / 1000.0) * 1000.0;
            }
        }

    }
}