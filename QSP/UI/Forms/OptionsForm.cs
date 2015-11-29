using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using QSP.Utilities;
using static QSP.UI.Utilities;
using static QSP.Utilities.ErrorLogger;
using QSP.Core;

namespace QSP
{

    public partial class OptionsForm
    {

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
                if (!File.Exists(DBFilepath + "\\" + i) )
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
            System.Windows.Forms.FolderBrowserDialog MyFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                DBPath_TxtBox.Text = MyFolderBrowser.SelectedPath;
            }

        }


        private void ok_button_Click(object sender, EventArgs e)
        {
            //if the database path is changed, then load the database. Otherwise do not reload the same database.

            if (SaveOptions())
            {
                QspCore.AppSettings.NavDBLocation = DBPath_TxtBox.Text;
                MainFormInstance().LoadNavDBUpdateStatusStrip(false);
            }
            this.Close();
        }

        /// <summary>
        /// Set the AppSettings as in the option form. Returns a boolean indicates whether the nav database location was modified.
        /// </summary>
        public bool SetAppOptions()
        {

            QspCore.AppSettings.ExportCommands.Clear();

            QspCore.AppSettings.ExportCommands.Add(new RouteExportCommand("PmdgCommon", TextBox1.Text, CheckBox1.Checked));
            QspCore.AppSettings.ExportCommands.Add(new RouteExportCommand("PmdgNGX", TextBox2.Text, CheckBox2.Checked));
            QspCore.AppSettings.ExportCommands.Add(new RouteExportCommand("Pmdg777", TextBox3.Text, CheckBox3.Checked));

            QspCore.AppSettings.PromptBeforeExit = DoubleCheckWhenExit_CheckBox.Checked;
            QspCore.AppSettings.AutoDLTracks = AutoDLNats_CheckBox.Checked;
            QspCore.AppSettings.AutoDLWind = AutoDLWind_CheckBox.Checked;

            if (QspCore.AppSettings.NavDBLocation == DBPath_TxtBox.Text)
            {
                return false;
            }
            else
            {
                QspCore.AppSettings.NavDBLocation = DBPath_TxtBox.Text;
                return true;
            }

        }

        /// <summary>
        /// Returns a boolean indicates whether the nav database location was modified.
        /// </summary>
        public bool SaveOptions()
        {

            var returnVal = SetAppOptions();

            //database path
            System.IO.Directory.CreateDirectory(QspCore.QspAppDataDirectory + "\\SavedStates");

            using (StreamWriter writer = new StreamWriter(QspCore.QspAppDataDirectory + "\\SavedStates\\options.xml"))
            {
                writer.Write(QspCore.AppSettings.ToXml());
            }

            return returnVal;

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
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
            QspCore.AppSettings.PromptBeforeExit = true;

            DBFound_Lbl.Text = "";
            Airac_Lbl.Text = "";
            AiracPeriod_Lbl.Text = "";
            Expired_Lbl.Text = "";

            AutoDLNats_CheckBox.Checked = true;
            AutoDLWind_CheckBox.Checked = true;

            LoadOptions();

        }

        private void options_2_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }


        public void SetControlsAsInOptions()
        {
            AutoDLNats_CheckBox.Checked = QspCore.AppSettings.AutoDLTracks;
            AutoDLWind_CheckBox.Checked = QspCore.AppSettings.AutoDLWind;
            DBPath_TxtBox.Text = QspCore.AppSettings.NavDBLocation;
            DoubleCheckWhenExit_CheckBox.Checked = QspCore.AppSettings.PromptBeforeExit;

            var command = QspCore.AppSettings.GetExportCommand("PmdgCommon");
            TextBox1.Text = command.FilePath;
            CheckBox1.Checked = command.Enabled;

            command = QspCore.AppSettings.GetExportCommand("PmdgNGX");
            TextBox2.Text = command.FilePath;
            CheckBox2.Checked = command.Enabled;

            command = QspCore.AppSettings.GetExportCommand("Pmdg777");
            TextBox3.Text = command.FilePath;
            CheckBox3.Checked = command.Enabled;

        }


        public void LoadOptions()
        {
            try
            {
                QspCore.AppSettings = new AppOptions(XDocument.Load(QspCore.QspAppDataDirectory + "\\SavedStates\\options.xml"));
                SetControlsAsInOptions();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog MyFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                TextBox1.Text = MyFolderBrowser.SelectedPath;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog MyFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                TextBox2.Text = MyFolderBrowser.SelectedPath;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog MyFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult dlgResult = MyFolderBrowser.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                TextBox3.Text = MyFolderBrowser.SelectedPath;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked == true)
            {
                TextBox1.Enabled = true;
            }
            else
            {
                TextBox1.Enabled = false;
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox2.Checked == true)
            {
                TextBox2.Enabled = true;
            }
            else
            {
                TextBox2.Enabled = false;
            }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox3.Checked == true)
            {
                TextBox3.Enabled = true;
            }
            else
            {
                TextBox3.Enabled = false;
            }
        }
        public OptionsForm()
        {
            Load += options_2_Load;
            InitializeComponent();
        }
    }
}
