using System;
using System.Collections.Generic;
namespace QSP.LandingPerfCalculation
{

    public class LandingCalcResult
	{

		private dataRow selectedBrks;

		private List<dataRow> allSettings;
		public LandingCalcResult()
		{
			allSettings = new List<dataRow>();
		}

		public void SetSelectedBrakesResult(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
		{
			selectedBrks = new dataRow(BrkSetting, ActualDisMeter, DisRemainMeter);
		}

		public void AddOtherResult(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
		{
			allSettings.Add(new dataRow(BrkSetting, ActualDisMeter, DisRemainMeter));
		}

		/// <summary>
		/// Add the result of selected brake to allSettings. Value of selectedBrks must be set already.
		/// </summary>
		public void AddOtherResult()
		{
			allSettings.Add(selectedBrks);
		}

		public string ToString(LengthUnit lengthUnit)
		{

			var unitStr = lengthUnit == QSP.LengthUnit.Meter ? "M" : "FT";
			System.Text.StringBuilder result = new System.Text.StringBuilder();

			result.AppendLine();
			result.AppendLine("           Actual landing distance    " + convertToFTIfNeeded(selectedBrks.ActualDisMeter, lengthUnit) + " " + unitStr);
			result.AppendLine("           Runway remaining           " + convertToFTIfNeeded(selectedBrks.DisRemainMeter, lengthUnit) + " " + unitStr);
			result.AppendLine();
			result.AppendLine(new string('-', 60));
			result.AppendLine("                    (OTHER BRK SETTINGS)");
			result.AppendLine("                  Landing distance   Runway remaining");


			foreach (var i in allSettings) {
				result.Append(("   " + i.BrkSetting).PadRight(18, ' '));
				result.Append((Convert.ToString(convertToFTIfNeeded(i.ActualDisMeter, lengthUnit)) + " " + unitStr).PadLeft(12, ' ').PadRight(19, ' '));
				result.AppendLine((Convert.ToString(convertToFTIfNeeded(i.DisRemainMeter, lengthUnit) + " " + unitStr)).PadLeft(11, ' '));

			}

			return result.ToString();

		}

		private int convertToFTIfNeeded(int disMeter, LengthUnit unit)
		{
			return unit == LengthUnit.Meter ? disMeter : Convert.ToInt32(disMeter * AviationTools.AviationConstants.M_FT_ratio);
		}

		private class dataRow
		{

			public string BrkSetting { get; set; }
			public int ActualDisMeter { get; set; }
			public int DisRemainMeter { get; set; }


			public dataRow(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
			{
				this.BrkSetting = BrkSetting;
				this.ActualDisMeter = ActualDisMeter;
				this.DisRemainMeter = DisRemainMeter;

			}

		}

	}

}
