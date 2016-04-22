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
        private AircraftConfig currentConfig;

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

            var ac = profiles.AcConfigs.Aircrafts;
            var acTypes = ac.Select(x => x.Config.AC).Distinct().ToArray();
            acItems.AddRange(acTypes);
        }

        private void refreshListView()
        {
            var listItems = elem.AcListView.Items;
            listItems.Clear();

            foreach (var i in profiles.AcConfigs.Aircrafts)
            {
                var c = i.Config;

                var lvi = new ListViewItem(c.AC);
                lvi.SubItems.Add(c.Registration);
                // lvi.ImageIndex = (int)i.Severity;
                listItems.Add(lvi);
            }
        }

        private void fillProperties(AircraftConfigItem config)
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
            var defaultConfig = new AircraftConfigItem("", "", NoToLdgProfileText,
                 NoToLdgProfileText, 0.0, 0.0, 0.0, WeightUnit.KG);

            fillProperties(defaultConfig);
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
            currentConfig = null;
        }

        private string selectedRegistration
        {
            get
            {
                var selected = elem.AcListView.SelectedItems;

                if (selected.Count == 0)
                {
                    return null;
                }

                return selected[0].SubItems[1].Text;
            }
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

        private bool trySaveConfig(AircraftConfigItem config, string filePath)
        {
            try
            {
                ConfigSaver.Save(config, filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void EditConfig(object sender, EventArgs e)
        {
            var reg = selectedRegistration;

            if (reg != null)
            {
                currentConfig = profiles.AcConfigs.FindRegistration(reg);

                fillProperties(currentConfig.Config);
                showPropertyGroupBox();
            }
        }

        private void removeOldConfig()
        {
            if (inEditMode)
            {
                profiles.AcConfigs.Remove(currentConfig.Config.Registration);
            }
        }

        // If false, then user is creating a new config.
        private bool inEditMode
        {
            get
            {
                return currentConfig != null;
            }
        }

        private string getFileName()
        {
            if (inEditMode == false)
            {
                return FileNameGenerator.Generate(
                    ConfigLoader.DefaultFolderPath,
                    elem.AcType.Text,
                    elem.Registration.Text);
            }
            else
            {
                return currentConfig.FilePath;
            }
        }

        public void SaveConfig(object sender, EventArgs e)
        {
            try
            {
                var config = new AcConfigValidator(elem).Validate();
                string fn = getFileName();

                if (trySaveConfig(config, fn))
                {
                    removeOldConfig();
                    profiles.AcConfigs.Add(new AircraftConfig(config, fn));
                    showSelectionGroupBox();
                }
                else
                {
                    MessageBox.Show("Failed to save config file.");
                }
            }
            catch (InvalidUserInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(
                    "Registration already exists. Please use another one.");
            }
            catch (NoFileNameAvailException)
            {
                // FileNameGenerator cannot generate a file name.
                MessageBox.Show("Failed to save config file.");
            }
        }
    }
}
