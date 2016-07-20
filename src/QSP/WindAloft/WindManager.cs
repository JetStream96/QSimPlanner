using System.Threading.Tasks;

namespace QSP.WindAloft
{
    public static class WindManager
    {
        public static readonly string DownloadFilePath =
            Constants.WxFileDownloadDirectory + @"\wx.grib2";
        
        /// <summary>
        /// Download, convert and read the files into WindTableCollection.
        /// </summary>
        /// <exception cref="ReadWindFileException"></exception>
        /// <exception cref="DownloadGribFileException"></exception> 
        public static WindTableCollection LoadWind()
        {
            new GribDownloader().DownloadGribFile(DownloadFilePath);
            GribConverter.ConvertGrib();

            var handler = new WindFileHandler();
            var tables = handler.ImportAllTables();
            handler.TryDeleteCsvFiles();
            return tables;
        }

        /// <exception cref="ReadWindFileException"></exception>
        /// <exception cref="DownloadGribFileException"></exception>
        public static async Task<WindTableCollection> LoadWindAsync()
        {
            return await Task.Factory.StartNew(LoadWind);
        }
    }
}
