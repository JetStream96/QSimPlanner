using QSP.LibraryExtension;
using System.Net;

namespace QSP.WindAloft
{
    public class LastestDataSetFinder
    {
        // If the html source is longer than this number,
        // it's considered a valid page to return.
        // (The source of filter page has about 30000 chars, 
        //  a page with file unavailable has only 700.)
        private const int sourceCodeLenCriteria = 9000;

        private int numLinksTried = 0;
        string mainPageSource;

        public FindResult Find()
        {
            string src = string.Empty;

            using (var client = new WebClient())
            {
                mainPageSource = client.DownloadString(
                    GribDownloader.HomePageUrl);

                string url = string.Empty;

                while (src.Length < sourceCodeLenCriteria)
                {
                    try
                    {
                        url = LastestUrl();
                        src = client.DownloadString(url);
                    }
                    catch
                    {
                        throw new WindNotAvailException();
                    }
                }

                return new FindResult() { Url = url, Source = src };
            }
        }

        public struct FindResult { public string Url; public string Source; }

        private string LastestUrl()
        {
            try
            {
                int i = mainPageSource.NthOccurence(
                    "<a href=\"", ++numLinksTried) +
                    "<a href=\"".Length;

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
