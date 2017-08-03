using System.Threading.Tasks;
using System.IO;

namespace QSP.WindAloft
{
    public static class WindManager
    {
        public static readonly string DownloadFilePath =
            Path.Combine(Constants.WxFileDownloadDirectory, "wx.grib2");

        /// <summary>
        /// @Throws
        /// Download, convert and read the files into WindTableCollection.
        /// </summary>
        public static WindTableCollection LoadWind()
        {
            new GribDownloader().DownloadGribFile(DownloadFilePath);
            GribConverter.ConvertGrib();

            var handler = new WindFileHandler();
            var tables = handler.ImportAllTables();
            handler.TryDeleteCsvFiles();
            return tables;
        }

        // @Throws
        public static async Task<WindTableCollection> LoadWindAsync()
        {
            return await Task.Factory.StartNew(LoadWind);
        }
    }
}
