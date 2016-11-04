using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using static UnitTest.RouteFinding.Common;
using static UnitTest.FuelCalculation.Calculations.InitialPlanCreatorTest;
using QSP.WindAloft;
using UnitTest.FuelCalculation.FuelData;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class ClimbNodesCreatorTest
    {
        [Test]
        public void CreateTest()
        {
            var wind = new WindUV(0.0, 0.0);
            var initPlan = GetCreator(new WindCollectionStub(wind)).Create();

            var creator = new ClimbNodesCreator(
                TestAirportManager(),
                TestRoute(),
                FuelDataItemTest.GetItem(),
                initPlan);

            var nodes = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = nodes[0];
            Assert.AreEqual(37000.0, first.Alt, 0.1);
            Assert.AreEqual(6084.4, first.FuelOnBoard, 1.0);
            Assert.AreEqual(61084.4, first.GrossWt, 1.0);
            Assert.AreEqual(37.85, first.TimeRemaining, 1.0);
        }
    }
}