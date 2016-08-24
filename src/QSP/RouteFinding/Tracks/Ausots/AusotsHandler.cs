using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
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
        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="GetTrackException"></exception>
        public override void GetAllTracks()
        {
            DownloadAndReadTracks(new AusotsDownloader());
            UndoEdit();
        }

        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="GetTrackException"></exception>
        public void GetAllTracks(IAusotsMessageProvider provider)
        {
            DownloadAndReadTracks(provider);
            UndoEdit();
        }

        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="GetTrackException"></exception>
        private void DownloadAndReadTracks(IAusotsMessageProvider provider)
        {
            TryDownload(provider);
            ReadMessage();
        }

        /// <exception cref="TrackParseException"></exception>
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

        /// <exception cref="TrackParseException"></exception>
        private List<AusTrack> TryParse()
        {
            try
            {
                return new AusotsParser(RawData, recorder, airportList)
                    .Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to parse AUSOTs.",
                    TrackType.Ausots);

                throw new TrackParseException("Failed to parse Ausots.", ex);
            }
        }

        /// <exception cref="GetTrackException"></exception>
        private void TryDownload(IAusotsMessageProvider provider)
        {
            try
            {
                RawData = provider.GetMessage();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to download AUSOTs.",
                    TrackType.Ausots);

                throw new GetTrackException(
                    "Failed to download Ausots.", ex);
            }
        }

        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="GetTrackException"></exception>
        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(() =>
            DownloadAndReadTracks(new AusotsDownloader()));
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
