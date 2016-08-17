using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
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
        private List<TrackNodes> nodes;
        public bool AddedToWptList { get; private set; }
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
            AddedToWptList = false;
        }

        /// <exception cref="GetTrackException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public override void GetAllTracks()
        {
            DownloadAndReadTracks(new PacotsDownloader());
            UndoEdit();
        }

        /// <exception cref="GetTrackException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public void GetAllTracks(IPacotsMessageProvider provider)
        {
            DownloadAndReadTracks(provider);
            UndoEdit();
        }

        /// <exception cref="GetTrackException"></exception>
        /// <exception cref="TrackParseException"></exception>
        private void DownloadAndReadTracks(IPacotsMessageProvider provider)
        {
            TryDownload(provider);
            ReadMessage();
        }

        /// <exception cref="TrackParseException"></exception>
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

        /// <exception cref="GetTrackException"></exception>
        /// <exception cref="TrackParseException"></exception>
        private void TryDownload(IPacotsMessageProvider provider)
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

        /// <exception cref="TrackParseException"></exception>
        private List<PacificTrack> TryParse()
        {
            try
            {
                return new PacotsParser(RawData, recorder, airportList)
                    .Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to parse PACOTs.",
                    TrackType.Pacots);

                throw new TrackParseException("Failed to parse Pacots.", ex);
            }
        }

        /// <exception cref="GetTrackException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(() =>
            DownloadAndReadTracks(new PacotsDownloader()));
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
