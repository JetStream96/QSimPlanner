using System.Collections.Generic;
using QSP.RouteFinding.Tracks.Common;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsHandler : TrackHandler<AusTrack>
    {

        #region Fields

        private WaypointList wptList;
        private StatusRecorder recorder;
        private AirportManager airportList;
        private TogglerTrackCommunicator communicator;

        private AusotsMessage rawData;
        private List<TrackNodes> nodes;

        #endregion

        public AusotsHandler(WaypointList wptList, StatusRecorder recorder, AirportManager airportList, TogglerTrackCommunicator communicator)
        {
            this.wptList = wptList;
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
            tryDownload();
            var trks = tryParse();

            var reader = new TrackReader<AusTrack>(wptList);
            nodes = new List<TrackNodes>();

            foreach (var i in trks)
            {
                try
                {
                    nodes.Add(reader.Read(i));
                }
                catch
                {
                    recorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one track.", TrackType.Ausots);
                }
            }
        }
        
        /// <exception cref="TrackParseException"></exception>
        private List<AusTrack> tryParse()
        {
            try
            {
                return new AusotsParser(rawData, recorder, airportList).Parse();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to parse AUSOTs.", TrackType.Ausots);
                throw new TrackParseException("Failed to parse Ausots.", ex);
            }
        }

        /// <exception cref="TrackDownloadException"></exception>
        private void tryDownload()
        {
            try
            {
                rawData = (AusotsMessage)new AusotsDownloader().Download();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download AUSOTs.", TrackType.Ausots);
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
            new TrackAdder<AusTrack>(wptList, recorder).AddToWaypointList(nodes);

            foreach (var i in nodes)
            {
                communicator.StageTrackData(i);
            }
            communicator.PushAllData(TrackType.Ausots);
        }
    }
}
