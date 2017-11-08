using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.WindAloft;
using System;
using System.Windows.Forms;
using QSP.UI.UserControls.RouteActions;
using static QSP.UI.Util.RouteDistanceDisplay;

namespace QSP.UI.Views.Route.Actions
{
    public class ActionContextMenu : ContextMenuStrip
    {
        public class ClickableToolStripMenuItem : ToolStripMenuItem, IClickable { }

        private RouteActionController controller;
        private ClickableToolStripMenuItem findToolStripMenuItem;
        private ClickableToolStripMenuItem analyzeToolStripMenuItem;
        private ClickableToolStripMenuItem mapToolStripMenuItem;
        private ClickableToolStripMenuItem mapInBrowserToolStripMenuItem;
        private ClickableToolStripMenuItem exportToolStripMenuItem;

        public RouteGroup Route => controller.Route;

        public ActionContextMenu(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
            Locator<CountryCodeCollection> checkedCodesLocator,
            Func<AvgWindCalculator> windCalcGetter,
            Label routeDisLbl,
            DistanceDisplayStyle displayStyle,
            Func<string> routeTxtGetter,
            Action<string> routeTxtSetter,
            Form parentForm) : base()
        {
            Init();

            controller = new RouteActionController(
                appOptionsLocator,
                airwayNetwork,
                origController,
                destController,
                checkedCodesLocator,
                windCalcGetter,
                routeDisLbl,
                displayStyle,
                routeTxtGetter,
                routeTxtSetter,
                findToolStripMenuItem,
                analyzeToolStripMenuItem,
                exportToolStripMenuItem,
                mapToolStripMenuItem,
                mapInBrowserToolStripMenuItem,
                parentForm);
        }

        public void Subscribe()
        {
            controller.Subscribe();
        }

        private void Init()
        {
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem = new ClickableToolStripMenuItem();
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            findToolStripMenuItem.Text = "Find Route";
            findToolStripMenuItem.Visible = true;
            // 
            // analyzeToolStripMenuItem
            // 
            analyzeToolStripMenuItem = new ClickableToolStripMenuItem();
            analyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
            analyzeToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            analyzeToolStripMenuItem.Text = "Analyze Route";
            // 
            // mapToolStripMenuItem
            // 
            mapToolStripMenuItem = new ClickableToolStripMenuItem();
            mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            mapToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            mapToolStripMenuItem.Text = "Show Map";
            // 
            // mapInBrowserToolStripMenuItem
            // 
            mapInBrowserToolStripMenuItem = new ClickableToolStripMenuItem();
            mapInBrowserToolStripMenuItem.Name = "mapInBrowserToolStripMenuItem";
            mapInBrowserToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            mapInBrowserToolStripMenuItem.Text = "Show Map (Open in browser)";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem = new ClickableToolStripMenuItem();
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            exportToolStripMenuItem.Text = "Export";
            // 
            // contextMenuStrip1
            // 
            ImageScalingSize = new System.Drawing.Size(20, 20);
            Items.AddRange(new ToolStripItem[]
            {
                findToolStripMenuItem,
                analyzeToolStripMenuItem,
                mapToolStripMenuItem,
                mapInBrowserToolStripMenuItem,
                exportToolStripMenuItem
            });

            Name = "contextMenuStrip1";
            Size = new System.Drawing.Size(182, 136);
        }
    }
}
