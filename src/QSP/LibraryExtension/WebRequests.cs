using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using QSP.Utilities;

namespace QSP.LibraryExtension
{
    public static class WebRequests
    {
        // E.g. If query is { "a" : 1, "b" : 2 }, the corresponding query string is "a=1&b=2"
        // Content type can also be "multipart/form-data", etc.
        public static HttpWebRequest GetPostRequest(string url, IDictionary<string, string> query,
            string contentType = "application/x-www-form-urlencoded")
        {
            byte[] buffer = Encoding.ASCII.GetBytes(GetQuery(query));
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            webReq.Method = "POST";
            webReq.ContentType = contentType;
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

        /// <summary>
        /// Usage:
        /// using (var wc = WebClientNoCache()) 
        /// {
        ///     ...
        /// }
        /// </summary>
        public static WebClient WebClientNoCache()
        {
            return new WebClient()
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
            };
        }

        /// <summary>
        /// Downloads the requested string at given uri. The returning result is 
        /// not from cache.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static string DownloadString(string uri)
        {
            using (var wc = WebClientNoCache())
            {
                return wc.DownloadString(uri);
            }
        }

        /// <summary>
        /// Asynchronously Downloads the requested string at given uri. The returning result is 
        /// not from cache.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static async Task<string> DownloadStringTaskAsync(string uri)
        {
            using (var wc = WebClientNoCache())
            {
                return await wc.DownloadStringTaskAsync(uri);
            }
        }

        /// <summary>
        /// Basically always use this to avoid errors due to .Net using old
        /// security protocols by default.
        /// </summary>
        public static void SetSecuityProtocol()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            catch (System.Exception e)
            {
                LoggerInstance.Log(e);
            }            
        }
    }
}