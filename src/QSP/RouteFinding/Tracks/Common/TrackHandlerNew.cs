using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static QSP.RouteFinding.Tracks.Common.Helpers;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackHandlerNew<T> where T : Track
    {
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> nodes = new List<TrackNodes>();
        private TrackType type = GetTrackType<T>();

        private bool _startedGettingTracks = false;
        public bool StartedGettingTracks => _startedGettingTracks;

        public bool AddedToWptList { get; private set; } = false;
        public ITrackMessageNew RawData { get; private set; }

        public TrackHandlerNew(
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
        public void GetAllTracks()
        {
            GetAllTracks(GetTrackDownloader<T>());
        }

        /// <summary>
        /// Load the tracks and undo previous edit to wptList.
        /// </summary>
        public void GetAllTracks(ITrackMessageProvider provider)
        {
            try
            {
                _startedGettingTracks = true;
                GetTracks(provider);
                ReadMessage();
            }
            catch { }

            UndoEdit();
        }

        public async Task GetAllTracksAsync(CancellationToken token)
        {
            try
            {
                _startedGettingTracks = true;
                await GetTracksAsync(GetTrackDownloader<T>(), token);
                ReadMessage();
            }
            catch { }

            UndoEdit();
        }

        // Can throw exception.
        private void ReadMessage()
        {
            var trks = Parse();

            var reader = new TrackReader<T>(wptList, airportList);
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
                        type);
                }
            }
        }

        public void AddToWaypointList()
        {
            if (AddedToWptList == false)
            {
                new TrackAdder(wptList, editor, recorder, type)
                    .AddToWaypointList(nodes);

                tracksInUse.UpdateTracks(nodes, type);
                AddedToWptList = true;
            }
        }

        // Can throw exception.
        private void GetTracks(ITrackMessageProvider provider)
        {
            try
            {
                RawData = provider.GetMessage();
            }
            catch
            {
                AddRecord();
                throw;
            }
        }

        // Can throw exception.
        private async Task GetTracksAsync(ITrackMessageProvider provider, CancellationToken token)
        {
            try
            {
                RawData = await provider.GetMessageAsync(token);
            }
            catch
            {
                AddRecord();
                throw;
            }
        }

        private void AddRecord()
        {
            recorder.AddEntry(
                   StatusRecorder.Severity.Critical,
                   $"Failed to download {type.TrackString()}.",
                   type);
        }

        // Can throw exception.
        private List<T> Parse()
        {
            try
            {
                return GetParser<T>(RawData, recorder, airportList).Parse();
            }
            catch
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    $"Failed to parse {type.TrackString()}.",
                    type);

                throw;
            }
        }

        public void UndoEdit()
        {
            editor.Undo();
            AddedToWptList = false;
        }
    }
}