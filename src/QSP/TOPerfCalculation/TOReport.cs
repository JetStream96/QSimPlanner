using QSP.AviationTools;
using QSP.Utilities.Units;
using System.Collections.Generic;
using System.Text;
using QSP.MathTools;
using static QSP.AviationTools.Constants;

namespace QSP.TOPerfCalculation
{
    public class TOReport
    {
        public TOReportRow PrimaryResult { get; }
        public IReadOnlyList<TOReportRow> AssumedTemp { get; }

        public TOReport(TOReportRow PrimaryResult, IReadOnlyList<TOReportRow> AssumedTemp)
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
            return Numbers.RoundToInt(tempUnit == TemperatureUnit.Fahrenheit ?
                ConversionTools.ToFahrenheit(tempCelsuis) : tempCelsuis);
        }

        private int LengthConvertUnit(double lengthMeter, LengthUnit unit)
        {
            return Numbers.RoundToInt(unit == LengthUnit.Feet ?
                lengthMeter * MeterFtRatio : lengthMeter);
        }
    }
}
