using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Tracks.Common;
using QSP.LibraryExtension;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.RouteFinding.RouteFindingCore;
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

        private AusotsRawData rawData;
        private List<TrackNodes> nodes;

        #endregion
                
        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        public override void GetAllTracks()
        {
            tryDownload();
            var trks = tryParse();

            var reader = new TrackReader<AusTrack>();
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

        private void tryDownload()
        {
            try
            {
                rawData = (AusotsRawData)new AusotsDownloader().Download();
            }
            catch (Exception ex)
            {
                recorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download AUSOTs.", TrackType.Ausots);
                throw new TrackDownloadException("Failed to download Ausots.", ex);
            }
        }
        
        public override async void GetAllTracksAsync()
        {
            await Task.Factory.StartNew(GetAllTracks);
        }

        public override void AddToWaypointList()
        {
            new TrackAdder<AusTrack>(wptList, recorder).AddToWaypointList(nodes);
        }
    }
}
