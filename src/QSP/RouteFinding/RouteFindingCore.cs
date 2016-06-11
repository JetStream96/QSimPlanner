using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Pacots;

namespace QSP.RouteFinding
{
    public static class RouteFindingCore
    {
        //===================================== These are critical data for route finding =====================================
        // Data used is Navigraph - Aerosoft Airbus X v1.22 or later
        
        public static RouteGroup RouteToDest;
        public static RouteGroup RouteToAltn;

        public static NatsHandler NatsManager;
        public static PacotsHandler PacotsManager;
        public static AusotsHandler AusotsManager;

        public static StatusRecorder TrackStatusRecorder;
        public static TrackInUseCollection TracksInUse;
        public static RouteTrackCommunicator RTCommunicator;
    }

}
