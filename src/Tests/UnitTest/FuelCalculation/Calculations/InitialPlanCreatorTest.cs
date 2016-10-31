using System;
using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.WindAloft;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class InitialPlanCreatorTest
    {
        [Test]
        public void CreateTest()
        {
          //  var creator = new InitialPlanCreator();
        }

        public class WindCollectionStub : IWindTableCollection
        {
            public WindUV GetWindUV(double lat, double lon, double altitudeFt)
            {
                return new WindUV(0.0, 0.0);
            }
        }
    }
}