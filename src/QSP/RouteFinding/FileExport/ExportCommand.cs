namespace QSP.RouteFinding.FileExport
{
    public class ExportCommand
    {
        public IExportProvider Provider { get; private set; }
        public string FilePath { get; private set; }
        public bool Enabled { get; private set; }

        public ExportCommand(
            IExportProvider Provider, string FilePath, bool Enabled)
        {
            this.Provider = Provider;
            this.FilePath = FilePath;
            this.Enabled = Enabled;
        }
    }
}
