using QSP.AircraftProfiles;

namespace QspLite.GlobalInfo
{
    public static class Information
    {
        public static ProfileManager Profiles { get; private set; }

        public static void InitializeProfiles()
        {
            Profiles = new ProfileManager();
            Profiles.Initialize();
        }        
    }
}
