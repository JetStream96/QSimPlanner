using QSP.Core;
using QSP.NavData;
using QSP.RouteFinding.Airports;
using QSP.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.Options
{
    public partial class OptionsControl : UserControl
    {
        private UserOption options;

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

            displayOptions();
            tryLoadAirports();
        }

        private void displayOptions()
        {
            sourceComboBox.SelectedIndex = options.SourceType;

            if (options.SourceType == 0)
            {
                pathTxtBox.Text = options.OpenDataPath;
            }
            else
            {
                // 1
                pathTxtBox.Text = options.PaywarePath;
            }
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
                if (sourceComboBox.SelectedIndex == 0)
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
                MessageBox.Show("Failed to save options.");
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
                MessageBox.Show(
                    "Cannot read nav data file.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (RwyDataFormatException ex)
            {
                LoggerInstance.WriteToLog(ex);
                MessageBox.Show(
                    "Nav data format is wrong.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return false;
        }

        private void cancelBtnClick(object sender, EventArgs e)
        {
            displayOptions();
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

        private void sourceComboBoxIndexChanged(object sender, EventArgs e)
        {
            if (options != null)
            {
                if (sourceComboBox.SelectedIndex == 0)
                {
                    pathTxtBox.Text = options.OpenDataPath;
                }
                else
                {
                    // 1
                    pathTxtBox.Text = options.PaywarePath;
                }
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
    }
}
