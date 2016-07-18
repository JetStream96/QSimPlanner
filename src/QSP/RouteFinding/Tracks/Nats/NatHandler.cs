using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsHandler : TrackHandler
    {
        private INatsDownloader downloader;
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;

        private NatsMessage rawData;
        private List<TrackNodes> nodes;

        public NatsHandler(
            INatsDownloader downloader,
            WaypointList wptList,
            WaypointListEditor editor,
            StatusRecorder recorder,
            AirportManager airportList,
            TrackInUseCollection tracksInUse)
        {
            this.downloader = downloader;
            this.wptList = wptList;
            this.editor = editor;
            this.recorder = recorder;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public override void GetAllTracks()
        {
            TryDownload();
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

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public override async Task GetAllTracksAsync()
        {
            await Task.Factory.StartNew(GetAllTracks);
        }

        public override void AddToWaypointList()
        {
            new TrackAdder(wptList, editor, recorder, TrackType.Nats)
                .AddToWaypointList(nodes);

            tracksInUse.UpdateTracks(nodes, TrackType.Nats);
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        private void TryDownload()
        {
            try
            {
                rawData = downloader.Download();
            }
            catch
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical,
                                  "Failed to download NATs.",
                                  TrackType.Nats);
                throw;
            }
        }

        /// <exception cref="TrackParseException"></exception>
        private List<NorthAtlanticTrack> TryParse()
        {
            try
            {
                return new NatsParser(rawData, recorder, airportList).Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(
                    StatusRecorder.Severity.Critical,
                    "Failed to parse NATs.",
                    TrackType.Nats);

                throw new TrackParseException("Failed to parse Nats.", ex);
            }
        }

        public void UndoEdit()
        {
            editor.Undo();
        }
    }
}
