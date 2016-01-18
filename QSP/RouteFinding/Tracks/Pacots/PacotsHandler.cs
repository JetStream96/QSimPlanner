using System.Collections.Generic;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.RouteFinding.RouteFindingCore;
using QSP.LibraryExtension;
using System;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsHandler : TrackHandler<PacificTrack>
    {
        #region Fields

        private WaypointList wptList;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TogglerTrackCommunicator communicator;

        private PacotsMessage rawData;
        private List<TrackNodes> nodes;

        #endregion

        public PacotsHandler(WaypointList wptList, StatusRecorder recorder, AirportManager airportList, TogglerTrackCommunicator communicator)
        {
            this.wptList = wptList;
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
                    recorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one track.", TrackType.Pacots);
                }
            }
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        private void tryDownload()
        {
            try
            {
                rawData = (PacotsMessage)new PacotsDownloader().Download();
            }
            catch
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download PACOTs.", TrackType.Pacots);
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
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to parse PACOTs.", TrackType.Pacots);
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
            new TrackAdder<PacificTrack>(wptList, recorder).AddToWaypointList(nodes);

            foreach (var i in nodes)
            {
                communicator.StageTrackData(i);
            }
            communicator.PushAllData(TrackType.Pacots);
        }
    }
}
