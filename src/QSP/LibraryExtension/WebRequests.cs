using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    public static class WebRequests
    {
        // TODO: This uses form instead of urlencoded.
        // E.g. If query is { "a" : 1, "b" : 2 }, the corresponding query string is "a=1&b=2"
        public static HttpWebRequest GetPostRequest(string url, IDictionary<string, string> query)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(GetQuery(query));
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = buffer.Length;

            Stream postData = webReq.GetRequestStream();

            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            return webReq;
        }

        public async static Task<HttpResponseMessage> PostRequestAsync(string url,
            IDictionary<string, string> query)
        {
            var content = new FormUrlEncodedContent(query);
            var res = await new HttpClient().PostAsync(url, content);
            return res;
        }

        public async static Task<string> PostRequestStringSync(string url,
            IDictionary<string, string> query)
        {
            var res = await PostRequestAsync(url, query);
            var str = await res.Content.ReadAsStringAsync();
            return str;
        }

        public static string GetResponseString(this WebResponse response)
        {
            Stream answer = response.GetResponseStream();
            return new StreamReader(answer).ReadToEnd();
        }
        
        public static string GetQuery(IDictionary<string, string> query)
        {
            var content = new FormUrlEncodedContent(query);
            return Task.Run(content.ReadAsStringAsync).Result;
        }
    }
}