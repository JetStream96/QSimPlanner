using System.Reflection;

namespace QSP.UI.Views
{
    public partial class Splash
    {
        public Splash()
        {
            InitializeComponent();
        }

        public void ShowVersion()
        {
            SmallTitleLbl.Text = "version " + AppProductVersion();
        }

        public static string AppProductVersion()
        {
            var ver = Assembly.GetEntryAssembly().GetName().Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }
    }
}
