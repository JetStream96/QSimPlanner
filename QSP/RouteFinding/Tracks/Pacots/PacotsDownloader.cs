using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{

    public static class PacotsDownloader
    {

        public static string GetHtml()
        {
            return StartPost();
        }
               
        public async static Task<string> GetHtmlAsync()
        {
            string s;
            s = await Task.Factory.StartNew(StartPost);
            return s;
        }

        private static string StartPost()
        {

            byte[] buffer = Encoding.ASCII.GetBytes("queryType=pacificTracks&actionType=advancedNOTAMFunctions");
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create("https://www.notams.faa.gov/dinsQueryWeb/advancedNotamMapAction.do");

            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = buffer.Length;

            Stream postData = webReq.GetRequestStream();

            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();

            Stream Answer = webResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);

            return _Answer.ReadToEnd();

        }

    }

}
