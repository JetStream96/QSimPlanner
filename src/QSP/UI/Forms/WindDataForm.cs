using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.WindAloft;
using static QSP.Utilities.LoggerInstance;
using QSP.LibraryExtension;
using System.IO;
using QSP.UI.Utilities;

namespace QSP.UI.Forms
{
    public partial class WindDataForm : Form
    {
        private Locator<WindTableCollection> windTableLocator;
        private ToolStripStatusLabel toolStripLbl;

        public WindDataForm()
        {
            InitializeComponent();
        }

        public void Init(
            ToolStripStatusLabel toolStripLbl,
            Locator<WindTableCollection> windTableLocator,
            WindDownloadStatus status)
        {
            this.toolStripLbl = toolStripLbl;
            this.windTableLocator = windTableLocator;
            ShowWindStatus(status);
        }

        private async void downlaodBtn_Click(object sender, EventArgs e)
        {
            ShowWindStatus(WindDownloadStatus.Downloading);

            try
            {
                windTableLocator.Instance = await WindManager.LoadWindAsync();
                ShowWindStatus(WindDownloadStatus.FinishedDownload);
            }
            catch (Exception ex) when (
                ex is ReadWindFileException ||
                ex is DownloadGribFileException)
            {
                WriteToLog(ex);
                ShowWindStatus(WindDownloadStatus.FailedToDownload);
            }
        }

        private void ShowWindStatus(WindDownloadStatus item)
        {
            toolStripLbl.Text = item.Text;
            toolStripLbl.Image = item.Image;

            statusLbl.Text = item.Text;
            statusPicBox.Image = item.Image;
        }

        private void saveFileBtn_Click(object sender, EventArgs e)
        {
            var sourceFile = WindManager.DownloadFilePath;

            if (File.Exists(sourceFile) == false)
            {
                ShowSaveFileError();
                return;
            }

            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter =
                "grib2 files (*.grib2)|*.grib2|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.InitialDirectory = Constants.WxFileDirectory;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = saveFileDialog.FileName;

                try
                {
                    File.Copy(sourceFile, file);
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    ShowSaveFileError();
                }
            }
        }

        private void ShowSaveFileError()
        {
            MsgBoxHelper.ShowWarning(
                   "No wind data has been downloaded or loaded from file.");
        }

        private void loadFileBtn_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter =
                "grib2 files (*.grib2)|*.grib2|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = Constants.WxFileDirectory;
            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog.FileName;

                try
                {
                    File.Delete(WindManager.DownloadFilePath);
                    File.Copy(file, WindManager.DownloadFilePath);

                    var handler = new WindFileHandler();
                    var tables = handler.ImportAllTables();
                    handler.TryDeleteCsvFiles();
                    windTableLocator.Instance = tables;

                    ShowWindStatus(new WindDownloadStatus(
                        $"Loaded from file ({file}).",
                        Properties.Resources.GreenLight));
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    MsgBoxHelper.ShowWarning(
                        $"Failed to load file {file}.");
                }
            }
        }
    }
}
