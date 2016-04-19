using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.ToLdgModule.AircraftMenu;

namespace QSP.UI.ToLdgModule.Forms
{
    public partial class QspLiteForm : Form
    {
        public QspLiteForm()
        {
            InitializeComponent();
            addControls();
        }

        private void addControls()
        {
            var acMenu = new AircraftMenuControl();

            acMenu.Location = new Point(30, 30);

            Controls.Add(acMenu);
        }
    }
}
