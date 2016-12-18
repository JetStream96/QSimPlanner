using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using System;
using System.Threading.Tasks;

namespace QSP.RouteFinding
{
    // Handles updating navigation data and reparse tracks after 
    // the update.
    //
    public class AirwayNetwork
    {
        private NatsHandler natsManager;
        private PacotsHandler pacotsManager;
        private AusotsHandler ausotsManager;

        public WaypointList WptList { get; private set; }
        public AirportManager AirportList { get; private set; }
        public TrackInUseCollection TracksInUse { get; private set; }
        public StatusRecorder StatusRecorder { get; private set; }

        public event EventHandler WptListChanged;
        public event EventHandler AirportListChanged;

        // Fires when any of the TrackMessage in the TrackHandlers changed.
        public event EventHandler TrackMessageUpdated;

        public event EventHandler TrackAddedToWptListChanged;

        public AirwayNetwork(WaypointList wptList, AirportManager airportList)
        {
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

            TracksInUse = new TrackInUseCollection();
            StatusRecorder = new StatusRecorder();

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

        /// <summary>
        /// Use this method when wptList and airportList are entirely change (probably
        /// due to loading a different nav data). The downloaded tracks will be reparsed
        /// and added to the wptList if the specific track system was enabled.
        /// </summary>
        public void Update(WaypointList wptList, AirportManager airportList)
        {// TODO: refactor method
            var natsData = natsManager.RawData;
            var pacotsData = pacotsManager.RawData;
            var ausotsData = ausotsManager.RawData;
            
            bool natsEnabled = NatsEnabled;
            bool pacotsEnabled = PacotsEnabled;
            bool ausotsEnabled = AusotsEnabled;

            bool natsStarted = natsManager.StartedGettingTracks;
            bool pacotsStarted = pacotsManager.StartedGettingTracks;
            bool ausotsStarted = ausotsManager.StartedGettingTracks;

            this.WptList = wptList;
            this.AirportList = airportList;

            SetTrackData();

            if (natsData != null)
            {
                natsManager.GetAllTracks(new NatsProvider(natsData));
            }
            else if (natsStarted)
            {
                Task.Run(() => natsManager.GetAllTracksAsync());
            }

            if (pacotsData != null)
            {
                pacotsManager.GetAllTracks(new PacotsProvider(pacotsData));
            }
            else if (pacotsStarted)
            {
                Task.Run(() => pacotsManager.GetAllTracksAsync());
            }

            if (ausotsData != null)
            {
                ausotsManager.GetAllTracks(new AusotsProvider(ausotsData));
            }
            else if (ausotsStarted)
            {
                Task.Run(() => ausotsManager.GetAllTracksAsync());
            }

            if (natsEnabled) natsManager.AddToWaypointList();
            if (pacotsEnabled) pacotsManager.AddToWaypointList();
            if (ausotsEnabled) ausotsManager.AddToWaypointList();

            WptListChanged?.Invoke(this, EventArgs.Empty);
            AirportListChanged?.Invoke(this, EventArgs.Empty);
            TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
        }

        private bool _natsEnabled =false;
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
                TrackAddedToWptListChanged.Invoke(this, EventArgs.Empty);
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
                TrackAddedToWptListChanged.Invoke(this, EventArgs.Empty);
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
                TrackAddedToWptListChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public bool TrackedLoaded(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    return NatsLoaded;

                case TrackType.Pacots:
                    return PacotsLoaded;

                case TrackType.Ausots:
                    return AusotsLoaded;

                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Returns whether the NATS has been downloaded or 
        /// imported from file.
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
                TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
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
                TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
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
                TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task DownloadNats()
        {
            StatusRecorder.Clear(TrackType.Nats);
            natsManager.UndoEdit();

            await natsManager.GetAllTracksAsync();
            TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
        }

        public async Task DownloadPacots()
        {
            StatusRecorder.Clear(TrackType.Pacots);
            pacotsManager.UndoEdit();

            await pacotsManager.GetAllTracksAsync();
            TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
        }

        public async Task DownloadAusots()
        {
            StatusRecorder.Clear(TrackType.Ausots);
            ausotsManager.UndoEdit();

            await ausotsManager.GetAllTracksAsync();
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
        }
    }
}
