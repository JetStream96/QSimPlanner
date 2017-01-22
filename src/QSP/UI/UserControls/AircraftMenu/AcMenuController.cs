using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Common;
using QSP.LibraryExtension;
using QSP.MathTools;
using QSP.UI.MsgBox;
using QSP.Utilities;
using QSP.Utilities.Units;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QSP.UI.UserControls.AircraftMenu
{
    // The aircraft configs are saved in PerformanceData/Aircrafts. They are split 
    // into two folders, 'Custom' and 'Default'. This is to enable updater to add new configs or
    // update configs that were never edited by the user. If two configs have the same 
    // registration, the one in 'Custom' folder shadows the one in 'Default' folder.
    // All configs shipped with this app is in 'Default' folder. 
    //
    // User actions:
    // (1) When user creates a config, it's saved in 'Custom' folder.
    // (2) When user deletes a config, the file is deleted. In addition, if it's in 'Custom' 
    //     folder, we check the 'Default' folder for any file with the same registration, and load 
    //     it if exists.
    // (3) When user edits a config, the file is saved in 'Custom' folder and the original one
    //     is deleted.

    public class AcMenuController
    {
        public const string NoToLdgProfileText = "None";

        public event EventHandler AircraftsChanged;

        private readonly AcMenuElements elem;
        private readonly ProfileCollection profiles;
        private AircraftConfig currentConfig;

        private Control ParentControl => elem.ParentControl;

        public AcMenuController(AcMenuElements elem, ProfileCollection profiles)
        {
            this.elem = elem;
            this.profiles = profiles;
        }

        public void InitializeControls()
        {
            ShowSelectionGroupBox();
            InitWtUnitCBox();
        }

        private void FillFuelTOLdgCBox()
        {
            var fuelItems = elem.FuelProfile.Items;
            fuelItems.Clear();
            fuelItems.Add(NoToLdgProfileText);
            fuelItems.AddRange(profiles.FuelData
                .Select(t => t.ProfileName)
                .Distinct()
                .OrderBy(s => s)
                .ToArray());
            elem.FuelProfile.SelectedIndex = 0;

            var toItems = elem.ToProfile.Items;
            toItems.Clear();
            toItems.Add(NoToLdgProfileText);
            toItems.AddRange(profiles.TOTables
                .Select(t => t.Entry.ProfileName)
                .Distinct()
                .OrderBy(s => s)
                .ToArray());
            elem.ToProfile.SelectedIndex = 0;

            var ldgItems = elem.LdgProfile.Items;
            ldgItems.Clear();
            ldgItems.Add(NoToLdgProfileText);
            ldgItems.AddRange(profiles.LdgTables
                .Select(t => t.Entry.ProfileName)
                .Distinct()
                .OrderBy(s => s)
                .ToArray());
            elem.LdgProfile.SelectedIndex = 0;
        }

        private void InitWtUnitCBox()
        {
            var cbox = elem.WeightUnitCBox;
            cbox.Items.Clear();
            cbox.Items.AddRange(new[] { "KG", "LB" });
            cbox.SelectedIndex = 0;

            cbox.SelectedIndexChanged += WtUnitChanged;
        }

        private void WtUnitChanged(object sender, EventArgs e)
        {
            var factor = elem.WeightUnitCBox.SelectedIndex == 0 ?
                Constants.LbKgRatio :
                Constants.KgLbRatio;

            var textBoxes = new[] { elem.Oew,
                elem.MaxToWt, elem.MaxLdgWt, elem.MaxZfw, elem.MaxFuel };

            foreach (var j in textBoxes)
            {
                double wt;

                if (double.TryParse(j.Text, out wt))
                {
                    j.Text = Numbers.RoundToInt(wt * factor).ToString();
                }
            }
        }

        private void FillAcTypes()
        {
            var acItems = elem.AcType.Items;
            acItems.Clear();

            var ac = profiles.AcConfigs.Aircrafts;

            var acTypes = ac
                .Select(x => x.Config.AC)
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            acItems.AddRange(acTypes);
        }

        private void RefreshListView()
        {
            var listItems = elem.AcListView.Items;
            listItems.Clear();

            var ac = profiles.AcConfigs.Aircrafts.ToList();
            ac.Sort(ConfigComparers.ConfigComparer());

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

        private void FillProperties(AircraftConfigItem config)
        {
            var e = elem;
            var c = config;

            // This is needed, so that if the FuelProfile, ToProfile or LdgProfile is not 
            // found in the corresponding ComboBox, it will show "None".
            e.FuelProfile.SelectedIndex = 0;
            e.ToProfile.SelectedIndex = 0;
            e.LdgProfile.SelectedIndex = 0;

            e.AcType.Text = c.AC;
            e.Registration.Text = c.Registration;
            e.FuelProfile.Text = c.FuelProfile;
            e.ToProfile.Text = c.TOProfile;
            e.LdgProfile.Text = c.LdgProfile;
            e.WeightUnitCBox.SelectedIndex = (int)config.WtUnit;
            e.Oew.Text = WtDisplay(c.OewKg);
            e.MaxToWt.Text = WtDisplay(c.MaxTOWtKg);
            e.MaxLdgWt.Text = WtDisplay(c.MaxLdgWtKg);
            e.MaxZfw.Text = WtDisplay(c.MaxZfwKg);
            e.MaxFuel.Text = WtDisplay(c.MaxFuelKg);
            e.Bias.Text = (c.FuelBias * 100.0).ToString("F1");
        }

        private void ShowDefaultConfig()
        {
            FillProperties(DefaultAcConfig);
        }

        private AircraftConfigItem DefaultAcConfig
        {
            get
            {
                return new AircraftConfigItem("", "", NoToLdgProfileText,
                    NoToLdgProfileText, NoToLdgProfileText, 0.0, 0.0,
                    0.0, 0.0, 0.0, 1.0, WeightUnit.KG);
            }
        }

        private string WtDisplay(double weightKg)
        {
            var factor = elem.WeightUnitCBox.SelectedIndex == 0 ? 1.0 : Constants.KgLbRatio;
            return Numbers.RoundToInt(weightKg * factor).ToString();
        }

        public void CreateConfig(object sender, EventArgs e)
        {
            ShowPropertyGroupBox();
            ShowDefaultConfig();
            currentConfig = null;
        }

        private string SelectedRegistration
        {
            get
            {
                var selected = elem.AcListView.SelectedItems;
                if (selected.Count == 0) return null;
                return selected[0].SubItems[1].Text;
            }
        }

        private void ShowSelectionGroupBox()
        {
            elem.SelectionBox.Visible = true;
            elem.PropertyBox.Visible = false;

            RefreshListView();
        }

        private void ShowPropertyGroupBox()
        {
            elem.PropertyBox.Visible = true;
            elem.SelectionBox.Visible = false;

            FillAcTypes();
            FillFuelTOLdgCBox();
        }

        private bool TrySaveConfig(AircraftConfigItem config, string filePath)
        {
            try
            {
                var serializer = new AircraftConfigItem.Serializer();
                var elem = serializer.Serialize(config, "Config");
                var doc = new XDocument(elem);
                File.WriteAllText(filePath, doc.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void EditConfig(object sender, EventArgs e)
        {
            var reg = SelectedRegistration;

            if (reg != null)
            {
                currentConfig = profiles.AcConfigs.Find(reg);

                ShowPropertyGroupBox();
                FillProperties(currentConfig.Config);
            }
        }

        private void RemoveOldConfig()
        {
            if (InEditMode)
            {
                profiles.AcConfigs.Remove(currentConfig.Config.Registration);
            }
        }

        // If false, then user is creating a new config.
        private bool InEditMode => currentConfig != null;

        // If user is creating a new aircraft config, generate an appropriate file name.
        // Otherwise, returns the old file name.
        /// <exception cref="NoFileNameAvailException"></exception>
        private string GetFileName()
        {
            // Editing a custom profile.
            if (InEditMode && !currentConfig.IsDefault) return currentConfig.FilePath;

            var nameBase = (elem.AcType.Text + "_" + elem.Registration.Text)
                .RemoveIllegalFileNameChars();

            var dir = ConfigLoader.CustomFolderPath;
            return FileNameGenerator.Generate(dir, nameBase, ".xml", (i) => "_" + i.ToString());
        }

        private AircraftConfigItem TryValidate()
        {
            try
            {
                return new AcConfigValidator(elem).Validate();
            }
            catch (InvalidUserInputException ex)
            {
                ParentControl.ShowWarning(ex.Message);
                return null;
            }
        }

        private string TryGetFileName()
        {
            try
            {
                return GetFileName();
            }
            catch (NoFileNameAvailException)
            {
                // FileNameGenerator cannot generate a file name.
                ParentControl.ShowError("Failed to save config file.");
                return null;
            }
        }

        private void DeleteCurrentConfigFile()
        {
            var file = currentConfig.FilePath;

            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                LoggerInstance.Log(e);
                ParentControl.ShowWarning("The config was saved but the old config cannot" +
                    $"be deleted. Please manually delete {Path.GetFullPath(file)}.");
            }
        }

        public void SaveConfig(object sender, EventArgs e)
        {
            var config = TryValidate();
            if (config == null) return;

            // New profile must have a unique registration.
            if (!InEditMode && profiles.AcConfigs.Find(config.Registration) != null)
            {
                ParentControl.ShowWarning("Registration already exists. Please use another one.");
                return;
            }

            var fn = TryGetFileName();
            if (fn == null) return;

            if (TrySaveConfig(config, fn))
            {
                if (InEditMode && fn != currentConfig.FilePath) DeleteCurrentConfigFile();
                RemoveOldConfig();
                profiles.AcConfigs.Add(new AircraftConfig(config, fn));
                ShowSelectionGroupBox();
                AircraftsChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ParentControl.ShowError("Failed to save config file.");
            }
        }

        public void DeleteConfig(object sender, EventArgs e)
        {
            var reg = SelectedRegistration;
            if (reg == null) return;

            var configs = profiles.AcConfigs;
            var item = configs.Find(reg);
            var path = item.FilePath;
            var ac = item.Config.AC;

            var result = ParentControl.ShowDialog(
                $"Permanently delete {reg} ({ac}) ?",
                MsgBoxIcon.Warning,
                "",
                DefaultButton.Button2,
                "Delete", "Cancel");

            if (result == MsgBoxResult.Button1 && TryDeleteConfig(path))
            {
                configs.Remove(reg);
                ReadShadowedProfile(reg);
                RefreshListView();
                AircraftsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ReadShadowedProfile(string registration)
        {
            var config = ConfigLoader.Find(registration);
            if (config == null) return;

            // We skip the profile validation here. If fuel, takeoff or landing profile 
            // cannot be found, they will appear as 'None' when user edits it.

            profiles.AcConfigs.Add(config);
        }

        private bool TryDeleteConfig(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                ParentControl.ShowError("Failed to delete the selected config.");
                return false;
            }
        }

        private bool ChangesMade()
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

            const double deltaWt = 1.0;
            const double deltaBias = 0.0001;

            if (InEditMode) return !config.Equals(currentConfig.Config, deltaWt, deltaBias);
            return !config.Equals(DefaultAcConfig, deltaWt, deltaBias);
        }

        public void CancelBtnClicked(object sender, EventArgs e)
        {
            if (ChangesMade() == false)
            {
                // No edit is done.
                // No need to show messageBox.
                ShowSelectionGroupBox();
                return;
            }

            var result = ParentControl.ShowDialog(
                "Discard the changes to config?",
                MsgBoxIcon.Warning,
                "",
                DefaultButton.Button1,
                "Discard", "Save", "Cancel");

            if (result == MsgBoxResult.Button1)
            {
                ShowSelectionGroupBox();
            }
            else if (result == MsgBoxResult.Button2)
            {
                SaveConfig(this, EventArgs.Empty);
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
