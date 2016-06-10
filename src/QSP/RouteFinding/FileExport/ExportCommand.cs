using QSP.RouteFinding.FileExport.Providers;

namespace QSP.RouteFinding.FileExport
{
    public class ExportCommand
    {
        public ProviderType ProviderType { get; private set; }
        public string Directory { get; private set; }
        public string Extension { get; private set; } 
        public bool Enabled { get; private set; }

        public ExportCommand(
            ProviderType ProviderType, 
            string Directory,
            bool Enabled)
        {
            this.ProviderType = ProviderType;
            this.Directory = Directory;
            this.Extension = Types.GetExtension(ProviderType);
            this.Enabled = Enabled;
        }
    }
}
