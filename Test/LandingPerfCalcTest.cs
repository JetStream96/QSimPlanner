using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LandingPerfCalculation;
namespace UnitTest
{

    [TestClass()]
	public class LandingPerfCalcTest
	{

		[TestMethod()]
		public void LandingPerfCalcTestOutput()
		{
            LoadedData.Load();
			var myCalc = LoadedData.GetPara(QSP.Aircraft.B737800);
			LandingParameters para = new LandingParameters(74800, 3500, 102, 12, 0.3, 12, 5, ReverserOption.Both, SurfaceCondition.Dry, 1,
			3);

			Debug.WriteLine(myCalc.GetLandingReport(para).ToString(QSP.LengthUnit.Meter));

		}

	}
}
