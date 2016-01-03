using System.Threading.Tasks;
using static QSP.Core.QspCore;

namespace QSP.WindAloft
{
    public class WindManager
    {
        public static readonly string SaveFileLocation = Utilities.wxFileDirectory + "\\wx.grib2";

        public void DownloadWind()
        {
            new GribDownloader().DownloadGribFile(SaveFileLocation);
            GribConverter.ConvertGrib();
            WxReader = new WxFileLoader();
            WxReader.ImportAllTables();
        }

        public async Task DownloadWindAsync()
        {
            await Task.Factory.StartNew(() => DownloadWind());
        }
    }
}
