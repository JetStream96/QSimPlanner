using System;
using System.Collections.Generic;
using System.Text;
using QSP.AviationTools;

namespace QSP.TakeOffPerfCalculation
{

    public class TOPerfResult
	{

		private dataRow primaryResult;

		private List<dataRow> assumedTemp;
		public TOPerfResult()
		{
			assumedTemp = new List<dataRow>();
		}

		public void SetPrimaryResult(int OatCelsius, int RwyRequiredMeter, int RwyRemainingMeter)
		{
			primaryResult = new dataRow(OatCelsius, RwyRequiredMeter, RwyRemainingMeter);
		}

		public void AddAssumedTemp(int OatCelsius, int RwyRequiredMeter, int RwyRemainingMeter)
		{
			assumedTemp.Add(new dataRow(OatCelsius, RwyRequiredMeter, RwyRemainingMeter));
		}

		public string ToString(TemperatureUnit tempUnit, LengthUnit lenUnit)
		{

			string tUnit = tempUnit == TemperatureUnit.Celsius ? "°C" : "°F";
			string lUnit = lenUnit == LengthUnit.Meter ? "M" : "FT";

			StringBuilder str = new StringBuilder();

			str.AppendLine();
			str.AppendLine("                (   OAT " + tempConvertUnit(primaryResult.OatCelsius, tempUnit) + tUnit + "   )");
			str.AppendLine("          Required distance     " + lengthConvertUnit(primaryResult.RwyRequiredMeter, lenUnit) + lUnit);
			str.AppendLine("          Runway remaining      " + lengthConvertUnit(primaryResult.RwyRemainingMeter, lenUnit) + lUnit);
			str.AppendLine();
			str.AppendLine(new string('-', 50));


			if (assumedTemp.Count > 0) {
				str.AppendLine("             ( ASSUMED TEMPERATURE )");
				str.AppendLine("  Temp(" + tUnit + ")  Required distance   Runway remaining");

				foreach (var i in assumedTemp) {
					str.Append(Convert.ToString(tempConvertUnit(i.OatCelsius, tempUnit)).PadLeft(6, ' '));
					str.Append(Convert.ToString(lengthConvertUnit(i.RwyRequiredMeter, lenUnit)).PadLeft(15, ' ') + " " + lUnit);
					str.AppendLine(Convert.ToString(lengthConvertUnit(i.RwyRemainingMeter, lenUnit)).PadLeft(17, ' ') + " " + lUnit);
				}


			}

			return str.ToString();

		}

		private int tempConvertUnit(int tempCelsuis, TemperatureUnit tempUnit)
		{
			return tempUnit == TemperatureUnit.Fahrenheit ? Convert.ToInt32(CoversionTools.ToFahrenheit(tempCelsuis)) : tempCelsuis;
		}

		private int lengthConvertUnit(int lengthMeter, LengthUnit unit)
		{
			return unit == LengthUnit.Feet ? Convert.ToInt32(lengthMeter * Constants.M_FT_ratio) : lengthMeter;
		}

		private class dataRow
		{

			public int OatCelsius { get; set; }
			public int RwyRequiredMeter { get; set; }
			public int RwyRemainingMeter { get; set; }


			public dataRow(int OatCelsius, int RwyRequiredMeter, int RwyRemainingMeter)
			{
				this.OatCelsius = OatCelsius;
				this.RwyRequiredMeter = RwyRequiredMeter;
				this.RwyRemainingMeter = RwyRemainingMeter;

			}

		}

	}

}
