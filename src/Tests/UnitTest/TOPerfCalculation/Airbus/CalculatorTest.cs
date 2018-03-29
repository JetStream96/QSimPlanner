using NUnit.Framework;
using QSP.AviationTools;
using QSP.TOPerfCalculation.Airbus;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using System.Linq;

namespace UnitTest.TOPerfCalculation.Airbus
{
    [TestFixture]
    public class CalculatorTest
    {
        public static Parameters GetParameters() => new Parameters()
        {
            AntiIce = 0,
            Flaps = "1+F",
            OatCelsius = 15,
            PacksOn = false,
            QNH = 1013.25,
            RwyElevationFt = 0,
            RwyHeading = 0,
            RwyLengthMeter = 3000,
            RwySlopePercent = 0,
            SurfaceWet = false,
            WeightKg = 152_000 * Constants.LbKgRatio,
            WindHeading = 0,
            WindSpeedKnots = 0
        };

        [Test]
        public void NoMatchingFlaps()
        {
            var p = GetParameters();
            p.Flaps = "2";

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(Calculator.Error.NoDataForSelectedFlaps, e);
        }

        [Test]
        public void HasMatchingFlaps()
        {
            var p = GetParameters();
            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(Calculator.Error.None, e);
        }

        [Test]
        public void Only1TableNoCorrection()
        {
            var p = GetParameters();

            var t = LoaderTest.GetTable();
            t.Tables = t.Tables.Where(x => x.IsaOffset < 1).ToList();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(5000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void WetCorrection()
        {
            var p = GetParameters();
            p.WeightKg -= 2273 * Constants.LbKgRatio;
            p.SurfaceWet = true;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(5000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void TwoTablesIsaOffset()
        {
            var p = GetParameters();
            p.OatCelsius += 7.5;
            p.WeightKg = (139_000 + 132_000) / 2.0 * Constants.LbKgRatio;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(4000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void NonZeroAltitude()
        {
            var p = GetParameters();
            p.RwyElevationFt = 2000;
            p.OatCelsius = 15 - 1.98 * 2;
            p.WeightKg = 146_000 * Constants.LbKgRatio;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(5000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void UphillCorrection()
        {
            var p = GetParameters();
            p.RwySlopePercent = 1.0;
            p.WeightKg = (139_000 * 80 + 152_000 * 920) / 1000.0 * Constants.LbKgRatio;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual((4920 + 524.8) * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void DownhillCorrection()
        {
            var p = GetParameters();
            p.RwySlopePercent = -1.0;
            p.WeightKg = (139_000 * 80 + 152_000 * 920) / 1000.0 * Constants.LbKgRatio;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual((4920 - 55.76) * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void HeadwindCorrection()
        {
            var p = GetParameters();
            p.WindSpeedKnots = 10;

            p.WeightKg = (139_000 * 80 + 152_000 * 920) / 1000.0 * Constants.LbKgRatio;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual((4920 - 10 * 21.32) * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void TailwindCorrection()
        {
            var p = GetParameters();
            p.WindSpeedKnots = 10;
            p.WindHeading = 180;
            p.WeightKg = (139_000 * 80 + 152_000 * 920) / 1000.0 * Constants.LbKgRatio;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual((4920 + 10 * 74.62) * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void EngineAICorrection()
        {
            var p = GetParameters();
            p.WeightKg -= 555.6 * Constants.LbKgRatio;
            p.AntiIce = 1;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(5000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void AllAICorrection()
        {
            var p = GetParameters();
            p.WeightKg -= 1666.7 * Constants.LbKgRatio;
            p.AntiIce = 2;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(5000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void PacksOnCorrection()
        {
            var p = GetParameters();
            p.WeightKg -= 4888.9 * Constants.LbKgRatio;
            p.PacksOn = true;

            var t = LoaderTest.GetTable();

            var (e, d) = Calculator.TakeOffDistanceMeter(t, p);
            Assert.AreEqual(5000 * Constants.FtMeterRatio, d, 10);
        }

        [Test]
        public void TakeOffReportShouldHaveAssumedTemp()
        {
            var p = GetParameters();
            var t = LoaderTest.GetTable();

            var (e, r) = Calculator.TakeOffReport(t, p);
            Assert.AreEqual(Calculator.Error.None, e);
        }

        [Test]
        public void TakeOffReportFlapsNotFound()
        {
            var p = GetParameters();
            p.Flaps = "2";
            var t = LoaderTest.GetTable();

            var (e, r) = Calculator.TakeOffReport(t, p);
            Assert.AreEqual(Calculator.Error.NoDataForSelectedFlaps, e);
        }

        [Test]
        public void TakeOffReportRunwayTooShort()
        {
            var p = GetParameters();
            p.RwyLengthMeter = 500;
            var t = LoaderTest.GetTable();

            var (e, r) = Calculator.TakeOffReport(t, p);
            Assert.AreEqual(Calculator.Error.RunwayTooShort, e);
        }
    }
}
