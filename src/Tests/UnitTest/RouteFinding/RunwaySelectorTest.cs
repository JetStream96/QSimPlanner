using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QSP.RouteFinding.Airports;
using Moq;
using QSP.RouteFinding;

namespace UnitTest.RouteFinding
{
    [TestFixture]
    public class RunwaySelectorTest
    {
        [Test]
        public void GroupIntoPairsTest()
        {
            var rwy0 = new Mock<IRwyData>();
            rwy0.Setup(r => r.RwyIdent).Returns("12");

            var rwy1 = new Mock<IRwyData>();
            rwy1.Setup(r => r.RwyIdent).Returns("15R");

            var rwy2 = new Mock<IRwyData>();
            rwy2.Setup(r => r.RwyIdent).Returns("33L");

            var rwy3 = new Mock<IRwyData>();
            rwy3.Setup(r => r.RwyIdent).Returns("30");

            var rwys = new[] { rwy0, rwy1, rwy2, rwy3 }.Select(r => r.Object);
            var groups = RunwaySelector.GroupIntoPairs(rwys).ToList();

            Assert.AreEqual(2, groups.Count);
            groups.ForEach(i =>
            {
                Assert.AreEqual(2, i.Count);
                if (i.Any(r => r.RwyIdent == "12")) Assert.IsTrue(i.Any(r => r.RwyIdent == "30"));
                if (i.Any(r => r.RwyIdent == "15R")) Assert.IsTrue(i.Any(r => r.RwyIdent == "33L"));
            });
        }
    }
}
