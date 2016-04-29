using QSP.AircraftProfiles;
using QSP.RouteFinding.Airports;
using QSP.Core;

namespace QspLite.Data
{
    public static class DataProvider
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
