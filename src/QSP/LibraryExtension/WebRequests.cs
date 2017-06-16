using System.IO;
using System.Net;
using System.Text;

namespace QSP.LibraryExtension
{
    public static class WebRequests
    {
        // Sample queryString: "queryType=pacificTracks&actionType=advancedNOTAMFunctions"
        public static HttpWebRequest GetPostRequest(string url, string queryString)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(queryString);
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