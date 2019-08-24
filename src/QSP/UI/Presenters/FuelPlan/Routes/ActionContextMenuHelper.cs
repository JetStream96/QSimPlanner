using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.Routes;
using QSP.UI.Models.FuelPlan;
using QSP.UI.UserControls;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static QSP.UI.Views.Factories.FormFactory;
using System;
using QSP.LibraryExtension;
using QSP.RouteFinding.Navaids;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public static class ActionContextMenuHelper
    {
        public static void ShowMap(this ISupportActionContextMenu view, RouteGroup route)
        {
            if (route == null)
            {
                view.ShowMessage("Please find a route first.", MessageLevel.Info);
                return;
            }

            view.ShowMap(route.Expanded);
        }
        
        public static void ExportRouteFiles(
            IMessageDisplay view,
            RouteGroup Route,
            IEnumerable<ExportCommand> cmds,
            MultiMap<string, Navaid> Navaids,
            AirportManager airportList,
            ExportMenu menu)
        {
            if (Route == null)
            {
                view.ShowMessage("Please find or analyze a route first.", MessageLevel.Info);
                return;
            }

            using (var frm = GetForm(new Size(1, 1)))
            {
                menu.Location = new Point(0, 0);
                menu.Route = Route;
                menu.Navaids = Navaids;
                menu.AirportList = airportList;

                frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                frm.AutoSize = true;
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.Controls.Add(menu);
                frm.ShowDialog();
                frm.Controls.Remove(menu);
            }
        }

        public static void ShowRouteTxt(this ISupportActionContextMenu view,
            RouteGroup route, AppOptions o)
        {
            var selected = o.ShowTrackIdOnly ? route.Folded : route.Expanded;
            var showDct = !o.HideDctInRoute;
            view.Route = selected.ToString(showDct);
            view.DistanceInfo = RouteDistanceDisplay.GetDisplay(route.Expanded, Style.Long);
        }

        public static string NonExistingAirportMsg(string icao)
        {
            return "Cannot find airport '" + icao + "'.";
        }

        public static string NonExistingWptMsg(string ident)
        {
            return "Cannot find waypoint '" + ident + "'";
        }
    }
}
