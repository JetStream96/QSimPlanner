using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Routes.Toggler;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;
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
            initManagers();
        }

        private static void initManagers()
        {
            NatsManager = new NatsHandler(new NatsDownloader(),
                                          WptList,
                                          WptList.GetEditor(),
                                          TrackStatusRecorder,
                                          AirportList,
                                          RTCommunicator);

            PacotsManager = new PacotsHandler(new PacotsDownloader(),
                                              WptList,
                                              WptList.GetEditor(),
                                              TrackStatusRecorder,
                                              AirportList,
                                              RTCommunicator);

            AusotsManager = new AusotsHandler(new AusotsDownloader(),
                                              WptList,
                                              WptList.GetEditor(),
                                              TrackStatusRecorder,
                                              AirportList,
                                              RTCommunicator);
        }
    }
}
