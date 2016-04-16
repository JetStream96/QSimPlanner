using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP;
using QSP.MathTools;
using QSP.Metar;

namespace UnitTest.Metar
{
    [TestClass]
    public class ParaExtractorTest
    {
        [TestMethod]
        public void GetWindTestVRB()
        {
            var w = ParaExtractor.GetWind("RCTP ... VRB05KT");
            Assert.AreEqual(0.0, (w.Direction - 360.0).Mod(360.0), 1E-6);
            Assert.AreEqual(0.0, w.Speed, 1E-6);
        }

        [TestMethod]
        public void GetWindTestNotFound()
        {
            Assert.IsNull(ParaExtractor.GetWind("RCTP ... ..."));
        }

        [TestMethod]
        public void GetWindTestNormalFormat()
        {
            var w = ParaExtractor.GetWind("RCTP ... 31005KT");
            Assert.AreEqual(0.0, (w.Direction - 310.0).Mod(360.0), 1E-6);
            Assert.AreEqual(5.0, w.Speed, 1E-6);

            var v = ParaExtractor.GetWind("ZSSS ... 31008MPS");
            Assert.AreEqual(0.0, (v.Direction - 310.0).Mod(360.0), 1E-6);
            Assert.AreEqual(8.0 / 0.514444444, v.Speed, 1E-6);

            var x = ParaExtractor.GetWind("RCTP ... 310/05KT");
            Assert.AreEqual(0.0, (w.Direction - 310.0).Mod(360.0), 1E-6);
            Assert.AreEqual(5.0, w.Speed, 1E-6);

            var y = ParaExtractor.GetWind("RCTP ... 31005G15KT");
            Assert.AreEqual(0.0, (w.Direction - 310.0).Mod(360.0), 1E-6);
            Assert.AreEqual(5.0, w.Speed, 1E-6);
        }

        [TestMethod]
        public void GetTempTest()
        {
            Assert.AreEqual(15, ParaExtractor.GetTemp("RCTP ... 15/12"));
            Assert.AreEqual(-5, ParaExtractor.GetTemp("EDDF ... M05/M06"));
            Assert.AreEqual(1, ParaExtractor.GetTemp("RJAA ... 01/M01"));
        }

        [TestMethod]
        public void GetTempTestNotFound()
        {
            Assert.AreEqual(int.MinValue, ParaExtractor.GetTemp("RCTP ... ..."));
        }

        [TestMethod]
        public void GetPressTestNotFound()
        {
            Assert.IsNull(ParaExtractor.GetPressure("RCTP ... ..."));
        }

        [TestMethod]
        public void GetPressTest()
        {
            var p = ParaExtractor.GetPressure("RCTP ... Q1015 ");
            Assert.AreEqual(PressureUnit.Mb, p.PressUnit);
            Assert.AreEqual(1015.0, p.Value, 1E-6);

            var q = ParaExtractor.GetPressure("KLAX ... A3001");
            Assert.AreEqual(PressureUnit.inHg, q.PressUnit);
            Assert.AreEqual(30.01, q.Value, 1E-6);
        }
    }
}
