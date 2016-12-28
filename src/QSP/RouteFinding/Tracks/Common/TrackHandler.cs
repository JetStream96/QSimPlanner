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
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> nodes = new List<TrackNodes>();
        private TrackType type = GetTrackType<T>();
        
        public bool InWptList { get; private set; } = false;
        public ITrackMessage Message { get; private set; }

        public TrackHandler(
            WaypointList wptList,
            WaypointListEditor editor,
            AirportManager airportList,
            TrackInUseCollection tracksInUse)
        {
            this.wptList = wptList;
            this.editor = editor;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
        }

        /// <summary>
        /// Download tracks, parse all track messages and undo previous edit to wptList.
        /// </summary>
        public void GetAllTracks(StatusRecorder r)
        {
            GetAllTracks(GetTrackDownloader<T>(), r);
        }

        /// <summary>
        /// Load the tracks and undo previous edit to wptList.
        /// </summary>
        public void GetAllTracks(ITrackMessageProvider provider, StatusRecorder r)
        {
            try
            {
                GetTracks(provider, r);
                ReadMessage(r);
            }
            catch { }

            UndoEdit();
        }

        public async Task GetAllTracksAsync(StatusRecorder r)
        {
            try
            {
                await GetTracksAsync(GetTrackDownloader<T>(), r);
                ReadMessage(r);
            }
            catch { }

            UndoEdit();
        }

        // Can throw exception.
        private void ReadMessage(StatusRecorder r)
        {
            var trks = Parse(r);

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
                    r.AddEntry(
                        StatusRecorder.Severity.Caution,
                        $"Unable to interpret track {i.Ident}.",
                        type);
                }
            }
        }

        /// <summary>
        /// Add the parsed tracks to WaypointList, if not added already.
        /// </summary>
        public void AddToWaypointList(StatusRecorder r)
        {
            if (InWptList == false)
            {
                new TrackAdder(wptList, editor, r, type).AddToWaypointList(nodes);
                tracksInUse.UpdateTracks(nodes, type);
                InWptList = true;
            }
        }

        // Can throw exception.
        private void GetTracks(ITrackMessageProvider provider, StatusRecorder r)
        {
            try
            {
                Message = provider.GetMessage();
            }
            catch
            {
                AddFailRecord(r);
                throw;
            }
        }

        // Can throw exception.
        private async Task GetTracksAsync(ITrackMessageProvider provider, StatusRecorder r)
        {
            try
            {
                Message = await provider.GetMessageAsync();
            }
            catch
            {
                AddFailRecord(r);
                throw;
            }
        }

        private void AddFailRecord(StatusRecorder r)
        {
            r.AddEntry(
                StatusRecorder.Severity.Critical,
                $"Failed to download {type.TrackString()}.",
                type);
        }

        // Can throw exception.
        private List<T> Parse(StatusRecorder r)
        {
            try
            {
                return GetParser<T>(Message, r, airportList).Parse();
            }
            catch
            {
                r.AddEntry(
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