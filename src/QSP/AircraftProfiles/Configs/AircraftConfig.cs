using System.IO;
using QSP.LibraryExtension;

namespace QSP.AircraftProfiles.Configs
{
    public class AircraftConfig
    {
        public AircraftConfigItem Config { get; }
        public string FilePath { get; }

        public bool IsDefault => Paths.PathsAreSame(
            Path.GetDirectoryName(FilePath), ConfigLoader.DefaultFolderPath);

        public AircraftConfig(AircraftConfigItem Config, string FilePath)
        {
            this.Config = Config;
            this.FilePath = FilePath;
        }
    }
}
