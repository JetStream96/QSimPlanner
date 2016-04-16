using System.Collections.Generic;
using System.Text;

namespace QSP.LandingPerfCalculation
{
    public class LandingReport
    {
        public DataRow SelectedBrks { get; private set; }
        public List<DataRow> AllSettings { get; private set; }

        public LandingReport()
        {
            AllSettings = new List<DataRow>();
        }

        public void SetSelectedBrakesResult(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
        {
            SelectedBrks = new DataRow(BrkSetting, ActualDisMeter, DisRemainMeter);
        }

        public void AddOtherResult(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
        {
            AllSettings.Add(new DataRow(BrkSetting, ActualDisMeter, DisRemainMeter));
        }

        /// <summary>
        /// Add the result of selected brake to allSettings. Value of selectedBrks must be set already.
        /// </summary>
        public void AddOtherResult()
        {
            AllSettings.Add(SelectedBrks);
        }

        public string ToString(LengthUnit lengthUnit)
        {
            var unitStr = lengthUnit == LengthUnit.Meter ? "M" : "FT";
            var result = new StringBuilder();

            result.AppendLine();

            result.AppendLine("           Actual landing distance    " +
                convertToFeetIfNeeded(SelectedBrks.ActualDisMeter, lengthUnit)
                + " " + unitStr);

            result.AppendLine("           Runway remaining           " +
                convertToFeetIfNeeded(SelectedBrks.DisRemainMeter, lengthUnit)
                + " " + unitStr);

            result.AppendLine();
            result.AppendLine(new string('-', 60));
            result.AppendLine("                    (OTHER BRK SETTINGS)");
            result.AppendLine("                  Landing distance   Runway remaining");


            foreach (var i in AllSettings)
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

        public class DataRow
        {
            public string BrkSetting { get; private set; }
            public int ActualDisMeter { get; private set; }
            public int DisRemainMeter { get; private set; }

            public DataRow(string BrkSetting, int ActualDisMeter, int DisRemainMeter)
            {
                this.BrkSetting = BrkSetting;
                this.ActualDisMeter = ActualDisMeter;
                this.DisRemainMeter = DisRemainMeter;
            }
        }
    }
}
