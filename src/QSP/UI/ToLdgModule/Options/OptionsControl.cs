using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.RouteFinding.Airports;
using QSP.NavData;
using System.Threading;
using QSP.Core;
using QSP.Utilities;
using System.IO;

namespace QSP.UI.ToLdgModule.Options
{
    public partial class OptionsControl : UserControl
    {
        public AirportManager airports { get; private set; }

        public event EventHandler SaveAirportsCompleted;
        public event EventHandler HideControlRequested;

        public OptionsControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            sourceComboBox.SelectedIndex = 0;
            UserOption options = new UserOption(0, "");

            try
            {
                options = OptionManager.ReadFromFile();

            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);

                bool notFound = 
                    ex is FileNotFoundException ||
                    ex is DirectoryNotFoundException;

                if (notFound == false)
                {
                    MessageBox.Show("Unable to read options file.");
                    // TODO: Exit app.
                }
            }

            sourceComboBox.SelectedIndex = options.SourceType;
            pathTxtBox.Text = options.SourcePath;
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

            airports = new AirportManager(col);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (tryLoadAirports() && trySaveOptions())
            {
                SaveAirportsCompleted?.Invoke(this, EventArgs.Empty);
            }
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

        private void button1_Click(object sender, EventArgs e)
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
