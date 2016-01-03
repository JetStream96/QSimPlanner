using QSP.RouteFinding.Tracks.Common;
using System.Threading.Tasks;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding.Tracks.Interaction
{
    public static class Interactions
    {
        public async static Task SetNats()
        {
            TrackStatusRecorder.Clear(TrackType.Nats);

            try
            {
                await Task.Factory.StartNew(() => NatsManager.DownloadNatsMsg());
            }
            catch 
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download NATs.", TrackType.Nats);
                return;
            }

            NatsManager.AddToWptList();
        }

        public async static Task SetPacots()
        {
            TrackStatusRecorder.Clear(TrackType.Pacots);
            await Task.Factory.StartNew(() => PacotsManager.GetAllTracks());
            PacotsManager.AddToWptList();
        }

        public async static Task SetAusots()
        {
            TrackStatusRecorder.Clear(TrackType.Ausots);
            await Task.Factory.StartNew(() => AusotsManager.GetAllTracks());
            AusotsManager.AddToWptList();
        }

        public async static Task SetAllTracksAsync()
        {

            TrackStatusRecorder.Clear();

            //these can be done asynchronously
            Task taskN = new Task(() => NatsManager.DownloadNatsMsg());
            Task taskP = new Task(() => PacotsManager.GetAllTracks());
            Task taskA = new Task(() => AusotsManager.GetAllTracks());

            taskN.Start();
            taskP.Start();
            taskA.Start();

            await Task.WhenAll(taskN, taskP, taskA);

            //these tasks MUST be done sequentially, as WptList is not thread-safe
            NatsManager.AddToWptList();
            PacotsManager.AddToWptList();
            AusotsManager.AddToWptList();

        }

    }

}
