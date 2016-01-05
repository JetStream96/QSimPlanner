using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Tracks.Common;
using QSP.LibraryExtension;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsHandler : TrackHandler<AusTrack>
    {
        private string trkMsg;

        public override void GetAllTracks()
        {
            tryDownloadMsg();
            var indices = trkMsg.IndicesOf("TDM TRK");
            fixLastEntry(indices);

            if (indices.Count < 2)
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to interpret Ausots message.", TrackType.Ausots);
                return;
            }

            for (int i = 0; i <= indices.Count - 2; i++)
            {
                tryAddTrk(indices, i);
            }
        }

        private void tryAddTrk(List<int> indices, int index)
        {
            try
            {
                var trk = new AusTrack(trkMsg.Substring(indices[index], indices[index + 1] - indices[index]));

                if (trk.Available)
                {
                    allTracks.Add(trk);
                }
            }
            catch
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one track.", TrackType.Ausots);
            }
        }

        private void tryDownloadMsg()
        {
            try
            {
                trkMsg = AusotsDownloader.DownloadMsg();

            }
            catch
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download Ausots.", TrackType.Ausots);
                throw;
            }
        }

        public override async void GetAllTracksAsync()
        {
            await Task.Run(() => GetAllTracks());
        }

        private void fixLastEntry(List<int> item)
        {
            int x = trkMsg.IndexOf("</pre>", item.Last());
            item.Add(x < 0 ? trkMsg.Length : x);
        }

        protected override string airwayIdent(AusTrack trk)
        {
            return "AUSOT" + trk.Ident;
        }
    }
}
