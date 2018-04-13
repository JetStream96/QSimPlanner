using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.WindAloft;
using UnitTest.FuelCalculation.FuelData;
using static UnitTest.FuelCalculation.Calculations.InitialPlanCreatorTest;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class ClimbNodesCreatorTest
    {
        [Test]
        public void CreateTest()
        {
            var wind = new WindUV(0.0, 0.0);
            var initPlan = GetCreator(GetWindCollectionStub(wind)).Create();

            var creator = new ClimbNodesCreator(
                TestAirportManager(),
                TestRoute(),
                FuelDataItemTest.GetItem(),
                initPlan);

            var nodes = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = nodes[0];
            Assert.AreEqual(7040.0, first.FuelOnBoard, 10.0);
            Assert.AreEqual(62040.0, first.GrossWt, 10.0);
            Assert.AreEqual(42.2, first.TimeRemaining, 1.0);
        }

        [Test]
        public void CalculatesWindEffectTest()
        {
            var wind = new WindUV(50.0, 50.0);
            var initPlan = GetCreator(GetWindCollectionStub(wind)).Create();

            var creator = new ClimbNodesCreator(
                TestAirportManager(),
                TestRoute(),
                FuelDataItemTest.GetItem(),
                initPlan);

            var nodes = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = nodes[0];
            Assert.AreEqual(6862.0, first.FuelOnBoard, 10.0);
            Assert.AreEqual(61862.0, first.GrossWt, 10.0);
            Assert.AreEqual(37.7, first.TimeRemaining, 1.0);
        }
    }
}