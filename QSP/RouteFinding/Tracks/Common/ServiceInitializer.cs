using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Routes.Toggler;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding.Tracks.Common
{
    public static class ServiceInitializer
    {
        public static void Initailize()
        {
            TrackStatusRecorder = new StatusRecorder();
            TracksInUse = new TrackInUseCollection();
            RTCommunicator = new RouteTrackCommunicator(TracksInUse);
        }
    }
}
