using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using QSP.LibraryExtension;

namespace QSP.WindAloft
{
    public class LastestDataSetFinder
    {

        // The source has about 30000 chars for the filter page, and only 700 for a page with file unavailable.
        // If the html source is longer than this number, it's considered a valid page to return.
        private const int sourceCodeLenCriteria = 9000;

        private int numLinksTried = 0;
        string mainPageSource;

        public Tuple<string, string> Find()
        {
            string src = string.Empty;

            using (var client = new WebClient())
            {
                mainPageSource = client.DownloadString(GribDownloader.HomePageUrl);
                string url = string.Empty;

                while (src.Length < sourceCodeLenCriteria)
                {
                    try
                    {
                        url = lastestUrl();
                        src = client.DownloadString(url);
                    }
                    catch
                    {
                        throw new WindNotAvailException();
                    }
                }
                return new Tuple<string, string>(url, src);
            }
        }

        private string lastestUrl()
        {
            try
            {
                int i = Strings.NthOccurence(mainPageSource, "<a href=\"", ++numLinksTried) + "<a href=\"".Length;
                int j = mainPageSource.IndexOf("\">", i);
                return mainPageSource.Substring(i, j - i);
            }
            catch
            {
                throw new WindNotAvailException();
            }
        }

    }
}
