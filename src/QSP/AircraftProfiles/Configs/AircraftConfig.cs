namespace QSP.AircraftProfiles.Configs
{
    public class AircraftConfig
    {
        public AircraftConfigItem Config { get; private set; }
        public string FilePath { get; private set; }

        public AircraftConfig(AircraftConfigItem Config, string FilePath)
        {
            this.Config = Config;
            this.FilePath = FilePath;
        }
    }
}
