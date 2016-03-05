using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.UI.AirportInfo;

namespace UnitTest.UI.AirportInfo
{
    [TestClass]
    public class SlopeComboBoxControllerTest
    {
        [TestMethod]
        public void NewControllerRangeCorrect()
        {
            var controller = new SlopeComboBoxController(-2.0, 2.0);
            var slopes = controller.items;

            for (int i = 0; i < 41; i++)
            {
                Assert.AreEqual(-2.0 + i * 0.1, slopes[i], 1E-6);
            }
        }

        [TestMethod]
        public void ResizeRequiredTest()
        {
            var controller = new SlopeComboBoxController(-3.0, 3.0);

            Assert.IsFalse(controller.ResizeRequired(2.98));
            Assert.IsTrue(controller.ResizeRequired(-3.02));
        }

        [TestMethod]
        public void NearestIndexWithInBound()
        {
            var controller = new SlopeComboBoxController(-2.0, 2.0);

            Assert.AreEqual(20, controller.NearestIndex(0.0));
            Assert.AreEqual(0, controller.NearestIndex(-2.04));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NearestIndexOutOfBoundShouldThrowException()
        {
            var controller = new SlopeComboBoxController(-2.0, 2.0);
            Assert.AreEqual(20, controller.NearestIndex(2.08));
        }
    }
}
