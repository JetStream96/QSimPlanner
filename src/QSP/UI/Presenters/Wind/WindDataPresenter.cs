using QSP.LibraryExtension;
using QSP.UI.Forms;
using QSP.UI.Models.Wind;
using QSP.WindAloft;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.LibraryExtension.Paths;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Presenters.Wind
{
    public class WindDataPresenter
    {
        private IWindDataView view;
        private Locator<IWindTableCollection> windTableLocator;
        public bool WindAvailable { get; private set; } = false;

        public WindDataPresenter(IWindDataView view, Locator<IWindTableCollection> windTableLocator)
        {
            this.view = view;
            this.windTableLocator = windTableLocator;
        }

        /// <summary>
        /// Save wind file and display log error message if there is any.
        /// </summary>
        public void SaveFile()
        {
            var sourceFile = WindManager.DownloadFilePath;

            if (!WindAvailable)
            {
                view.ShowWarning("No wind data has been downloaded or loaded from file.");
                return;
            }

            if (!File.Exists(sourceFile))
            {
                view.ShowWarning("The temporary wind data file was deleted. Unable to proceed.");
                return;
            }

            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "grib2 files (*.grib2)|*.grib2|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var targetPath = saveFileDialog.FileName;
                try
                {
                    File.Delete(targetPath);
                    File.Copy(sourceFile, targetPath);
                }
                catch (Exception ex)
                {
                    Log(ex);
                    view.ShowWarning("Failed to save file.");
                }
            }
        }

        /// <summary>
        /// Does not throw exception.
        /// </summary>
        public async Task LoadFile(string file)
        {
            var oldStatus = view.ToolStripWindStatus;

            try
            {
                view.ShowWindStatus(WindDownloadStatus.LoadingFromFile);

                await Task.Factory.StartNew(() => LoadWindFile(file));

                var fileNameShort = Path.GetFileName(file);
                var fileNameMsg = fileNameShort.Length > 10 ? "" : $"({fileNameShort})";

                view.ShowWindStatus(new WindDownloadStatus(
                     $"Loaded from file {fileNameMsg}",
                     Properties.Resources.GreenLight));
                WindAvailable = true;
            }
            catch (Exception ex)
            {
                Log(ex);
                view.ShowWarning($"Failed to load file {file}");
                view.ShowWindStatus(oldStatus);
            }
        }
        
        // May throw exception.
        private WindTableCollection LoadWindFile(string path)
        {
            if (!GetUri(path).Equals(GetUri(WindManager.DownloadFilePath)))
            {
                File.Delete(WindManager.DownloadFilePath);
                File.Copy(path, WindManager.DownloadFilePath);
            }

            GribConverter.ConvertGrib();
            var handler = new WindFileHandler();
            var result = handler.ImportAllTables();
            handler.TryDeleteCsvFiles();
            return result;
        }

        /// <summary>
        /// Downloads the wind and updates the wind tables.
        /// If failed, errors are logged and displayed.
        /// </summary>
        /// <returns></returns>
        public async Task DownloadWind()
        {
            view.ShowWindStatus(WindDownloadStatus.Downloading);

            try
            {
                windTableLocator.Instance = await WindManager.LoadWindAsync();
                view.ShowWindStatus(WindDownloadStatus.FinishedDownload);
                WindAvailable = true;
            }
            catch (Exception ex)
            {
                Log(ex);
                view.ShowWindStatus(WindDownloadStatus.FailedToDownload);
            }
        }
    }
}
