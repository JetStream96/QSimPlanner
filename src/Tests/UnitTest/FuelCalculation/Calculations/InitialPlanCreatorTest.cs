using System;
using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.RouteFinding.Data.Interfaces;
using QSP.WindAloft;
using UnitTest.FuelCalculation.FuelData;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class InitialPlanCreatorTest
    {
        [Test]
        public void CreateTest()
        {
            var creator = new InitialPlanCreator(
 
                new CrzAltProviderStub(),
                new CrzAltProvider(),
                new WindCollectionStub(),
                ,
                FuelDataItemTest.GetItem(),
                55000.0,
                5000.0,
                41000.0);

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