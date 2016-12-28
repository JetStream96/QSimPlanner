using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsDownloader : ITrackMessageProvider
    {
        private static readonly string url = "https://www.notams.faa.gov/dinsQueryWeb/advancedNotamMapAction.do";

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
            byte[] buffer = Encoding.ASCII.GetBytes("queryType=pacificTracks&actionType=advancedNOTAMFunctions");
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = buffer.Length;

            Stream postData = webReq.GetRequestStream();

            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            return webReq;
        }
    }
}
