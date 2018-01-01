using QSP.GoogleMap;
using QSP.RouteFinding.Routes;
using QSP.UI.Views.Factories;
using QSP.Utilities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class ShowMapHelper
    {
        private const string tmpFilePath = "./tmp/route-map.html";

        public static void ShowMap(Route Route, Size size, Control parentControl,
            bool canAnalyze = true, bool openInBrowser = false)
        {
            if (Route == null)
            {
                var msg = canAnalyze ?
                    "Please find or analyze a route first." :
                    "Please find a route first.";

                parentControl.ShowWarning(msg);
                return;
            }

            Func<bool, string> getHtml = inBroser =>
            {
                // No need for size in external browser.
                var s = inBroser ? new Size(0, 0) : size;

                var GoogleMapDrawRoute = RouteDrawing.MapDrawString(
                   Route.AllWaypoints().ToList(),
                   s.Width - 20,
                   s.Height - 30);

                return GoogleMapDrawRoute.ToString();
            };

            if (openInBrowser)
            {
                ShowInBrowser(getHtml(true), parentControl);
            }
            else
            {
                ShowInForm(size, getHtml(false));
            }
        }

        private static void ShowInBrowser(string html, Control parentControl)
        {
            if (CreateTmpHtml(html))
            {
                OpenFileHelper.TryOpenFile(Path.GetFullPath(tmpFilePath));
            }
            else
            {
                MsgBoxHelper.ShowError(parentControl,
                    "Failed to write to file: " + Path.GetFullPath(tmpFilePath));
            }
        }

        // Returns whether the creation was successful.
        private static bool CreateTmpHtml(string html)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(tmpFilePath));
                File.WriteAllText(tmpFilePath, html);
                return true;
            }
            catch (Exception e)
            {
                LoggerInstance.Log(e);
                return false;
            }
        }

        private static void ShowInForm(Size size, string html)
        {
            var wb = new WebBrowser()
            {
                Size = size,
                DocumentText = html
            };

            using (var frm = FormFactory.GetForm(wb.Size))
            {
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Controls.Add(wb);
                frm.ShowDialog();
            }
        }
    }
}
