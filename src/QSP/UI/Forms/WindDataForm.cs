using QSP.LibraryExtension;
using QSP.UI.Controllers;
using QSP.UI.Utilities;
using QSP.WindAloft;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.MsgBox;
using static QSP.LibraryExtension.Paths;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Forms
{
    public partial class WindDataForm : Form
    {
        private Locator<IWindTableCollection> windTableLocator;
        private ToolStripStatusLabel toolStripLbl;
        private bool windAvailable;

        public WindDataForm()
        {
            InitializeComponent();
        }

        public void Init(
            ToolStripStatusLabel toolStripLbl,
            Locator<IWindTableCollection> windTableLocator,
            WindDownloadStatus status)
        {
            this.toolStripLbl = toolStripLbl;
            this.windTableLocator = windTableLocator;
            ShowWindStatus(status);
            windAvailable = false;
            SetButtonColorStyles();

            downloadBtn.Click += async (s, e) => await DownloadWind();
            saveFileBtn.Click += SaveFile;
            loadFileBtn.Click += (s, e) => LoadFile(s, e);
        }

        private void SetButtonColorStyles()
        {
            var colorStyle = new ControlDisableStyleController.ColorStyle(
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            var downloadBtnStyle = new ControlDisableStyleController(downloadBtn, colorStyle);
            var loadFileBtnStyle = new ControlDisableStyleController(loadFileBtn, colorStyle);

            downloadBtnStyle.Activate();
            loadFileBtnStyle.Activate();
            downloadBtn.Enabled = true;
            loadFileBtn.Enabled = true;
        }

        private async Task downlaodBtn_Click(object sender, EventArgs e)
        {
            await DownloadWind();
        }

        public async Task DownloadWind()
        {
            downloadBtn.Enabled = false;
            loadFileBtn.Enabled = false;
            ShowWindStatus(WindDownloadStatus.Downloading);

            try
            {
                windTableLocator.Instance = await WindManager.LoadWindAsync();
                ShowWindStatus(WindDownloadStatus.FinishedDownload);
                windAvailable = true;
            }
            catch (Exception ex)
            {
                Log(ex);
                ShowWindStatus(WindDownloadStatus.FailedToDownload);
            }

            downloadBtn.Enabled = true;
            loadFileBtn.Enabled = true;
        }

        private void ShowWindStatus(WindDownloadStatus item)
        {
            toolStripLbl.Text = item.Text;
            toolStripLbl.Image = item.Image;

            statusLbl.Text = "Status : " + item.Text;
            statusPicBox.BackgroundImage = item.Image;
        }

        private void SaveFile(object sender, EventArgs e)
        {
            var sourceFile = WindManager.DownloadFilePath;

            if (windAvailable == false)
            {
                MsgBoxHelper.ShowWarning("No wind data has been downloaded or loaded from file.");
                return;
            }

            if (File.Exists(sourceFile) == false)
            {
                MsgBoxHelper.ShowWarning(
                    "The temporary wind data file was deleted. Unable to proceed.");
                return;
            }

            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "grib2 files (*.grib2)|*.grib2|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = saveFileDialog.FileName;

                try
                {
                    File.Delete(file);
                    File.Copy(sourceFile, file);
                }
                catch (Exception ex)
                {
                    Log(ex);
                    MsgBoxHelper.ShowWarning("Failed to save file.");
                }
            }
        }

        private async Task LoadFile(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter =
                "grib2 files (*.grib2)|*.grib2|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog.FileName;
                downloadBtn.Enabled = false;
                loadFileBtn.Enabled = false;

                try
                {
                    ShowWindStatus(WindDownloadStatus.LoadingFromFile);

                    await Task.Factory.StartNew(() => LoadFromFile(file));

                    var fileNameShort = Path.GetFileName(file);
                    var fileNameMsg = fileNameShort.Length > 10 ?
                        "" : $"({fileNameShort})";

                    ShowWindStatus(new WindDownloadStatus(
                        $"Loaded from file {fileNameMsg}",
                        Properties.Resources.GreenLight));
                    windAvailable = true;
                }
                catch (Exception ex)
                {
                    Log(ex);
                    MsgBoxHelper.ShowWarning($"Failed to load file {file}");
                }

                downloadBtn.Enabled = true;
                loadFileBtn.Enabled = true;
            }
        }

        private void LoadFromFile(string file)
        {
            if (!GetUri(file).Equals(GetUri(WindManager.DownloadFilePath)))
            {
                File.Delete(WindManager.DownloadFilePath);
                File.Copy(file, WindManager.DownloadFilePath);
            }

            GribConverter.ConvertGrib();
            var handler = new WindFileHandler();
            windTableLocator.Instance = handler.ImportAllTables();
            handler.TryDeleteCsvFiles();
        }
    }
}
