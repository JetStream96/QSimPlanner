using QSP.AviationTools;
using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Text;
using static QSP.AviationTools.Constants;

namespace QSP.TOPerfCalculation
{
    public class TOReport
    {
        public DataRow PrimaryResult { get; }
        public IReadOnlyList<DataRow> AssumedTemp { get; }

        public TOReport(DataRow PrimaryResult, IReadOnlyList<DataRow> AssumedTemp)
        {
            this.PrimaryResult = PrimaryResult;
            this.AssumedTemp = AssumedTemp;
        }

        public string ToString(TemperatureUnit tempUnit, LengthUnit lenUnit)
        {
            string tUnit = tempUnit == TemperatureUnit.Celsius ? "°C" : "°F";
            string lUnit = lenUnit == LengthUnit.Meter ? "M" : "FT";

            var str = new StringBuilder();

            str.AppendLine();

            str.AppendLine("                (   OAT " +
                TempConvertUnit(PrimaryResult.OatCelsius, tempUnit) +
                tUnit + "   )");

            str.AppendLine("          Required distance     " +
                LengthConvertUnit(PrimaryResult.RwyRequiredMeter, lenUnit)
                + " " + lUnit);

            str.AppendLine("          Runway remaining      " +
                LengthConvertUnit(PrimaryResult.RwyRemainingMeter, lenUnit)
                + " " + lUnit);

            str.AppendLine();
            str.AppendLine(new string('-', 50));

            if (AssumedTemp.Count > 0)
            {
                str.AppendLine("             ( ASSUMED TEMPERATURE )");
                str.AppendLine($"  Temp({tUnit})  Required distance   Runway remaining");

                foreach (var i in AssumedTemp)
                {
                    str.Append(TempConvertUnit(i.OatCelsius, tempUnit)
                                .ToString()
                                .PadLeft(6, ' '));

                    str.Append(LengthConvertUnit(i.RwyRequiredMeter, lenUnit)
                                .ToString()
                                .PadLeft(15, ' ')
                                + " " + lUnit);

                    str.AppendLine(LengthConvertUnit(i.RwyRemainingMeter, lenUnit)
                                .ToString()
                                .PadLeft(17, ' ')
                                + " " + lUnit);
                }
            }

            return str.ToString();
        }

        private int TempConvertUnit(double tempCelsuis, TemperatureUnit tempUnit)
        {
            return Convert.ToInt32(tempUnit == TemperatureUnit.Fahrenheit ?
                ConversionTools.ToFahrenheit(tempCelsuis) : tempCelsuis);
        }

        private int LengthConvertUnit(double lengthMeter, LengthUnit unit)
        {
            return Convert.ToInt32(unit == LengthUnit.Feet ?
                lengthMeter * MeterFtRatio : lengthMeter);
        }

        public class DataRow
        {
            public double OatCelsius { get; }
            public double RwyRequiredMeter { get; }
            public double RwyRemainingMeter { get; }

            public DataRow(double OatCelsius, double RwyRequiredMeter, double RwyRemainingMeter)
            {
                this.OatCelsius = OatCelsius;
                this.RwyRequiredMeter = RwyRequiredMeter;
                this.RwyRemainingMeter = RwyRemainingMeter;
            }
        }
    }
}
