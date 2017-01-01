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
using QSP.UI.Utilities;
using static QSP.UI.Utilities.OpenFileHelper;

namespace QSP.UI.Forms.NavigationBar
{
    public partial class HelpMenu : UserControl
    {
        public HelpMenu()
        {
            InitializeComponent();
        }

        private void HelpMenu_Load(object sender, EventArgs e)
        {
            this.GetAllControls<Label>().ForEach(c => MenuController.EnableColorController(c));
        }

        private void ManualLbl_Click(object sender, EventArgs e)
        {
            TryOpenFile(Path.GetFullPath("manual/manual.html"));
        }
    }
}
