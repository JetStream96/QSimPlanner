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
        public event EventHandler GetTracksFinished;

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

            if (natsEnabled) natsManager.AddToWaypointList();
            if (pacotsEnabled) pacotsManager.AddToWaypointList();
            if (ausotsEnabled) ausotsManager.AddToWaypointList();

            AirportListChanged?.Invoke(this, EventArgs.Empty);
            GetTracksFinished?.Invoke(this, EventArgs.Empty);
        }

        public void EnableNats() { natsManager.AddToWaypointList(); }
        public void EnablePacots() { pacotsManager.AddToWaypointList(); }
        public void EnableAusots() { ausotsManager.AddToWaypointList(); }

        public void DisableNats() { natsManager.UndoEdit(); }
        public void DisablePacots() { pacotsManager.UndoEdit(); }
        public void DisableAusots() { ausotsManager.UndoEdit(); }

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

        public bool NatsLoaded
        {
            get { return GetNatsMessage() != null; }
        }

        public bool PacotsLoaded
        {
            get { return GetPacotsMessage() != null; }
        }

        public bool AusotsLoaded
        {
            get { return GetAusotsMessage() != null; }
        }

        public NatsMessage GetNatsMessage()
        {
            return natsManager.RawData;
        }

        public PacotsMessage GetPacotsMessage()
        {
            return pacotsManager.RawData;
        }

        public AusotsMessage GetAusotsMessage()
        {
            return ausotsManager.RawData;
        }

        public async Task DownloadNats()
        {
            StatusRecorder.Clear(TrackType.Nats);
            natsManager.UndoEdit();

            try
            {
                await natsManager.GetAllTracksAsync();
                GetTracksFinished?.Invoke(this, EventArgs.Empty);
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
                GetTracksFinished?.Invoke(this, EventArgs.Empty);
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
                GetTracksFinished?.Invoke(this, EventArgs.Empty);
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
