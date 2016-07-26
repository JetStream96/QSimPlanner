using System;
using System.Diagnostics;
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
            var assembly = Assembly.GetExecutingAssembly();
            return FileVersionInfo.GetVersionInfo(assembly.Location)
                .ProductVersion;
        }
    }
}
