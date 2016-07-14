using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Interaction
{
    public static class Interactions
    {
        public async static Task SetNats(
            NatsHandler handler, StatusRecorder recorder)
        {
            recorder.Clear(TrackType.Nats);
            handler.UndoEdit();

            try
            {
                await handler.GetAllTracksAsync();
                handler.AddToWaypointList();
            }
            catch
            { }
        }

        public async static Task SetPacots(
            PacotsHandler handler, StatusRecorder recorder)
        {
            recorder.Clear(TrackType.Pacots);
            handler.UndoEdit();

            try
            {
                await handler.GetAllTracksAsync();
                handler.AddToWaypointList();
            }
            catch
            { }
        }

        public async static Task SetAusots(
            AusotsHandler handler, StatusRecorder recorder)
        {
            recorder.Clear(TrackType.Ausots);
            handler.UndoEdit();

            try
            {
                await handler.GetAllTracksAsync();
                handler.AddToWaypointList();
            }
            catch
            { }
        }

        public async static Task SetAllTracksAsync(
            NatsHandler NatsManager,
            PacotsHandler PacotsManager,
            AusotsHandler AusotsManager,
            StatusRecorder TrackStatusRecorder)
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
