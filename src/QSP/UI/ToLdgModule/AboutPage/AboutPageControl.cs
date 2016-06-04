using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.AboutPage
{
    public partial class AboutPageControl : UserControl
    {
        public AboutPageControl()
        {
            InitializeComponent();
            initControls();
        }

        private void initControls()
        {
            panel1.BackColor = Color.FromArgb(160, Color.White);
        }
    }
}
