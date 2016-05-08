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
        public AirportManager Airports { get; private set; }

        public event EventHandler SaveAirportsCompleted;
        public event EventHandler HideControlRequested;

        public OptionsControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            sourceComboBox.SelectedIndex = 0;
            var options = new UserOption(0, "");
            
            try
            {
                options = OptionManager.ReadFromFile();
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
            }

            sourceComboBox.SelectedIndex = options.SourceType;
            pathTxtBox.Text = options.SourcePath;

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
                        new NavData.AAX.AirportDataLoader(source.Path);
                    col = loader2.LoadFromFile();
                    break;

                default:
                    throw new EnumNotSupportedException();
            }

            Airports = new AirportManager(col);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            saveBtn.ForeColor = Color.Black;
            saveBtn.BackColor = Color.LightGray;
            saveBtn.Text = "Saving ...";
            Refresh();

            if (tryLoadAirports() && trySaveOptions())
            {
                SaveAirportsCompleted?.Invoke(this, EventArgs.Empty);
            }

            saveBtn.ForeColor = Color.White;
            saveBtn.BackColor = Color.Green;
            saveBtn.Text = "Save";
        }

        private bool trySaveOptions()
        {
            try
            {
                var option = new UserOption(
                    sourceComboBox.SelectedIndex, pathTxtBox.Text);

                OptionManager.Save(option);
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
                MessageBox.Show("Cannot read nav data file.");
            }
            catch (RwyDataFormatException ex)
            {
                LoggerInstance.WriteToLog(ex);
                MessageBox.Show("Nav data format is wrong.");
            }

            return false;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            HideControlRequested?.Invoke(this, EventArgs.Empty);
        }

        private void broserBtn_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.SelectedPath = pathTxtBox.Text;
            var dlgResult = folderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                pathTxtBox.Text = folderBrowser.SelectedPath;
            }
        }
    }
}
