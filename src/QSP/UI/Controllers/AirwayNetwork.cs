using System;
using System.Threading;
using System.Threading.Tasks;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Tasks;
using QSP.UI.Forms;

namespace QSP.UI.Controllers
{
    // Handles updating navigation data and reparse tracks after 
    // the update.
    //
    public class AirwayNetwork
    {
        private TrackTaskQueue NatsQueue = new TrackTaskQueue();
        private TrackTaskQueue PacotsQueue = new TrackTaskQueue();
        private TrackTaskQueue AusotsQueue = new TrackTaskQueue();

        private NatsHandler natsManager;
        private PacotsHandler pacotsManager;
        private AusotsHandler ausotsManager;

        private TracksForm form;
        public WaypointList WptList { get; private set; }
        public AirportManager AirportList { get; private set; }
        public TrackInUseCollection TracksInUse { get; private set; }
        public StatusRecorder StatusRecorder { get; private set; }

        public event EventHandler WptListChanged;
        public event EventHandler AirportListChanged;

        // Fires when any TrackMessage in the TrackHandlers changed.
        public event EventHandler TrackMessageUpdated;

        public AirwayNetwork(TracksForm form, WaypointList wptList, AirportManager airportList)
        {
            this.form = form;
            this.WptList = wptList;
            this.AirportList = airportList;

            SetTrackData();
        }

        // It's vital that new instances are created for the related objects.
        // Otherwise, the old track managers (e.g. natsManager) may be still downloading or 
        // parsing data, and adding unwanted information to TrackInUse and StatusRecorder.
        private void SetTrackData()
        {
            natsManager?.UndoEdit();
            pacotsManager?.UndoEdit();
            ausotsManager?.UndoEdit();

            TracksInUse.Clear();
            StatusRecorder.Clear();

            natsManager = new NatsHandler(
                WptList,
                WptList.GetEditor(),
                StatusRecorder,
                AirportList,
                TracksInUse);

            pacotsManager = new PacotsHandler(
                WptList,
                WptList.GetEditor(),
                StatusRecorder,
                AirportList,
                TracksInUse);

            ausotsManager = new AusotsHandler(
                WptList,
                WptList.GetEditor(),
                StatusRecorder,
                AirportList,
                TracksInUse);
        }

        public void EnqueueNatsTask(
            Func<Task> taskGetter, CancellationTokenSource ts, Action cleanup)
        {
            NatsQueue.Add(taskGetter, ts, cleanup);
        }

        public void EnqueuePacotsTask(
            Func<Task> taskGetter, CancellationTokenSource ts, Action cleanup)
        {
            PacotsQueue.Add(taskGetter, ts, cleanup);
        }

        public void EnqueueAusotsTask(
            Func<Task> taskGetter, CancellationTokenSource ts, Action cleanup)
        {
            AusotsQueue.Add(taskGetter, ts, cleanup);
        }

        /// <summary>
        /// Use this method when wptList and airportList are entirely change (probably
        /// due to loading a different nav data). The downloaded tracks will be reparsed
        /// and added to the wptList if the specific track system was enabled.
        /// </summary>
        public void Update(WaypointList wptList, AirportManager airportList)
        {
            var natsData = natsManager.RawData;
            var pacotsData = pacotsManager.RawData;
            var ausotsData = ausotsManager.RawData;
            
            bool natsStarted = natsManager.StartedGettingTracks;
            bool pacotsStarted = pacotsManager.StartedGettingTracks;
            bool ausotsStarted = ausotsManager.StartedGettingTracks;

            this.WptList = wptList;
            this.AirportList = airportList;

            SetTrackData();

            if (natsData != null)
            {
                // These are fast operation, so they can be done on the main thread.
                natsManager.GetAllTracks(new NatsProvider(natsData));
                if (form.NatsEnabled) natsManager.AddToWaypointList();
            }
            else if (natsStarted)
            {
                // The GetAllTracks was called but the download has not finished yet, so 
                // the natsData is still null. We redownload the data.
                form.DownloadNats();
            }

            if (pacotsData != null)
            {
                pacotsManager.GetAllTracks(new PacotsProvider(pacotsData));
                if (form.PacotsEnabled) pacotsManager.AddToWaypointList();
            }
            else if (pacotsStarted)
            {
               form.DownloadPacots();
            }

            if (ausotsData != null)
            {
                ausotsManager.GetAllTracks(new AusotsProvider(ausotsData));
                if (form.AusotsEnabled) ausotsManager.AddToWaypointList();
            }
            else if (ausotsStarted)
            {
                form.DownloadAusots();
            }

            WptListChanged?.Invoke(this, EventArgs.Empty);
            AirportListChanged?.Invoke(this, EventArgs.Empty);
            InvokeTrackMessageUpdated();
        }

