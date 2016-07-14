using QSP.Common;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.RouteFinding.Tracks.Interaction.Interactions;
using static QSP.RouteFinding.Tracks.Interaction.StatusRecorder;

namespace QSP
{
    public partial class TracksForm
    {
        private TrackInUseCollection tracksInUse;
        private NatsHandler NatsManager;
        private PacotsHandler PacotsManager;
        private AusotsHandler AusotsManager;
        private StatusRecorder TrackStatusRecorder;

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
            WaypointList wptList, 
            AirportManager airportList, 
            TrackInUseCollection tracksInUse,
            ToolStripStatusLabel statusLbl)
        {
            this.tracksInUse = tracksInUse;
            this.statusLbl = statusLbl;

            InitData(wptList, airportList);
            InitImages();
            InitCBox();
            InitPicBoxes();

            // Initialize enums
            natsAvail = Severity.Advisory;
            pacotsAvail = Severity.Advisory;
            ausotsAvail = Severity.Advisory;

            // The event handlers are added after the form is created. 
            // This way the events won't fire at form creation.
            CBoxNatsEnabled.SelectedIndexChanged += CBoxNatsEnabledChanged;
            CBoxPacotsEnabled.SelectedIndexChanged += CBoxPacotsEnabledChanged;
            CBoxAusotsEnabled.SelectedIndexChanged += CBoxAusotsEnabledChanged;
            Closing += CloseForm;
        }

        private void InitData(WaypointList wptList, AirportManager airportList)
        {
            NatsManager = new NatsHandler(
                new NatsDownloader(),
                wptList,
                wptList.GetEditor(),
                TrackStatusRecorder,
                airportList,
                tracksInUse);

            PacotsManager = new PacotsHandler(
                new PacotsDownloader(),
                wptList,
                wptList.GetEditor(),
                TrackStatusRecorder,
                airportList,
                tracksInUse);

            AusotsManager = new AusotsHandler(
                new AusotsDownloader(),
                wptList,
                wptList.GetEditor(),
                TrackStatusRecorder,
                airportList,
                tracksInUse);

            TrackStatusRecorder = new StatusRecorder();
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

            AddToListView(TrackStatusRecorder.Records, type);
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

        private void AddToListView(ReadOnlyCollection<Entry> records, TrackType para)
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
                lvi.SubItems.Add("All tracks succesfully added.");
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
            await DnNats();
        }

        private async void BtnPacotsDn_Click(object sender, EventArgs e)
        {
            await DnPacots();
        }

        private async void BtnAusotsDn_Click(object sender, EventArgs e)
        {
            await DnAusots();
        }

        private void CBoxNatsEnabledChanged(object sender, EventArgs e)
        {
            if (CBoxNatsEnabled.SelectedIndex == 0)
            {
                //enabled
                NatsManager.AddToWaypointList();
            }
            else
            {
                NatsManager.UndoEdit();
            }
        }

        private void CBoxPacotsEnabledChanged(object sender, EventArgs e)
        {
            if (CBoxPacotsEnabled.SelectedIndex == 0)
            {
                //enabled
                PacotsManager.AddToWaypointList();
            }
            else
            {
                PacotsManager.UndoEdit();
            }

        }

        private void CBoxAusotsEnabledChanged(object sender, EventArgs e)
        {
            if (CBoxAusotsEnabled.SelectedIndex == 0)
            {
                //enabled
                AusotsManager.AddToWaypointList();
            }
            else
            {
                AusotsManager.UndoEdit();
            }
        }

        private async Task DnNats()
        {
            BtnNatsDn.Enabled = false;
            BtnNatsDn.Text = "Downloading";

            await SetNats(NatsManager, TrackStatusRecorder);
            RefreshStatus(TrackType.Nats);

            BtnNatsDn.Enabled = true;
            BtnNatsDn.Text = "Download";
        }

        private async Task DnPacots()
        {
            BtnPacotsDn.Enabled = false;
            BtnPacotsDn.Text = "Downloading";

            await SetPacots(PacotsManager, TrackStatusRecorder);
            RefreshStatus(TrackType.Pacots);

            BtnPacotsDn.Enabled = true;
            BtnPacotsDn.Text = "Download";
        }

        private async Task DnAusots()
        {
            BtnAusotsDn.Enabled = false;
            BtnAusotsDn.Text = "Downloading";

            await SetAusots(AusotsManager, TrackStatusRecorder);
            RefreshStatus(TrackType.Ausots);

            BtnAusotsDn.Enabled = true;
            BtnAusotsDn.Text = "Download";
        }

        private void CloseForm(object sender, CancelEventArgs e)
        {
            // Do NOT close this form. Hide instead.
            e.Cancel = true;
            Hide();
        }
    }
}
