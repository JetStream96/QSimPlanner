using QSP.MathTools.TableNew;
using NUnit.Framework;

namespace UnitTest.MathTools.TableNew
{
    [TestFixture]
    public class IntervalTest
    {
        [Test]
        public void ComparerTest()
        {
            var comparer = new Interval.Comparer();
            var a = new Interval(8.0, 10.0);
            var b = new Interval(7.0, 9.0);
            var c = new Interval(5.0, 6.0);

            Assert.AreEqual(0, comparer.Compare(a, b));
            Assert.AreEqual(0, comparer.Compare(b, a));
            Assert.AreEqual(1, comparer.Compare(a, c));
            Assert.AreEqual(-1, comparer.Compare(c, a));
        }
    }
}