        private async Task GetNats(bool enable, CancellationToken token)
        {
            await DownloadNats(token);
            NatsEnabled = enable;
        }

        private async Task GetPacots(bool enable, CancellationToken token)
        {
            await DownloadPacots(token);
            PacotsEnabled = enable;
        }

        private async Task GetAusots(bool enable, CancellationToken token)
        {
            await DownloadAusots(token);
            AusotsEnabled = enable;
        }

        private bool _natsEnabled = false;
        public bool NatsEnabled
        {
            get { return _natsEnabled; }

            set
            {
                if (value)
                {
                    natsManager.AddToWaypointList();
                }
                else
                {
                    natsManager.UndoEdit();
                }

                _natsEnabled = value;
            }
        }

        private bool _pacotsEnabled = false;
        public bool PacotsEnabled
        {
            get { return _pacotsEnabled; }

            set
            {
                if (value)
                {
                    pacotsManager.AddToWaypointList();
                }
                else
                {
                    pacotsManager.UndoEdit();
                }

                _pacotsEnabled = value;
            }
        }

        private bool _ausotsEnabled = false;
        public bool AusotsEnabled
        {
            get { return _ausotsEnabled; }

            set
            {
                if (value)
                {
                    ausotsManager.AddToWaypointList();
                }
                else
                {
                    ausotsManager.UndoEdit();
                }

                _ausotsEnabled = value;
            }
        }

        public bool TrackedLoaded(TrackType type)
        {
            return new[] { NatsLoaded, PacotsLoaded, AusotsLoaded }[(int)type];
        }

        /// <summary>
        /// Returns whether the NATS has been downloaded or imported from file.
        /// </summary>
        public bool NatsLoaded => NatsMessage != null;

        public bool PacotsLoaded => PacotsMessage != null;

        public bool AusotsLoaded => AusotsMessage != null;

        public NatsMessage NatsMessage
        {
            get { return natsManager.RawData; }

            set
            {
                StatusRecorder.Clear(TrackType.Nats);
                natsManager.UndoEdit();
                natsManager.GetAllTracks(new NatsProvider(value));
                InvokeTrackMessageUpdated();
            }
        }

        public PacotsMessage PacotsMessage
        {
            get { return pacotsManager.RawData; }

            set
            {
                StatusRecorder.Clear(TrackType.Pacots);
                pacotsManager.UndoEdit();
                pacotsManager.GetAllTracks(new PacotsProvider(value));
                InvokeTrackMessageUpdated();
            }
        }

        public AusotsMessage AusotsMessage
        {
            get { return ausotsManager.RawData; }

            set
            {
                StatusRecorder.Clear(TrackType.Ausots);
                ausotsManager.UndoEdit();
                ausotsManager.GetAllTracks(new AusotsProvider(value));
                InvokeTrackMessageUpdated();
            }
        }

        public async Task DownloadNats(CancellationToken token)
        {
            StatusRecorder.Clear(TrackType.Nats);
            natsManager.UndoEdit();

            await natsManager.GetAllTracksAsync(token);
            InvokeTrackMessageUpdated();
        }

        public async Task DownloadPacots(CancellationToken token)
        {
            StatusRecorder.Clear(TrackType.Pacots);
            pacotsManager.UndoEdit();

            await pacotsManager.GetAllTracksAsync(token);
            InvokeTrackMessageUpdated();
        }

        public async Task DownloadAusots(CancellationToken token)
        {
            StatusRecorder.Clear(TrackType.Ausots);
            ausotsManager.UndoEdit();

            await ausotsManager.GetAllTracksAsync(token);
            InvokeTrackMessageUpdated();
        }

        private void InvokeTrackMessageUpdated()
        {
            TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
        }

        private class NatsProvider : INatsMessageProvider
        {
            private NatsMessage msg;

            public NatsProvider(NatsMessage msg)
            {
                this.msg = msg;
            }

            public NatsMessage GetMessage()
            {
                return msg;
            }

            public Task<NatsMessage> GetMessageAsync(CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }

        private class PacotsProvider : IPacotsMessageProvider
        {
            private PacotsMessage msg;

            public PacotsProvider(PacotsMessage msg)
            {
                this.msg = msg;
            }

            public PacotsMessage GetMessage()
            {
                return msg;
            }

            public Task<PacotsMessage> GetMessageAsync(CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }

        private class AusotsProvider : IAusotsMessageProvider
        {
            private AusotsMessage msg;

            public AusotsProvider(AusotsMessage msg)
            {
                this.msg = msg;
            }

            public AusotsMessage GetMessage()
            {
                return msg;
            }

            public Task<AusotsMessage> GetMessageAsync(CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
