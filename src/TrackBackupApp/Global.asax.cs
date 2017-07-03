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
        private readonly double refreshIntervalSec = 60 * 10;
        private readonly string dummyPageUrl = "/DummyPage.aspx";
        private const string dummyCacheItemKey = "dummyKey";

        private static string serverUrl;
        private static string unloggedError = "";
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
            if (HttpContext.Current.Cache[dummyCacheItemKey] != null)
            {
                return false;
            }

            var onRemove = new CacheItemRemovedCallback(CacheItemRemovedCallback);

            HttpContext.Current.Cache.Add(dummyCacheItemKey,
                                          "Test",
                                          null,
                                          DateTime.Now.AddSeconds(refreshIntervalSec),
                                          Cache.NoSlidingExpiration,
                                          CacheItemPriority.Normal,
                                          onRemove);

            WriteToLog("Cache Added.");
            return true;
        }

        public void CacheItemRemovedCallback(string key, object value,
            CacheItemRemovedReason reason)
        {
            WriteToLog("Cache item callback, Reason: " + reason.ToString());
            DoWork();
            TryAndLogIfFail(HitPage);
        }

        // Does not throw exception.
        private void TryAndLogIfFail(Action a)
        {
            try
            {
                a();
            }
            catch (Exception e)
            {
                WriteToLog(e.ToString());
            }
        }

        private void HitPage()
        {
            new WebClient().DownloadData(serverUrl + dummyPageUrl);
        }

        // Does not throw exception.
        private void DoWork()
        {
            TryAndLogIfFail(SaveNats);
        }

        // Does not throw exception.
        public void WriteToLog(string msg)
        {
            try
            {
                WriteLogFile(msg);
            }
            catch (Exception e)
            {
                unloggedError += AddTimeStamp(msg) + "\n" + AddTimeStamp(e.ToString());
            }
        }

        private void WriteLogFile(string msg)
        {
            using (var wr = File.AppendText(HostingEnvironment.MapPath("~/log.txt")))
            {
                wr.WriteLine(AddTimeStamp(msg));
            }
        }

        private string AddTimeStamp(string msg)
        {
            return DateTime.Now.ToString() + "  " + msg;
        }

        private void SaveNats()
        {
            var result = new NatsDownloader().DownloadFromNotam();
            Directory.CreateDirectory(HostingEnvironment.MapPath("~/nats"));
            bool westUpdated = false;
            bool eastUpdated = false;

            foreach (var i in result)
            {
                if (i.Direction == NatsDirection.East)
                {
                    eastUpdated = SaveEastbound(i);
                }
                else
                {
                    westUpdated = SaveWestbound(i);
                }
            }

            WriteToLog(SaveNatsMsg(westUpdated, eastUpdated));
        }

        // Test if the eastbound track needs to be saved. If yes, saved the file and 
        // return true. Otherwise, returns false.
        private bool SaveEastbound(IndividualNatsMessage i)
        {
            var filepath = "~/nats/Eastbound.xml";
            var (success, newTime) = NatsMessage.ParseDate(i.LastUpdated);
            if (success && newTime > lastUpdateTimeEast)
            {
                lastUpdateTimeEast = newTime;
                File.WriteAllText(HostingEnvironment.MapPath(filepath),
                    i.ConvertToXml().ToString());
                return true;
            }

            return false;
        }

        private bool SaveWestbound(IndividualNatsMessage i)
        {
            var filepath = "~/nats/Westbound.xml";
            var (success, newTime) = NatsMessage.ParseDate(i.LastUpdated);
            if (success && newTime > lastUpdateTimeWest)
            {
                lastUpdateTimeWest = newTime;
                File.WriteAllText(HostingEnvironment.MapPath(filepath),
                    i.ConvertToXml().ToString());
                return true;
            }

            return false;
        }

        private string SaveNatsMsg(bool westUpdated, bool eastUpdated)
        {
            if (westUpdated)
            {
                return eastUpdated ? "Both directions updated." : "Westbound updated.";
            }

            return eastUpdated ? "Eastbound updated." : "Neither direction updated.";
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            SetServerUrl();

            var pq = HttpContext.Current.Request.Url.PathAndQuery.ToLower();

            if (pq == dummyPageUrl.ToLower())
            {
                // Add the item in cache and when succesful, do the work.
                RegisterCacheEntry();
            }
            else if (pq == "/unloggederr")
            {
                RespondWithUnloggedErrors();
            }
        }

        private void RespondWithUnloggedErrors()
        {
            Response.Write(unloggedError == "" ? "No unlogged error." : unloggedError);
            Response.End();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Fires when the application is started
            WriteToLog("Application started.");
            SaveNats();
            RegisterCacheEntry();
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