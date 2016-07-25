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
        
        public event EventHandler AirportListChanged;

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

            bool natsEnabled = natsManager.AddedToWptList;
            bool pacotsEnabled = pacotsManager.AddedToWptList;
            bool ausotsEnabled = ausotsManager.AddedToWptList;

            this.WptList = wptList;
            this.AirportList = airportList;

            TracksInUse.Clear();
            StatusRecorder.Clear();
            SetTrackData();
            
            if (natsEnabled)
            {
                natsManager.GetAllTracks(new NatsProvider(natsData));
                natsManager.AddToWaypointList();
            }

            if (pacotsEnabled)
            {
                pacotsManager.GetAllTracks(new PacotsProvider(pacotsData));
                pacotsManager.AddToWaypointList();
            }

            if (ausotsEnabled)
            {
                ausotsManager.GetAllTracks(new AusotsProvider(ausotsData));
                ausotsManager.AddToWaypointList();
            }

            AirportListChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <exception cref="ArgumentException"></exception>
        public async Task DownloadTrack(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    await SetNats();
                    break;

                case TrackType.Pacots:
                    await SetPacots();
                    break;

                case TrackType.Ausots:
                    await SetAusots();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        /// <exception cref="ArgumentException"></exception>
        public void EnableTrack(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    natsManager.AddToWaypointList();
                    break;

                case TrackType.Pacots:
                    pacotsManager.AddToWaypointList();
                    break;

                case TrackType.Ausots:
                    ausotsManager.AddToWaypointList();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        /// <exception cref="ArgumentException"></exception>
        public void DisableTrack(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    natsManager.UndoEdit();
                    break;

                case TrackType.Pacots:
                    pacotsManager.UndoEdit();
                    break;

                case TrackType.Ausots:
                    ausotsManager.UndoEdit();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        private async Task SetNats()
        {
            StatusRecorder.Clear(TrackType.Nats);
            natsManager.UndoEdit();

            try
            {
                await natsManager.GetAllTracksAsync();
                natsManager.AddToWaypointList();
            }
            catch { }
        }

        private async Task SetPacots()
        {
            StatusRecorder.Clear(TrackType.Pacots);
            pacotsManager.UndoEdit();

            try
            {
                await pacotsManager.GetAllTracksAsync();
                pacotsManager.AddToWaypointList();
            }
            catch { }
        }

        private async Task SetAusots()
        {
            StatusRecorder.Clear(TrackType.Ausots);
            ausotsManager.UndoEdit();

            try
            {
                await ausotsManager.GetAllTracksAsync();
                ausotsManager.AddToWaypointList();
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
