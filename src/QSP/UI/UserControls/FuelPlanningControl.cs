using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;
using QSP.AircraftProfiles.Configs;
using QSP.AircraftProfiles;
using QSP.FuelCalculation;

namespace QSP.UI.UserControls
{
    public partial class FuelPlanningControl : UserControl
    {
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        private WeightTextBoxController zfw;
        private WeightController weightControl;
        private AcConfigManager aircrafts;
        private IEnumerable<FuelData> fuelData;

        public FuelPlanningControl()
        {
            InitializeComponent();
        }

        public void Init(
            AcConfigManager aircrafts, IEnumerable<FuelData> fuelData)
        {
            this.aircrafts = aircrafts;
            this.fuelData = fuelData;

            SetWeightController();

            acListComboBox.Items.Clear();
            acListComboBox.Items.AddRange(
                aircrafts.Aircrafts
                .Select(c => c.Config.AC)
                .ToArray());

            
            acListComboBox.SelectedIndexChanged += RefreshRegistrations;
            registrationComboBox.SelectedIndexChanged += RegistrationChanged;

            if (acListComboBox.Items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }
        }

        private void SetWeightController()
        {
            oew = new WeightTextBoxController(oewTxtBox);
            payload = new WeightTextBoxController(payloadTxtBox);
            zfw = new WeightTextBoxController(zfwTxtBox);

            weightControl = new WeightController(
                oew, payload, zfw, payloadTrackBar);
            weightControl.Enable();
        }

        private bool FuelProfileExists(string profileName)
        {
            return fuelData.Any(c => c.ProfileName == profileName);
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac =
                    aircrafts
                    .FindAircraft(acListComboBox.Text);

                var items = registrationComboBox.Items;

                items.Clear();

                items.AddRange(
                    ac
                    .Where(c => FuelProfileExists(c.Config.TOProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());

                if (items.Count > 0)
                {
                    registrationComboBox.SelectedIndex = 0;
                }
            }
        }

        private void RegistrationChanged(object sender, EventArgs e)
        {
            if (registrationComboBox.SelectedIndex < 0)
            {
                //RefreshWtColor();
                return;
            }

            weightControl.AircraftConfig =
                aircrafts.Find(registrationComboBox.Text).Config;
        }
    }
}
