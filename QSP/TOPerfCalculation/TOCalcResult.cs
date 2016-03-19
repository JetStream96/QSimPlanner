using QSP.AviationTools;
using System;
using System.Collections.Generic;
using System.Text;
using static QSP.AviationTools.Constants;

namespace QSP.TOPerfCalculation
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

            var str = new StringBuilder();

            str.AppendLine();
            str.AppendLine("                (   OAT " + tempConvertUnit(primaryResult.OatCelsius, tempUnit) + tUnit + "   )");
            str.AppendLine("          Required distance     " + lengthConvertUnit(primaryResult.RwyRequiredMeter, lenUnit) + lUnit);
            str.AppendLine("          Runway remaining      " + lengthConvertUnit(primaryResult.RwyRemainingMeter, lenUnit) + lUnit);
            str.AppendLine();
            str.AppendLine(new string('-', 50));

            if (assumedTemp.Count > 0)
            {
                str.AppendLine("             ( ASSUMED TEMPERATURE )");
                str.AppendLine("  Temp(" + tUnit + ")  Required distance   Runway remaining");

                foreach (var i in assumedTemp)
                {
                    str.Append(tempConvertUnit(i.OatCelsius, tempUnit)
                                .ToString()
                                .PadLeft(6, ' '));

                    str.Append(lengthConvertUnit(i.RwyRequiredMeter, lenUnit)
                                .ToString()
                                .PadLeft(15, ' ')
                                + " " + lUnit);

                    str.AppendLine(lengthConvertUnit(i.RwyRemainingMeter, lenUnit)
                                .ToString()
                                .PadLeft(17, ' ')
                                + " " + lUnit);
                }
            }

            return str.ToString();
        }

        private int tempConvertUnit(int tempCelsuis, TemperatureUnit tempUnit)
        {
            return tempUnit == TemperatureUnit.Fahrenheit ?
                Convert.ToInt32(CoversionTools.ToFahrenheit(tempCelsuis)) :
                tempCelsuis;
        }

        private int lengthConvertUnit(int lengthMeter, LengthUnit unit)
        {
            return unit == LengthUnit.Feet ?
                (int)(lengthMeter * MeterFtRatio) :
                lengthMeter;
        }

        private class dataRow
        {
            public int OatCelsius { get; private set; }
            public int RwyRequiredMeter { get; private set; }
            public int RwyRemainingMeter { get; private set; }

            public dataRow(int OatCelsius, int RwyRequiredMeter, int RwyRemainingMeter)
            {
                this.OatCelsius = OatCelsius;
                this.RwyRequiredMeter = RwyRequiredMeter;
                this.RwyRemainingMeter = RwyRemainingMeter;
            }
        }
    }
}
