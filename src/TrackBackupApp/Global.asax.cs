using QSP.RouteFinding.Tracks.Nats;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace TrackBackupApp
{
    public class Global : HttpApplication
    {
        private readonly double refreshIntervalMin = 1.0;
        private readonly string DummyPageUrl = "/DummyPage.aspx";
        private const string DummyCacheItemKey = "dummyKey";

        private static string serverUrl;
        private static DateTime lastUpdateTimeWest = new DateTime();
        private static DateTime lastUpdateTimeEast = new DateTime();

        private void SetServerUrl()
        {
            if (serverUrl == null)
            {
                serverUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            }
        }

        private bool RegisterCacheEntry()
        {
            if (HttpContext.Current.Cache[DummyCacheItemKey] != null)
            {
                return false;
            }

            var onRemove = new CacheItemRemovedCallback(CacheItemRemovedCallback);

            HttpContext.Current.Cache.Add(DummyCacheItemKey,
                                          "Test",
                                          null,
                                          DateTime.Now.AddMinutes(refreshIntervalMin),
                                          Cache.NoSlidingExpiration,
                                          CacheItemPriority.Normal,
                                          onRemove);

            WriteToLog(3);
            return true;
        }

        public void CacheItemRemovedCallback(string key, object value,
            CacheItemRemovedReason reason)
        {
            WriteToLog(" Cache item callback, Reason: " + reason.ToString());
            DoWork();
            HitPage();
        }

        private void HitPage()
        {
            new WebClient().DownloadData(serverUrl + DummyPageUrl);
        }

        private void DoWork()
        {
            try
            {
                SaveNats();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.ToString());
            }
        }

        private void WriteToLog(int para)
        {
            var msgs = new[]
            {
                " Application is starting.",
                " Saving the NATs.",
                " Cache item callback.",
                " Cache Added."
            };

            WriteToLog(msgs[para]);
        }


        public void WriteToLog(string msg)
        {
            using (var wr = File.AppendText(HostingEnvironment.MapPath("~/log.txt")))
            {
                wr.WriteLine(DateTime.Now.ToString() + " " + msg);
            }
        }

        private void SaveNats()
        {
            var result = new NatsDownloader().DownloadFromNotam();
            Directory.CreateDirectory(HostingEnvironment.MapPath("~/nats"));

            foreach (var i in result)
            {
                if (i.Direction == NatsDirection.East)
                {
                    var filepath = "~/nats/Eastbound.xml";
                    var (success, newTime) = NatsMessage.ParseDate(i.LastUpdated);
                    if (success && newTime > lastUpdateTimeEast)
                    {
                        lastUpdateTimeEast = newTime;
                        File.WriteAllText(HostingEnvironment.MapPath(filepath),
                            i.ConvertToXml().ToString());
                    }
                }
                else
                {
                    var filepath = "~/nats/Westbound.xml";
                    var (success, newTime) = NatsMessage.ParseDate(i.LastUpdated);
                    if (success && newTime > lastUpdateTimeWest)
                    {
                        lastUpdateTimeWest = newTime;
                        File.WriteAllText(HostingEnvironment.MapPath(filepath),
                            i.ConvertToXml().ToString());
                    }
                }
            }

            WriteToLog(1);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            SetServerUrl();

            if (HttpContext.Current.Request.Url.PathAndQuery.Equals(
                DummyPageUrl, StringComparison.OrdinalIgnoreCase))
            {
                // Add the item in cache And when succesful, do the work.
                RegisterCacheEntry();
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Fires when the application is started
            SaveNats();
            RegisterCacheEntry();
            WriteToLog(0);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}