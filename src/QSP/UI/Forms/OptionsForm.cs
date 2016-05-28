using QSP.Common.Options;
using QSP.Utilities;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static QSP.UI.FormInstanceGetter;
using static QSP.Utilities.LoggerInstance;

namespace QSP
{
    public partial class OptionsForm
    {
        public AppOptions AppSettings { get; set; }

        private void DBPath_TxtBox_TextChanged(object sender, EventArgs e)
        {
            LoadDBStatusDisplay(DBPath_TxtBox.Text);
        }

        public void LoadDBStatusDisplay(string DBFilepath)
        {
            //DBFilepath = the path in the TxtBox

            //This section is to determine whether the database files are found or not.
            bool DBFound = true;

            string[] FilesToCheck = {
                "airports.txt",
                "ats.txt",
                "cycle.txt",
                "navaids.txt",
                "waypoints.txt"
            };
            foreach (var i in FilesToCheck)
            {
                if (!File.Exists(DBFilepath + "\\" + i))
                {
                    DBFound = false;
                    break;
                }
            }

            if (DBFound == true)
            {
                DBFound_Lbl.Text = "Found";
            }
            else
            {
                DBFound_Lbl.Text = "Not Found";
            }

            try
            {
                Tuple<string, string> t = AiracCyclePeriod(DBFilepath);
                Airac_Lbl.Text = t.Item1;
                AiracPeriod_Lbl.Text = t.Item2;

                bool expired = !AiracTools.AiracValid(t.Item2);
                if (expired == true)
                {
                    Expired_Lbl.Text = "(Expired)";
                    Expired_Lbl.ForeColor = Color.Red;
                }
                else
                {
                    Expired_Lbl.Text = "(Within Valid Period)";
                    Expired_Lbl.ForeColor = Color.Green;
                }


            }
            catch (Exception ex)
            {
                DBFound_Lbl.Text = "Failed to load";
                Airac_Lbl.Text = "N/A";
                AiracPeriod_Lbl.Text = "N/A";
                Expired_Lbl.Text = "";

                WriteToLog(ex);
            }

        }

        /// <summary>
        /// Reads from file and gets the AIRAC cycle and valid period. 
        /// e.g. The first item in tuple can be 1407, second item can be 26JUN23JUL/14.
        /// </summary>
        /// <param name="DBFilepath">The folder containing cycle.txt.</param>
        public static Tuple<string, string> AiracCyclePeriod(string DBFilepath)
        {

            string str = File.ReadAllText(DBFilepath + "\\cycle.txt");
            string[] s = str.Split(',');
            return new Tuple<string, string>(s[0], s[1]);

        }
        
        private void browseFolderBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog MyFolderBrowser = new FolderBrowserDialog();
            DialogResult dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                DBPath_TxtBox.Text = MyFolderBrowser.SelectedPath;
            }

        }
        
        private void okBtnClick(object sender, EventArgs e)
        {
            //if the database path is changed, then load the database. Otherwise do not reload the same database.

            if (OptionManager.TrySaveFile(AppSettings))
            {
                AppSettings.NavDataLocation = DBPath_TxtBox.Text;
                MainFormInstance().LoadNavDBUpdateStatusStrip(false);
            }

            Close();
        }

        /// <summary>
        /// Set the AppSettings as in the option form. Returns a boolean indicates whether the nav database location was modified.
        /// </summary>
        public bool SetAppOptions()
        {

            AppSettings.ExportCommands.Clear();

            AppSettings.ExportCommands.Add(new RouteExportCommand("PmdgCommon", TextBox1.Text, CheckBox1.Checked));
            AppSettings.ExportCommands.Add(new RouteExportCommand("PmdgNGX", TextBox2.Text, CheckBox2.Checked));
            AppSettings.ExportCommands.Add(new RouteExportCommand("Pmdg777", TextBox3.Text, CheckBox3.Checked));

            AppSettings.PromptBeforeExit = DoubleCheckWhenExit_CheckBox.Checked;
            AppSettings.AutoDLTracks = AutoDLNats_CheckBox.Checked;
            AppSettings.AutoDLWind = AutoDLWind_CheckBox.Checked;

            if (AppSettings.NavDataLocation == DBPath_TxtBox.Text)
            {
                return false;
            }
            else
            {
                AppSettings.NavDataLocation = DBPath_TxtBox.Text;
                return true;
            }

        }
        
        private void cancelBtnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void InitializeForm()
        {
            //default form state
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            CheckBox3.Checked = false;
            TextBox1.Enabled = false;
            TextBox2.Enabled = false;
            TextBox3.Enabled = false;
            AppSettings.PromptBeforeExit = true;

            DBFound_Lbl.Text = "";
            Airac_Lbl.Text = "";
            AiracPeriod_Lbl.Text = "";
            Expired_Lbl.Text = "";

            AutoDLNats_CheckBox.Checked = true;
            AutoDLWind_CheckBox.Checked = true;

            SetControlsAsInOptions();
        }

        private void optionsLoad(object sender, EventArgs e)
        {
            InitializeForm();
        }

        public void SetControlsAsInOptions()
        {
            AutoDLNats_CheckBox.Checked = AppSettings.AutoDLTracks;
            AutoDLWind_CheckBox.Checked = AppSettings.AutoDLWind;
            DBPath_TxtBox.Text = AppSettings.NavDataLocation;
            DoubleCheckWhenExit_CheckBox.Checked = AppSettings.PromptBeforeExit;

            var command = AppSettings.GetExportCommand("PmdgCommon");
            TextBox1.Text = command.FilePath;
            CheckBox1.Checked = command.Enabled;

            command = AppSettings.GetExportCommand("PmdgNGX");
            TextBox2.Text = command.FilePath;
            CheckBox2.Checked = command.Enabled;

            command = AppSettings.GetExportCommand("Pmdg777");
            TextBox3.Text = command.FilePath;
            CheckBox3.Checked = command.Enabled;
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

        public OptionsForm()
        {
            Load += optionsLoad;
            InitializeComponent();
        }
    }
}
