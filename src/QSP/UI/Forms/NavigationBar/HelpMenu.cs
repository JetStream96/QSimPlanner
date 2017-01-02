using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.UI.ToLdgModule.AboutPage;
using QSP.UI.Utilities;
using static QSP.UI.Utilities.OpenFileHelper;

namespace QSP.UI.Forms.NavigationBar
{
    public partial class HelpMenu : UserControl
    {
        private AboutPageControl about;
        private Action hideAll;

        public HelpMenu()
        {
            InitializeComponent();
        }

        public void Init(AboutPageControl about, Action hideAll)
        {
            this.about = about;
            this.hideAll = hideAll;
        }

        private void HelpMenu_Load(object sender, EventArgs e)
        {
            this.GetAllControls<Label>().ForEach(c => MenuController.EnableColorController(c));
        }

        private void ManualLbl_Click(object sender, EventArgs e)
        {
            TryOpenFile(Path.GetFullPath("manual/manual.html"));
        }

        private void AboutLbl_Click(object sender, EventArgs e)
        {
            hideAll();
            about.Show();
        }
    }
}
