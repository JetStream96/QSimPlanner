using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Common;
using QSP.Utilities.Units;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;
using static QSP.UI.Utilities.MsgBoxHelper;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuController
    {
        public const string NoToLdgProfileText = "None";

        public event EventHandler AircraftsChanged;

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

            int index = ((ComboBox)sender).SelectedIndex;

            foreach (var i in c)
            {
                if (i != sender)
                {
                    i.SelectedIndex = index;
                }
            }

            convertWeights(index);

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

            var acTypes =
                ac.Select(x => x.Config.AC)
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            acItems.AddRange(acTypes);
        }

        private void refreshListView()
        {
            var listItems = elem.AcListView.Items;
            listItems.Clear();

            var ac = profiles.AcConfigs.Aircrafts.ToList();
            ac.Sort(new ConfigComparer());

            foreach (var i in ac)
            {
                var c = i.Config;

                var lvi = new ListViewItem(c.AC);
                lvi.SubItems.Add(c.Registration);
                listItems.Add(lvi);
            }

            ListViewSelectedChanged(null, null);
            elem.AcListView.Columns[1].Width = -2;
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
            e.Zfw.Text = wtDisplay(c.OewKg);
            e.MaxToWt.Text = wtDisplay(c.MaxTOWtKg);
            e.MaxLdgWt.Text = wtDisplay(c.MaxLdgWtKg);
        }

        private void showDefaultConfig()
        {
            fillProperties(defaultAcConfig);
        }

        private AircraftConfigItem defaultAcConfig
        {
            get
            {
                return new AircraftConfigItem("", "", NoToLdgProfileText,
                    NoToLdgProfileText, 0.0, 0.0, 0.0, WeightUnit.KG);
            }
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
                currentConfig = profiles.AcConfigs.Find(reg);

                showPropertyGroupBox();
                fillProperties(currentConfig.Config);
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

        /// <exception cref="NoFileNameAvailException"></exception>
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

        private AircraftConfigItem tryValidate()
        {
            try
            {
                return new AcConfigValidator(elem).Validate();
            }
            catch (InvalidUserInputException ex)
            {
                ShowWarning(ex.Message);
                return null;
            }
        }

        private string tryGetFileName()
        {
            try
            {
                return getFileName();
            }
            catch (NoFileNameAvailException)
            {
                // FileNameGenerator cannot generate a file name.
                ShowError("Failed to save config file.");
                return null;
            }
        }

        public void SaveConfig(object sender, EventArgs e)
        {
            var config = tryValidate();

            if (config == null)
            {
                return;
            }

            if (inEditMode == false &&
                profiles.AcConfigs.Find(config.Registration) != null)
            {
                ShowWarning(
                   "Registration already exists. Please use another one.");
                return;
            }

            var fn = tryGetFileName();

            if (fn == null)
            {
                return;
            }

            if (trySaveConfig(config, fn))
            {
                removeOldConfig();
                profiles.AcConfigs.Add(new AircraftConfig(config, fn));
                showSelectionGroupBox();
                AircraftsChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowError("Failed to save config file.");
            }
        }

        public void DeleteConfig(object sender, EventArgs e)
        {
            var reg = selectedRegistration;

            if (reg == null)
            {
                return;
            }

            var configs = profiles.AcConfigs;
            var item = configs.Find(reg);
            var path = item.FilePath;
            var ac = item.Config.AC;

            var result =
                MessageBox.Show(
                    $"Permanently delete {reg} ({ac}) ?",
                    "",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes &&
                tryDeleteConfig(path))
            {
                configs.Remove(reg);
                refreshListView();
                AircraftsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool tryDeleteConfig(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                ShowError("Failed to delete the selected config.");
                return false;
            }
        }

        private bool changesMade()
        {
            AircraftConfigItem config = null;

            try
            {
                config = new AcConfigValidator(elem).Read();
            }
            catch
            {
                return true;
            }

            if (inEditMode)
            {
                return !config.Equals(currentConfig.Config);
            }
            else
            {
                return !config.Equals(defaultAcConfig);
            }
        }

        public void CancelBtnClicked(object sender, EventArgs e)
        {
            if (changesMade() == false)
            {
                // No edit is done.
                // No need to show messageBox.
                showSelectionGroupBox();
                return;
            }

            var result =
                MessageBox.Show(
                    "Discard the changes to config?",
                    "",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                showSelectionGroupBox();
            }
        }

        public void ListViewSelectedChanged(object sender, EventArgs e)
        {
            var edit = elem.EditBtn;
            var del = elem.DeleteBtn;

            if (elem.AcListView.SelectedIndices.Count == 0)
            {
                edit.ForeColor = Color.White;
                edit.Enabled = false;
                del.ForeColor = Color.White;
                del.Enabled = false;
            }
            else
            {
                edit.Enabled = true;
                edit.ForeColor = Color.Black;
                del.Enabled = true;
                del.ForeColor = Color.Black;
            }
        }
    }
}
