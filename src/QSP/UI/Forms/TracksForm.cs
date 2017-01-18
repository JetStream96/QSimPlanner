using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Tracks;
using QSP.RouteFinding.Tracks.Actions;
using QSP.RouteFinding.Tracks.Common;
using QSP.UI.Controllers;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using QSP.UI.MsgBox;
using static QSP.RouteFinding.Tracks.AirwayNetwork;
using static QSP.RouteFinding.Tracks.Common.Helpers;
using static QSP.RouteFinding.Tracks.Interaction.StatusRecorder;
using static QSP.Utilities.ExceptionHelpers;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Forms
{
    public partial class TracksForm
    {
        private static readonly string trackFileFolder = "Tracks";
        private static readonly string trackFileExtension = ".track";

        private AirwayNetwork airwayNetwork;
        private ImageList imageList;
        private ToolStripStatusLabel statusLbl;

        public TracksForm()
        {
            InitializeComponent();
        }

        public void Init(AirwayNetwork airwayNetwork, ToolStripStatusLabel statusLbl)
        {
            this.airwayNetwork = airwayNetwork;
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
                DownloadBtn(t).Click += (s, e) => DownloadAndEnableTracks(t);
                EnabledCBox(t).Enabled = false;
                EnabledCBox(t).SelectedIndexChanged += (s, e) => SetTrackEnabled(t);
                ViewBtn(t).Click += (s, e) => ViewTracks(t);
                DownloadBtn(t).EnabledChanged += RefreshDownloadAllBtnEnabled;
            });

            downloadAllBtn.Click += (s, e) => DownloadAndEnableTracks();
            downloadAllBtn.EnabledChanged += (s, e) => importBtn.Enabled = downloadAllBtn.Enabled;
            airwayNetwork.TrackMessageUpdated += (s, e) => RefreshViewTrackBtns();
            airwayNetwork.StatusChanged += (s, e) => RefreshStatus(((TrackEventArg)e).TrackType);
            Closing += CloseForm;
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

            TrackTypes.ForEach(t =>
            {
                ViewBtn(t).Enabled = false;
                DownloadBtn(t).Enabled = true;
            });

            downloadAllBtn.Enabled = true;
        }

        private Button DownloadBtn(TrackType t) =>
            new[] { BtnNatsDn, BtnPacotsDn, BtnAusotsDn }[(int)t];

        private Button ViewBtn(TrackType t) =>
            new[] { viewNatsBtn, viewPacotsBtn, viewAusotsBtn }[(int)t];

        private ComboBox EnabledCBox(TrackType t) =>
            new[] { CBoxNatsEnabled, CBoxPacotsEnabled, CBoxAusotsEnabled }[(int)t];

        private PictureBox PicBox(TrackType t) =>
            new[] { PicBoxNats, PicBoxPacots, PicBoxAusots }[(int)t];

        private void ViewTracks(TrackType t)
        {
            txtRichTextBox.Text = airwayNetwork.GetTrackMessage(t)
                .ToString()
                .TrimEmptyLines();
        }

        private Image[] statusImages = new[]
        {
            Properties.Resources.okIcon,
            Properties.Resources.CautionIcon,
            Properties.Resources.errorIcon
        };

        private void InitImages()
        {
            imageList = new ImageList();

            var newSize = new Size(22, 22);
            imageList.ImageSize = newSize;
            statusImages.ForEach(i => imageList.Images.Add(ImageUtil.Resize(i, newSize)));
            ListView1.SmallImageList = imageList;
        }

        private void InitCBox()
        {
            TrackTypes.ForEach(t => EnabledCBox(t).SelectedIndex = 0);
        }

        public void RefreshStatus(TrackType type)
        {
            var records = airwayNetwork.StatusRecorder.Records;
            AddToListView(records);
            RefreshListViewColumnWidth();
            SetPicBox(records, type);
            SetMainFormTrackStatus(records);
        }

        private void InitPicBoxes()
        {
            TrackTypes.ForEach(t => PicBox(t).Image = null);
        }

        private void SetPicBox(IEnumerable<Entry> records, TrackType t)
        {
            if (airwayNetwork.TracksLoaded(t))
            {
                var severity = (int)MaxSeverity(records, t);
                PicBox(t).SetImageHighQuality(statusImages[severity]);
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

        private bool AllTracksAdded => TrackTypes.All(t => airwayNetwork.InWptList(t));

        private bool AllTracksNotAdded => TrackTypes.All(t => !airwayNetwork.InWptList(t));

        private void SetMainFormTrackStatus(IEnumerable<Entry> records)
        {
            var loadedTypes = TrackTypes.Where(t => airwayNetwork.TracksLoaded(t));
            var maxSeverity = loadedTypes.Select(t => MaxSeverity(records, t)).ToList();

            if (maxSeverity.All(s => s == Severity.Advisory) && AllTracksAdded)
            {
                statusLbl.Image = Properties.Resources.GreenLight;
                statusLbl.Text = "Tracks: Ready";
            }
            else if (maxSeverity.All(s => s == Severity.Critical) || AllTracksNotAdded)
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
            TrackTypes.ForEach(t => ViewBtn(t).Enabled = airwayNetwork.TracksLoaded(t));
        }

        private void DisableUserInputs(TrackType t)
        {
            DownloadBtn(t).Enabled = false;
            EnabledCBox(t).Enabled = false;
        }

        private void EnableUserInputs(TrackType t)
        {
            DownloadBtn(t).Enabled = true;
            EnabledCBox(t).Enabled = true;
        }

        public void DownloadAndEnableTracks()
        {
            TrackTypes.ForEach(t => DownloadAndEnableTracks(t));
        }

        private void SetProcessingImage(TrackType t)
        {
            PicBox(t).SetImageHighQuality(Properties.Resources.processing);
        }

        public void DownloadAndEnableTracks(TrackType t)
        {
            var action = new ActionSequence(
                () =>
                {
                    DisableUserInputs(t);
                    SetProcessingImage(t);
                    EnabledCBox(t).SelectedIndex = 0;
                },
                () => EnableUserInputs(t));

            airwayNetwork.DownloadAndEnableTracks(t, action);
        }

        public async Task Update(WaypointList wptList, AirportManager airportList)
        {
            var action = new NetworkUpdateAction(
                (t) => SetTrackEnabled(t),
                (t) => DownloadAndEnableTracks(t));

            await airwayNetwork.Update(wptList, airportList, action);
        }

        public bool TrackEnabled(TrackType t) => EnabledCBox(t).SelectedIndex == 0;

        public void SetTrackEnabled(TrackType t)
        {
            var actions = new ActionSequence(
                () => DownloadBtn(t).Enabled = false,
                () => DownloadBtn(t).Enabled = true);

            airwayNetwork.SetTrackEnabled(t, TrackEnabled(t), actions);
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
            var msg = TrackTypes
                .Select(t => airwayNetwork.GetTrackMessage(t))
                .Where(m => m != null)
                .ToList();

            if (msg.Count == 0)
            {
                this.ShowWarning("No track has been downloaded or imported.");
                return;
            }

            IgnoreException(() => Directory.CreateDirectory(trackFileFolder));
            var saveFileDialog = new SaveFileDialog();

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
                    Log(ex);
                    this.ShowWarning("Failed to save file.");
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
                    Log(ex);
                    this.ShowWarning($"Failed to load file {file}");
                }
            }
        }

        private void LoadXDoc(XDocument doc)
        {
            var root = doc.Root;
            var sys = root.Element("TrackSystem").Value;
            var type = sys.ToTrackType();

            var seq = new ActionSequence(
                () =>
                {
                    DisableUserInputs(type);
                    SetProcessingImage(type);
                    EnabledCBox(type).SelectedIndex = 0;
                },
                () => EnableUserInputs(type));

            airwayNetwork.SetTrackMessageAndEnable(type, GetTrackMessage(type, doc), seq);
        }
    }
}
