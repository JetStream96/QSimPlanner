using QSP.AircraftProfiles;
using QSP.Core.Options;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.Utilities;

namespace QSimPlanner.GlobalInfo
{
    public static class Information
    {
        public static ProfileManager Profiles { get; private set; }

        public static void InitializeProfiles()
        {
            Profiles = new ProfileManager();
            Profiles.Initialize();
        }

        public static WaypointList WptList { get; private set; }

        public static void InitializeWptList()
        {
            string navDataPath = AppSettings.NavDataLocation;

            WptList =
                new WptListLoader(navDataPath)
                .LoadFromFile();
        }

        public static AirportManager AirportList { get; private set; }

        public static void InitializeAirportList()
        {
            string navDataPath = AppSettings.NavDataLocation;

            AirportList =
            new AirportManager(
                new AirportDataLoader(navDataPath + @"\Airports.txt")
                .LoadFromFile());
        }
        
        public static AppOptions AppSettings { get; set; }

        public static void InitializeSettings()
        {
            AppSettings = OptionManager.ReadFromFile();
        }        
    }
}
