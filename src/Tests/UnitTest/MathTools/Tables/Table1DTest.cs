using NUnit.Framework;
using QSP.MathTools.Tables;
using System;

namespace UnitTest.MathTools.Tables
{
    [TestFixture]
    public class Table1DTest
    {
        [Test]
        public void ValidateTest()
        {
            var table = new Table1D(new[] { 3.0, 4.0, 5.0 },
                new[] { 8.0, -7.5, 1.35 });

            table.Validate();
        }

        [Test]
        public void TableTooSmallNotValid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var table = new Table1D(
                        new[] { 3.0, 4.0, 5.0 },
                        new[] { 8.0, -7.5 });

                table.Validate();
            });
        }
    }
}
