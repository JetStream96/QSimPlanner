using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QSP.LibraryExtension;
using QSP.LibraryExtension.Tasks;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.UI.Forms;

namespace QSP.RouteFinding.Tracks
{
    // Handles updating navigation data and reparse tracks after 
    // the update.
    //
    public class AirwayNetwork
    {
        private static readonly int TrackSysCount = Helpers.TrackTypes.Count;

        private TaskQueue[] queues = new TaskQueue[TrackSysCount];
        private bool[] trackEnabled = new bool[TrackSysCount];

        private TrackHandler<NorthAtlanticTrack> natsHandler;
        private TrackHandler<PacificTrack> pacotsHandler;
        private TrackHandler<AusTrack> ausotsHandler;

        private IReadOnlyList<ITrackHandler> Handlers => new ITrackHandler[]
        {
            natsHandler, pacotsHandler, ausotsHandler
        };

        // Remember to set this.
        public TracksForm TrackForm { get; set; }

        public WaypointList WptList { get; private set; }
        public AirportManager AirportList { get; private set; }
        public TrackInUseCollection TracksInUse { get; private set; } = new TrackInUseCollection();
        public StatusRecorder StatusRecorder { get; private set; } = new StatusRecorder();

        public event EventHandler WptListChanged;
        public event EventHandler AirportListChanged;

        // Fires when any TrackMessage in the TrackHandlers changed.
        public event EventHandler TrackMessageUpdated;

        // Fires if the status is changed. However, this event firing does not imply
        // that the status has changed.
        public event EventHandler StatusChanged;

        public AirwayNetwork(WaypointList wptList, AirportManager airportList)
        {
            this.WptList = wptList;
            this.AirportList = airportList;

            for (int i = 0; i < queues.Length; i++)
            {
                queues[i] = new TaskQueue();
                trackEnabled[i] = false;
            }

            SetTrackData();
        }

        private void SetTrackData()
        {
            Helpers.TrackTypes.ForEach(t =>
            {
                var h = Handlers[(int)t];
                EnqueueSyncTask(t, () => h?.UndoEdit());
            });

            TracksInUse.Clear();
            StatusRecorder.Clear();

            natsHandler = new TrackHandler<NorthAtlanticTrack>(
                WptList,
                WptList.GetEditor(),
                AirportList,
                TracksInUse);

            pacotsHandler = new TrackHandler<PacificTrack>(
                WptList,
                WptList.GetEditor(),
                AirportList,
                TracksInUse);

            ausotsHandler = new TrackHandler<AusTrack>(
                WptList,
                WptList.GetEditor(),
                AirportList,
                TracksInUse);
        }

        public void EnqueueSyncTask(TrackType type, Action action)
        {
            Func<Task> task = () =>
            {
                action();
                return Task.FromResult(0);
            };

            EnqueueTask(type, task);
        }

        public void EnqueueTask(TrackType type, Func<Task> taskGetter)
        {
            queues[(int)type].Add(taskGetter);
        }

        /// <summary>
        /// Use this method when wptList and airportList are entirely change (probably
        /// due to loading a different nav data). The downloaded tracks will be reparsed
        /// and added to the wptList if the specific track system was enabled.
        /// </summary>
        public void Update(WaypointList wptList, AirportManager airportList)
        {
            var messages = Handlers.Select(h => h.RawData).ToList();
            var started = Handlers.Select(h => h.StartedGettingTracks).ToList();

            this.WptList = wptList;
            this.AirportList = airportList;

            SetTrackData();

            var newHandlers = Handlers;

            for (int i = 0; i < messages.Count; i++)
            {
                var h = newHandlers[i];
                var type = (TrackType)i;
                var msg = messages[i];

                if (msg != null)
                {
                    EnqueueSyncTask(type, () =>
                    {
                        h.GetAllTracks(new TrackProvider(msg), StatusRecorder);
                        if (TrackForm.TrackEnabled(type)) h.AddToWaypointList(StatusRecorder);
                        InvokeStatusChanged();
                        InvokeTrackMessageUpdated();
                    });
                }
                else if (started[i])
                {
                    // The GetAllTracks was called but the download has not finished yet, so 
                    // the messages is still null. We redownload the data.
                    TrackForm.DownloadAndEnableTracks(type);
                }
            }

            WptListChanged?.Invoke(this, EventArgs.Empty);
            AirportListChanged?.Invoke(this, EventArgs.Empty);
            InvokeTrackMessageUpdated();
        }

        public bool GetTrackEnabled(TrackType t) => trackEnabled[(int)t];

        // Note that this method does not update the StatusRecoder.
        // To work around this, make sure to call AddToWptList immediately after
        // any calls to GetAllTracks or GetAllTracksAsync.
        //
        public void SetTrackEnabled(TrackType t, bool enabled)
        {
            var h = Handlers[(int)t];

            if (enabled)
            {
                h.AddToWaypointList(new StatusRecorder());
            }
            else
            {
                h.UndoEdit();
            }

            trackEnabled[(int)t] = enabled;
        }

        /// <summary>
        /// Returns whether the NATs has been downloaded or imported from file.
        /// </summary>
        public bool TracksLoaded(TrackType type)
        {
            return GetTrackMessage(type) != null;
        }

        public ITrackMessage GetTrackMessage(TrackType type) => Handlers[(int)type].RawData;

        /// <summary>
        /// Sets the TrackMessage and enable them.
        /// </summary>
        public void SetTrackMessageAndEnable(TrackType type, ITrackMessage message)
        {
            var h = Handlers[(int)type];
            StatusRecorder.Clear(type);
            h.UndoEdit();
            h.GetAllTracks(new TrackProvider(message), StatusRecorder);
            h.AddToWaypointList(StatusRecorder);
            InvokeTrackMessageUpdated();
            InvokeStatusChanged();
        }

        public async Task DownloadAndEnableTracks(TrackType type)
        {
            var h = Handlers[(int)type];
            StatusRecorder.Clear(type);
            h.UndoEdit();

            await h.GetAllTracksAsync(StatusRecorder);
            h.AddToWaypointList(StatusRecorder);
            InvokeStatusChanged();
            InvokeTrackMessageUpdated();
        }

        private void InvokeTrackMessageUpdated()
        {
            TrackMessageUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void InvokeStatusChanged()
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool InWptList(TrackType t) => Handlers[(int)t].InWptList;

        // TODO: Refactor this.
        private class TrackProvider : ITrackMessageProvider
        {
            private ITrackMessage msg;

            public TrackProvider(ITrackMessage msg)
            {
                this.msg = msg;
            }

            public ITrackMessage GetMessage() => msg;

            public Task<ITrackMessage> GetMessageAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
