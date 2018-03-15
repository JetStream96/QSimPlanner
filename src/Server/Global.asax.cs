using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Nats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using static CommonLibrary.LibraryExtension.Tasks.Util;

namespace Server
{
    public class Global : HttpApplication
    {
        private readonly double RefreshIntervalSec =
#if DEBUG
            30;
#else
            60 * 5;
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

        private static List<IndividualNatsMessage> DownloadMessage()
        {
            return new NatsDownloader().DownloadFromNotam();
        }

        /// <exception cref="Exception"></exception>
        private void SaveNats()
        {
            var result = DownloadMessage();
            Directory.CreateDirectory(HostingEnvironment.MapPath(Shared.NatsDir));
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
            var filepath = Shared.EastNatsFile;
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
            var filepath = Shared.WestNatsFile;
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
            var withoutquery = rq.Url.GetLeftPart(UriPartial.Path).ToLower();
            var path = new Uri(withoutquery).PathAndQuery;

            switch (path)
            {
                case "/error-report":
                    CollectErrorReport(rq);
                    break;

                default:
                    Response.StatusCode = 400;
                    break;
            }
        }

        private void EndReq()
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void RespondWithFile(string relativePath)
        {
            try
            {
                var p = HostingEnvironment.MapPath(relativePath);

                if (File.Exists(p))
                {
                    Response.WriteFile(p);
                }
                else
                {
                    Response.StatusCode = 404;
                }
            }
            catch (Exception e)
            {
                Shared.Logger.Log(e.ToString());
                Response.StatusCode = 404;
            }
            
            EndReq();
        }

        private void RespondWithWebPage(string relativePath)
        {
            var p = HostingEnvironment.MapPath(relativePath);
            Response.Redirect(relativePath);
            EndReq();
        }

        private void RespondWithContent(string content)
        {
            Response.Write(content);
            EndReq();
        }

        private void CollectErrorReport(HttpRequest rq)
        {
            var ip = rq.UserHostAddress;
            var body = rq.Form["data"];

            if (!Shared.AntiSpam.Value.DecrementToken(ip) &&
                body.Length < ErrorReportWriter.MaxBodaySize)
            {
                Shared.ErrReportWriter.Write(ip, body);
                Shared.Logger.Log($"Error report from {ip} recorded.");
            }
            else
            {
                Shared.Logger.Log($"Error report from {ip} not recorded.");
            }

            Response.Write("OK");
            EndReq();
        }

        // @NoThrow
        private void HandleGetRequest(HttpRequest rq)
        {
            // ISS is case-insensitive.
            var pq = rq.Url.PathAndQuery.ToLower();

            if (pq == "/nats/westbound.xml")
            {
                Shared.Logger.Log("Westbound download from " + rq.UserHostAddress + ".");
                Request.ContentType = "text/xml; encoding='utf-8'";
                RespondWithFile(Shared.WestNatsFile);
            }
            else if (pq == "/nats/eastbound.xml")
            {
                Shared.Logger.Log("Eastbound download from " + rq.UserHostAddress + ".");
                Request.ContentType = "text/xml; encoding='utf-8'";
                RespondWithFile(Shared.EastNatsFile);
            }
            else if (pq == "/updates/info.xml")
            {
                Shared.Logger.Log("Update check from " + rq.UserHostAddress + ".");
                Request.ContentType = "text/xml; encoding='utf-8'";
                RespondWithFile(Shared.UpdateInfoFile);
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
            EndReq();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Fires when the application is started
            Shared.Logger.Log("Application started.");
            WebRequests.SetSecuityProtocol();

            Action saveNatsWithLock = () =>
            {
                lock (Shared.NatsFileLock)
                {
                    try
                    {
                        SaveNats();
                    }
                    catch (Exception ex)
                    {
                        Shared.Logger.Log(ex.ToString());
                    }
                }
            };

            NoAwait(() => RunPeriodicAsync(saveNatsWithLock,
                new TimeSpan(0, 0, (int)RefreshIntervalSec), new CancellationToken()));
            Shared.AntiSpam.Execute(a => a.Start());
        }

        protected void Session_Start(object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e) { }
    }
}