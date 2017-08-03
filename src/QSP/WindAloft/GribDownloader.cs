using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace QSP.WindAloft
{
    public class GribDownloader
    {
        public static readonly string HomePageUrl =
            "http://nomads.ncep.noaa.gov/cgi-bin/filter_gfs_1p00.pl";

        private string webPageUrl;
        private string webPageSrc;

        // @Throws
        public void DownloadGribFile(string filePath)
        {
            var x = LastestDataSetFinder.Find();
            webPageUrl = x.Url;
            webPageSrc = x.Source;

            try
            {
                using (var client = new WebClient())
                {
                    var dir = new FileInfo(filePath).DirectoryName;
                    Directory.CreateDirectory(dir);
                    client.DownloadFile(FileUrl(), filePath);
                }
            }
            catch (Exception ex)
            {
                throw new DownloadGribFileException("", ex);
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
            var items = new List<string>()
            {
                "file=" + GetFilePara(),
                "leftlon=0",
                "rightlon=360",
                "toplat=90",
                "bottomlat=-90"
            };
            
            foreach (var i in Constants.FullWindDataSet)
            {
                items.Add($"lev_{i}_mb=on");
            }

            items.Add("var_UGRD=on");
            items.Add("var_VGRD=on");

            return items;
        }
    }
}
