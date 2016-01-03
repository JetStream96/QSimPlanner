using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Airports;

namespace Tests.RouteFindingTest.TestDataGenerators
{
    public static class TestDataProvider
    {
        private static string AXYZAllTxt;
        private static SidCollection SidCollection;

        public static string GetAXYZAllTxt()
        {
            if (AXYZAllTxt == null)
            {
                AXYZAllTxt = CommentLineRemover.RemoveComments(File.ReadAllText("RouteFindingTest\\TestData\\PROC\\AXYZ_WithComments.txt"));
            }
            return AXYZAllTxt;
        }

        public static SidCollection GetSidCollection()
        {
            if (SidCollection == null)
            {
                var allTxt = GetAXYZAllTxt();
                var reader = new SidReader(allTxt);

                SidCollection = reader.Parse();
            }
            return SidCollection;
        }        
    }
}
