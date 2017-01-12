using System.Linq;
using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures.Parser;

namespace UnitTest.RouteFinding.TerminalProcedures.Parser
{
    [TestFixture]
    public class SectionSplitterTest
    {
        private static string allTxt =
@"A,AXYZ,Test Airport 01,25.0,50.0,15,5000,8000,3500,0
R,18,180,3500,60,0,0.000,0,25.0003, 50.0001,15,3.00,50,1,0
R,36,360,3500,60,0,0.000,0,25.0001, 50.0005,15,3.00,50,1,0

SID,SID1,18,4
DF,WPT102,25.0150,50.0800,1, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT103,25.0175,50.1300,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT104,25.0225,50.1800,0, ,0.0,0.0,0.0,0.0,2,4000,0,0,0,0,0,0,

NOTSID,SID2,18,5
VA,0,53.0,2,600,0,1,210,0,0,0, 

SID,SID3,18,5
VA,0,53.0,2,600,0,1,210,0,0,0, ";

        [Test]
        public void SplitTest()
        {
            var entries = SectionSplitter.Split(allTxt.Lines(), SectionSplitter.Type.Sid);
            var list = entries.ToList();

            Assert.AreEqual(2, list.Count);

            var expected0 = new[]
            {

                "SID,SID1,18,4",
                "DF,WPT102,25.0150,50.0800,1, ,0.0,0.0,0,0,0,0,0,0,0,0, ",
                "TF,WPT103,25.0175,50.1300,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, ",
                "TF,WPT104,25.0225,50.1800,0, ,0.0,0.0,0.0,0.0,2,4000,0,0,0,0,0,0,"
            };

            Assert.IsTrue(list[0].Lines.SequenceEqual(expected0));

            var expected1 = new[]
            {
                "SID,SID3,18,5",
                "VA,0,53.0,2,600,0,1,210,0,0,0, "
            };

            Assert.IsTrue(list[1].Lines.SequenceEqual(expected1));
        }
    }
}