using QSP.Core;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.RouteFinding.Tracks.Interaction.Interactions;
using static QSP.RouteFinding.Tracks.Interaction.StatusRecorder;
using static QSP.UI.Utilities;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP
{
    public partial class TracksForm
    {
        private MainForm frmMain;
        private ImageList myImageList;

        private Severity natsAvail;
        private Severity pacotsAvail;
        private Severity ausotsAvail;

        private void initImages()
        {
            myImageList = new ImageList();

            myImageList.ImageSize = new Size(24, 24);
            myImageList.Images.Add(Properties.Resources.checkIconLarge);
            myImageList.Images.Add(Properties.Resources.CautionIcon);
            myImageList.Images.Add(Properties.Resources.deleteIconLarge);

            ListView1.SmallImageList = myImageList;
        }

        private void initCBox()
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

        private void refreshStatus(TrackType type)
        {
            //remove old items for the same type
            var items = ListView1.Items;
            var text = TrackString(type);

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].Text == text)
                {
                    items.RemoveAt(i);
                }
            }

            addToListView(TrackStatusRecorder.Records, type);
            setPBox(type);
            setMainFormTrackStatus();
        }

        private void initPBoxes()
        {
            PBoxNats.Image = null;
            PBoxPacots.Image = null;
            PBoxAusots.Image = null;
        }

        private void setPBox(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    PBoxNats.Image = myImageList.Images[(int)natsAvail];
                    break;

                case TrackType.Pacots:
                    PBoxPacots.Image = myImageList.Images[(int)pacotsAvail];
                    break;

                case TrackType.Ausots:
                    PBoxAusots.Image = myImageList.Images[(int)ausotsAvail];
                    break;

                default:
                    throw new EnumNotSupportedException();
            }
        }

        private void setAvail(TrackType trkType, Severity severity)
        {
            switch (trkType)
            {
                case TrackType.Nats:
                    natsAvail = (Severity)Math.Max((int)natsAvail, (int)severity);

                    break;
                case TrackType.Pacots:
                    pacotsAvail = (Severity)Math.Max((int)pacotsAvail, (int)severity);

                    break;
                case TrackType.Ausots:
                    ausotsAvail = (Severity)Math.Max((int)ausotsAvail, (int)severity);

                    break;
                default:

                    throw new EnumNotSupportedException();
            }
        }

        private void addToListView(ReadOnlyCollection<Entry> records, TrackType para)
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

                    setAvail(para, i.Severity);
                    noError = false;
                }
            }

            if (noError)
            {
                ListViewItem lvi = new ListViewItem(TrackString(para));
                lvi.SubItems.Add("All tracks succesfully added.");
                lvi.ImageIndex = 0;
                ListView1.Items.Add(lvi);

                setAvail(para, 0);
            }
        }

        private void setMainFormTrackStatus()
        {
            if (natsAvail == Severity.Advisory && pacotsAvail == Severity.Advisory && ausotsAvail == Severity.Advisory)
            {
                frmMain.LblTrackDownloadStatus.Image = Properties.Resources.GreenLight;
                frmMain.LblTrackDownloadStatus.Text = "Tracks: Ready";

            }
            else if (natsAvail == Severity.Critical && pacotsAvail == Severity.Critical && ausotsAvail == Severity.Critical)
            {
                frmMain.LblTrackDownloadStatus.Image = Properties.Resources.RedLight;
                frmMain.LblTrackDownloadStatus.Text = "Tracks: Not Available";

            }
            else
            {
                frmMain.LblTrackDownloadStatus.Image = Properties.Resources.YellowLight;
                frmMain.LblTrackDownloadStatus.Text = "Tracks: Partly Ready";
            }
        }

        private void TracksForm_Load(object sender, EventArgs e)
        {
            frmMain = MainFormInstance();
            initImages();
            initCBox();
            initPBoxes();
            initManagers();

            //initialize enums
            natsAvail = Severity.Advisory;
            pacotsAvail = Severity.Advisory;
            ausotsAvail = Severity.Advisory;

            // The event handlers are added after the form is created. This way the events won't fire at form creation.
            CBoxNatsEnabled.SelectedIndexChanged += CBoxNatsEnabled_SelectedIndexChanged;
            CBoxPacotsEnabled.SelectedIndexChanged += CBoxPacotsEnabled_SelectedIndexChanged;
            CBoxAusotsEnabled.SelectedIndexChanged += CBoxAusotsEnabled_SelectedIndexChanged;
        }

        private async void BtnNatsDn_Click(object sender, EventArgs e)
        {
            await dnNats();
        }

        private async void BtnPacotsDn_Click(object sender, EventArgs e)
        {
            await dnPacots();
        }

        private async void BtnAusotsDn_Click(object sender, EventArgs e)
        {
            await dnAusots();
        }

        private void CBoxNatsEnabled_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBoxNatsEnabled.SelectedIndex == 0)
            {
                //enabled
                NatsManager.AddToWptList();
            }
            else
            {
                WptList.DisableTrack(TrackType.Nats);
            }
        }

        private void CBoxPacotsEnabled_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBoxPacotsEnabled.SelectedIndex == 0)
            {
                //enabled
                PacotsManager.AddToWptList();
            }
            else
            {
                WptList.DisableTrack(TrackType.Pacots);
            }

        }

        private void CBoxAusotsEnabled_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBoxAusotsEnabled.SelectedIndex == 0)
            {
                //enabled
                AusotsManager.AddToWptList();
            }
            else
            {
                WptList.DisableTrack(TrackType.Ausots);
            }

        }

        private async Task dnNats()
        {

            BtnNatsDn.Enabled = false;
            BtnNatsDn.Text = "Downloading";

            await SetNats();
            refreshStatus(TrackType.Nats);

            BtnNatsDn.Enabled = true;
            BtnNatsDn.Text = "Download";

        }

        private async Task dnPacots()
        {

            BtnPacotsDn.Enabled = false;
            BtnPacotsDn.Text = "Downloading";

            await SetPacots();
            refreshStatus(TrackType.Pacots);

            BtnPacotsDn.Enabled = true;
            BtnPacotsDn.Text = "Download";

        }

        private async Task dnAusots()
        {

            BtnAusotsDn.Enabled = false;
            BtnAusotsDn.Text = "Downloading";

            await SetAusots();
            refreshStatus(TrackType.Ausots);

            BtnAusotsDn.Enabled = true;
            BtnAusotsDn.Text = "Download";

        }

        private void closeForm(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Do NOT close this form
            e.Cancel = true;
            this.Hide();
        }

        private static void initManagers()
        {
            NatsManager = new NatHandler();
            PacotsManager = new PacotsHandler();
            AusotsManager = new AusotsHandler();
        }
        public TracksForm()
        {
            Closing += closeForm;
            Load += TracksForm_Load;
            InitializeComponent();
        }

    }
}
