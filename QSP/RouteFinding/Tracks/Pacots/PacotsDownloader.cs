using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public static class PacotsDownloader
    {
        /// <summary>
        /// Returns the html file of PACOTs web page.
        /// </summary>
        public static string DownloadTrackMessage()
        {
            return StartPost();
        }

        /// <summary>
        /// Returns the html file of PACOTs web page.
        /// </summary>
        public async static Task<string> DownloadTrackMessageAsync()
        {
            return await Task.Factory.StartNew(StartPost);
        }

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

            Stream Answer = webResp.GetResponseStream();
            var _Answer = new StreamReader(Answer);

            return _Answer.ReadToEnd();

        }
    }
}
