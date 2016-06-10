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

        private IEnumerable<RouteExportMatching> exports;

        public OptionsForm()
        {
            InitializeComponent();
        }
        
        public void Init(AppOptions options)
        {
            this.AppSettings = options;
            initExports();
            addBrowseBtnHandler();
            addCheckBoxEventHandler();
            setDefaultState();
            SetControlsAsInOptions();            
        }

        private void initExports()
        {
            var exports = new List<RouteExportMatching>();

            exports.Add(
                new RouteExportMatching(
                    "PmdgCommon",
                    ProviderType.Pmdg,
                    CheckBox1,
                    TextBox1,
                    Button1));

            exports.Add(
               new RouteExportMatching(
                   "PmdgNGX",
                   ProviderType.Pmdg,
                   CheckBox2,
                   TextBox2,
                   Button2));

            exports.Add(
               new RouteExportMatching(
                   "Pmdg777",
                   ProviderType.Pmdg,
                   CheckBox3,
                   TextBox3,
                   Button3));

            this.exports = exports;
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
                var t = AiracCyclePeriod(navDataPath);
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

            foreach (var i in exports)
            {
                cmds.Add(i.Key,
                    new ExportCommand(
                        i.Type, i.TxtBox.Text, i.CheckBox.Checked));
            }

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

        private void setDefaultState()
        {
            foreach (var i in exports)
            {
                i.CheckBox.Checked = false;
                i.TxtBox.Enabled = false;
            }

            PromptBeforeExit.Checked = true;

            navDataFoundLbl.Text = "";
            airacLbl.Text = "";
            airacPeriodLbl.Text = "";
            expiredLbl.Text = "";

            AutoDLTracksCheckBox.Checked = true;
            AutoDLWindCheckBox.Checked = true;
        }

        public void SetControlsAsInOptions()
        {
            AutoDLTracksCheckBox.Checked = AppSettings.AutoDLTracks;
            AutoDLWindCheckBox.Checked = AppSettings.AutoDLWind;
            navDataPathTxtBox.Text = AppSettings.NavDataLocation;
            PromptBeforeExit.Checked = AppSettings.PromptBeforeExit;
            setExports();
        }

        private void setExports()
        {
            foreach (var i in exports)
            {
                ExportCommand cmd;

                if (AppSettings.ExportCommands.TryGetValue(i.Key, out cmd))
                {
                    i.TxtBox.Text = cmd.Directory;
                    i.CheckBox.Checked = cmd.Enabled;
                }
            }
        }

        private void addBrowseBtnHandler()
        {
            foreach (var i in exports)
            {
                i.BrowserBtn.Click += (sender, e) =>
                {
                    var MyFolderBrowser = new FolderBrowserDialog();
                    var dlgResult = MyFolderBrowser.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        i.TxtBox.Text = MyFolderBrowser.SelectedPath;
                    }
                };  
            }
        }

        private void addCheckBoxEventHandler()
        {
            foreach (var i in exports)
            {
                i.CheckBox.CheckedChanged += (sender, e) =>
                {
                    i.TxtBox.Enabled = i.CheckBox.Checked;
                };
            }
        }
        
        private class RouteExportMatching
        {
            public string Key { get; private set; }
            public ProviderType Type { get; private set; }
            public CheckBox CheckBox { get; private set; }
            public TextBox TxtBox { get; private set; }
            public Button BrowserBtn { get; private set; }

            public RouteExportMatching(
                string Key,
                ProviderType Type,
                CheckBox CheckBox,
                TextBox TxtBox,
                Button BrowserBtn)
            {
                this.Key = Key;
                this.Type = Type;
                this.CheckBox = CheckBox;
                this.TxtBox = TxtBox;
                this.BrowserBtn = BrowserBtn;
            }
        }
    }
}
