namespace QSP.RouteFinding.FileExport
{
    public interface IExportProvider
    {
        /// <summary>
        /// Returns the text of the file to export.
        /// </summary>
        string GetExportText();
    }
}
