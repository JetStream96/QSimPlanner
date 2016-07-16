using System.Windows.Forms;
using QSP.UI.Forms;
using QSP.RouteFinding.Airports;
using System;

namespace QSP.UI.UserControls
{
    public partial class AlternateRowItems : UserControl
    {
        private Func<string> icaoGetter;
        private AirportManager airportList;

        public AlternateRowItems()
        {
            InitializeComponent();
        }

        public void Init(Func<string> icaoGetter, AirportManager airportList)
        {
            this.icaoGetter = icaoGetter;
            this.airportList = airportList;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            using (var frm = new FindAltnForm())
            {
                frm.AlternateSet += (_s, _e) =>
                {
                    IcaoTxtBox.Text = frm.SelectedIcao;
                };

                frm.Init(icaoGetter(), airportList);
                frm.ShowDialog();
            }
        }
    }
}
