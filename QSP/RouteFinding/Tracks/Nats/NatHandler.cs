using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsHandler : TrackHandler
    {
        #region Fields

        private INatsDownloader downloader;
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private RouteTrackCommunicator communicator;

        private NatsMessage rawData;
        private List<TrackNodes> nodes;

        #endregion

        public NatsHandler(INatsDownloader downloader,
                           WaypointList wptList,
                           WaypointListEditor editor,
                           StatusRecorder recorder,
                           AirportManager airportList,
                           RouteTrackCommunicator communicator)
        {
            this.downloader = downloader;
            this.wptList = wptList;
            this.editor = editor;
            this.recorder = recorder;
            this.airportList = airportList;
            this.communicator = communicator;
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public override void GetAllTracks()
        {
            tryDownload();
            var trks = tryParse();

            var reader = new TrackReader<NorthAtlanticTrack>(wptList, airportList);
            nodes = new List<TrackNodes>();

            foreach (var i in trks)
            {
                try
                {
                    nodes.Add(reader.Read(i));
                }
                catch
                {
                    recorder.AddEntry(StatusRecorder.Severity.Caution,
                                      string.Format("Unable to interpret track {0}.", i.Ident),
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
            new TrackAdder(wptList, editor, recorder, TrackType.Nats).AddToWaypointList(nodes);

            foreach (var i in nodes)
            {
                communicator.StageTrackData(i);
            }
            communicator.PushAllData(TrackType.Nats);
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        private void tryDownload()
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
        private List<NorthAtlanticTrack> tryParse()
        {
            try
            {
                return new NatsParser(rawData, recorder, airportList).Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical,
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
