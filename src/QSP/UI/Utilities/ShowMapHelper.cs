using QSP.GoogleMap;
using QSP.RouteFinding.Routes;
using QSP.UI.Factories;
using QSP.UI.MsgBox;
using QSP.Utilities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Utilities
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

            // TODO: Do we need size in external browser?
            var GoogleMapDrawRoute = RouteDrawing.MapDrawString(
                Route.AllWaypoints().ToList(),
                size.Width - 20,
                size.Height - 30);

            var html = GoogleMapDrawRoute.ToString();

            if (openInBrowser)
            {
                ShowInBrowser(html, parentControl);
            }
            else
            {
                ShowInForm(size, html);
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
