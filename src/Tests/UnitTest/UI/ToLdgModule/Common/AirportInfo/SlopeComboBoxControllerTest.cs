using NUnit.Framework;
using QSP.UI.ToLdgModule.Common.AirportInfo;
using System;

namespace UnitTest.UI.ToLdgModule.Common.AirportInfo
{
    [TestFixture]
    public class SlopeComboBoxControllerTest
    {
        [Test]
        public void NewControllerRangeCorrect()
        {
            var controller = new SlopeComboBoxController(-2.0, 2.0);
            var slopes = controller.items;

            for (int i = 0; i < 41; i++)
            {
                Assert.AreEqual(-2.0 + i * 0.1, slopes[i], 1E-6);
            }
        }

        [Test]
        public void ResizeRequiredTest()
        {
            var controller = new SlopeComboBoxController(-3.0, 3.0);

            Assert.IsFalse(controller.ResizeRequired(2.98));
            Assert.IsTrue(controller.ResizeRequired(-3.02));
        }

        [Test]
        public void NearestIndexWithInBound()
        {
            var controller = new SlopeComboBoxController(-2.0, 2.0);

            Assert.AreEqual(20, controller.NearestIndex(0.0));
            Assert.AreEqual(0, controller.NearestIndex(-2.04));
        }

        [Test]
        public void NearestIndexOutOfBoundShouldThrowException()
        {
            var controller = new SlopeComboBoxController(-2.0, 2.0);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            controller.NearestIndex(2.08));
        }
    }
}
