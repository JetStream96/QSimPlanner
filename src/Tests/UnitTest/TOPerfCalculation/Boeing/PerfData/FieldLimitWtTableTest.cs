﻿using NUnit.Framework;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace UnitTest.TOPerfCalculation.Boeing.PerfData
{
    [TestFixture]
    public class FieldLimitWtTableTest
    {
        private const double delta = 1E-7;

        private FieldLimitWtTable table = new FieldLimitWtTable(
                new[] { 0.0, 2000.0 },
                new[] { 2000.0, 3000.0 },
                new[] { 0.0, 20.0 },
                new[] {
                    new[]
                    {
                        new[] { 200.0, 190.0 },
                        new[] { 300.0, 280.0 }
                    },
                    new[]
                    {
                        new[] { 195.0, 185.0 },
                        new[] { 290.0, 270.0 }
                    }
                });

        [Test]
        public void FieldLimitWeightTest()
        {
            Assert.AreEqual(212.5, table.FieldLimitWeight(1000.0, 2250.0, 15.0), delta);
        }

        [Test]
        public void CorrectedLengthRequiredTest()
        {
            Assert.AreEqual(2250.0, table.CorrectedLengthRequired(1000.0, 15.0, 212.5), delta);
        }
    }
}
