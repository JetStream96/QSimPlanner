using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsHandler : TrackHandler
    {
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> nodes = new List<TrackNodes>();
        public bool AddedToWptList { get; private set; } = false;
        public AusotsMessage RawData { get; private set; }

        public AusotsHandler(
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
        /// Download and parse all track messages.
        /// </summary>
        public override void GetAllTracks()
        {
            GetAndReadTracks(new AusotsDownloader());
            UndoEdit();
        }

        public void GetAllTracks(IAusotsMessageProvider provider)
        {
            GetAndReadTracks(provider);
            UndoEdit();
        }

        private void GetAndReadTracks(IAusotsMessageProvider provider)
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

            var reader = new TrackReader<AusTrack>(wptList, airportList);
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
                        TrackType.Ausots);
                }
            }
        }

        // Can throw exception.
        private List<AusTrack> TryParse()
        {
            try
            {
                return new AusotsParser(RawData, recorder, airportList)
                    .Parse();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to parse AUSOTs.",
                    TrackType.Ausots);

                throw;
            }
        }

        // Can throw exception.
        private void TryGetTracks(IAusotsMessageProvider provider)
        {
            try
            {
                RawData = provider.GetMessage();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to download AUSOTs.",
                    TrackType.Ausots);

                throw;
            }
        }

        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(() =>
            GetAndReadTracks(new AusotsDownloader()));
            UndoEdit();
        }

        public override void AddToWaypointList()
        {
            if (AddedToWptList == false)
            {
                new TrackAdder(wptList, editor, recorder, TrackType.Ausots)
                    .AddToWaypointList(nodes);

                tracksInUse.UpdateTracks(nodes, TrackType.Ausots);
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
