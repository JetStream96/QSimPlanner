using QSP.AircraftProfiles;

namespace QspLite.Data
{
    public static class AircraftProfiles
    {
        public static ProfileManager Profiles { get; private set; }

        public static void Initialize()
        {
            Profiles = new ProfileManager();
            Profiles.Initialize();
        }
    }
}
