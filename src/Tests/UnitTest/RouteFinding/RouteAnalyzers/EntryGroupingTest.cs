using System.Linq;
using NUnit.Framework;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestFixture]
    public class EntryGroupingTest
    {
        [Test]
        public void GroupTest()
        {
            var input = new RouteString("A B AUTO C D RAND E".Split(' '));
            var result = EntryGrouping.Group(input);

            Assert.AreEqual(5, result.Count);
            AssertSegment(result[0], new string[] { "A", "B" });
            Assert.IsTrue(result[1].IsAuto);
            AssertSegment(result[2], new string[] { "C", "D" });
            Assert.IsTrue(result[3].IsRand);
            AssertSegment(result[4], new string[] { "E"});
        }

        [Test]
        public void GroupNoSegmentShouldBeEmpty()
        {
            var input = new RouteString("AUTO A B".Split(' '));
            var result = EntryGrouping.Group(input);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result[0].IsAuto);
            AssertSegment(result[1], new string[] { "A", "B" });
        }

        private static void AssertSegment(RouteSegment seg, string[] s)
        {
            Assert.IsTrue(Enumerable.SequenceEqual(seg.RouteString, s));
        }
    }
}
