using System;
using System.Windows.Forms;
using static QSP.AviationTools.ConversionTools;
using static QSP.MathTools.Doubles;
using static QSP.UI.Factories.ToolTipFactory;

namespace QSP.UI.ToLdgModule.Common
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
            var tp = GetToolTip();
            tp.SetToolTip(GetMetarBtn, "Download METAR and fill all weather info.");
        }

        private void tempUnitComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            double temp;

            if (double.TryParse(oatTxtBox.Text, out temp))
            {
                if (tempUnitComboBox.SelectedIndex == 0)
                {
                    // deg F -> deg C
                    temp = ToCelsius(temp);
                }
                else
                {
                    // deg C -> deg F
                    temp = ToFahrenheit(temp);
                }

                oatTxtBox.Text = RoundToInt(temp).ToString();
            }
        }

        private void pressUnitComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            double press;

            if (double.TryParse(pressTxtBox.Text, out press))
            {
                if (pressUnitComboBox.SelectedIndex == 0)
                {
                    // inHg -> hPa
                    press *= 1013.0 / 29.92;
                    pressTxtBox.Text = RoundToInt(press).ToString();
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
