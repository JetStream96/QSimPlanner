using NUnit.Framework;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
using System;

namespace UnitTest.MathTools.Tables.Readers
{
    [TestFixture]
    public class TableReader1DTest
    {
        private const double delta = 1E-7;

        private string format1 =
              @"
              1500 51.2 
              1600 52.8 
              1800 55.9
              ";

        private string format2 =
              @"1500 51.2
              1600 52.8
              1800 55.9";

        private string format3 =
              @"
              1500 51.2 
              1600 52.8 

              1800 55.9
              ";

        [Test]
        public void ReadTest1()
        {
            AssertTable(format1);
        }

        [Test]
        public void ReadTest2()
        {
            AssertTable(format2);
        }

        [Test]
        public void ReadTest3()
        {
            AssertTable(format3);
        }

        [Test]
        public void ReadCustomParser()
        {
            string s =
                @"10 3
                  15 9";

            Func<string, double> customParser = (str) =>
            {
                var x = double.Parse(str);
                return x * x;
            };

            var result = TableReader1D.Read(s, double.Parse, customParser);

            var expected = new Table1D(
               new [] { 10.0, 15.0 },
               new [] { 9.0, 81.0 });

            Assert.IsTrue(result.Equals(expected, delta));
        }

        private void AssertTable(string source)
        {
            var table = TableReader1D.Read(source);

            var expected = new Table1D(
                new [] { 1500.0, 1600.0, 1800.0 },
                new [] { 51.2, 52.8, 55.9 });

            Assert.IsTrue(table.Equals(expected, delta));
        }
    }
}
