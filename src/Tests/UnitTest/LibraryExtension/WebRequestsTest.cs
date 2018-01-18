using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static QSP.LibraryExtension.WebRequests;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class WebRequestsTest
    {
        [Test]
        public void FormUrlEncodedContentTest()
        {
            // Make sure the library method works.
            var query = new Dictionary<string, string>()
            {
                ["a b c"] = "1 2",
                ["1\"2"] = "3"
            };

            var content = new FormUrlEncodedContent(query);
            var s = Task.Run(content.ReadAsStringAsync).Result;
            Assert.AreEqual("a+b+c=1+2&1%222=3", s);
        }

        [Test]
        public void UriIsHttpOrHttpsTest()
        {
            Assert.IsTrue(UriIsHttpOrHttps("http://google.com"));
            Assert.IsTrue(UriIsHttpOrHttps("https://google.com"));
            Assert.IsFalse(UriIsHttpOrHttps("file://path/file.txt"));
        }
    }
}
