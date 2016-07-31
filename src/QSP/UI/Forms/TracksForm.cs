using QSP.Common;
using QSP.LibraryExtension;
using QSP.RouteFinding;
using QSP.RouteFinding.Tracks.Common;
using QSP.UI.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static QSP.RouteFinding.Tracks.Interaction.StatusRecorder;

namespace QSP.UI.Forms
{
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

            viewPacotsBtnStyle.Activate();
            viewNatsBtnStyle.Activate();
            viewAusotsBtnStyle.Activate();
            downloadNatsBtnStyle.Activate();
            downloadPacotsBtnStyle.Activate();
            downloadAusotsBtnStyle.Activate();

            viewPacotsBtn.Enabled = false;
            viewNatsBtn.Enabled = false;
            viewAusotsBtn.Enabled = false;
            BtnNatsDn.Enabled = true;
            BtnPacotsDn.Enabled = true;
            BtnAusotsDn.Enabled = true;
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
                    return "NATs";

                case TrackType.Pacots:
                    return "PACOTs";

                case TrackType.Ausots:
                    return "AUSOTs";

                default:
                    throw new EnumNotSupportedException();
            }
        }

        private void RefreshStatus(TrackType type)
        {
            // Remove old items for the same type
            var items = ListView1.Items;
            var text = TrackString(type);

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].Text == text)
                {
                    items.RemoveAt(i);
                }
            }

            AddToListView(airwayNetwork.StatusRecorder.Records, type);
            RefreshListViewColumnWidth();
            SetPBox(type);
            SetMainFormTrackStatus();
        }

        private void InitPicBoxes()
        {
            PicBoxNats.Image = null;
            PicBoxPacots.Image = null;
            PicBoxAusots.Image = null;
        }

        private void SetPBox(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    PicBoxNats.Image = myImageList.Images[(int)natsAvail];
                    break;

                case TrackType.Pacots:
                    PicBoxPacots.Image = myImageList.Images[(int)pacotsAvail];
                    break;

                case TrackType.Ausots:
                    PicBoxAusots.Image = myImageList.Images[(int)ausotsAvail];
                    break;

                default:
                    throw new EnumNotSupportedException();
            }
        }

        private void SetAvail(TrackType trkType, Severity severity)
        {
            switch (trkType)
            {
                case TrackType.Nats:
                    natsAvail =
                        (Severity)Math.Max((int)natsAvail, (int)severity);
                    break;

                case TrackType.Pacots:
                    pacotsAvail =
                        (Severity)Math.Max((int)pacotsAvail, (int)severity);
                    break;

                case TrackType.Ausots:
                    ausotsAvail =
                        (Severity)Math.Max((int)ausotsAvail, (int)severity);
                    break;

                default:
                    throw new EnumNotSupportedException();
            }
        }

        private void AddToListView(IEnumerable<Entry> records, TrackType para)
        {
            bool noError = true;

            foreach (var i in records)
            {
                if (i.Type == para)
                {
                    ListViewItem lvi = new ListViewItem(TrackString(para));
                    lvi.SubItems.Add(i.Message);
                    lvi.ImageIndex = (int)i.Severity;
                    ListView1.Items.Add(lvi);

                    SetAvail(para, i.Severity);
                    noError = false;
                }
            }

            if (noError)
            {
                var lvi = new ListViewItem(TrackString(para));
                lvi.SubItems.Add("All tracks successfully downloaded.");
                lvi.ImageIndex = 0;
                ListView1.Items.Add(lvi);

                SetAvail(para, 0);
            }
        }

        private void SetMainFormTrackStatus()
        {
            if (natsAvail == Severity.Advisory &&
                pacotsAvail == Severity.Advisory &&
                ausotsAvail == Severity.Advisory)
            {
                statusLbl.Image = Properties.Resources.GreenLight;
                statusLbl.Text = "Tracks: Ready";

            }
            else if (natsAvail == Severity.Critical &&
                pacotsAvail == Severity.Critical &&
                ausotsAvail == Severity.Critical)
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
            RefreshStatus(TrackType.Nats);
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
            RefreshStatus(TrackType.Pacots);
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
            RefreshStatus(TrackType.Ausots);
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

        private void txtRichTextBox_ContentsResized(
            object sender, ContentsResizedEventArgs e)
        {
            txtRichTextBox.Height = e.NewRectangle.Height + 10;
        }
    }
}
