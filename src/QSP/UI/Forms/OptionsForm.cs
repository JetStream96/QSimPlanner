using QSP.Common.Options;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.FileExport;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static QSP.UI.FormInstanceGetter;
using static QSP.Utilities.LoggerInstance;

namespace QSP
{
    public partial class OptionsForm
    {
        public event EventHandler AppSettingChanged;
        public AppOptions AppSettings { get; private set; }

        private Dictionary<string, ProviderType> exportTypes =
               new Dictionary<string, ProviderType>();

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void initExportTypes()
        {
            exportTypes.Add("PmdgCommon", ProviderType.Pmdg);
            exportTypes.Add("PmdgNGX", ProviderType.Pmdg);
            exportTypes.Add("Pmdg777", ProviderType.Pmdg);
        }

        private void navDataPathTxtBox_TextChanged(object sender, EventArgs e)
        {
            displayNavDataStatus(navDataPathTxtBox.Text);
        }

        private void displayNavDataStatus(string navDataPath)
        {
            // This section is to determine whether the database
            // files are found or not.
            bool navDataFound = true;

            string[] FilesToCheck = {
                "airports.txt",
                "ats.txt",
                "cycle.txt",
                "navaids.txt",
                "waypoints.txt"
            };

            foreach (var i in FilesToCheck)
            {
                if (File.Exists(navDataPath + @"\" + i) == false)
                {
                    navDataFound = false;
                    break;
                }
            }

            navDataFoundLbl.Text = navDataFound ? "Found" : "Not Found";

            try
            {
                Tuple<string, string> t = AiracCyclePeriod(navDataPath);
                airacLbl.Text = t.Item1;
                airacPeriodLbl.Text = t.Item2;

                bool expired = !AiracTools.AiracValid(t.Item2);
                if (expired)
                {
                    expiredLbl.Text = "(Expired)";
                    expiredLbl.ForeColor = Color.Red;
                }
                else
                {
                    expiredLbl.Text = "(Within Valid Period)";
                    expiredLbl.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);

                navDataFoundLbl.Text = "Failed to load";
                airacLbl.Text = "N/A";
                airacPeriodLbl.Text = "N/A";
                expiredLbl.Text = "";
            }
        }

        /// <summary>
        /// Reads from file and gets the AIRAC cycle and valid period. 
        /// e.g. The first item in tuple can be 1407, second item can be 26JUN23JUL/14.
        /// </summary>
        /// <param name="DBFilepath">The folder containing cycle.txt.</param>
        public static Tuple<string, string> AiracCyclePeriod(string DBFilepath)
        {
            string str = File.ReadAllText(DBFilepath + "/cycle.txt");
            var s = str.Split(',');
            return new Tuple<string, string>(s[0], s[1]);
        }

        private void browseFolderBtn_Click(object sender, EventArgs e)
        {
            var MyFolderBrowser = new FolderBrowserDialog();
            var dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                navDataPathTxtBox.Text = MyFolderBrowser.SelectedPath;
            }
        }

        // TODO: this is totally wrong
        private void okBtnClick(object sender, EventArgs e)
        {
            var newSetting = validateSetting();

            // TODO:
            // If the database path is changed, then load the database. 
            // Otherwise do not reload the same database.
            AppSettings = newSetting;

            if (OptionManager.TrySaveFile(AppSettings))
            {
                AppSettings.NavDataLocation = navDataPathTxtBox.Text;
                MainFormInstance().LoadNavDBUpdateStatusStrip(false);
                AppSettingChanged?.Invoke(this, e);
            }

            Close();
        }

        private AppOptions validateSetting()
        {
            return new AppOptions(
                navDataPathTxtBox.Text,
                PromptBeforeExit.Checked,
                AutoDLTracksCheckBox.Checked,
                AutoDLWindCheckBox.Checked,
                getCommands());
        }

        private Dictionary<string, ExportCommand> getCommands()
        {
            var cmds = new Dictionary<string, ExportCommand>();

            cmds.Add("PmdgCommon",
                new ExportCommand(
                    exportTypes["PmdgCommon"],
                    TextBox1.Text,
                    CheckBox1.Checked));

            cmds.Add("PmdgNGX",
                new ExportCommand(
                    exportTypes["PmdgNGX"],
                    TextBox2.Text,
                    CheckBox2.Checked));

            cmds.Add("Pmdg777",
                new ExportCommand(
                    exportTypes["Pmdg777"],
                    TextBox2.Text,
                    CheckBox2.Checked));

            return cmds;
        }

        /// <summary>
        /// Set the AppSettings as in the option form. Returns whether 
        /// the nav database location was modified.
        /// </summary>
        //public bool SetAppOptions()
        //{
        //    AppSettings.ExportCommands.Clear();

        //    AppSettings.ExportCommands.Add(
        //        new ExportCommandEntry(
        //            new ExportCommand(
        //                exportTypes["PmdgCommon"],
        //                TextBox1.Text,
        //                CheckBox1.Checked),
        //            "PmdgCommon"));

        //    AppSettings.ExportCommands.Add(
        //        new ExportCommandEntry(
        //            new ExportCommand(
        //                exportTypes["PmdgNGX"],
        //                TextBox2.Text,
        //                CheckBox2.Checked),
        //            "PmdgNGX"));

        //    AppSettings.ExportCommands.Add(
        //        new ExportCommandEntry(
        //            new ExportCommand(
        //                exportTypes["Pmdg777"],
        //                TextBox2.Text,
        //                CheckBox2.Checked),
        //            "Pmdg777"));

        //    AppSettings.PromptBeforeExit = PromptBeforeExit.Checked;
        //    AppSettings.AutoDLTracks = AutoDLTracksCheckBox.Checked;
        //    AppSettings.AutoDLWind = AutoDLWindCheckBox.Checked;

        //    if (AppSettings.NavDataLocation == navDataPathTxtBox.Text)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        AppSettings.NavDataLocation = navDataPathTxtBox.Text;
        //        return true;
        //    }

        //}

        private void cancelBtnClick(object sender, EventArgs e)
        {
            Close();
        }

        public void Init(AppOptions options)
        {
            this.AppSettings = options;

            //default form state
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            CheckBox3.Checked = false;
            TextBox1.Enabled = false;
            TextBox2.Enabled = false;
            TextBox3.Enabled = false;
            PromptBeforeExit.Checked = true;

            navDataFoundLbl.Text = "";
            airacLbl.Text = "";
            airacPeriodLbl.Text = "";
            expiredLbl.Text = "";

            AutoDLTracksCheckBox.Checked = true;
            AutoDLWindCheckBox.Checked = true;

            SetControlsAsInOptions();
            initExportTypes();
        }

        public void SetControlsAsInOptions()
        {
            AutoDLTracksCheckBox.Checked = AppSettings.AutoDLTracks;
            AutoDLWindCheckBox.Checked = AppSettings.AutoDLWind;
            navDataPathTxtBox.Text = AppSettings.NavDataLocation;
            PromptBeforeExit.Checked = AppSettings.PromptBeforeExit;

            try
            {
                var command = AppSettings.GetExportCommand("PmdgCommon");
                TextBox1.Text = command.Directory;
                CheckBox1.Checked = command.Enabled;
            }
            catch
            { }

            try
            {
                var command = AppSettings.GetExportCommand("PmdgNGX");
                TextBox2.Text = command.Directory;
                CheckBox2.Checked = command.Enabled;
            }
            catch
            { }

            try
            {
                var command = AppSettings.GetExportCommand("Pmdg777");
                TextBox3.Text = command.Directory;
                CheckBox3.Checked = command.Enabled;
            }
            catch
            { }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var MyFolderBrowser = new FolderBrowserDialog();
            var dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                TextBox1.Text = MyFolderBrowser.SelectedPath;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var MyFolderBrowser = new FolderBrowserDialog();
            var dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                TextBox2.Text = MyFolderBrowser.SelectedPath;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var MyFolderBrowser = new FolderBrowserDialog();
            var dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                TextBox3.Text = MyFolderBrowser.SelectedPath;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            TextBox1.Enabled = CheckBox1.Checked;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            TextBox2.Enabled = CheckBox2.Checked;
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            TextBox3.Enabled = CheckBox3.Checked;
        }

        private class RouteExportMatching
        {

        }
    }
}
