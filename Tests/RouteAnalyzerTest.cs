using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Tests.Common.Utilities;
using QSP.RouteFinding.Routes;

namespace Tests
{

    [TestClass()]
    public class RouteAnalyzerTest
    {

        [TestMethod()]

        public void SID_Case2_STAR_Case3()
        {
            PrepareTest();

            const string orig = "RCTP";
            const string dest = "RJAA";

            string origRwy = RouteFindingCore.AirportList.RwyIdentList(orig).First();
            string destRwy = RouteFindingCore.AirportList.RwyIdentList(dest).Last();

            var genRoute = new RouteFinder().FindRoute(orig, origRwy, new SidHandler(orig).GetSidList(origRwy), dest, destRwy, new StarHandler(dest).GetStarList(destRwy));

            string rte = genRoute.ToString(false, false);
            Debug.WriteLine("Generated route: " + rte);
            Debug.WriteLine("Distance = {0} nm\n", genRoute.TotalDistance);

            var rteA = new RouteAnalyzer(orig, origRwy, dest, destRwy, rte).Parse();
            Debug.WriteLine("Analyzed Route: " + rteA.ToString());
            Debug.WriteLine("Distance = {0} nm", rteA.TotalDistance);

        }

        [TestMethod()]

        public void SID_Case1()
        {
            PrepareTest();

            const string orig = "KORD";
            const string dest = "KSLC";

            string origRwy = RouteFindingCore.AirportList.RwyIdentList(orig).First();
            string destRwy = RouteFindingCore.AirportList.RwyIdentList(dest).First();

            var genRoute = new RouteFinder().FindRoute(orig, origRwy, new SidHandler(orig).GetSidList(origRwy), dest, destRwy, new StarHandler(dest).GetStarList(destRwy));

            string rte = genRoute.ToString(false, false);
            Debug.WriteLine("Generated route: " + rte);
            Debug.WriteLine("Distance = {0} nm\n", genRoute.TotalDistance);

            var rteA = new RouteAnalyzer(orig, origRwy, dest, destRwy, rte).Parse();
            Debug.WriteLine("Analyzed Route: " + rteA.ToString());
            Debug.WriteLine("Distance = {0} nm", rteA.TotalDistance);

        }

        [TestMethod()]

        public void STAR_Case1()
        {
            PrepareTest();

            const string orig = "VHHH";
            const string dest = "RCYU";

            string origRwy = RouteFindingCore.AirportList.RwyIdentList(orig).First();
            string destRwy = RouteFindingCore.AirportList.RwyIdentList(dest).First();

            var genRoute = new RouteFinder().FindRoute(orig, origRwy, new SidHandler(orig).GetSidList(origRwy), dest, destRwy, new StarHandler(dest).GetStarList(destRwy));

            string rte = genRoute.ToString(false, false);
            Debug.WriteLine("Generated route: " + rte);
            Debug.WriteLine("Distance = {0} nm\n", genRoute.TotalDistance);

            var rteA = new RouteAnalyzer(orig, origRwy, dest, destRwy, rte).Parse();
            Debug.WriteLine("Analyzed Route: " + rteA.ToString());
            Debug.WriteLine("Distance = {0} nm", rteA.TotalDistance);

        }

        [TestMethod()]

        public void SID_Case4_STAR_Case2()
        {
            PrepareTest();

            const string orig = "VHHH";
            const string dest = "RCKH";

            string origRwy = RouteFindingCore.AirportList.RwyIdentList(orig).First();
            string destRwy = RouteFindingCore.AirportList.RwyIdentList(dest).First();

            var genRoute = new RouteFinder().FindRoute(orig, origRwy, new SidHandler(orig).GetSidList(origRwy), dest, destRwy, new StarHandler(dest).GetStarList(destRwy));

            string rte = genRoute.ToString(false, false);
            Debug.WriteLine("Generated route: " + rte);
            Debug.WriteLine("Distance = {0} nm\n", genRoute.TotalDistance);

            var rteA = new RouteAnalyzer(orig, origRwy, dest, destRwy, rte).Parse();
            Debug.WriteLine("Analyzed Route: " + rteA.ToString());
            Debug.WriteLine("Distance = {0} nm", rteA.TotalDistance);

        }

        [TestMethod()]

        public void SID_Case3_STAR_Case2()
        {
            PrepareTest();

            const string orig = "KBOS";
            const string dest = "KSFO";

            string origRwy = RouteFindingCore.AirportList.RwyIdentList(orig)[6];
            string destRwy = RouteFindingCore.AirportList.RwyIdentList(dest).First();
            var sidList = new List<string>();
            sidList.Add(new SidHandler(orig).GetSidList(origRwy)[0]);

            var genRoute = new RouteFinder().FindRoute(orig, origRwy, sidList, dest, destRwy, new StarHandler(dest).GetStarList(destRwy));

            string rte = genRoute.ToString(false, false);
            Debug.WriteLine("Generated route: " + rte);
            Debug.WriteLine("Distance = {0} nm\n", genRoute.TotalDistance);

            var rteA = new RouteAnalyzer(orig, origRwy, dest, destRwy, rte).Parse();
            Debug.WriteLine("Analyzed Route: " + rteA.ToString());
            Debug.WriteLine("Distance = {0} nm", rteA.TotalDistance);

        }

    }
}
