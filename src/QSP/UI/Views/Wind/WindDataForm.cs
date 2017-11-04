using QSP.LibraryExtension;
using QSP.UI.Models.Wind;
using QSP.UI.Presenters.Wind;
using QSP.UI.Util;
using QSP.WindAloft;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.Forms
{
    public partial class WindDataForm : Form, IWindDataView
    {
        private WindDataPresenter presenter;
        private ToolStripStatusLabel toolStripLbl;

        public WindDataForm()
        {
            InitializeComponent();
        }

        public void Init(
            ToolStripStatusLabel toolStripLbl,
            Locator<IWindTableCollection> windTableLocator,
            WindDownloadStatus status)
        {
            presenter = new WindDataPresenter(this, windTableLocator);
            this.toolStripLbl = toolStripLbl;
            ShowWindStatus(status);
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

        public void ShowWarning(string message) => MsgBoxHelper.ShowWarning(this, message);

        public async Task DownloadWind()
        {
            downloadBtn.Enabled = false;
            loadFileBtn.Enabled = false;

            await presenter.DownloadWind();

            downloadBtn.Enabled = true;
            loadFileBtn.Enabled = true;
        }

        public WindDownloadStatus ToolStripWindStatus => 
            new WindDownloadStatus(toolStripLbl.Text, toolStripLbl.Image);

        public void ShowWindStatus(WindDownloadStatus item)
        {
            toolStripLbl.Text = item.Text;
            toolStripLbl.Image = item.Image;

            statusLbl.Text = "Status : " + item.Text;
            statusPicBox.BackgroundImage = item.Image;
        }

        private void SaveFile(object sender, EventArgs e)
        {
            presenter.SaveFile();
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
                downloadBtn.Enabled = false;
                loadFileBtn.Enabled = false;

                await presenter.LoadFile(openFileDialog.FileName);

                downloadBtn.Enabled = true;
                loadFileBtn.Enabled = true;
            }
        }
    }
}
