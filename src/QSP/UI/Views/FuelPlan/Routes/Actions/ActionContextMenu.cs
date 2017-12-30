using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan.Routes.Actions
{
    // Passive view.
    public class ActionContextMenu : ContextMenuStrip
    {
        public class ClickableToolStripMenuItem : ToolStripMenuItem, IClickable { }

        public ClickableToolStripMenuItem FindToolStripMenuItem;
        public ClickableToolStripMenuItem AnalyzeToolStripMenuItem;
        public ClickableToolStripMenuItem MapToolStripMenuItem;
        public ClickableToolStripMenuItem MapInBrowserToolStripMenuItem;
        public ClickableToolStripMenuItem ExportToolStripMenuItem;

        public ActionContextMenu() : base()
        {
            InitControls();
        }

        private void InitControls()
        {
            // 
            // findToolStripMenuItem
            // 
            FindToolStripMenuItem = new ClickableToolStripMenuItem();
            FindToolStripMenuItem.Name = "findToolStripMenuItem";
            FindToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            FindToolStripMenuItem.Text = "Find Route";
            FindToolStripMenuItem.Visible = true;
            // 
            // analyzeToolStripMenuItem
            // 
            AnalyzeToolStripMenuItem = new ClickableToolStripMenuItem();
            AnalyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
            AnalyzeToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            AnalyzeToolStripMenuItem.Text = "Analyze Route";
            // 
            // mapToolStripMenuItem
            // 
            MapToolStripMenuItem = new ClickableToolStripMenuItem();
            MapToolStripMenuItem.Name = "mapToolStripMenuItem";
            MapToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            MapToolStripMenuItem.Text = "Show Map";
            // 
            // mapInBrowserToolStripMenuItem
            // 
            MapInBrowserToolStripMenuItem = new ClickableToolStripMenuItem();
            MapInBrowserToolStripMenuItem.Name = "mapInBrowserToolStripMenuItem";
            MapInBrowserToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            MapInBrowserToolStripMenuItem.Text = "Show Map (Open in browser)";
            // 
            // exportToolStripMenuItem
            // 
            ExportToolStripMenuItem = new ClickableToolStripMenuItem();
            ExportToolStripMenuItem.Name = "exportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // contextMenuStrip1
            // 
            ImageScalingSize = new System.Drawing.Size(20, 20);
            Items.AddRange(new ToolStripItem[]
            {
                FindToolStripMenuItem,
                AnalyzeToolStripMenuItem,
                MapToolStripMenuItem,
                MapInBrowserToolStripMenuItem,
                ExportToolStripMenuItem
            });

            Name = "contextMenuStrip1";
            Size = new System.Drawing.Size(182, 136);
        }
    }
}
