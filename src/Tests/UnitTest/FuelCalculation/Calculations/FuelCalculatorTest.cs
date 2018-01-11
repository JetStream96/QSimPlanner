using Moq;
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
            var calc = GetCalculator(NoWind, TestRoute());
            var plan = calc.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var first = plan.AllNodes[0];
            Assert.AreEqual(7040.0, first.FuelOnBoard, 10.0);
            Assert.AreEqual(62040.0, first.GrossWt, 10.0);
            Assert.AreEqual(42.2, first.TimeRemaining, 1.0);
        }

        [Test]
        public void ShortDistanceFlightShouldReCalculateAlt()
        {
            var calc = GetCalculator(NoWind, ShortTestRoute());
            var plan = calc.Create();

            // If you want to edit this, make sure the nodes are manually 
            // analyzed to make sure it's correct.
            var toc = plan.AllNodes[10];
            Assert.AreEqual(9000.0, toc.Alt, 10.0);
        }

        [Test]
        public void VeryShortRouteShouldAllowInvalidAltitude()
        {
            var calc = GetCalculator(NoWind, VeryShortTestRoute0());
            var plan = calc.Create();

            var (isValidAlt, _) = CalculationUtil.CruiseAltValid(
                new CrzAltProviderStub(), plan.AllNodes);
            Assert.IsFalse(isValidAlt);
        }

        [Test]
        public void VeryShortRouteIfElevationDifferenceTooLargeShouldStillFindResult()
        {
            var calc = GetCalculator(NoWind, VeryShortTestRoute1());
            var plan = calc.Create();

            var (isValidAlt, _) = CalculationUtil.CruiseAltValid(
                new CrzAltProviderStub(), plan.AllNodes);
            Assert.IsFalse(isValidAlt);
        }

        private static IWindTableCollection NoWind 
        {
            get
            {
                var mock = new Mock<IWindTableCollection>();

                mock.Setup(t => t.GetWindUV(It.IsAny<double>(), 
                                            It.IsAny<double>(), 
                                            It.IsAny<double>()))
                    .Returns(new WindUV(0.0, 0.0));

                return mock.Object;
            }
        }

        private static FuelCalculator GetCalculator(IWindTableCollection w, Route route)
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

        private static Route VeryShortTestRoute0()
        {
            return GetRoute(
                new Waypoint("ABCD12R", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("EFGH", 0.15, 0.15));
        }

        private static Route VeryShortTestRoute1()
        {
            return GetRoute(
                new Waypoint("ABCD12R", 0.0, 0.0), "DCT", -1.0,
                new Waypoint("EFGH", 0.1, 0.1));
        }
    }
}