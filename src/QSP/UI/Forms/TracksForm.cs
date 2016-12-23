using QSP.LibraryExtension;
using QSP.RouteFinding;
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
            BtnNatsDn.Click += (s, e) => DownloadNats();
            BtnPacotsDn.Click += (s, e) => DownloadPacots();
            BtnAusotsDn.Click += (s, e) => DownloadAusots();
            CBoxNatsEnabled.SelectedIndexChanged += CBoxNatsEnabledChanged;
            CBoxPacotsEnabled.SelectedIndexChanged += CBoxPacotsEnabledChanged;
            CBoxAusotsEnabled.SelectedIndexChanged += CBoxAusotsEnabledChanged;
            viewNatsBtn.Click += ViewNatsBtnClick;
            viewPacotsBtn.Click += ViewPacotsBtnClick;
            viewAusotsBtn.Click += ViewAusotsBtnClick;
            BtnNatsDn.EnabledChanged += RefreshDownloadAllBtnEnabled;
            BtnPacotsDn.EnabledChanged += RefreshDownloadAllBtnEnabled;
            BtnAusotsDn.EnabledChanged += RefreshDownloadAllBtnEnabled;
            downloadAllBtn.EnabledChanged += (s, e) => importBtn.Enabled = downloadAllBtn.Enabled;
            airwayNetwork.TrackMessageUpdated += (s, e) => RefreshViewTrackBtns();
            airwayNetwork.StatusRecorder.StatusChanged += (s, e) => RefreshStatus();
            Closing += CloseForm;

            airwayNetwork.NatsEnabled = NatsEnabled;
            airwayNetwork.AusotsEnabled = AusotsEnabled;
            airwayNetwork.PacotsEnabled = PacotsEnabled;
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

        private void ViewAusotsBtnClick(object sender, EventArgs e)
        {
            txtRichTextBox.Text = airwayNetwork.AusotsMessage
                .ToString()
                .TrimEmptyLines();
        }

        private void ViewPacotsBtnClick(object sender, EventArgs e)
        {
            txtRichTextBox.Text = airwayNetwork.PacotsMessage
                .ToString()
                .TrimEmptyLines();
        }

        private void ViewNatsBtnClick(object sender, EventArgs e)
        {
            txtRichTextBox.Text = airwayNetwork.NatsMessage
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
            CBoxNatsEnabled.SelectedIndex = 0;
            CBoxPacotsEnabled.SelectedIndex = 0;
            CBoxAusotsEnabled.SelectedIndex = 0;
        }

        public static string TrackString(TrackType item)
        {
            return new[] { "NATs", "PACOTs", "AUSOTS" }[(int)item];
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
            if (airwayNetwork.NatsLoaded)
            {
                var severity = (int)MaxSeverity(records, TrackType.Nats);
                PicBoxNats.Image = myImageList.Images[severity];
            }

            if (airwayNetwork.PacotsLoaded)
            {
                var severity = (int)MaxSeverity(records, TrackType.Pacots);
                PicBoxPacots.Image = myImageList.Images[severity];
            }

            if (airwayNetwork.AusotsLoaded)
            {
                var severity = (int)MaxSeverity(records, TrackType.Ausots);
                PicBoxAusots.Image = myImageList.Images[severity];
            }
        }

        private static readonly IReadOnlyList<TrackType> trackTypes = new[]
        {
            TrackType.Nats,
            TrackType.Pacots,
            TrackType.Ausots
        };

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
                var lvi = new ListViewItem(TrackString(type));
                lvi.SubItems.Add(i.Message);
                lvi.ImageIndex = (int)i.Severity;
                ListView1.Items.Add(lvi);
            }

            foreach (var type in trackTypes)
            {
                if (airwayNetwork.TrackedLoaded(type) && NoErrors(records, type))
                {
                    var lvi = new ListViewItem(TrackString(type));
                    lvi.SubItems.Add("All tracks successfully loaded.");
                    lvi.ImageIndex = 0;
                    ListView1.Items.Add(lvi);
                }
            }
        }

        private void SetMainFormTrackStatus(IEnumerable<Entry> records)
        {
            var loadedTypes = trackTypes.Where(t => airwayNetwork.TrackedLoaded(t));

            var maxSeverity = loadedTypes.Select(t => MaxSeverity(records, t));

            if (maxSeverity.All(s => s == Severity.Advisory))
            {
                statusLbl.Image = Properties.Resources.GreenLight;
                statusLbl.Text = "Tracks: Ready";
            }
            else if (maxSeverity.All(s => s == Severity.Critical))
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
            viewNatsBtn.Enabled = airwayNetwork.NatsLoaded;
            viewPacotsBtn.Enabled = airwayNetwork.PacotsLoaded;
            viewAusotsBtn.Enabled = airwayNetwork.AusotsLoaded;
        }

        /// <summary>
        /// Download NATs and enable depends on the selection on the UI.
        /// During the download the 'download' button is disabled.
        /// </summary>
        public void DownloadNats()
        {
            var ts = new CancellationTokenSource();

            Action cleanup = () =>
            {
                RefreshViewTrackBtns();
                BtnNatsDn.Enabled = true;
            };

            Func<Task> task = async () =>
            {
                BtnNatsDn.Enabled = false;

                await airwayNetwork.DownloadNats(ts.Token);
                airwayNetwork.NatsEnabled = NatsEnabled;
                RefreshStatus();

                cleanup();
            };

            airwayNetwork.EnqueueNatsTask(task, ts, cleanup);
        }

        public void DownloadPacots()
        {
            var ts = new CancellationTokenSource();

            Action cleanup = () =>
            {
                RefreshViewTrackBtns();
                BtnPacotsDn.Enabled = true;
            };

            Func<Task> task = async () =>
            {
                BtnPacotsDn.Enabled = false;

                await airwayNetwork.DownloadPacots(ts.Token);
                airwayNetwork.PacotsEnabled = PacotsEnabled;
                RefreshStatus();

                cleanup();
            };

            airwayNetwork.EnqueuePacotsTask(task, ts, cleanup);
        }

        public void DownloadAusots()
        {
            var ts = new CancellationTokenSource();

            Action cleanup = () =>
            {
                RefreshViewTrackBtns();
                BtnAusotsDn.Enabled = true;
            };

            Func<Task> task = async () =>
            {
                BtnAusotsDn.Enabled = false;

                await airwayNetwork.DownloadAusots(ts.Token);
                airwayNetwork.AusotsEnabled = AusotsEnabled;
                RefreshStatus();

                cleanup();
            };

            airwayNetwork.EnqueueAusotsTask(task, ts, cleanup);
        }

        public bool NatsEnabled => CBoxNatsEnabled.SelectedIndex == 0;

        public bool PacotsEnabled => CBoxPacotsEnabled.SelectedIndex == 0;

        public bool AusotsEnabled => CBoxAusotsEnabled.SelectedIndex == 0;

        private void CBoxNatsEnabledChanged(object sender, EventArgs e)
        {
            airwayNetwork.NatsEnabled = NatsEnabled;
        }

        private void CBoxPacotsEnabledChanged(object sender, EventArgs e)
        {
            airwayNetwork.PacotsEnabled = PacotsEnabled;
        }

        private void CBoxAusotsEnabledChanged(object sender, EventArgs e)
        {
            airwayNetwork.AusotsEnabled = AusotsEnabled;
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
            DownloadNats();
            DownloadPacots();
            DownloadAusots();
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
            var msg = new List<TrackMessage>();

            if (a.NatsLoaded) msg.Add(a.NatsMessage);
            if (a.PacotsLoaded) msg.Add(a.PacotsMessage);
            if (a.AusotsLoaded) msg.Add(a.AusotsMessage);

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

            if (sys == NatsMessage.TrackSystem)
            {
                airwayNetwork.NatsMessage = new NatsMessage(doc);
                airwayNetwork.NatsEnabled = NatsEnabled;
            }
            else if (sys == PacotsMessage.TrackSystem)
            {
                airwayNetwork.PacotsMessage = new PacotsMessage(doc);
                airwayNetwork.PacotsEnabled = PacotsEnabled;
            }
            else if (sys == AusotsMessage.TrackSystem)
            {
                airwayNetwork.AusotsMessage = new AusotsMessage(doc);
                airwayNetwork.AusotsEnabled = AusotsEnabled;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
