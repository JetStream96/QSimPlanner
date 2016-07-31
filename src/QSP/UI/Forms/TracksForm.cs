using QSP.Common;
using QSP.LibraryExtension;
using QSP.RouteFinding;
using QSP.RouteFinding.Tracks.Common;
using QSP.UI.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.RouteFinding.Tracks.Interaction.StatusRecorder;

namespace QSP.UI.Forms
{
    // TODO: Add save and import feature.
    public partial class TracksForm
    {
        private AirwayNetwork airwayNetwork;

        private ImageList myImageList;
        private ToolStripStatusLabel statusLbl;
        private Severity natsAvail;
        private Severity pacotsAvail;
        private Severity ausotsAvail;

        public TracksForm()
        {
            InitializeComponent();
        }

        public void Init(
            AirwayNetwork airwayNetwork,
            ToolStripStatusLabel statusLbl)
        {
            this.airwayNetwork = airwayNetwork;
            this.statusLbl = statusLbl;

            RefreshListViewColumnWidth();
            InitImages();
            InitCBox();
            InitPicBoxes();
            InitBtns();

            // Initialize enums
            natsAvail = Severity.Advisory;
            pacotsAvail = Severity.Advisory;
            ausotsAvail = Severity.Advisory;

            // The event handlers are added after the form is created. 
            // This way the events won't fire at form creation.
            CBoxNatsEnabled.SelectedIndexChanged += CBoxNatsEnabledChanged;
            CBoxPacotsEnabled.SelectedIndexChanged += CBoxPacotsEnabledChanged;
            CBoxAusotsEnabled.SelectedIndexChanged += CBoxAusotsEnabledChanged;
            viewNatsBtn.Click += ViewNatsBtnClick;
            viewPacotsBtn.Click += ViewPacotsBtnClick;
            viewAusotsBtn.Click += ViewAusotsBtnClick;
            BtnNatsDn.EnabledChanged += RefreshDownloadAllBtnEnabled;
            BtnPacotsDn.EnabledChanged += RefreshDownloadAllBtnEnabled;
            BtnAusotsDn.EnabledChanged += RefreshDownloadAllBtnEnabled;
            airwayNetwork.GetTracksFinished += (s, e) => RefreshStatus();
            Closing += CloseForm;
        }

        private void RefreshListViewColumnWidth()
        {
            ListView1.Columns[1].Width = -2;
        }

