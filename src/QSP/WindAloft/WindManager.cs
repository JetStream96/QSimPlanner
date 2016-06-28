using System.Threading.Tasks;

namespace QSP.WindAloft
{
    public static class WindDownloader
    {
        public static readonly string SaveFileLocation =
            Utilities.WxFileDirectory + "\\wx.grib2";

        // TODO: And what other exceptions?
        /// <exception cref="ReadWindFileException"></exception>
        public static WindTableCollection DownloadWind()
        {
            new GribDownloader().DownloadGribFile(SaveFileLocation);
            GribConverter.ConvertGrib();
            return new WindFileLoader().ImportAllTables();
        }

        public static async Task<WindTableCollection> DownloadWindAsync()
        {
            return await Task.Factory.StartNew(DownloadWind);
        }
    }
}
