using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsHandler : TrackHandler
    {
        #region Fields

        private IPacotsDownloader downloader;
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private RouteTrackCommunicator communicator;

        private PacotsMessage rawData;
        private List<TrackNodes> nodes;

        #endregion

        public PacotsHandler(IPacotsDownloader downloader, 
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

        // TODO: Maybe have different exception messages to distinguish west/east parse error?
        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public override void GetAllTracks()
        {
            tryDownload();
            var trks = tryParse();

            var reader = new TrackReader<PacificTrack>(wptList);
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
                                      "Unable to interpret one track.",
                                      TrackType.Pacots);
                }
            }
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
                                  "Failed to download PACOTs.", 
                                  TrackType.Pacots);
                throw;
            }
        }

        /// <exception cref="TrackParseException"></exception>
        private List<PacificTrack> tryParse()
        {
            try
            {
                return new PacotsParser(rawData, recorder, airportList).Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical, 
                                  "Failed to parse PACOTs.", 
                                  TrackType.Pacots);

                throw new TrackParseException("Failed to parse Pacots.", ex);
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
            new TrackAdder(wptList, editor, recorder, TrackType.Pacots)
                    .AddToWaypointList(nodes);

            foreach (var i in nodes)
            {
                communicator.StageTrackData(i);
            }
            communicator.PushAllData(TrackType.Pacots);
        }

        public void UndoEdit()
        {
            editor.Undo();
        }
    }
}
