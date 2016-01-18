using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools;

namespace Tests.AviationToolsTest
{
    [TestClass()]
	public class LatLonConversionTest
	{

		[TestMethod()]
		public void IsCorrectFormatTest()
		{
			Assert.IsTrue(LatLonConversion.Is7DigitFormat("37N055E"));
			Assert.IsTrue(LatLonConversion.Is7DigitFormat("37S155W"));
		}

		[TestMethod()]
		public void IsWrongFormatTest()
		{
			Assert.IsFalse(LatLonConversion.Is7DigitFormat("37U055E"));
			Assert.IsFalse(LatLonConversion.Is7DigitFormat("37S155S"));
			Assert.IsFalse(LatLonConversion.Is7DigitFormat("37S15AW"));
			Assert.IsFalse(LatLonConversion.Is7DigitFormat("37S155"));
		}

		[TestMethod()]
		public void ConverterTest()
		{
			Assert.AreEqual("36N70", LatLonConversion.Convert7DigitTo5Digit("36N170W"));
			Assert.AreEqual("3670N", LatLonConversion.Convert7DigitTo5Digit("36N070W"));
			Assert.AreEqual("57S00", LatLonConversion.Convert7DigitTo5Digit("57S100E"));
			Assert.AreEqual("57S13", LatLonConversion.Convert7DigitTo5Digit("57S113E"));
			Assert.AreEqual("57W10", LatLonConversion.Convert7DigitTo5Digit("57S110W"));
			Assert.AreEqual("4334E", LatLonConversion.Convert7DigitTo5Digit("43N034E"));
		}

	}
}