        private void InitBtns()
        {
            var colorStyle = new ControlDisableStyleController.ColorStyle(
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            var viewNatsBtnStyle = new ControlDisableStyleController(
                viewNatsBtn, colorStyle);

            var viewPacotsBtnStyle = new ControlDisableStyleController(
                viewPacotsBtn, colorStyle);

            var viewAusotsBtnStyle = new ControlDisableStyleController(
                 viewAusotsBtn, colorStyle);

            var downloadNatsBtnStyle = new ControlDisableStyleController(
                BtnNatsDn, colorStyle);

            var downloadPacotsBtnStyle = new ControlDisableStyleController(
                BtnPacotsDn, colorStyle);

            var downloadAusotsBtnStyle = new ControlDisableStyleController(
                BtnAusotsDn, colorStyle);

            var downloadAllBtnStyle = new ControlDisableStyleController(
                downloadAllBtn, colorStyle);

            viewPacotsBtnStyle.Activate();
            viewNatsBtnStyle.Activate();
            viewAusotsBtnStyle.Activate();
            downloadNatsBtnStyle.Activate();
            downloadPacotsBtnStyle.Activate();
            downloadAusotsBtnStyle.Activate();
            downloadAllBtnStyle.Activate();

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
            txtRichTextBox.Text = airwayNetwork.GetAusotsMessage()
                .ToString()
                .TrimEmptyLines();
        }

        private void ViewPacotsBtnClick(object sender, EventArgs e)
        {
            txtRichTextBox.Text = airwayNetwork.GetPacotsMessage()
                .ToString()
                .TrimEmptyLines();
        }

        private void ViewNatsBtnClick(object sender, EventArgs e)
        {
            txtRichTextBox.Text = airwayNetwork.GetNatsMessage()
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
            switch (item)
            {
                case TrackType.Nats:
                    return "NATS";

                case TrackType.Pacots:
                    return "PACOTS";

                case TrackType.Ausots:
                    return "AUSOTS";

                default:
                    throw new EnumNotSupportedException();
            }
        }

        private void RefreshStatus()
        {
            var records = airwayNetwork.StatusRecorder.Records;
            AddToListView(records);
            RefreshListViewColumnWidth();
            InitPicBoxes();
            SetPicBox();
            SetMainFormTrackStatus(records);
        }

        private void InitPicBoxes()
        {
            PicBoxNats.Image = null;
            PicBoxPacots.Image = null;
            PicBoxAusots.Image = null;
        }

        private void SetPicBox()
        {
            if (airwayNetwork.NatsLoaded)
            {
                PicBoxNats.Image = myImageList.Images[(int)natsAvail];
            }

            if (airwayNetwork.PacotsLoaded)
            {
                PicBoxPacots.Image = myImageList.Images[(int)pacotsAvail];
            }

            if (airwayNetwork.AusotsLoaded)
            {
                PicBoxAusots.Image = myImageList.Images[(int)ausotsAvail];
            }
        }

        private TrackType[] trackTypes =
        {
            TrackType.Nats,
            TrackType.Pacots,
            TrackType.Ausots
        };

        private static Severity MaxSeverity(
            IEnumerable<Entry> records, TrackType type)
        {
            var filtered = records.Where(r => r.Type == type).ToList();
            if (filtered.Any() == false) return Severity.Advisory;
            return (Severity)filtered.Max(i => (int)i.Severity);
        }

        private static bool NoErrors(
            IEnumerable<Entry> records, TrackType type)
        {
            return MaxSeverity(records, type) == Severity.Advisory;
        }

        private void AddToListView(IEnumerable<Entry> records)
        {
            ListView1.Items.Clear();

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
                if (airwayNetwork.TrackedLoaded(type) &&
                    NoErrors(records, type))
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
            var loadedTypes = trackTypes.Where(
                t => airwayNetwork.TrackedLoaded(t));

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

        private async void BtnNatsDn_Click(object sender, EventArgs e)
        {
            BtnNatsDn.Enabled = false;
            BtnNatsDn.Text = "Downloading";

            await airwayNetwork.DownloadNats();
            if (NatsEnabled) airwayNetwork.EnableNats();
            viewNatsBtn.Enabled = true;

            BtnNatsDn.Enabled = true;
            BtnNatsDn.Text = "Download";
        }

        private async void BtnPacotsDn_Click(object sender, EventArgs e)
        {
            BtnPacotsDn.Enabled = false;
            BtnPacotsDn.Text = "Downloading";

            await airwayNetwork.DownloadPacots();
            if (PacotsEnabled) airwayNetwork.EnablePacots();
            viewPacotsBtn.Enabled = true;

            BtnPacotsDn.Enabled = true;
            BtnPacotsDn.Text = "Download";
        }

        private async void BtnAusotsDn_Click(object sender, EventArgs e)
        {
            BtnAusotsDn.Enabled = false;
            BtnAusotsDn.Text = "Downloading";

            await airwayNetwork.DownloadAusots();
            if (AusotsEnabled) airwayNetwork.EnableAusots();
            viewAusotsBtn.Enabled = true;

            BtnAusotsDn.Enabled = true;
            BtnAusotsDn.Text = "Download";
        }

        private bool NatsEnabled
        {
            get { return CBoxNatsEnabled.SelectedIndex == 0; }
        }

        private bool PacotsEnabled
        {
            get { return CBoxPacotsEnabled.SelectedIndex == 0; }
        }

        private bool AusotsEnabled
        {
            get { return CBoxAusotsEnabled.SelectedIndex == 0; }
        }

        private void CBoxNatsEnabledChanged(object sender, EventArgs e)
        {
            if (NatsEnabled)
            {
                airwayNetwork.EnableNats();
            }
            else
            {
                airwayNetwork.DisableNats();
            }
        }

        private void CBoxPacotsEnabledChanged(object sender, EventArgs e)
        {
            if (PacotsEnabled)
            {
                airwayNetwork.EnablePacots();
            }
            else
            {
                airwayNetwork.DisablePacots();
            }
        }

        private void CBoxAusotsEnabledChanged(object sender, EventArgs e)
        {
            if (AusotsEnabled)
            {
                airwayNetwork.EnableAusots();
            }
            else
            {
                airwayNetwork.DisableAusots();
            }
        }

        private void CloseForm(object sender, CancelEventArgs e)
        {
            // Do NOT close this form. Hide instead.
            e.Cancel = true;
            Hide();
        }

        private void TxtRichTextBoxContentsResized(
            object sender, ContentsResizedEventArgs e)
        {
            txtRichTextBox.Height = e.NewRectangle.Height + 10;
        }

        private void DownloadAllBtnClick(object sender, EventArgs e)
        {
            BtnNatsDn_Click(this, EventArgs.Empty);
            BtnPacotsDn_Click(this, EventArgs.Empty);
            BtnAusotsDn_Click(this, EventArgs.Empty);
        }

        private void RefreshDownloadAllBtnEnabled(object sender, EventArgs e)
        {
            downloadAllBtn.Enabled = BtnNatsDn.Enabled &&
                BtnPacotsDn.Enabled && BtnAusotsDn.Enabled;
        }
    }
}
