using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using System;

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

        public AirwayNetwork(
            WaypointList wptList, AirportManager airportList)
        {
            this.WptList = wptList;
            this.AirportList = airportList;
            SetTrackData();
        }

        private void SetTrackData()
        {
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
            SetTrackData();

            natsManager.GetAllTracks(new NatsProvider(natsData));
            pacotsManager.GetAllTracks(new PacotsProvider(pacotsData));
            ausotsManager.GetAllTracks(new AusotsProvider(ausotsData));

            if (natsEnabled)
            {
                natsManager.AddToWaypointList();
            }

            if (pacotsEnabled)
            {
                pacotsManager.AddToWaypointList();
            }

            if (natsEnabled)
            {
                natsManager.AddToWaypointList();
            }
        }

        /// <exception cref="ArgumentException"></exception>
        public void DownloadTrack(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    SetNats();
                    break;

                case TrackType.Pacots:
                    SetPacots();
                    break;

                case TrackType.Ausots:
                    SetAusots();
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

        private async void SetNats()
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

        private async void SetPacots()
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

        private async void SetAusots()
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
