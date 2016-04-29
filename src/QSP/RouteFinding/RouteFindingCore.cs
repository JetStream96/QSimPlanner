using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Communication;
using QSP.RouteFinding.Routes.Toggler;

namespace QSP.RouteFinding
{
    public static class RouteFindingCore
    {
        //===================================== These are critical data for route finding =====================================
        // Data used is Navigraph - Aerosoft Airbus X v1.22 or later

        public static WaypointList WptList;
        //public static AirportManager AirportList;

        public static ManagedRoute RouteToDest;
        public static ManagedRoute RouteToAltn;

        public static NatsHandler NatsManager;
        public static PacotsHandler PacotsManager;
        public static AusotsHandler AusotsManager;

        public static StatusRecorder TrackStatusRecorder;
        public static TrackInUseCollection TracksInUse;
        public static RouteTrackCommunicator RTCommunicator;
    }

}
