using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.UI.Controllers;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static QSP.RouteFinding.Tracks.Common.Helpers;
using static QSP.RouteFinding.Tracks.Interaction.StatusRecorder;
using static QSP.Utilities.LoggerInstance;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.UI.Forms
{
    public partial class TracksForm
    {
        private static readonly string trackFileFolder = "Tracks";
        private static readonly string trackFileExtension = ".track";

        private AirwayNetwork airwayNetwork;
        private ImageList myImageList;
        private ToolStripStatusLabel statusLbl;

        public TracksForm()
        {
            InitializeComponent();
        }

        public void Init(AirwayNetwork airwayNetwork, ToolStripStatusLabel statusLbl)
        {
            this.airwayNetwork = airwayNetwork;
            airwayNetwork.TrackForm = this;
            this.statusLbl = statusLbl;

            RefreshListViewColumnWidth();
            InitImages();
            InitCBox();
            InitPicBoxes();
            InitBtns();

            // The event handlers are added after the form is created. 
            // This way the events won't fire at form creation.
            TrackTypes.ForEach(t =>
            {
                DownloadBtn(t).Click += (s, e) => DownloadTracks(t);
                EnabledCBox(t).SelectedIndexChanged += (s, e) => SyncCBoxEnabled(t);
                ViewBtn(t).Click += (s, e) => ViewTracks(t);
                DownloadBtn(t).EnabledChanged += RefreshDownloadAllBtnEnabled;
            });

            downloadAllBtn.EnabledChanged += (s, e) => importBtn.Enabled = downloadAllBtn.Enabled;
            airwayNetwork.TrackMessageUpdated += (s, e) => RefreshViewTrackBtns();
            Closing += CloseForm;

            // TODO: Move up?
            TrackTypes.ForEach(t => SyncCBoxEnabled(t));
        }

        private void RefreshListViewColumnWidth()
        {
            ListView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListView1.Columns[1].Width = -2;
        }

        private void InitBtns()
        {
            var colorStyle = new ControlDisableStyleController.ColorStyle(
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            Button[] buttons =
            {
                viewNatsBtn,
                viewPacotsBtn,
                viewAusotsBtn,
                BtnNatsDn,
                BtnPacotsDn,
                BtnAusotsDn,
                downloadAllBtn,
                importBtn
            };

            buttons.ForEach(b =>
            {
                var styleController = new ControlDisableStyleController(b, colorStyle);
                styleController.Activate();
            });

            viewPacotsBtn.Enabled = false;
            viewNatsBtn.Enabled = false;
            viewAusotsBtn.Enabled = false;
            BtnNatsDn.Enabled = true;
            BtnPacotsDn.Enabled = true;
            BtnAusotsDn.Enabled = true;
            downloadAllBtn.Enabled = true;
        }

        private Button DownloadBtn(TrackType t) =>
            new[] { BtnNatsDn, BtnPacotsDn, BtnAusotsDn }[(int)t];

        private Button ViewBtn(TrackType t) =>
            new[] { viewNatsBtn, viewPacotsBtn, viewAusotsBtn }[(int)t];

        private ComboBox EnabledCBox(TrackType t) =>
            new[] { CBoxNatsEnabled, CBoxPacotsEnabled, CBoxAusotsEnabled }[(int)t];

        private void ViewTracks(TrackType t)
        {
            txtRichTextBox.Text = airwayNetwork.GetTrackMessage(t)
                .ToString()
                .TrimEmptyLines();
        }

        private void InitImages()
        {
            myImageList = new ImageList();

            myImageList.ImageSize = new Size(24, 24);
            myImageList.Images.Add(Properties.Resources.checkIconLarge);
            myImageList.Images.Add(Properties.Resources.CautionIcon);
            myImageList.Images.Add(Properties.Resources.deleteIconLarge);

            ListView1.SmallImageList = myImageList;
        }

        private void InitCBox()
        {
            TrackTypes.ForEach(t => EnabledCBox(t).SelectedIndex = 0);
        }

        public void RefreshStatus()
        {
            var records = airwayNetwork.StatusRecorder.Records;
            AddToListView(records);
            RefreshListViewColumnWidth();
            InitPicBoxes();
            SetPicBox(records);
            SetMainFormTrackStatus(records);
        }

        private void InitPicBoxes()
        {
            PicBoxNats.Image = null;
            PicBoxPacots.Image = null;
            PicBoxAusots.Image = null;
        }

        private void SetPicBox(IEnumerable<Entry> records)
        {
            if (airwayNetwork.TracksLoaded(TrackType.Nats))
            {
                var severity = (int)MaxSeverity(records, TrackType.Nats);
                PicBoxNats.Image = myImageList.Images[severity];
            }

            if (airwayNetwork.TracksLoaded(TrackType.Pacots))
            {
                var severity = (int)MaxSeverity(records, TrackType.Pacots);
                PicBoxPacots.Image = myImageList.Images[severity];
            }

            if (airwayNetwork.TracksLoaded(TrackType.Ausots))
            {
                var severity = (int)MaxSeverity(records, TrackType.Ausots);
                PicBoxAusots.Image = myImageList.Images[severity];
            }
        }

        private static Severity MaxSeverity(IEnumerable<Entry> records, TrackType type)
        {
            var filtered = records.Where(r => r.Type == type).ToList();
            if (!filtered.Any()) return Severity.Advisory;
            return (Severity)filtered.Max(i => (int)i.Severity);
        }

        private static bool NoErrors(IEnumerable<Entry> records, TrackType type)
        {
            return MaxSeverity(records, type) == Severity.Advisory;
        }

        private void AddToListView(IEnumerable<Entry> records)
        {
            ListView1.Items.Clear();

            records = records.OrderBy(i => i.Type);

            foreach (var i in records)
            {
                var type = i.Type;
                var lvi = new ListViewItem(type.TrackString());
                lvi.SubItems.Add(i.Message);
                lvi.ImageIndex = (int)i.Severity;
                ListView1.Items.Add(lvi);
            }

            foreach (var type in TrackTypes)
            {
                if (airwayNetwork.TracksLoaded(type) && NoErrors(records, type))
                {
                    var lvi = new ListViewItem(type.TrackString());
                    lvi.SubItems.Add("All tracks successfully loaded.");
                    lvi.ImageIndex = 0;
                    ListView1.Items.Add(lvi);
                }
            }
        }

        // TODO: Is loaded enough to determine 'ready'?
        private bool AllTracksReady => TrackTypes.All(t => airwayNetwork.TracksLoaded(t));

        private bool AllTracksNotReady => TrackTypes.All(t => !airwayNetwork.TracksLoaded(t));

        private void SetMainFormTrackStatus(IEnumerable<Entry> records)
        {
            var loadedTypes = TrackTypes.Where(t => airwayNetwork.TracksLoaded(t));

            var maxSeverity = loadedTypes.Select(t => MaxSeverity(records, t));

            if (maxSeverity.All(s => s == Severity.Advisory) && AllTracksReady)
            {
                statusLbl.Image = Properties.Resources.GreenLight;
                statusLbl.Text = "Tracks: Ready";
            }
            else if (maxSeverity.All(s => s == Severity.Critical) || AllTracksNotReady)
            {
                statusLbl.Image = Properties.Resources.RedLight;
                statusLbl.Text = "Tracks: Not Available";
            }
            else
            {
                statusLbl.Image = Properties.Resources.YellowLight;
                statusLbl.Text = "Tracks: Partly Ready";
            }
        }

        private void RefreshViewTrackBtns()
        {
            viewNatsBtn.Enabled = airwayNetwork.TracksLoaded(TrackType.Nats);
            viewPacotsBtn.Enabled = airwayNetwork.TracksLoaded(TrackType.Pacots);
            viewAusotsBtn.Enabled = airwayNetwork.TracksLoaded(TrackType.Ausots);
        }

        /// <summary>
        /// Download all tracks in the specified track system and enable depends on 
        /// the selection on the UI. During the download the 'download' button is disabled.
        /// </summary>
        public void DownloadTracks(TrackType t)
        {
            var ts = new CancellationTokenSource();
            var downloadBtn = DownloadBtn(t);

            Action cleanup = () =>
            {
                RefreshViewTrackBtns();
                downloadBtn.Enabled = true;
            };

            Func<Task> task = async () =>
            {
                downloadBtn.Enabled = false;

                await airwayNetwork.DownloadTracks(t, ts.Token);
                SyncCBoxEnabled(t);
                RefreshStatus();

                cleanup();
            };

            airwayNetwork.EnqueueTask(t, task, ts, cleanup);
        }
        
        public bool TrackEnabled(TrackType t) =>
            new[]
            {
                CBoxNatsEnabled, CBoxPacotsEnabled, CBoxAusotsEnabled
            }[(int)t].SelectedIndex == 0;
        
        private void SyncCBoxEnabled(TrackType t)
        {
            airwayNetwork.SetTrackEnabled(t, TrackEnabled(t));
        }

        private void CloseForm(object sender, CancelEventArgs e)
        {
            // Do NOT close this form. Hide instead.
            e.Cancel = true;
            Hide();
        }

        private void TxtRichTextBoxContentsResized(object sender, ContentsResizedEventArgs e)
        {
            txtRichTextBox.Height = e.NewRectangle.Height + 10;
        }

        private void DownloadAllBtnClick(object sender, EventArgs e)
        {
            DownloadAllTracks();
        }

        public void DownloadAllTracks()
        {
            TrackTypes.ForEach(t => DownloadTracks(t));
        }

        private void RefreshDownloadAllBtnEnabled(object sender, EventArgs e)
        {
            downloadAllBtn.Enabled = BtnNatsDn.Enabled &&
                BtnPacotsDn.Enabled && BtnAusotsDn.Enabled;
        }

        private static string GetFileDialogFilter()
        {
            var ext = trackFileExtension;
            return $"track files (*{ext})|*{ext}|All files (*.*)|*.*";
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var a = airwayNetwork;
            var msg = TrackTypes
                .Select(t => airwayNetwork.GetTrackMessage(t))
                .Where(m => m != null)
                .ToList();

            if (msg.Count == 0)
            {
                MsgBoxHelper.ShowWarning("No track has been downloaded or imported.");
                return;
            }

            IgnoreException(() => Directory.CreateDirectory(trackFileFolder));
            var saveFileDialog = new SaveFileDialog();

            var ext = trackFileExtension;
            saveFileDialog.Filter = GetFileDialogFilter();
            saveFileDialog.InitialDirectory = Path.GetFullPath(trackFileFolder);
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = saveFileDialog.FileName;

                try
                {
                    File.Delete(file);
                    Directory.CreateDirectory(trackFileFolder);
                    TrackFiles.SaveToFile(msg, file);
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    MsgBoxHelper.ShowWarning("Failed to save file.");
                }
            }
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = GetFileDialogFilter();
            openFileDialog.InitialDirectory = Path.GetFullPath(trackFileFolder);
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog.FileName;

                try
                {
                    var docs = TrackFiles.ReadFromFile(file);
                    docs.ForEach(d => LoadXDoc(d));
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    MsgBoxHelper.ShowWarning($"Failed to load file {file}");
                }
            }

            RefreshViewTrackBtns();
            RefreshStatus();
        }

        private void LoadXDoc(XDocument doc)
        {
            var root = doc.Root;
            var sys = root.Element("TrackSystem").Value;
            var type = sys.ToTrackType();

            airwayNetwork.SetTrackMessage(type, GetTrackMessage(type, doc));
            SyncCBoxEnabled(type);
        }
    }
}
