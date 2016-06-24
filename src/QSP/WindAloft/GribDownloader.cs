using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace QSP.WindAloft
{
    public class GribDownloader
    {
        public static readonly string HomePageUrl = 
            "http://nomads.ncep.noaa.gov/cgi-bin/filter_gfs_1p00.pl";

        private string webPageUrl;
        private string webPageSrc;

        public void DownloadGribFile(string filePath)
        {
            var x = new LastestDataSetFinder().Find();
            webPageUrl = x.Item1;
            webPageSrc = x.Item2;

            using (var client = new WebClient())
            {
                Directory.CreateDirectory(new FileInfo(filePath).DirectoryName);
                client.DownloadFile(FileUrl(), filePath);
            }
        }

        private string FileUrl()
        {
            var sb = new StringBuilder();
            sb.Append(webPageUrl.Substring(0, webPageUrl.IndexOf('?') + 1));

            foreach (var i in GetParaString())
            {
                sb.Append(i + '&');
            }
            sb.Append(GetParameters());

            return sb.ToString();
        }

        private string GetParameters()
        {
            int x = webPageUrl.IndexOf('?');
            return webPageUrl.Substring(x + 1, webPageUrl.Length - x - 1);
        }

        private string GetFilePara()
        {
            int index = webPageSrc.IndexOf("<select name=\"file\">");
            int x = webPageSrc.IndexOf("<option value=\"", index) + 
                "<option value=\"".Length;
            int y = webPageSrc.IndexOf("\">", x);

            return webPageSrc.Substring(x, y - x);
        }

        private List<string> GetParaString()
        {
            var items = new List<string>();

            items.Add("file=" + GetFilePara());
            items.Add("leftlon=0");
            items.Add("rightlon=360");
            items.Add("toplat=90");
            items.Add("bottomlat=-90");

            foreach (var i in Utilities.FullWindDataSet)
            {
                items.Add("lev_" + i.ToString() + "_mb=on");
            }

            items.Add("var_UGRD=on");
            items.Add("var_VGRD=on");

            return items;
        }
    }
}
