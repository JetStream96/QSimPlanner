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

            var mapHtml = RouteDrawing.MapDrawString(
               Route.AllWaypoints().ToList());

            if (!CreateTmpHtml(mapHtml))
            {
                MsgBoxHelper.ShowError(parentControl,
                    "Failed to write to file: " + Path.GetFullPath(tmpFilePath));
                return;
            }

            if (openInBrowser)
            {
                ShowInBrowser(mapHtml, parentControl);
            }
            else
            {
                ShowInForm(size, mapHtml);
            }
        }

        private static void ShowInBrowser(string html, Control parentControl)
        {
            OpenFileHelper.TryOpenFile(Path.GetFullPath(tmpFilePath));
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
            using (var frm = FormFactory.GetForm(size))
            {
                using (var wb = new WebBrowser() { Size = size })
                {
                    frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Controls.Add(wb);

                    // Use navigate instead of setting DocumentText, otherwise 
                    // relative paths in web page do not work.
                    wb.Navigate(Path.GetFullPath(tmpFilePath));
                    wb.Dock = DockStyle.Fill;

                    frm.FormBorderStyle = FormBorderStyle.Sizable;
                    frm.ShowDialog();
                }

            }
        }
    }
}
