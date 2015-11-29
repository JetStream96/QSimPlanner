using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;

namespace QSP.RouteFinding.Tracks.Pacots
{

    public class PacotsHandler : TrackHandler
    {
        private PacotsMessage msg;

        public override void GetAllTracks()
        {
            string htmlFile = null;

            try
            {
                htmlFile = PacotsDownloader.GetHtml();

            }
            catch 
            {
                RouteFindingCore.TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download Pacots.", TrackType.Pacots);
                throw;

            }

            try
            {
                msg = new PacotsMessage(htmlFile);
            }
            catch
            {
                RouteFindingCore.TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to interpret Pacots message.", TrackType.Pacots);
                throw;
            }

            allTracks = new List<ITrack>();

            foreach (var i in msg.WestboundTracks)
            {
                try
                {
                    allTracks.Add(new PacificTrack(i, PacotDirection.Westbound));
                }
                catch
                {
                    RouteFindingCore.TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one westbound track.", TrackType.Pacots);
                }
            }

            try
            {
                allTracks.AddRange(EastTracksParser.CreateEastboundTracks(msg));

            }
            catch 
            {
                RouteFindingCore.TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret eastbound tracks.", TrackType.Pacots);

            }


        }

        public override async void GetAllTracksAsync()
        {
            await Task.Run(() => GetAllTracks());
        }

        protected override string airwayIdent(ITrack trk)
        {
            return "PACOT" + trk.Ident;
        }

    }

}
