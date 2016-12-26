using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using System.Threading.Tasks;
using static QSP.RouteFinding.Tracks.Common.Helpers;

namespace QSP.RouteFinding.Tracks.Common
{
    /// <summary>
    /// Provides some easy-to-use methods to manage tracks.
    /// </summary>
    public class TrackHandler<T> : ITrackHandler where T : Track
    {
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> nodes = new List<TrackNodes>();
        private TrackType type = GetTrackType<T>();
        
        /// <summary>
        /// Indicates whether GetAllTracks or GetAllTracksAsync has been called.
        /// </summary>
        public bool StartedGettingTracks { get; private set; } = false;
        
        public bool InWptList { get; private set; } = false;
        public ITrackMessage RawData { get; private set; }

        public TrackHandler(
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
        /// Download tracks, parse all track messages and undo previous edit to wptList.
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
                StartedGettingTracks = true;
                GetTracks(provider);
                ReadMessage();
            }
            catch { }

            UndoEdit();
        }

        public async Task GetAllTracksAsync()
        {
            try
            {
                StartedGettingTracks = true;
                await GetTracksAsync(GetTrackDownloader<T>());
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

        /// <summary>
        /// Add the parsed tracks to WaypointList, if not added already.
        /// </summary>
        public void AddToWaypointList()
        {
            if (InWptList == false)
            {
                new TrackAdder(wptList, editor, recorder, type)
                    .AddToWaypointList(nodes);

                tracksInUse.UpdateTracks(nodes, type);
                InWptList = true;
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
        private async Task GetTracksAsync(ITrackMessageProvider provider)
        {
            try
            {
                RawData = await provider.GetMessageAsync();
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

        /// <summary>
        /// Undo the actions of AddToWaypointList().
        /// </summary>
        public void UndoEdit()
        {
            editor.Undo();
            InWptList = false;
        }
    }
}