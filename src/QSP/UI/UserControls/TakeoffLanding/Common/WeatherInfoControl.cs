using System;
using System.Windows.Forms;
using QSP.AviationTools;
using QSP.MathTools;
using QSP.UI.Factories;

namespace QSP.UI.UserControls.TakeoffLanding.Common
{
    public partial class WeatherInfoControl : UserControl
    {
        public WeatherInfoControl()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            windSpdTxtBox.Text = "0";
            windDirTxtBox.Text = "0";

            tempUnitComboBox.Items.Clear();
            tempUnitComboBox.Items.AddRange(new[] { "°C", "°F" });
            tempUnitComboBox.SelectedIndex = 0; // Celsius

            pressUnitComboBox.Items.Clear();
            pressUnitComboBox.Items.AddRange(new[] { "hPa", "inHg" });
            pressUnitComboBox.SelectedIndex = 0; // hPa

            AddToolTip();
        }

        private void AddToolTip()
        {
            var tp = ToolTipFactory.GetToolTip();
            tp.SetToolTip(GetMetarBtn, "Download METAR and fill all weather info.");
        }

        private void tempUnitComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (double.TryParse(oatTxtBox.Text, out var temp))
            {
                if (tempUnitComboBox.SelectedIndex == 0)
                {
                    // deg F -> deg C
                    temp = ConversionTools.ToCelsius(temp);
                }
                else
                {
                    // deg C -> deg F
                    temp = ConversionTools.ToFahrenheit(temp);
                }

                oatTxtBox.Text = Numbers.RoundToInt(temp).ToString();
            }
        }

        private void pressUnitComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (double.TryParse(pressTxtBox.Text, out var press))
            {
                if (pressUnitComboBox.SelectedIndex == 0)
                {
                    // inHg -> hPa
                    press *= 1013.0 / 29.92;
                    pressTxtBox.Text = Numbers.RoundToInt(press).ToString();
                }
                else
                {
                    // hPa -> inHg
                    press *= 29.92 / 1013.0;
                    pressTxtBox.Text = Math.Round(press, 2).ToString("0.00");
                }
            }
        }
    }
}
