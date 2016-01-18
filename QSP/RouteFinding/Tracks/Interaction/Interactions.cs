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
                await NatsManager.GetAllTracksAsync();
            }
            catch
            {
                TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Critical, "Failed to download NATs.", TrackType.Nats);
                return;
            }

            NatsManager.AddToWaypointList();
        }

        public async static Task SetPacots()
        {
            TrackStatusRecorder.Clear(TrackType.Pacots);
            await PacotsManager.GetAllTracksAsync();
            PacotsManager.AddToWaypointList();
        }

        public async static Task SetAusots()
        {
            TrackStatusRecorder.Clear(TrackType.Ausots);
            await AusotsManager.GetAllTracksAsync();
            AusotsManager.AddToWaypointList();
        }

        public async static Task SetAllTracksAsync()
        {
            TrackStatusRecorder.Clear();

            //these can be done asynchronously
            Task taskN = NatsManager.GetAllTracksAsync();
            Task taskP = PacotsManager.GetAllTracksAsync();
            Task taskA = AusotsManager.GetAllTracksAsync();

            taskN.Start();
            taskP.Start();
            taskA.Start();

            await Task.WhenAll(taskN, taskP, taskA);

            //these tasks MUST be done sequentially, as WptList is not thread-safe
            NatsManager.AddToWaypointList();
            PacotsManager.AddToWaypointList();
            AusotsManager.AddToWaypointList();
        }
    }
}
