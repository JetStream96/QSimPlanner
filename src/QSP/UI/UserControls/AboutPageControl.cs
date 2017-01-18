using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using QSP.UI.Utilities;

namespace QSP.UI.UserControls
{
    public partial class AboutPageControl : UserControl
    {
        public AboutPageControl()
        {
            InitializeComponent();
        }

        public void Init(string appName)
        {
            appNameLbl.Text = appName;

            var ver = Assembly.GetEntryAssembly().GetName().Version;
            versionLbl.Text = $"v{ver.Major}.{ver.Minor}.{ver.Build}";
            DoubleBufferUtil.SetDoubleBuffered(tableLayoutPanel3);
            tableLayoutPanel3.BackColor = Color.FromArgb(148, 255, 255, 255);
        }
        
        private void licenseBtn_Click(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile(Path.GetFullPath("LICENSE.txt"), this);
        }

        private void siteBtn_Click(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile("https://qsimplan.wordpress.com/", this);
        }

        private void githubBtn_Click(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile("https://github.com/JetStream96/QSimPlanner", this);
        }
    }
}
