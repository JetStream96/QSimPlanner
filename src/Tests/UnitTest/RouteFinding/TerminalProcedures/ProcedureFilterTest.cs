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
            f["ABCD", "08R", false] = new FilterEntry(false, new[] { "SID0", "SID1" }, false);

            var entry = f["ABCD", "08R", false];
            Assert.IsFalse(entry.IsBlackList);
            Assert.AreEqual(2, entry.Procedures.Count);
            Assert.IsTrue(entry.Procedures.Contains("SID0"));
            Assert.IsTrue(entry.Procedures.Contains("SID1"));
        }

        [Test]
        public void InsertionAndModifyTest()
        {
            var f = new ProcedureFilter();
            f["ABCD", "08R", true] = new FilterEntry(false, new[] { "SID0" }, true);
            f["ABCD", "08R", true] = new FilterEntry(false, new[] { "SID1" }, true);

            var entry = f["ABCD", "08R", true];
            Assert.IsFalse(entry.IsBlackList);
            Assert.AreEqual(1, entry.Procedures.Count);
            Assert.IsTrue(entry.Procedures.Contains("SID1"));
        }

        [Test]
        public void TryGetEntryTest()
        {
            var f = new ProcedureFilter();
            var a = f.TryGetEntry("ABCD", "08R", true);

            Assert.IsNull(a);

            f["ABCD", "08R", true] = new FilterEntry(false, new[] { "SID0" }, true);

            var b = f.TryGetEntry("ABCD", "08R", true);

            var entry = f["ABCD", "08R", true];
            Assert.IsFalse(entry.IsBlackList);
            Assert.AreEqual(1, entry.Procedures.Count);
            Assert.IsTrue(entry.Procedures.Contains("SID0"));
        }

        [Test]
        public void ExistsTest()
        {
            var f = new ProcedureFilter();
            Assert.IsFalse(f.Exists("ABCD", "08R", true));

            f["ABCD", "08R", true] = new FilterEntry(false, new[] { "SID0" }, true);
            Assert.IsTrue(f.Exists("ABCD", "08R", true));
        }
    }
}
