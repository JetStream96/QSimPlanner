using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public static bool UriIsHttpOrHttps(string uriName)
        {
            return Uri.TryCreate(uriName, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || 
                   uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}