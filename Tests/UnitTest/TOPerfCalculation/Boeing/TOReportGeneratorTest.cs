using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.Core;
using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace UnitTest.TOPerfCalculation.Boeing
{
    [TestClass]
    public class TOReportGeneratorTest
    {
        private static BoeingPerfTable perfTable =
               new PerfDataLoader()
               .ReadTable(XDocument.Parse(new TestData().PerfXml).Root);

        [TestMethod]
        public void TakeOffReportTest()
        {
            var para = new TOParameters(
                   4000.0,             // rwy length
                   1000.0,             // elevation
                   210.0,              // rwy heading
                   -1.8,               // slope
                   240.0,              // wind direction
                   10.0,               // wind speed
                   4.0,                // oat
                   1000.0,             // QHN
                   false,              // surface is wet?
                   250.0 * 1000.0,     // weight kg
                   0,                  // thrust rating
                   AntiIceOption.Off,
                   true,               // packs on
                   0);                 // flaps

            var report = new TOReportGenerator(perfTable, para)
                         .TakeOffReport();

            var calc = new TOCalculator(perfTable, para);

            assertReport(report, calc, para);
        }

        private static void assertReport(TOPerfResult report,
            TOCalculator calc, TOParameters para)
        {
            // Rwy remaining
            assertRwyRemaining(report, calc, para);

            // Primary result.
            var primary = report.PrimaryResult;
            Assert.AreEqual(para.OatCelsius, primary.OatCelsius, 0.5);

            double expectedDis = calc.TakeoffDistanceMeter(primary.OatCelsius);
            Assert.AreEqual(expectedDis, primary.RwyRequiredMeter, 0.5);

            // Assumed temperatures
            foreach (var i in report.AssumedTemp)
            {
                expectedDis = calc.TakeoffDistanceMeter(i.OatCelsius);
                Assert.AreEqual(expectedDis, i.RwyRequiredMeter, 0.5);
            }
        }

        private static void assertRwyRemaining(TOPerfResult report,
            TOCalculator calc, TOParameters para)
        {
            var primary = report.PrimaryResult;
            double expectedRemaining = para.RwyLengthMeter -
                calc.TakeoffDistanceMeter(primary.OatCelsius);

            Assert.AreEqual(expectedRemaining, primary.RwyRemainingMeter, 0.5);

            foreach (var i in report.AssumedTemp)
            {
                expectedRemaining = para.RwyLengthMeter -
                calc.TakeoffDistanceMeter(i.OatCelsius);

                Assert.AreEqual(expectedRemaining, i.RwyRemainingMeter, 0.5);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RunwayTooShortException))]
        public void WhenRwyIsTooShortShouldThrowException()
        {
            var para = new TOParameters(
                   0.0,                // rwy length
                   1000.0,             // elevation
                   210.0,              // rwy heading
                   -1.8,               // slope
                   240.0,              // wind direction
                   10.0,               // wind speed
                   4.0,                // oat
                   1000.0,             // QHN
                   false,              // surface is wet?
                   250.0 * 1000.0,     // weight kg
                   0,                  // thrust rating
                   AntiIceOption.Off,
                   true,               // packs on
                   0);                 // flaps

            var report = new TOReportGenerator(perfTable, para)
                         .TakeOffReport();
        }

        [TestMethod]
        [ExpectedException(typeof(PoorClimbPerformanceException))]
        public void WhenRwyIsEnoughButUnableToClimbShouldThrowException()
        {
            var para = new TOParameters(
                   4000.0,             // rwy length
                   1000.0,             // elevation
                   210.0,              // rwy heading
                   -1.8,               // slope
                   240.0,              // wind direction
                   10.0,               // wind speed
                   4.0,                // oat
                   1000.0,             // QHN
                   false,              // surface is wet?
                   250.0 * 1000.0,     // weight kg
                   0,                  // thrust rating
                   AntiIceOption.Off,
                   true,               // packs on
                   0);                 // flaps

            var calc = new TOCalculator(perfTable, para);
            double wt = calc.ClimbLimitWeightTon();

            // Make it heavier than climb limit weight.
            setProperty(para, "WeightKg", wt * 1000.0 + 1.0);

            var report = new TOReportGenerator(perfTable, para)
                         .TakeOffReport();

        }

        private static void setProperty(object target, string propertyName,
            object propertyValue)
        {
            Type type = target.GetType();
            PropertyInfo prop = type.GetProperty(propertyName);
            prop.SetValue(target, propertyValue, null);
        }
    }
}
