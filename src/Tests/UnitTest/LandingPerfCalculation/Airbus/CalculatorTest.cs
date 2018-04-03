using NUnit.Framework;
using QSP.AviationTools;
using QSP.Common;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Airbus;
using System.Linq;

namespace UnitTest.LandingPerfCalculation.Airbus
{
    [TestFixture]
    public class CalculatorTest
    {
        private static LandingParameters GetParameters => new LandingParameters()
        {
            WeightKG = 88106 * Constants.LbKgRatio,
            RwyLengthMeter = 3000,
            ElevationFT = 0,
            HeadwindKts = 0,
            SlopePercent = 0,
            TempCelsius = 15,
            QNH = 1013.25,
            AppSpeedIncrease = 0,
            Reverser = 0,
            SurfaceCondition = 0,
            FlapsIndex = 0,
            BrakeIndex = 0
        };

        [Test]
        public void NoCorrection()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2460 * Constants.FtMeterRatio, dis, 10);
        }

        [Test]
        public void ElevationCorrectionDry()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.ElevationFT = 1000;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2460 * Constants.FtMeterRatio * 1.03, dis, 10);
        }

        [Test]
        public void ElevationCorrectionWet()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.SurfaceCondition = 1;
            d.Parameters.ElevationFT = -1000;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2952 * Constants.FtMeterRatio * 0.97, dis, 10);
        }

        [Test]
        public void HeadwindCorrectionDry()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.HeadwindKts = 10;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2460 * Constants.FtMeterRatio, dis, 10);
        }

        [Test]
        public void HeadwindCorrectionWet()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.HeadwindKts = 10;
            d.Parameters.SurfaceCondition = 1;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2952 * Constants.FtMeterRatio, dis, 10);
        }

        [Test]
        public void TailwindCorrectionDry()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.HeadwindKts = -10;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2460 * Constants.FtMeterRatio * 1.20, dis, 10);
        }

        [Test]
        public void TailwindCorrectionWet()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.HeadwindKts = -10;
            d.Parameters.SurfaceCondition = 1;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2952 * Constants.FtMeterRatio * 1.26, dis, 10);
        }

        [Test]
        public void BothReversersCorrectionDry()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.Reverser = 1;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2460 * Constants.FtMeterRatio * 0.97, dis, 10);
        }

        [Test]
        public void BothReversersCorrectionWet()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.Reverser = 1;
            d.Parameters.SurfaceCondition = 1;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2952 * Constants.FtMeterRatio * 0.94, dis, 10);
        }

        [Test]
        public void SpeedCorrection()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.AppSpeedIncrease = 5;
            var dis = Calculator.LandingDistanceMeter(d);
            Assert.AreEqual(2460 * Constants.FtMeterRatio * 1.08, dis, 10);
        }

        [Test]
        public void LandingReportTest()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            var r = Calculator.LandingReport(d);

            Assert.IsTrue(r.AllBrakes.All(x => x.RemainingDistanceMeter >= 0));
            Assert.IsTrue(r.SelectedBrake.RemainingDistanceMeter >= 0);
        }

        [Test]
        public void LandingReportRunwayTooShort()
        {
            var d = new CalculatorData()
            {
                Table = LoaderTest.GetTable(),
                Parameters = GetParameters
            };

            d.Parameters.RwyLengthMeter = 100;
            Assert.Throws<RunwayTooShortException>(() => Calculator.LandingReport(d));
        }
    }
}
