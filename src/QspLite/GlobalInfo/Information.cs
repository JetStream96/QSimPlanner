using QSP.AircraftProfiles;
using QSP.RouteFinding.Airports;

namespace QspLite.GlobalInfo
{
    public static class Information
    {
        public static ProfileManager Profiles { get; private set; }
        public static AirportManager AirportList { get; private set; }

        public static void InitializeProfiles()
        {
            Profiles = new ProfileManager();
            Profiles.Initialize();
        }
    }
}
