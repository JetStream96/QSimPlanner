using System.Threading.Tasks;

namespace QSP.WindAloft
{
    public static class WindManager
    {
        public static readonly string SaveFileLocation =
            Utilities.WxFileDirectory + "\\wx.grib2";
        
        // TODO: And what other exceptions?
        /// <summary>
        /// Download, convert and read the files into WindTableCollection.
        /// </summary>
        /// <exception cref="ReadWindFileException"></exception>
        public static WindTableCollection LoadWind()
        {
            new GribDownloader().DownloadGribFile(SaveFileLocation);
            GribConverter.ConvertGrib();
            return new WindFileLoader().ImportAllTables();
        }

        public static async Task<WindTableCollection> LoadWindAsync()
        {
            return await Task.Factory.StartNew(LoadWind);
        }
    }
}
