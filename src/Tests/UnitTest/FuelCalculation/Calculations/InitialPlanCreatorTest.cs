using FakeItEasy;
using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
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
            var wind = new WindUV(0.0, 0.0);
            var creator = GetCreator(GetWindCollectionStub(wind));

            var nodes = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = nodes[0];
            Assert.AreEqual(37000.0, first.Alt, 0.1);
            Assert.AreEqual(6089.0, first.FuelOnBoard, 10.0);
            Assert.AreEqual(61089.0, first.GrossWt, 10.0);
            Assert.AreEqual(38.0, first.TimeRemaining, 1.0);
        }

        [Test]
        public void CalculatesWindEffectTest()
        {
            var wind = new WindUV(50.0, 50.0);
            var creator = GetCreator(GetWindCollectionStub(wind));

            var nodes = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = nodes[0];
            Assert.AreEqual(37000.0, first.Alt, 0.1);
            Assert.AreEqual(5924.0, first.FuelOnBoard, 10.0);
            Assert.AreEqual(60924.0, first.GrossWt, 10.0);
            Assert.AreEqual(33.8, first.TimeRemaining, 1.0);
        }

        public static InitialPlanCreator GetCreator(IWxTableCollection w)
        {
            return new InitialPlanCreator(
                TestAirportManager(),
                new CrzAltProviderStub(),
                w,
                TestRoute(),
                FuelDataItemTest.GetItem(),
                55000.0,
                5000.0,
                41000.0);
        }

        public static Route TestRoute()
        {
            return GetRoute(
                new Waypoint("ABCD12R", 0.0, 0.0), "SID", -1.0,
                new Waypoint("A", 1.0, 0.0), "1", -1.0,
                new Waypoint("B", 1.0, 2.0), "STAR", -1.0,
                new Waypoint("EFGH", 2.0, 3.0));
        }

        public static AirportManager TestAirportManager()
        {
            var abcd = GetAirport("ABCD", 0);
            var efgh = GetAirport("EFGH", 3000);
            return GetAirportManager(abcd, efgh);
        }

        private static IAirport GetAirport(string icao, int alt)
        {
            var airport = A.Fake<IAirport>();
            A.CallTo(() => airport.Icao).Returns(icao);
            A.CallTo(() => airport.Elevation).Returns(alt);
            return airport;
        }

        public static IWxTableCollection GetWindCollectionStub(WindUV wind)
        {
            var s = A.Fake<IWxTableCollection>();
            A.CallTo(() => s.GetWindUV(A<double>.Ignored,A<double>.Ignored,A<double>.Ignored))
             .Returns(wind);
            return s;
        }

        public class CrzAltProviderStub : ICrzAltProvider
        {
            public double ClosestAlt(ICoordinate c, double heading, double altitude) =>
                Math.Round(altitude / 1000.0) * 1000.0;

            public double ClosestAltBelow(ICoordinate c, double heading, double altitude) =>
                Math.Floor(altitude / 1000.0) * 1000.0;

            public bool IsValidCrzAlt(ICoordinate c, double heading, double altitude) =>
                altitude.Mod(1000.0) < 1.0;
        }
    }
}