using Gecko;
using Gecko.IO;
using QSP.GoogleMap;
using QSP.RouteFinding.Routes;
using QSP.UI.Views.Factories;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class ShowMapHelper
    {
        public static void ShowMap(Route Route, Size size, Control parentControl,
            bool canAnalyze = true)
        {
            if (Route == null)
            {
                var msg = canAnalyze ?
                    "Please find or analyze a route first." :
                    "Please find a route first.";

                parentControl.ShowWarning(msg);
                return;
            }

            var data = RouteDrawing.GetPostData(Route.AllWaypoints().ToList());
            ShowInForm(size, "https://qsimplanner.azurewebsites.net/map/route", data);
        }

        private static void ShowInForm(Size size, string url, string postQuery)
        {
            using (var frm = FormFactory.GetForm(size))
            {
                if (!Xpcom.IsInitialized)
                {
                    Xpcom.Initialize("Firefox");
                }

                using (var wb = new GeckoWebBrowser())
                {
                    frm.ShowIcon = false;
                    frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Controls.Add(wb);

                    wb.Dock = DockStyle.Fill;
                    var postData = MimeInputStream.Create();
                    postData.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    postData.AddContentLength = true;
                    postData.SetData(postQuery);

                    wb.Navigate(url, GeckoLoadFlags.BypassCache, "", postData);

                    frm.FormBorderStyle = FormBorderStyle.Sizable;
                    frm.ShowDialog();
                }
            }
        }
    }
}
