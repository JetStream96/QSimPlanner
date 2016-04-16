using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.MathTools.Tables;
using System;

namespace UnitTest.MathTools.Tables
{
    [TestClass]
    public class Table1DTest
    {
        [TestMethod]
        public void ValidateTest()
        {
            var table = new Table1D(new double[] { 3.0, 4.0, 5.0 },
                new double[] { 8.0, -7.5, 1.35 });

            table.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableTooSmallNotValid()
        {
            var table = new Table1D(new double[] { 3.0, 4.0, 5.0 },
                new double[] { 8.0, -7.5 });

            table.Validate();
        }
    }
}
