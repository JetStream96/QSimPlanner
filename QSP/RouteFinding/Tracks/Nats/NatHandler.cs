using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Airports;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Nats
{

    public class NatsHandler : TrackHandler<NorthAtlanticTrack>
    {
        #region Fields

        private WaypointList wptList;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TogglerTrackCommunicator communicator;

        private NatsMessage rawData;
        private List<TrackNodes> nodes;

        #endregion

        public NatsHandler(WaypointList wptList, StatusRecorder recorder, AirportManager airportList, TogglerTrackCommunicator communicator)
        {
            this.wptList = wptList;
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

            var reader = new TrackReader<NorthAtlanticTrack>(wptList);
            nodes = new List<TrackNodes>();

            foreach (var i in trks)
            {
                try
                {
                    nodes.Add(reader.Read(i));
                }
                catch
                {
                    recorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one track.", TrackType.Nats);
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
            new TrackAdder<NorthAtlanticTrack>(wptList, recorder).AddToWaypointList(nodes);

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
                rawData = (NatsMessage)new NatsDownloader().Download();
            }
            catch
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download NATs.", TrackType.Nats);
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
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to parse NATs.", TrackType.Nats);
                throw new TrackParseException("Failed to parse Nats.", ex);
            }
        }
    }
}
