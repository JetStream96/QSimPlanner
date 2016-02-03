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
            NatsManager.UndoEdit();

            try
            {
                await NatsManager.GetAllTracksAsync();
                NatsManager.AddToWaypointList();
            }
            catch
            {
            }
        }

        public async static Task SetPacots()
        {
            TrackStatusRecorder.Clear(TrackType.Pacots);
            PacotsManager.UndoEdit();

            try
            {
                await PacotsManager.GetAllTracksAsync();
                PacotsManager.AddToWaypointList();
            }
            catch
            {
            }
        }

        public async static Task SetAusots()
        {
            TrackStatusRecorder.Clear(TrackType.Ausots);
            AusotsManager.UndoEdit();

            try
            {
                await AusotsManager.GetAllTracksAsync();
                AusotsManager.AddToWaypointList();
            }
            catch
            {
            }
        }

        public async static Task SetAllTracksAsync()
        {
            TrackStatusRecorder.Clear();
            NatsManager.UndoEdit();
            PacotsManager.UndoEdit();
            AusotsManager.UndoEdit();

            //these can be done asynchronously
            Task taskN = NatsManager.GetAllTracksAsync();
            Task taskP = PacotsManager.GetAllTracksAsync();
            Task taskA = AusotsManager.GetAllTracksAsync();

            bool dnNats = true;
            bool dnPacots = true;
            bool dnAusots = true;

            try
            {
                taskN.Start();
            }
            catch
            {
                dnNats = false;
            }

            try
            {
                taskP.Start();
            }
            catch
            {
                dnPacots = false;
            }

            try
            {
                taskA.Start();
            }
            catch
            {
                dnAusots = false;
            }

            await Task.WhenAll(taskN, taskP, taskA);

            // AddToWaypointList methods MUST be done on main thread, 
            // since WaypointList is not thread-safe.

            if (dnNats)
            {
                NatsManager.AddToWaypointList();
            }

            if (dnPacots)
            {
                PacotsManager.AddToWaypointList();
            }

            if (dnAusots)
            {
                AusotsManager.AddToWaypointList();
            }
        }
    }
}
