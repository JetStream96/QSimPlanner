using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsHandler : TrackHandler
    {
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> nodes = new List<TrackNodes>();

        private bool _startedGettingTracks = false;
        public override bool StartedGettingTracks => _startedGettingTracks;

        public bool AddedToWptList { get; private set; } = false;
        public PacotsMessage RawData { get; private set; }

        public PacotsHandler(
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

        public override void GetAllTracks()
        {
            GetAndReadTracks(new PacotsDownloader());
            UndoEdit();
        }

        public void GetAllTracks(IPacotsMessageProvider provider)
        {
            GetAndReadTracks(provider);
            UndoEdit();
        }

        private void GetAndReadTracks(IPacotsMessageProvider provider)
        {
            try
            {
                _startedGettingTracks = true;
                TryGetTracks(provider);
                ReadMessage();
            }
            catch { }
        }

        // Can throw exception.
        private void ReadMessage()
        {
            var trks = TryParse();

            var reader = new TrackReader<PacificTrack>(wptList, airportList);
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
                        TrackType.Pacots);
                }
            }
        }

        // Can throw exception.
        private void TryGetTracks(IPacotsMessageProvider provider)
        {
            try
            {
                RawData = provider.GetMessage();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to download PACOTs.",
                    TrackType.Pacots);

                throw;
            }
        }

        // Can throw exception.
        private List<PacificTrack> TryParse()
        {
            try
            {
                return new PacotsParser(RawData, recorder, airportList)
                    .Parse();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to parse PACOTs.",
                    TrackType.Pacots);

                throw;
            }
        }

        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(() => GetAndReadTracks(new PacotsDownloader()));
            UndoEdit();
        }

        public override void AddToWaypointList()
        {
            if (AddedToWptList == false)
            {
                new TrackAdder(wptList, editor, recorder, TrackType.Pacots)
                   .AddToWaypointList(nodes);

                tracksInUse.UpdateTracks(nodes, TrackType.Pacots);
                AddedToWptList = true;
            }
        }

        public override void UndoEdit()
        {
            editor.Undo();
            AddedToWptList = false;
        }
    }
}
