using System.IO;
using System.Net;
using System.Text;
using System;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsDownloader : IPacotsMessageProvider
    {
        /// <exception cref="GetTrackException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public PacotsMessage GetMessage()
        {
            string html;

            try
            {
                html = StartPost();
            }
            catch (Exception ex)
            {
                throw new GetTrackException("Failed to download PACOTs.", ex);
            }

            try
            {
                return new PacotsMessage(html);
            }
            catch (Exception ex)
            {
                throw new TrackParseException("Failed to parse PACOTs.", ex);
            }
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

            Stream Answer = webResp.GetResponseStream();
            var _Answer = new StreamReader(Answer);

            return _Answer.ReadToEnd();
        }
    }
}
