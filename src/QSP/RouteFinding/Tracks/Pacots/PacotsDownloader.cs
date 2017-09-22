using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsDownloader : ITrackMessageProvider
    {
        private static readonly string url =
            "https://www.notams.faa.gov/dinsQueryWeb/advancedNotamMapAction.do";

        /// <exception cref="Exception"></exception>
        public ITrackMessage GetMessage() => new PacotsMessage(GetPostMessage());

        /// <exception cref="Exception"></exception>
        public async Task<ITrackMessage> GetMessageAsync()
        {
            return new PacotsMessage(await GetPostMessageAsync());
        }

        /// <summary>
        /// Returns the html file of PACOTs web page.
        /// </summary>
        private static string GetPostMessage()
        {
            var webResp = (HttpWebResponse)GetRequest().GetResponse();
            return webResp.GetResponseString();
        }


        private static Dictionary<string, string> Query => new Dictionary<string, string>()
        {
            ["queryType"] = "pacificTracks",
            ["actionType"] = "advancedNOTAMFunctions"
        };

        private static async Task<string> GetPostMessageAsync()
        {
            return await WebRequests.PostRequestStringSync(url, Query);
        }

        private static HttpWebRequest GetRequest() => WebRequests.GetPostRequest(url, Query);
    }
}
