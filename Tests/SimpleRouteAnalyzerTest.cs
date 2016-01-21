using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Tests.Common.Utilities;
using QSP.RouteFinding.RouteAnalyzers;

namespace Tests
{
    [TestClass()]
    public class SimpleRouteAnalyzerTest
    {
        [TestMethod()]

        public void ResultFound()
        {
            PrepareTest();

            var rte = new AutoSelectFirstWaypointAnalyzer("HLG A1 ELATO CONGA", 25.0, 125.0).Parse();
            Debug.WriteLine(rte.ToString(true, true));
        }
    }
}
