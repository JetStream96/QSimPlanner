using System.Collections.Generic;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsHandler : TrackHandler<PacificTrack>
    {
        private PacotsMessage msg;

        public override void GetAllTracks()
        {
            string htmlFile = null;

            try
            {
                htmlFile = PacotsDownloader.DownloadTrackMessage();
            }
            catch
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download Pacots.", TrackType.Pacots);
                throw;
            }

            try
            {
                msg = new PacotsMessage(htmlFile);
            }
            catch
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to interpret Pacots message.", TrackType.Pacots);
                throw;
            }

            allTracks = new List<PacificTrack>();

            foreach (var i in msg.WestboundTracks)
            {
                try
                {
                    allTracks.Add(new PacificTrack(i, PacotDirection.Westbound));
                }
                catch
                {
                    TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one westbound track.", TrackType.Pacots);
                }
            }

            try
            {
                allTracks.AddRange(EastTracksParser.CreateEastboundTracks(msg));
            }
            catch
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret eastbound tracks.", TrackType.Pacots);

            }
        }

        public override async void GetAllTracksAsync()
        {
            await Task.Run(() => GetAllTracks());
        }        
    }
}
