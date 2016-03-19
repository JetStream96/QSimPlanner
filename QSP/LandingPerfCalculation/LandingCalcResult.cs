using System.Collections.Generic;
using System.Text;

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
            var unitStr = lengthUnit == LengthUnit.Meter ? "M" : "FT";
            var result = new StringBuilder();

            result.AppendLine();

            result.AppendLine("           Actual landing distance    " +
                convertToFeetIfNeeded(selectedBrks.ActualDisMeter, lengthUnit)
                + " " + unitStr);

            result.AppendLine("           Runway remaining           " +
                convertToFeetIfNeeded(selectedBrks.DisRemainMeter, lengthUnit)
                + " " + unitStr);

            result.AppendLine();
            result.AppendLine(new string('-', 60));
            result.AppendLine("                    (OTHER BRK SETTINGS)");
            result.AppendLine("                  Landing distance   Runway remaining");


            foreach (var i in allSettings)
            {
                result.Append(("   " + i.BrkSetting).PadRight(18, ' '));

                result.Append((convertToFeetIfNeeded(i.ActualDisMeter, lengthUnit)
                               .ToString() +
                               " " +
                               unitStr)
                               .PadLeft(12, ' ')
                               .PadRight(19, ' '));

                result.AppendLine(((convertToFeetIfNeeded(i.DisRemainMeter, lengthUnit) +
                                    " " +
                                    unitStr)
                                    .ToString())
                                    .PadLeft(11, ' '));
            }
            return result.ToString();
        }

        private int convertToFeetIfNeeded(int disMeter, LengthUnit unit)
        {
            return unit == LengthUnit.Meter ?
                disMeter :
                (int)(disMeter * AviationTools.Constants.MeterFtRatio);
        }

        private class dataRow
        {
            public string BrkSetting { get; private set; }
            public int ActualDisMeter { get; private set; }
            public int DisRemainMeter { get; private set; }

            public dataRow(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
            {
                this.BrkSetting = BrkSetting;
                this.ActualDisMeter = ActualDisMeter;
                this.DisRemainMeter = DisRemainMeter;
            }
        }
    }
}
