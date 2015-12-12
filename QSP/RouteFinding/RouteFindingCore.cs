using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Nats;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Ausots;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding
{
    public static class RouteFindingCore
	{
		//===================================== These are critical data for route finding =====================================
		//All DB used is Aerosoft Airbus X v1.22 or later

		public static TrackedWptList WptList;
		public static LatLonSearchUtility<int> WptFinder;

		public static AirportManager AirportList;

		public static Route RouteToDest;
		public static Route RouteToAltn;

		public static StatusRecorder TrackStatusRecorder = new StatusRecorder();
		public static NatHandler NatsManager;
		public static PacotsHandler PacotsManager;
		public static AusotsHandler AusotsManager;
	}

}
