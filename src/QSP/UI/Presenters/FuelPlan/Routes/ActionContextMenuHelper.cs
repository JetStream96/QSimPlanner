using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.Routes;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public static class ActionContextMenuHelper
    {
        public static void ShowReports(IMessageDisplay view, List<FileExporter.Status> reports)
        {
            if (!reports.Any())
            {
                view.ShowMessage(
                    "No route file to export. Please set export settings in options page.",
                    MessageLevel.Info);
                return;
            }

            var msg = new StringBuilder();
            var success = reports.Where(r => r.Successful).ToList();

            if (success.Any())
            {
                msg.AppendLine($"{success.Count} company route(s) exported:");

                foreach (var i in success)
                {
                    msg.AppendLine(i.FilePath);
                }
            }

            var errors = reports.Where(r => !r.Successful).ToList();

            if (errors.Any())
            {
                msg.AppendLine($"\n\nFailed to export {errors.Count} file(s) into:");

                foreach (var j in errors)
                {
                    msg.AppendLine(j.FilePath);
                }
            }

            if (errors.Any(e => e.MayBePermissionIssue))
            {
                msg.AppendLine("\nYou can try to run this application " +
                    "as administrator.");
            }

            view.ShowMessage(
                msg.ToString(),
                errors.Any() ? MessageLevel.Warning : MessageLevel.Info);
        }

        public static void ShowMap(this ISupportActionContextMenu view, RouteGroup route)
        {
            if (route == null)
            {
                view.ShowMessage("Please find a route first.", MessageLevel.Info);
                return;
            }

            view.ShowMap(route.Expanded);
        }

        public static void ShowMapBrowser(this ISupportActionContextMenu view, RouteGroup route)
        {
            if (route == null)
            {
                view.ShowMessage("Please find a route first.", MessageLevel.Info);
                return;
            }

            view.ShowMapBrowser(route.Expanded);
        }

        public static void ExportRouteFiles(
            IMessageDisplay view,
            RouteGroup Route,
            IEnumerable<ExportCommand> cmds,
            AirportManager airportList)
        {
            if (Route == null)
            {
                view.ShowMessage("Please find or analyze a route first.", MessageLevel.Info);
                return;
            }

            var writer = new FileExporter(Route.Expanded, airportList, cmds);
            IEnumerable<FileExporter.Status> reports = null;

            try
            {
                reports = writer.Export();
            }
            catch (Exception ex)
            {
                view.ShowMessage(ex.Message, MessageLevel.Warning);
                return;
            }

            ShowReports(view, reports.ToList());
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
