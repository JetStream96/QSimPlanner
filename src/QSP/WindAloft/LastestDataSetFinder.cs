using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Linq;
using static QSP.LibraryExtension.WebRequests;

namespace QSP.WindAloft
{
    public static class LastestDataSetFinder
    {
        // If the html source is longer than this number,
        // it's considered a valid page to return.
        // (The source of filter page has about 30000 chars, 
        //  a page with file unavailable has only 700.)
        private const int sourceCodeLenCriteria = 9000;

        // @Throws
        public static FindResult Find()
        {
            using (var client = WebClientNoCache())
            {
                var mainPageSource = client.DownloadString(GribDownloader.HomePageUrl);
                var urls = GetUrls(mainPageSource);

                foreach (var i in urls)
                {
                    try
                    {
                        var src = client.DownloadString(i);

                        if (src.Length >= sourceCodeLenCriteria)
                        {
                            return new FindResult() { Url = i, Source = src };
                        }
                    }
                    catch { }
                }

                throw new WindNotAvailException();
            }
        }

        public struct FindResult { public string Url, Source; }

        private static IEnumerable<string> GetUrls(string html)
        {
            var pattern = "<a href=\"([^\"]+)\">";
            var matches = Regex.Matches(html, pattern);
            return matches.Cast<Match>().Select(m => m.Groups[1].Value);
        }
    }
}
