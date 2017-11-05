using QSP.RouteFinding.Airports;
using QSP.UI.Presenters;
using QSP.UI.Views;
using System;
using System.Windows.Forms;
using QSP.UI.Views.Route;

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

        public void Init(Func<string> destIcaoGetter, Func<AirportManager> airportListGetter)
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

                var presenter = new FindAltnPresenter(frm, airportListGetter());
                frm.Init(destIcaoGetter(), presenter);
                frm.ShowDialog();
            }
        }
    }
}
