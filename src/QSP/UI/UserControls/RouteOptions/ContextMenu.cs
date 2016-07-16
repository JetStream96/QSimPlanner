using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.UI.Controllers;
using QSP.UI.Controls;
using System;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.UserControls.RouteOptions
{
    public class ContextMenu : ContextMenuStrip
    {
        public class ClickableToolStripMenuItem : ToolStripItem, IClickable { }

        private RouteOptionController controller;
        private ClickableToolStripMenuItem findToolStripMenuItem;
        private ClickableToolStripMenuItem analyzeToolStripMenuItem;
        private ClickableToolStripMenuItem mapToolStripMenuItem;
        private ClickableToolStripMenuItem exportToolStripMenuItem;

        public RouteGroup Route { get { return controller.Route; } }

        public ContextMenu(
            AppOptions appSettings,
            WaypointList wptList,
            AirportManager airportList,
            TrackInUseCollection tracksInUse,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
            Label routeDisLbl,
            DistanceDisplayStyle displayStyle,
            Func<string> routeTxtGetter,
            Action<string> routeTxtSetter) : base()
        {
            controller = new RouteOptionController(
                  appSettings,
                  wptList,
                  airportList,
                  tracksInUse,
                  origController,
                  destController,
                  routeDisLbl,
                  displayStyle,
                  routeTxtGetter,
                  routeTxtSetter,
                  findToolStripMenuItem,
                  analyzeToolStripMenuItem,
                  exportToolStripMenuItem);

            Init();
        }

        public void Subscribe()
        {
            controller.Subscribe();
        }

        private void Init()
        {
            // 
            // contextMenuStrip1
            // 
            ImageScalingSize = new System.Drawing.Size(20, 20);
            Items.AddRange(new ToolStripItem[] {
            findToolStripMenuItem,
            analyzeToolStripMenuItem,
            mapToolStripMenuItem,
            exportToolStripMenuItem});
            Name = "contextMenuStrip1";
            Size = new System.Drawing.Size(182, 136);
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem = new ClickableToolStripMenuItem();
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            findToolStripMenuItem.Text = "Find";
            // 
            // analyzeToolStripMenuItem
            // 
            analyzeToolStripMenuItem = new ClickableToolStripMenuItem();
            analyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
            analyzeToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            analyzeToolStripMenuItem.Text = "Analyze";
            // 
            // mapToolStripMenuItem
            // 
            mapToolStripMenuItem = new ClickableToolStripMenuItem();
            mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            mapToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            mapToolStripMenuItem.Text = "Map";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem = new ClickableToolStripMenuItem();
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            exportToolStripMenuItem.Text = "Export";
        }
    }
}
