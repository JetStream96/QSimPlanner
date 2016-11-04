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
            Assert.AreEqual(6958.7, first.FuelOnBoard, 1.0);
            Assert.AreEqual(61958.7, first.GrossWt, 1.0);
            Assert.AreEqual(40.93, first.TimeRemaining, 1.0);
        }

        [Test]
        public void CalculatesWindEffectTest()
        {
            var wind = new WindUV(50.0, 50.0);
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
            Assert.AreEqual(6790.2, first.FuelOnBoard, 1.0);
            Assert.AreEqual(61790.2, first.GrossWt, 1.0);
            Assert.AreEqual(36.51, first.TimeRemaining, 1.0);
        }
    }
}