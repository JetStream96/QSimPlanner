using QSP.GoogleMap;
using QSP.RouteFinding.Routes;
using QSP.UI.Factories;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class ShowMapHelper
    {
        public static void ShowMap(
            RouteGroup Route, Size size, bool canAnalyze = true)
        {
            if (Route == null)
            {
                var msg = canAnalyze ?
                    "Please find or analyze a route first." :
                    "Please find a route first.";

                MsgBoxHelper.ShowWarning(msg);
                return;
            }

            var wb = new WebBrowser();
            wb.Size = size;

            var GoogleMapDrawRoute = RouteDrawing.MapDrawString(
                Route.Expanded, wb.Size.Width - 20, wb.Size.Height - 30);

            wb.DocumentText = GoogleMapDrawRoute.ToString();

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
