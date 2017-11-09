using QSP.Utilities;

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
            SmallTitleLbl.Text = "version " + Version.AppProductVersion();
        }
    }
}
