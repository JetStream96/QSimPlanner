using System;
using System.Windows.Forms;
using static QSP.AviationTools.CoversionTools;
using static QSP.MathTools.Doubles;

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
            tempUnitComboBox.Items.AddRange(new string[] { "°C", "°F" });
            tempUnitComboBox.SelectedIndex = 0; // Celsius

            pressUnitComboBox.Items.Clear();
            pressUnitComboBox.Items.AddRange(new string[] { "hPa", "inHg" });
            pressUnitComboBox.SelectedIndex = 0; // hPa

            AddToolTip();
        }

        private void AddToolTip()
        {
            var tp = new ToolTip();

            tp.AutoPopDelay = 5000;
            tp.InitialDelay = 1000;
            tp.ReshowDelay = 500;
            tp.ShowAlways = true;

            tp.SetToolTip(GetMetarBtn,
                "Download METAR and fill all weather info.");
        }

        private void tempUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
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

        private void pressUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
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
