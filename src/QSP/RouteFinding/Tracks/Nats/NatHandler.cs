using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsHandler : TrackHandler
    {
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> nodes = new List<TrackNodes>();
        public bool AddedToWptList { get; private set; } = false;
        public NatsMessage RawData { get; private set; }

        public NatsHandler(
            WaypointList wptList,
            WaypointListEditor editor,
            StatusRecorder recorder,
            AirportManager airportList,
            TrackInUseCollection tracksInUse)
        {
            this.wptList = wptList;
            this.editor = editor;
            this.recorder = recorder;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
        }
        
        /// <summary>
        /// Download tracks and undo previous edit to wptList.
        /// </summary>
        public override void GetAllTracks()
        {
            GetAndReadTracks(new NatsDownloader());
            UndoEdit();
        }

        /// <summary>
        /// Load the tracks and undo previous edit to wptList.
        /// </summary>
        public void GetAllTracks(INatsMessageProvider provider)
        {
            GetAndReadTracks(provider);
            UndoEdit();
        }
        
        private void GetAndReadTracks(INatsMessageProvider provider)
        {
            try
            {
                TryGetTracks(provider);
                ReadMessage();
            }
            catch { }
        }

        // Can throw exception.
        private void ReadMessage()
        {
            var trks = TryParse();

            var reader = new TrackReader<NorthAtlanticTrack>(
                wptList, airportList);
            nodes = new List<TrackNodes>();

            foreach (var i in trks)
            {
                try
                {
                    nodes.Add(reader.Read(i));
                }
                catch
                {
                    recorder.AddEntry(
                        StatusRecorder.Severity.Caution,
                        $"Unable to interpret track {i.Ident}.",
                        TrackType.Nats);
                }
            }
        }
        
        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(() => 
            GetAndReadTracks(new NatsDownloader()));
            UndoEdit();
        }
        
        public override void AddToWaypointList()
        {
            if (AddedToWptList == false)
            {
                new TrackAdder(wptList, editor, recorder, TrackType.Nats)
                    .AddToWaypointList(nodes);

                tracksInUse.UpdateTracks(nodes, TrackType.Nats);
                AddedToWptList = true;
            }
        }

        // Can throw exception.
        private void TryGetTracks(INatsMessageProvider provider)
        {
            try
            {
                RawData = provider.GetMessage();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to download NATs.",
                    TrackType.Nats);

                throw;
            }
        }

        // Can throw exception.
        private List<NorthAtlanticTrack> TryParse()
        {
            try
            {
                return new NatsParser(RawData, recorder, airportList).Parse();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to parse NATs.",
                    TrackType.Nats);

                throw;
            }
        }

        public override void UndoEdit()
        {
            editor.Undo();
            AddedToWptList = false;
        }
    }
}
