using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System.Xml.Linq;
using System;
using QSP.MathTools.Interpolation;
using static QSP.AviationTools.Constants;
using QSP.MathTools;
using static QSP.AviationTools.CoversionTools;

namespace UnitTest.TOPerfCalculation.Boeing
{
    [TestClass]
    public class TOCalculatorTest
    {
        private static BoeingPerfTable perfTable =
            new PerfDataLoader()
            .ReadTable(XDocument.Parse(PerfDataLoaderTest.PerfXml).Root);

        [TestMethod]
        public void TODistanceInterpolationTest()
        {
            var para = new TOParameters(
                    0.0,
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

            double distanceMeter = calc.TakeoffDistanceMeter();

            Assert.AreEqual(expectedDistance(para), distanceMeter, 1E-7);
        }

        private static double expectedDistance(TOParameters para)
        {
            double headWind = para.WindSpeed *
                Math.Cos(Utilities.ToRadian(para.WindHeading - para.RwyHeading));

            double pressAlt = PressureAltitudeFt(para.RwyElevationFt, para.QNH);

            double length0ft14deg = Interpolate1D.Interpolate(
                252.3, 258.8, 5550.0 * FtMeterRatio,
                5800.0 * FtMeterRatio, para.WeightKg / 1000.0);

            double length0ftM40deg = Interpolate1D.Interpolate(
                280.1, 287.3, 5550.0 * FtMeterRatio,
                5800.0 * FtMeterRatio, para.WeightKg / 1000.0);

            double length2000ft14deg = Interpolate1D.Interpolate(
                239.1, 245.3, 5550.0 * FtMeterRatio,
                5800.0 * FtMeterRatio, para.WeightKg / 1000.0);

            double length2000ftM40deg = Interpolate1D.Interpolate(
                262.6, 269.4, 5550.0 * FtMeterRatio,
                5800.0 * FtMeterRatio, para.WeightKg / 1000.0);

            double correctedDistance0ft = Interpolate1D.Interpolate(
                14.0, -40.0, length0ft14deg, length0ftM40deg, para.OatCelsius);

            double correctedDistance2000ft = Interpolate1D.Interpolate(
                14.0, -40.0, length2000ft14deg, length2000ftM40deg, para.OatCelsius);

            double correctedDis = Interpolate1D.Interpolate(
                0.0, 2000.0, correctedDistance0ft, correctedDistance2000ft, pressAlt);

            double disPreWindCorrectionM10kts = Interpolate1D.Interpolate(
                3480.0 * FtMeterRatio, 3840.0 * FtMeterRatio,
                4200.0 * FtMeterRatio, 4600.0 * FtMeterRatio, correctedDis);

            double disPreWindCorrectionM15kts = Interpolate1D.Interpolate(
                3120.0 * FtMeterRatio, 3460.0 * FtMeterRatio,
                4200.0 * FtMeterRatio, 4600.0 * FtMeterRatio, correctedDis);

            double disPreWindCorrection = Interpolate1D.Interpolate(
                -15.0, -10.0, disPreWindCorrectionM15kts,
                disPreWindCorrectionM10kts, headWind);

            double disPreSlopeCorrectionM1p5 = Interpolate1D.Interpolate(
                4330.0 * FtMeterRatio, 4760.0 * FtMeterRatio,
                4200.0 * FtMeterRatio, 4600.0 * FtMeterRatio, disPreWindCorrection);

            double disPreSlopeCorrectionM2 = Interpolate1D.Interpolate(
                4370.0 * FtMeterRatio, 4810.0 * FtMeterRatio,
                4200.0 * FtMeterRatio, 4600.0 * FtMeterRatio, disPreWindCorrection);

            double disPreSlopeCorrection = Interpolate1D.Interpolate(
                -2.0, -1.5, disPreSlopeCorrectionM2,
                disPreSlopeCorrectionM1p5, para.RwySlope);

            return disPreSlopeCorrection;
        }
    }
}
