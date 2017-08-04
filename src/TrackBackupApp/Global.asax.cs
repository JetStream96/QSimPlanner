using QSP.RouteFinding.Tracks.Nats;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml.Linq;

namespace TrackBackupApp
{
    public class Global : HttpApplication
    {
        private readonly double refreshIntervalSec = 60 * 10;
        private readonly string dummyPageUrl = "/DummyPage.aspx";
        private const string dummyCacheItemKey = "dummyKey";

        private static readonly string configFile =
#if DEBUG 
            "~/config_debug.xml";
#else
            "~/config.xml";
#endif

        private static string serverUrl;
        private static string unloggedError = "";

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

        // @NoThrow
        public static void TryAndLogIfFail(Action a)
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

        // @NoThrow
        private void DoWork()
        {
            TryAndLogIfFail(SaveNats);
        }

        // @NoThrow
        public static void WriteToLog(string msg)
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

        private static void WriteLogFile(string msg)
        {
            using (var wr = File.AppendText(HostingEnvironment.MapPath("~/log.txt")))
            {
                wr.WriteLine(AddTimeStamp(msg));
            }
        }

        private static string AddTimeStamp(string msg)
        {
            return DateTime.UtcNow.ToString() + "  " + msg;
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
                    if (eastUpdated) LastUpdateTime.SaveEast();
                }
                else
                {
                    westUpdated = SaveWestbound(i);
                    if (westUpdated) LastUpdateTime.SaveWest();
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
            if (success && newTime > LastUpdateTime.EastUtc)
            {
                LastUpdateTime.EastUtc = newTime;
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
            if (success && newTime > LastUpdateTime.WestUtc)
            {
                LastUpdateTime.WestUtc = newTime;
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
            TryAndLogIfFail(SetServerUrlFromConfigFile);
            SaveNats();
            RegisterCacheEntry();
        }

        private static void SetServerUrlFromConfigFile()
        {
            if (serverUrl == null) ReadConfigFile();
        }

        // Returns the server url.
        private static string ReadConfigFile()
        {
            var path = HostingEnvironment.MapPath(configFile);
            var root = XDocument.Load(path).Root;
            return root.Element("ServerUrl").Value;
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