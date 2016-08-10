using System;
using System.Reflection;

namespace QSP.UI.Forms
{
    public partial class Splash
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            versionLbl.Text = "version " + AppProductVersion();
        }

        public static string AppProductVersion()
        {
            var ver = Assembly.GetEntryAssembly().GetName().Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }
    }
}
