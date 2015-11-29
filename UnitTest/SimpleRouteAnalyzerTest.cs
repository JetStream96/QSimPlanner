using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding;
using static UnitTest.Common.Utilities;

namespace UnitTest
{

    [TestClass()]
	public class SimpleRouteAnalyzerTest
	{

		[TestMethod()]

		public void ResultFound()
		{
			PrepareTest();

			var rte = new SimpleRouteAnalyzer("HLG A1 ELATO CONGA", 25, 125).Parse();
			Debug.WriteLine(rte.ToString());

		}

	}
}
