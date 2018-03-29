using NUnit.Framework;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System;
using System.Xml.Linq;
using QSP.TOPerfCalculation;
using static QSP.AviationTools.ConversionTools;
using static QSP.MathTools.Angles;

namespace UnitTest.TOPerfCalculation.Boeing
{
    [TestFixture]
    public class TOCalculatorTest
    {
        private static BoeingPerfTable perfTable = new PerfDataLoader()
            .ReadTable(XDocument.Parse(new TestData().PerfXml).Root);

        [Test]
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

            Assert.AreEqual(ExpectedDistance1(para),
                distanceMeter, 1E-7);
        }

        private static double ExpectedDistance1(TOParameters para)
        {
            return ExpectedDistance1(para, para.WeightKg / 1000.0);
        }

        private static double ExpectedDistance1(TOParameters para, double wtTon)
        {
            double headWind = para.WindSpeedKnots *
                Math.Cos(ToRadian(para.WindHeading - para.RwyHeading));

            double pressAlt = PressureAltitudeFt(para.RwyElevationFt, para.QNH);
            var table = perfTable.Tables[para.FlapsIndex];

            double correctedLength = table.WeightTableDry.CorrectedLengthRequired(
                pressAlt, para.OatCelsius, wtTon);

            double slopeCorrectedLength = table.WindCorrDry.SlopeCorrectedLength(
                headWind, correctedLength);

            return table.SlopeCorrDry.FieldLengthRequired(
                para.RwySlopePercent, slopeCorrectedLength);
        }

        [Test]
        public void TODistancePackAiCorrectionTest()
        {
            var para = new TOParameters(
                    0.0,
                    1000.0,                 // elevation
                    210.0,                  // rwy heading
                    -1.8,                   // slope
                    240.0,                  // wind direction
                    10.0,                   // wind speed
                    4.0,                    // oat
                    1000.0,                 // QHN
                    true,                   // surface is wet?
                    250.0 * 1000.0,         // weight kg
                    0,                      // thrust rating
                    AntiIceOption.EngAndWing,
                    false,                  // packs on
                    0);                     // flaps

            var calc = new TOCalculator(perfTable, para);

            double distanceMeter = calc.TakeoffDistanceMeter();

            Assert.AreEqual(ExpectedDistance2(para),
                distanceMeter, 1E-7);
        }

        private static double ExpectedDistance2(TOParameters para)
        {
            double headWind = para.WindSpeedKnots *
                Math.Cos(ToRadian(para.WindHeading - para.RwyHeading));

            double pressAlt = PressureAltitudeFt(para.RwyElevationFt, para.QNH);
            double wtTon = (para.WeightKg + 2200.0 - 500.0) / 1000.0;

            var table = perfTable.Tables[para.FlapsIndex];

            double correctedLength = table.WeightTableWet.CorrectedLengthRequired(
                pressAlt, para.OatCelsius, wtTon);

            double slopeCorrectedLength = table.WindCorrWet.SlopeCorrectedLength(
                headWind, correctedLength);

            return table.SlopeCorrWet.FieldLengthRequired(
                para.RwySlopePercent, slopeCorrectedLength);
        }

        [Test]
        public void TODistanceDerateTest()
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
                   2,                  // thrust rating
                   AntiIceOption.Off,
                   true,               // packs on
                   0);                 // flaps

            var calc = new TOCalculator(perfTable, para);

            double distanceMeter = calc.TakeoffDistanceMeter();

            Assert.AreEqual(
                ExpectedDistance1(
                    para,
                    perfTable.Tables[para.FlapsIndex]
                    .AlternateThrustTables[para.ThrustRating - 1]
                    .EquivalentFullThrustWeight(
                        para.WeightKg / 1000.0,
                        AlternateThrustTable.TableType.Dry)),
                distanceMeter, 1E-7);
        }

        [Test]
        public void FieldLimitWtTest()
        {
            var para = new TOParameters(
                   2500.0,             // rwy length
                   1000.0,             // elevation
                   210.0,              // rwy heading
                   -1.8,               // slope
                   240.0,              // wind direction
                   10.0,               // wind speed
                   4.0,                // oat
                   1000.0,             // QHN
                   false,              // surface is wet?
                   250.0 * 1000.0,     // weight kg
                   2,                  // thrust rating
                   AntiIceOption.Off,
                   true,               // packs on
                   0);                 // flaps

            var calc = new TOCalculator(perfTable, para);

            double limitWt = calc.FieldLimitWeightTon();

            double expectedLimitWt = perfTable.Tables[para.FlapsIndex]
                    .AlternateThrustTables[para.ThrustRating - 1]
                    .CorrectedLimitWeight(ExpectedLimitWt1(para),
                    AlternateThrustTable.TableType.Dry);

            Assert.AreEqual(expectedLimitWt, limitWt, 1E-7);
        }

        private static double ExpectedLimitWt1(TOParameters para)
        {
            var table = perfTable.Tables[para.FlapsIndex];
            double pressAlt = PressureAltitudeFt(para.RwyElevationFt, para.QNH);
            double windSpd = para.WindSpeedKnots *
                Math.Cos(ToRadian(para.WindHeading - para.RwyHeading));

            double slopeCorrectedLength = table.SlopeCorrDry.CorrectedLength(
                para.RwyLengthMeter, para.RwySlopePercent);

            double windCorrectedLength = table.WindCorrDry.CorrectedLength(
                slopeCorrectedLength, windSpd);

            return table.WeightTableDry.FieldLimitWeight(
                pressAlt, windCorrectedLength, para.OatCelsius);
        }

        [Test]
        public void ClimbLimitWtTest()
        {
            var para = new TOParameters(
                   2500.0,             // rwy length
                   1000.0,             // elevation
                   210.0,              // rwy heading
                   -1.8,               // slope
                   240.0,              // wind direction
                   10.0,               // wind speed
                   4.0,                // oat
                   1000.0,             // QHN
                   false,              // surface is wet?
                   250.0 * 1000.0,     // weight kg
                   2,                  // thrust rating
                   AntiIceOption.Off,
                   false,              // packs on
                   0);                 // flaps

            var calc = new TOCalculator(perfTable, para);

            double distanceMeter = calc.ClimbLimitWeightTon();

            double wtWithPackCorrection = ExpectedClimbLimit1(para) + 1.7;

            double expectedLimitWt = perfTable.Tables[para.FlapsIndex]
                .AlternateThrustTables[para.ThrustRating - 1]
                .CorrectedLimitWeight(wtWithPackCorrection, AlternateThrustTable.TableType.Climb);

            Assert.AreEqual(expectedLimitWt, distanceMeter, 1E-7);
        }

        private static double ExpectedClimbLimit1(TOParameters para)
        {
            var table = perfTable.Tables[para.FlapsIndex];
            double pressAlt = PressureAltitudeFt(para.RwyElevationFt, para.QNH);
            return table.ClimbLimitWt.ClimbLimitWeight(pressAlt, para.OatCelsius);
        }
    }
}
