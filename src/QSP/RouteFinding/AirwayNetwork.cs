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

        public AirwayNetwork(
            WaypointList wptList, AirportManager airportList)
        {
            this.WptList = wptList;
            this.AirportList = airportList;
            TracksInUse = new TrackInUseCollection();
            StatusRecorder = new StatusRecorder();
            SetTrackData();
        }

        private void SetTrackData()
        {
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

        public void Update(WaypointList wptList, AirportManager airportList)
        {
            var natsData = natsManager.RawData;
            var pacotsData = pacotsManager.RawData;
            var ausotsData = ausotsManager.RawData;

            this.WptList = wptList;
            this.AirportList = airportList;

            TracksInUse.Clear();
            StatusRecorder.Clear();
            SetTrackData();

            if (NatsLoaded)
            {
                natsManager.GetAllTracks(new NatsProvider(natsData));
            }

            if (PacotsLoaded)
            {
                pacotsManager.GetAllTracks(new PacotsProvider(pacotsData));
            }

            if (AusotsLoaded)
            {
                ausotsManager.GetAllTracks(new AusotsProvider(ausotsData));
            }

            if (NatsEnabled) natsManager.AddToWaypointList();
            if (PacotsEnabled) pacotsManager.AddToWaypointList();
            if (AusotsEnabled) ausotsManager.AddToWaypointList();

            WptListChanged?.Invoke(this, EventArgs.Empty);
            AirportListChanged?.Invoke(this, EventArgs.Empty);
            TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
        }

        public bool NatsEnabled
        {
            get { return natsManager.AddedToWptList; }

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
            }
        }

        public bool PacotsEnabled
        {
            get { return pacotsManager.AddedToWptList; }

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
            }
        }

        public bool AusotsEnabled
        {
            get { return ausotsManager.AddedToWptList; }

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
        public bool NatsLoaded
        {
            get { return NatsMessage != null; }
        }

        public bool PacotsLoaded
        {
            get { return PacotsMessage != null; }
        }

        public bool AusotsLoaded
        {
            get { return AusotsMessage != null; }
        }

        public NatsMessage NatsMessage
        {
            get { return natsManager.RawData; }

            set
            {
                StatusRecorder.Clear(TrackType.Nats);
                natsManager.UndoEdit();

                try
                {
                    natsManager.GetAllTracks(new NatsProvider(value));
                    TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        public PacotsMessage PacotsMessage
        {
            get { return pacotsManager.RawData; }

            set
            {
                StatusRecorder.Clear(TrackType.Pacots);
                pacotsManager.UndoEdit();

                try
                {
                    pacotsManager.GetAllTracks(new PacotsProvider(value));
                    TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        public AusotsMessage AusotsMessage
        {
            get { return ausotsManager.RawData; }

            set
            {
                StatusRecorder.Clear(TrackType.Ausots);
                ausotsManager.UndoEdit();

                try
                {
                    ausotsManager.GetAllTracks(new AusotsProvider(value));
                    TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        public async Task DownloadNats()
        {
            StatusRecorder.Clear(TrackType.Nats);
            natsManager.UndoEdit();

            try
            {
                await natsManager.GetAllTracksAsync();
                TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        public async Task DownloadPacots()
        {
            StatusRecorder.Clear(TrackType.Pacots);
            pacotsManager.UndoEdit();

            try
            {
                await pacotsManager.GetAllTracksAsync();
                TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        public async Task DownloadAusots()
        {
            StatusRecorder.Clear(TrackType.Ausots);
            ausotsManager.UndoEdit();

            try
            {
                await ausotsManager.GetAllTracksAsync();
                TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch { }
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
