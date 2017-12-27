using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.Forms
{
    public partial class LicenseForm : Form
    {
        public bool Agreed { get; private set; } = false;

        public LicenseForm()
        {
            InitializeComponent();
        }

        private void agreeBtn_Click(object sender, EventArgs e)
        {
            Agreed = true;
        }
    }
}
