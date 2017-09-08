using QSP.RouteFinding.Tracks.Nats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using static QSP.LibraryExtension.Tasks.Util;

namespace TrackBackupApp
{
    public class Global : HttpApplication
    {
        private readonly double RefreshIntervalSec = 60 * 5;
        
        private static readonly int StatsSavePeriodMs =
#if DEBUG
            10 * 1000;
#else
            10 * 60 * 1000;
#endif
        
        // @NoThrow
        public static void TryAndLogIfFail(Action a)
        {
            try
            {
                a();
            }
            catch (Exception e)
            {
                Shared.Logger.Log(e.ToString());
            }
        }

        public static string AddTimeStamp(string msg)
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

            Shared.Logger.Log(SaveNatsMsg(westUpdated, eastUpdated));
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
            var rq = HttpContext.Current.Request;

            switch (rq.HttpMethod)
            {
                case "GET":
                    HandleGetRequest(rq);
                    break;

                case "POST":
                    HandlePostRequest(rq);
                    break;

                default:
                    Response.StatusCode = 400;
                    break;
            }
        }

        // @NoThrow
        private void HandlePostRequest(HttpRequest rq)
        {
            var pq = rq.Url.PathAndQuery.ToLower();

            switch (pq)
            {
                case "/error-report":
                    CollectErrorReport(rq);
                    break;

                default:
                    Response.StatusCode = 400; 
                    break;
            }
        }

        private void RespondWithFile(string relativePath)
        {
            var p = HostingEnvironment.MapPath(relativePath);
            Response.WriteFile(p);
            Response.End();
        }

        private void RespondWithWebPage(string relativePath)
        {
            var p = HostingEnvironment.MapPath(relativePath);
            Response.Redirect(relativePath);
            Response.End();
        }

        private void RespondWithContent(string content)
        {
            Response.Write(content);
            Response.End();
        }
        
        private void CollectErrorReport(HttpRequest rq)
        {
            var ip = rq.UserHostAddress;
            var body = GetDocumentContents(rq);
            if (!Shared.AntiSpam.Value.DecrementToken(ip) &&
                body.Length < ErrorReportWriter.MaxBodaySize)
            {
                Shared.ErrReportWriter.Write(ip, body);
            }

            Response.Write("OK");
            Response.End();
        }

        private static string GetDocumentContents(HttpRequest r)
        {
            using (var receiveStream = r.InputStream)
            {
                using (var readStream = new StreamReader(receiveStream, r.ContentEncoding))
                {
                    return readStream.ReadToEnd();
                }
            }
        }

        // @NoThrow
        private void HandleGetRequest(HttpRequest rq)
        {
            // ISS is case-insensitive.
            var pq = rq.Url.PathAndQuery.ToLower();

            if (Shared.HiddenFileSet.Contains(pq))
            {
                Response.StatusCode = 403;
            }
            else if (pq == "/nats/westbound.xml")
            {
                Shared.Stats.Execute(s => s.WestboundDownloads++);
            }
            else if (pq == "/nats/eastbound.xml")
            {
                Shared.Stats.Execute(s => s.EastboundDownloads++);
            }
            else if (pq == "/updates/info.xml")
            {
                Shared.Stats.Execute(s => s.UpdateChecks++);
            }
            else if (pq == "/err")
            {
                //TODO:???
                RespondWithUnloggedErrors();
            }
        }

        private void RespondWithUnloggedErrors()
        {
            var e = Shared.UnloggedError.Value;
            Response.Write(e == "" ? "No unlogged error." : e);
            Response.End();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Fires when the application is started
            Shared.Logger.Log("Application started.");

            Action saveNatsWithLock = () =>
            {
                lock (Shared.NatsFileLock)
                {
                    SaveNats();
                }
            };

            saveNatsWithLock();
            NoAwait(() => RunPeriodicAsync(saveNatsWithLock,
                new TimeSpan(0, 0, (int)RefreshIntervalSec), new CancellationToken()));
            Shared.AntiSpam.Execute(a => a.Start());
            NoAwait(() => Stats.Helpers.SavePeriodic(Shared.Stats.Value, StatsSavePeriodMs,
                Shared.StatsFileLock));
        }
        
        protected void Session_Start(object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e) { }
    }
}