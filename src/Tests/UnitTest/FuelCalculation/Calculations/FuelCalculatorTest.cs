using NUnit.Framework;
using QSP.FuelCalculation.Calculations;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using UnitTest.FuelCalculation.FuelData;
using static UnitTest.FuelCalculation.Calculations.InitialPlanCreatorTest;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.FuelCalculation.Calculations
{
    [TestFixture]
    public class FuelCalculatorTest
    {
        [Test]
        public void CreateTest()
        {
            var wind = new WindUV(0.0, 0.0);
            var creator = GetCalculator(
                new WindCollectionStub(wind), TestRoute());

            var plan = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = plan.AllNodes[0];
            Assert.AreEqual(6971.0, first.FuelOnBoard, 10.0);
            Assert.AreEqual(61971.0, first.GrossWt, 10.0);
            Assert.AreEqual(41.1, first.TimeRemaining, 1.0);
        }

        [Test]
        public void ShortDistanceFlightShouldReCalculateAlt()
        {
            var wind = new WindUV(0.0, 0.0);
            var creator = GetCalculator(
                new WindCollectionStub(wind), ShortTestRoute());

            var plan = creator.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var toc = plan.AllNodes[10];
            Assert.AreEqual(9000.0, toc.Alt, 10.0);
        }

        private static FuelCalculator GetCalculator(
            IWindTableCollection w,
            Route route)
        {
            return new FuelCalculator(
                TestAirportManager(),
                new CrzAltProviderStub(),
                w,
                route,
                FuelDataItemTest.GetItem(),
                55000.0,
                5000.0,
                41000.0);
        }

        private static Route ShortTestRoute()
        {
            return GetRoute(
                new Waypoint("ABCD12R", 0.0, 0.0), "SID", -1.0,
                new Waypoint("A", 0.25, 0.25), "STAR", -1.0,
                new Waypoint("EFGH", 0.5, 0.5));
        }
    }
}