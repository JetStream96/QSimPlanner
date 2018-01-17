using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using QSP.UI.Util;

namespace QSP.UI.Views
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
        
        private void LicenseBtnClick(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile(Path.GetFullPath("LICENSE.txt"), this);
        }

        private void SiteBtnClick(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile("https://qsimplan.wordpress.com/", this);
        }

        private void GithubBtnClick(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile("https://github.com/JetStream96/QSimPlanner", this);
        }

        private void ChangelogBtnClick(object sender, EventArgs e)
        {
            OpenFileHelper.TryOpenFile(Path.GetFullPath("ChangeLog.txt"), this);
        }
    }
}
