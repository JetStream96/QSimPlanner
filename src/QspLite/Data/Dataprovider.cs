using QSP.AircraftProfiles;
using QSP.RouteFinding.Airports;

namespace QspLite.Data
{
    public static class DataProvider
    {
        public static ProfileManager Profiles { get; private set; }
        public static AirportManager AirportList { get; private set; }

        public static void Initialize()
        {
            Profiles = new ProfileManager();
            Profiles.Initialize();


        }
    }
}
