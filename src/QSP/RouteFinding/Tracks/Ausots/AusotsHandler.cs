using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsHandler : TrackHandler
    {

        #region Fields

        private IAusotsDownloader downloader;
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private RouteTrackCommunicator communicator;

        private AusotsMessage rawData;
        private List<TrackNodes> nodes;

        #endregion

        public AusotsHandler(IAusotsDownloader downloader,
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

        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        /// <exception cref="TrackParseException"></exception>
        /// <exception cref="TrackDownloadException"></exception>
        public override void GetAllTracks()
        {
            TryDownload();
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
                    recorder.AddEntry(StatusRecorder.Severity.Caution,
                                      string.Format("Unable to interpret track {0}.", i.Ident),
                                      TrackType.Ausots);
                }
            }
        }

        /// <exception cref="TrackParseException"></exception>
        private List<AusTrack> TryParse()
        {
            try
            {
                return new AusotsParser(rawData, recorder, airportList).Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical,
                                  "Failed to parse AUSOTs.",
                                  TrackType.Ausots);
                throw new TrackParseException("Failed to parse Ausots.", ex);
            }
        }

        /// <exception cref="TrackDownloadException"></exception>
        private void TryDownload()
        {
            try
            {
                rawData = downloader.Download();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical,
                                  "Failed to download AUSOTs.",
                                  TrackType.Ausots);
                throw new TrackDownloadException("Failed to download Ausots.", ex);
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
            new TrackAdder(wptList, editor, recorder, TrackType.Ausots).AddToWaypointList(nodes);

            foreach (var i in nodes)
            {
                communicator.StageTrackData(i);
            }
            communicator.PushAllData(TrackType.Ausots);
        }

        public void UndoEdit()
        {
            editor.Undo();
        }
    }
}
