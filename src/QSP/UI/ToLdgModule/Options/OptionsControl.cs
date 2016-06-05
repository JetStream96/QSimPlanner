using QSP.Common;
using QSP.NavData;
using QSP.RouteFinding.Airports;
using QSP.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;
using static QSP.UI.Utilities.MsgBoxHelper;

namespace QSP.UI.ToLdgModule.Options
{
    public partial class OptionsControl : UserControl
    {
        // Will not be null after Initialize() call.
        private UserOption options;
        private Panel popUpPanel;

        public AirportManager Airports { get; set; }

        public event EventHandler SaveAirportsCompleted;

        public OptionsControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            sourceComboBox.SelectedIndex = 0;

            try
            {
                options = OptionManager.ReadOrCreateFile();
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
                MessageBox.Show(
                    "Failed to read options.ini. " +
                    "Please make sure it is not opened by other programs." +
                    "\nThe application will quit now.",
                    "File read error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Environment.Exit(0);
            }

            refreshNavSource();
            tryLoadAirports();
        }

        private void loadAirports(DataSource source)
        {
            AirportCollection col;

            switch (source.DataType)
            {
                case DataSource.Type.OpenData:
                    var loader1 =
                        new NavData.OpenData.AirportDataLoader(source.Path);
                    col = loader1.LoadFromFile();
                    break;

                case DataSource.Type.Navigraph:
                    var loader2 =
                        new NavData.AAX.AirportDataLoader(
                            source.Path + @"\airports.txt");
                    col = loader2.LoadFromFile();
                    break;

                default:
                    throw new EnumNotSupportedException();
            }

            Airports = new AirportManager(col);
        }

        private void saveBtnClick(object sender, EventArgs e)
        {
            saveBtn.ForeColor = Color.Black;
            saveBtn.Enabled = false;
            saveBtn.BackColor = Color.FromArgb(224, 224, 224);
            saveBtn.Text = "Saving ...";
            Refresh();

            if (tryLoadAirports() && trySaveOptions())
            {
                SaveAirportsCompleted?.Invoke(this, EventArgs.Empty);
            }

            saveBtn.ForeColor = Color.White;
            saveBtn.BackColor = Color.Green;
            saveBtn.Text = "Save";
            saveBtn.Enabled = true;
        }

        private bool trySaveOptions()
        {
            try
            {
                options.SourceType = sourceComboBox.SelectedIndex;

                if (options.SourceType == 0)
                {
                    options.OpenDataPath = pathTxtBox.Text;
                }
                else
                {
                    // 1
                    options.PaywarePath = pathTxtBox.Text;
                }

                OptionManager.Save(options);
                return true;
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
                ShowError("Failed to save options.");
                return false;
            }
        }

        private bool tryLoadAirports()
        {
            try
            {
                var type = (DataSource.Type)sourceComboBox.SelectedIndex;
                var source = new DataSource(type, pathTxtBox.Text);
                loadAirports(source);
                return true;
            }
            catch (ReadAirportFileException ex)
            {
                LoggerInstance.WriteToLog(ex);
                ShowError("Cannot read nav data file.");
            }
            catch (RwyDataFormatException ex)
            {
                LoggerInstance.WriteToLog(ex);
                ShowError("Nav data format is wrong.");
            }

            return false;
        }

        private void cancelBtnClick(object sender, EventArgs e)
        {
            refreshNavSource();
        }

        private void broserBtnClick(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.SelectedPath = pathTxtBox.Text;
            var dlgResult = folderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                pathTxtBox.Text = folderBrowser.SelectedPath;
            }
        }

        private void refreshNavSource()
        {
            refreshNavSource(options.SourceType);
        }

        private void refreshNavSource(int sourceType)
        {
            sourceComboBox.SelectedIndex = sourceType;

            if (sourceType == 0)
            {
                pathTxtBox.Text = options.OpenDataPath;
                pathTxtBox.Enabled = false;
            }
            else
            {
                // 1
                pathTxtBox.Text = options.PaywarePath;
                pathTxtBox.Enabled = true;
            }
        }

        private void sourceChanged(object sender, EventArgs e)
        {
            if (options != null)
            {
                refreshNavSource(sourceComboBox.SelectedIndex);
            }
        }

        private void pathTxtBoxTextChanged(object sender, EventArgs e)
        {
            if (options != null)
            {
                if (sourceComboBox.SelectedIndex == 0)
                {
                    options.OpenDataPath = pathTxtBox.Text;
                }
                else
                {
                    // 1
                    options.PaywarePath = pathTxtBox.Text;
                }
            }
        }

        private void infoLbl_MouseEnter(object sender, EventArgs e)
        {
            infoLbl.Font = new Font(infoLbl.Font, FontStyle.Underline);

            popUpPanel = infoPanel(
                (DataSource.Type)sourceComboBox.SelectedIndex);
            Controls.Add(popUpPanel);
            popUpPanel.BringToFront();
        }

        private void infoLbl_MouseLeave(object sender, EventArgs e)
        {
            infoLbl.Font = new Font(infoLbl.Font, FontStyle.Regular);

            Controls.Remove(popUpPanel);
            popUpPanel = null;
        }

        private Panel infoPanel(DataSource.Type type)
        {
            var panel = new Panel();
            panel.Size = new Size(450, 100);
            panel.BackColor = Color.FromArgb(216, 244, 215);
            var pt = infoLbl.Location;
            panel.Location = new Point(pt.X - 150, pt.Y + 100);

            var lbl = new Label();
            lbl.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular);
            lbl.ForeColor = Color.DarkGreen;
            lbl.Text = lblTxt(type);
            lbl.Location = new Point(0, 0);
            lbl.AutoSize = true;

            panel.Controls.Add(lbl);
            return panel;
        }

        private string lblTxt(DataSource.Type type)
        {
            switch (type)
            {
                case DataSource.Type.OpenData:
                    return
@"This is the nav data which came with this application.";

                case DataSource.Type.Navigraph:
                    return
@"Location of either Aerosoft's NavDataPro or Navigraph's
FMS Data (both are payware). Use the version of Aerosoft
Airbus A318/A319/A320/A321. Select the folder which
contains Airports.txt.";

                default:
                    throw new EnumNotSupportedException();
            }
        }
    }
}
