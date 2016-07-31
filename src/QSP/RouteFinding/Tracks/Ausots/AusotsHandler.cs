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
        private List<TrackNodes> nodes;
        public bool AddedToWptList { get; private set; }
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
            AddedToWptList = false;
        }

        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="TrackDownloadException"></exception>
        public override void GetAllTracks()
        {
            TryDownload(new AusotsDownloader());
            ReadMessage();
        }

        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="TrackDownloadException"></exception>
        public void GetAllTracks(IAusotsMessageProvider provider)
        {
            TryDownload(provider);
            ReadMessage();
        }

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

        /// <exception cref="TrackDownloadException"></exception>
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

                throw new TrackDownloadException(
                    "Failed to download Ausots.", ex);
            }
        }

        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="TrackDownloadException"></exception>
        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(GetAllTracks);
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
        }
    }
}
