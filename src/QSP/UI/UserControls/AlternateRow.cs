using System.Windows.Forms;
using QSP.UI.Forms;
using QSP.RouteFinding.Airports;
using System;

namespace QSP.UI.UserControls
{
    public partial class AlternateRowItems : UserControl
    {
        private Func<string> destIcaoGetter;
        private Func<AirportManager> airportListGetter;

        public AlternateRowItems()
        {
            InitializeComponent();
        }

        public void Init(Func<string> destIcaoGetter,
            Func<AirportManager> airportListGetter)
        {
            this.destIcaoGetter = destIcaoGetter;
            this.airportListGetter = airportListGetter;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            using (var frm = new FindAltnForm())
            {
                frm.AlternateSet += (_s, _e) =>
                {
                    IcaoTxtBox.Text = frm.SelectedIcao;
                };

                frm.Init(destIcaoGetter(), airportListGetter());
                frm.ShowDialog();
            }
        }
    }
}
