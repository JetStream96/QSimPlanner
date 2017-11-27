using NUnit.Framework;
using QSP.RouteFinding.TerminalProcedures;
using System.Linq;

namespace UnitTest.RouteFinding.TerminalProcedures
{
    [TestFixture]
    public class ProcedureFilterTest
    {
        [Test]
        public void InsertionAndGetTest()
        {
            var f = new ProcedureFilter();
            f["ABCD", "08R"] = new FilterEntry(false, new[] { "SID0", "SID1" });

            var entry = f["ABCD", "08R"];
            Assert.IsFalse(entry.IsBlackList);
            Assert.AreEqual(2, entry.Procedures.Count);
            Assert.IsTrue(entry.Procedures.Contains("SID0"));
            Assert.IsTrue(entry.Procedures.Contains("SID1"));
        }

        [Test]
        public void InsertionAndModifyTest()
        {
            var f = new ProcedureFilter();
            f["ABCD", "08R"] = new FilterEntry(false, new[] { "SID0" });
            f["ABCD", "08R"] = new FilterEntry(false, new[] { "SID1" });

            var entry = f["ABCD", "08R"];
            Assert.IsFalse(entry.IsBlackList);
            Assert.AreEqual(1, entry.Procedures.Count);
            Assert.IsTrue(entry.Procedures.Contains("SID1"));
        }
    }
}
