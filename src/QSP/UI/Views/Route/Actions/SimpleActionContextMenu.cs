using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Views.Route.Actions
{
    public class SimpleActionContextMenu : ContextMenuStrip
    {
        public ToolStripMenuItem FindToolStripMenuItem { get; private set; }
        public ToolStripMenuItem MapToolStripMenuItem { get; private set; }

        public SimpleActionContextMenu() : base()
        {
            Init();
        }
        
        private void Init()
        {
            // 
            // findToolStripMenuItem
            // 
            FindToolStripMenuItem = new ToolStripMenuItem();
            FindToolStripMenuItem.Name = "findToolStripMenuItem";
            FindToolStripMenuItem.Size = new Size(181, 26);
            FindToolStripMenuItem.Text = "Find Route";
            FindToolStripMenuItem.Visible = true;            
            // 
            // mapToolStripMenuItem
            // 
            MapToolStripMenuItem = new ToolStripMenuItem();
            MapToolStripMenuItem.Name = "mapToolStripMenuItem";
            MapToolStripMenuItem.Size = new Size(181, 26);
            MapToolStripMenuItem.Text = "Show Map";
            // 
            // contextMenuStrip1
            // 
            ImageScalingSize = new Size(20, 20);
            Items.AddRange(new ToolStripItem[] {
            FindToolStripMenuItem,
            MapToolStripMenuItem});
            Name = "contextMenuStrip1";
            Size = new Size(182, 136);
        }
    }
}
