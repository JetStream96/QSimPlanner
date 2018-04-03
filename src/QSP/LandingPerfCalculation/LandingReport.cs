using QSP.Utilities.Units;
using System.Collections.Generic;
using System.Text;

namespace QSP.LandingPerfCalculation
{
    public class LandingReport
    {
        public ReportRow SelectedBrake { get; set; }
        public List<ReportRow> AllBrakes { get; set; }

        public string ToString(LengthUnit lengthUnit)
        {
            var unitStr = lengthUnit == LengthUnit.Meter ? "M" : "FT";
            var result = new StringBuilder();

            result.AppendLine();

            result.AppendLine("           Actual landing distance    " +
                ConvertToFeetIfNeeded(SelectedBrake.RequiredDistanceMeter, lengthUnit)
                + " " + unitStr);

            result.AppendLine("           Runway remaining           " +
                ConvertToFeetIfNeeded(SelectedBrake.RemainingDistanceMeter, lengthUnit)
                + " " + unitStr);

            result.AppendLine();
            result.AppendLine(new string('-', 60));
            result.AppendLine("                    (OTHER BRK SETTINGS)");
            result.AppendLine("                  Landing distance   Runway remaining");


            foreach (var i in AllBrakes)
            {
                result.Append(("   " + i.BrakeSetting).PadRight(18, ' '));

                result.Append((ConvertToFeetIfNeeded(i.RequiredDistanceMeter, lengthUnit)
                               .ToString() +
                               " " +
                               unitStr)
                               .PadLeft(12, ' ')
                               .PadRight(19, ' '));

                result.AppendLine(
                    (ConvertToFeetIfNeeded(i.RemainingDistanceMeter, lengthUnit) + " " + unitStr)
                    .ToString()
                    .PadLeft(11, ' '));
            }
            return result.ToString();
        }

        private int ConvertToFeetIfNeeded(int disMeter, LengthUnit unit)
        {
            return unit == LengthUnit.Meter ?
                disMeter :
                (int)(disMeter * AviationTools.Constants.MeterFtRatio);
        }
    }
}
