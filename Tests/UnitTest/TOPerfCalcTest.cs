using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    // TODO: Enable the test.
    [TestClass()]
	public class TOPerfCalcTest
	{

        //private void PrepareTest()
        //{
        //    if (LoadedData.)
        //    LoadedData.Load();
        //}

		//[TestMethod()]
		//public void TOPerfCalc_RwyRequiredSameAsManual1()
		//{
  //          LoadedData.Load();
  //          TOPerfCalculator calc = new TOPerfCalculator(new TOPerfParameters(3600, 0, 0, 0.0, 0, 0, 46, 1013, false, 83100,
		//	ThrustRatingOption.Normal, AntiIceOption.Off, true), Aircraft.B737800);

		//	Assert.AreEqual(3600.0, calc.TakeoffDistanceMeter());

		//}

		//[TestMethod()]
		//public void TOPerfCalc_RwyRequiredSameAsManual2()
		//{
  //          LoadedData.Load();
  //          TOPerfCalculator calc2 = new TOPerfCalculator(new TOPerfParameters(3500, 0, 0, 0.0, 0, 0, 46, 1013, false, 82100,
		//	ThrustRatingOption.Normal, AntiIceOption.Off, true), Aircraft.B737800);

		//	Assert.AreEqual(3500.0, calc2.TakeoffDistanceMeter());

		//}

		//[TestMethod()]
		//public void TOPerfCalc_RwyRequiredSameAsManual3()
		//{
  //          LoadedData.Load();
  //          TOPerfCalculator calc3 = new TOPerfCalculator(new TOPerfParameters(3500, 1000, 0, 2.0, 0, 0, 42, 1013, true, 74500,
		//	ThrustRatingOption.Normal, AntiIceOption.Off, true), Aircraft.B737800);

		//	Assert.IsTrue( WithinTolerance(3355.555555, calc3.TakeoffDistanceMeter(), 0.1));

		//}


		//public void TOPerfCalc_ReportTest()
		//{
  //          LoadedData.Load();
  //          TOPerfCalculator calc = new TOPerfCalculator(new TOPerfParameters(3500, 1000, 0, 2.0, 0, 0, 26, 1013, true, 74500,
		//	ThrustRatingOption.Normal, AntiIceOption.Off, true), Aircraft.B737800);

		//	Debug.Write(calc.TakeOffReport().ToString(TemperatureUnit.Celsius, LengthUnit.Meter));
		//}

		//public static bool WithinTolerance(double expected, double actual, double percentage)
		//{

		//	if ((expected - actual) <= expected * percentage / 100) {
		//		return true;
		//	} else {
		//		return false;
		//	}

		//}

		//[TestMethod()]

		//public void TOPerfCalc_FieldLimitWeightTest()
		//{
  //          LoadedData.Load();
  //          var toPara = new TOPerfParameters(3600, 0, 0, 0.0, 0, 0, 46, 1013, false, 82100,
		//	ThrustRatingOption.Normal, AntiIceOption.Off, true);
		//	TOPerfCalculator calc = new TOPerfCalculator(toPara, Aircraft.B737800);

		//	Assert.AreEqual(83.1, calc.FieldLimitWeightTon());

		//	toPara.RwyElevationFt = 1000;
		//	toPara.SurfaceWet = true;
		//	toPara.RwyLengthMeter = 3500;
		//	toPara.OatCelsius = 30;

		//	Assert.AreEqual(84.55, calc.FieldLimitWeightTon());

		//	toPara.AntiIce = AntiIceOption.Engine;

		//	Assert.AreEqual(84.55 - 0.2, calc.FieldLimitWeightTon());

		//}

		//[TestMethod()]
		//public void TOPerfCalc_ClimbLimitWeightTest1()
		//{
  //          LoadedData.Load();
  //          var toPara = new TOPerfParameters(3600, 0, 0, 0.0, 0, 0, 26, 1013, false, 282100,
		//	ThrustRatingOption.Normal, AntiIceOption.Off, true);
		//	TOPerfCalculator calc = new TOPerfCalculator(toPara, Aircraft.B777200LR);

		//	Assert.AreEqual(350.2, calc.ClimbLimitWeightTon());
		//}

		//[TestMethod()]
		//public void TOPerfCalc_ClimbLimitWeightTest2()
		//{
  //          LoadedData.Load();
  //          var toPara = new TOPerfParameters(3600, 0, 0, 0.0, 0, 0, 26, 1013, false, 282100,
		//	ThrustRatingOption.TO1, AntiIceOption.Off, true);
		//	TOPerfCalculator calc = new TOPerfCalculator(toPara, Aircraft.B777200LR);

		//	Assert.AreEqual(true, WithinTolerance(310.874, calc.ClimbLimitWeightTon(), 0.1));

		//}

	}
}
