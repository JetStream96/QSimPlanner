using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuController
    {
        public static string NoToLdgProfileText = "None";

        private AcMenuElements elem;
        private ProfileManager profiles;
        private AircraftConfig config;

        public AcMenuController(AcMenuElements elem, ProfileManager profiles)
        {
            this.elem = elem;
            this.profiles = profiles;
        }

        public void InitializeControls()
        {
            elem.SelectionBox.Location = new Point(0, 0);
            elem.PropertyBox.Location = new Point(0, 0);
            showSelectionGroupBox();
                        
            initWtUnitCBox();
        }

        private void fillToLdgCBox()
        {
            var toItems = elem.ToProfile.Items;
            toItems.Clear();
            toItems.Add(NoToLdgProfileText);
            toItems.AddRange(profiles.TOTables
                .Select(t => t.Entry.ProfileName).ToArray());
            elem.ToProfile.SelectedIndex = 0;

            var ldgItems = elem.LdgProfile.Items;
            ldgItems.Clear();
            ldgItems.Add(NoToLdgProfileText);
            ldgItems.AddRange(profiles.LdgTables
                .Select(t => t.Entry.ProfileName).ToArray());
            elem.LdgProfile.SelectedIndex = 0;
        }

        private void initWtUnitCBox()
        {
            var units = new string[] { "KG", "LB" };

            var zfwUnit = elem.ZfwUnit;
            zfwUnit.Items.Clear();
            zfwUnit.Items.AddRange(units);
            zfwUnit.SelectedIndex = 0;

            var toUnit = elem.MaxToWtUnit;
            toUnit.Items.Clear();
            toUnit.Items.AddRange(units);
            toUnit.SelectedIndex = 0;

            var ldgUnit = elem.MaxLdgWtUnit;
            ldgUnit.Items.Clear();
            ldgUnit.Items.AddRange(units);
            ldgUnit.SelectedIndex = 0;

            wtUnitConnect();
        }

        private void wtUnitConnect()
        {
            elem.ZfwUnit.SelectedIndexChanged += wtUnitChanged;
            elem.MaxToWtUnit.SelectedIndexChanged += wtUnitChanged;
            elem.MaxLdgWtUnit.SelectedIndexChanged += wtUnitChanged;
        }

        private void wtUnitDisconnect()
        {
            elem.ZfwUnit.SelectedIndexChanged -= wtUnitChanged;
            elem.MaxToWtUnit.SelectedIndexChanged -= wtUnitChanged;
            elem.MaxLdgWtUnit.SelectedIndexChanged -= wtUnitChanged;
        }

        private void wtUnitChanged(object sender, EventArgs e)
        {
            var c = new ComboBox[] { elem.ZfwUnit,
                elem.MaxToWtUnit, elem.MaxLdgWtUnit };

            wtUnitDisconnect();

            int selIndex = ((ComboBox)sender).SelectedIndex;

            foreach (var i in c)
            {
                if (i != sender)
                {
                    i.SelectedIndex = selIndex;
                }
            }

            convertWeights(selIndex);

            wtUnitConnect();
        }

        private void convertWeights(int selIndex)
        {
            var factor = selIndex == 0 ?
                            Constants.LbKgRatio :
                            Constants.KgLbRatio;

            var textBoxes = new TextBox[] { elem.Zfw,
                elem.MaxToWt, elem.MaxLdgWt };

            foreach (var j in textBoxes)
            {
                double wt;

                if (double.TryParse(j.Text, out wt))
                {
                    j.Text = RoundToInt(wt * factor).ToString();
                }
            }
        }

        private void fillAcTypes()
        {
            var acItems = elem.AcType.Items;
            acItems.Clear();

            acItems.AddRange(profiles.AcConfigs.Aircrafts.ToArray());
        }

        private void refreshListView()
        {
            var listItems = elem.AcListView.Items;
            listItems.Clear();

            foreach (var i in profiles.AcConfigs.Aircrafts)
            {
                var lvi = new ListViewItem(i.AC);
                lvi.SubItems.Add(i.Registration);
                // lvi.ImageIndex = (int)i.Severity;
                listItems.Add(lvi);
            }
        }

        //public void SelectedAcChanged(object sender, EventArgs e)
        //{
        //    selectedConfig = profiles.AcConfigs.Aircrafts
        //        .ElementAt(elem.AcListView.SelectedIndices[0]);

        //    fillProperties();
        //}

        private void fillProperties(AircraftConfig config)
        {
            var e = elem;
            var c = config;

            e.AcType.Text = c.AC;
            e.Registration.Text = c.Registration;
            e.ToProfile.Text = c.TOProfile;
            e.LdgProfile.Text = c.LdgProfile;
            e.ZfwUnit.SelectedIndex = c.WtUnit == WeightUnit.KG ? 0 : 1;
            e.Zfw.Text = wtDisplay(c.ZfwKg);
            e.MaxToWt.Text = wtDisplay(c.MaxTOWtKg);
            e.MaxLdgWt.Text = wtDisplay(c.MaxLdgWtKg);
        }

        private void showDefaultConfig()
        {
            config = new AircraftConfig("", "", NoToLdgProfileText,
                NoToLdgProfileText, 0.0, 0.0, 0.0, WeightUnit.KG);

            fillProperties(config);
        }

        private string wtDisplay(double weightKg)
        {
            if (elem.ZfwUnit.SelectedIndex == 0)
            {
                // KG
                return RoundToInt(weightKg).ToString();
            }
            else
            {
                // LB
                return RoundToInt(weightKg * Constants.KgLbRatio).ToString();
            }
        }

        public void CreateConfig(object sender, EventArgs e)
        {
            showPropertyGroupBox();
            showDefaultConfig();
        }

        //public void AcTypeChanged(object sender, EventArgs e)
        //{
        //    var lvi = elem.AcListView.SelectedItems[0];
        //    lvi.Text = elem.AcType.Text;
        //}

        public void RegistrationChanged(object sender, EventArgs e)
        {
            //var lvi = elem.AcListView.SelectedItems[0];
            //var si = lvi.SubItems;

            //si.RemoveAt(si.Count - 1);
            //si.Add(elem.Registration.Text);
        }

        private void showSelectionGroupBox()
        {
            elem.SelectionBox.Visible = true;
            elem.PropertyBox.Visible = false;

            refreshListView();
        }

        private void showPropertyGroupBox()
        {
            elem.PropertyBox.Visible = true;
            elem.SelectionBox.Visible = false;

            fillAcTypes();
            fillToLdgCBox();
        }

        public void SaveConfig(object sender, EventArgs e)
        {
            try
            {
                var config = new AcConfigValidator(elem).Validate();

                profiles.AcConfigs.Add(config);

                showSelectionGroupBox();
            }
            catch (InvalidUserInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(ArgumentException)
            {
                MessageBox.Show(
                    "Registration already exists. Please use another one.");
            }
        }
    }
}
