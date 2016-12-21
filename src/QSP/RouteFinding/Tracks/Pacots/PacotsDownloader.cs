using System.IO;
using System.Net;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsDownloader : IPacotsMessageProvider
    {
        /// <exception cref="Exception"></exception>
        public PacotsMessage GetMessage()
        {
                return new PacotsMessage(StartPost());
        }

        /// <exception cref="Exception"></exception>
        public Task<PacotsMessage> GetMessageAsync(CancellationToken token)
        {

        }

        /// <summary>
        /// Returns the html file of PACOTs web page.
        /// </summary>
        private static string StartPost()
        {
            byte[] buffer = Encoding.ASCII.GetBytes("queryType=pacificTracks&actionType=advancedNOTAMFunctions");
            var webReq = (HttpWebRequest)WebRequest.Create("https://www.notams.faa.gov/dinsQueryWeb/advancedNotamMapAction.do");

            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = buffer.Length;

            Stream postData = webReq.GetRequestStream();

            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            var webResp = (HttpWebResponse)webReq.GetResponse();
            Stream answer = webResp.GetResponseStream();

            return new StreamReader(answer).ReadToEnd();
        }

    }
}
