using System.IO;
using System.Net;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsDownloader : ITrackMessageProvider
    {
        private static readonly string url =
            "https://www.notams.faa.gov/dinsQueryWeb/advancedNotamMapAction.do";

        /// <exception cref="Exception"></exception>
        public ITrackMessage GetMessage()
        {
            return new PacotsMessage(GetPostMessage());
        }

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
            return GetResponseString(webResp);
        }

        private static async Task<string> GetPostMessageAsync()
        {
            var req = GetRequest();
            var webResp = (HttpWebResponse)await req.GetResponseAsync();
            return GetResponseString(webResp);
        }

        private static string GetResponseString(HttpWebResponse response)
        {
            Stream answer = response.GetResponseStream();
            return new StreamReader(answer).ReadToEnd();
        }

        private static HttpWebRequest GetRequest()
        {
            return WebRequests.GetPostRequest(url,
                "queryType=pacificTracks&actionType=advancedNOTAMFunctions");
        }
    }
}
