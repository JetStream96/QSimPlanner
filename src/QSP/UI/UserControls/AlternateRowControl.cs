using QSP.RouteFinding.Airports;
using QSP.UI.Presenters;
using System;
using System.Windows.Forms;
using QSP.UI.Views.Route;
using System.Collections.Generic;
using QSP.UI.Util;

namespace QSP.UI.UserControls
{
    public partial class AlternateRowControl : UserControl, IAlternateRowView
    {
        private AlternateRowPresenter presenter;
        private Func<string> destIcaoGetter;
        private Func<AirportManager> airportListGetter;

        public AlternateRowControl()
        {
            InitializeComponent();
        }

        public string ICAO { set => IcaoTxtBox.Text = value; }
        public IEnumerable<string> RunwayList { set => RwyComboBox.SetItems(value); }
        public string DistanceInfo { set => DisLbl.Text = value; }

        public void Init(Func<string> destIcaoGetter, Func<AirportManager> airportListGetter,
            AlternateRowPresenter presenter)
        {
            this.destIcaoGetter = destIcaoGetter;
            this.airportListGetter = airportListGetter;
            this.presenter = presenter;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            using (var frm = new FindAltnForm())
            {
                frm.AlternateSet += (_s, _e) => IcaoTxtBox.Text = frm.SelectedIcao;

                var presenter = new FindAltnPresenter(frm, airportListGetter());
                frm.Init(destIcaoGetter(), presenter);
                frm.ShowDialog();
            }
        }
    }
}
