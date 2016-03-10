using System;
using System.Windows.Forms;
using static QSP.AviationTools.CoversionTools;

namespace QSP.UI.ToLdgModule.Common
{
    public partial class WeatherInfoControl : UserControl
    {
        public WeatherInfoControl()
        {
            InitializeComponent();
            initializeControls();
        }

        private void initializeControls()
        {
            windSpdTxtBox.Text = "0";
            windDirTxtBox.Text = "0";

            tempUnitComboBox.Items.Clear();
            tempUnitComboBox.Items.AddRange(new object[] { "°C", "°F" });
            tempUnitComboBox.SelectedIndex = 0; // Celsius

            pressUnitComboBox.Items.Clear();
            pressUnitComboBox.Items.AddRange(new object[] { "hPa", "inHg" });
            pressUnitComboBox.SelectedIndex = 0; // hPa
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

                oatTxtBox.Text = ((int)Math.Round(temp)).ToString();
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
                    press *= 1013 / 29.92;
                    pressTxtBox.Text = ((int)Math.Round(press)).ToString();
                }
                else
                {
                    // hPa -> inHg
                    press *= 29.92 / 1013;
                    pressTxtBox.Text = Math.Round(press, 2).ToString("0.00");
                }
            }
        }
    }
}
