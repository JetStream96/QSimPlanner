using QSP.RouteFinding.Airports;
using QSP.UI.Presenters;
using QSP.UI.UserControls;
using QSP.UI.Util;
using QSP.UI.Views.Route;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QSP.UI.Presenters.FuelPlan;

namespace QSP.UI.Views.FuelPlan
{
    public partial class AlternateRowControl : UserControl, IAlternateRowView
    {
        private AlternateRowPresenter presenter;

        public AlternateRowControl()
        {
            InitializeComponent();
        }

        public string ICAO { set => IcaoTxtBox.Text = value; }
        public IEnumerable<string> RunwayList { set => RwyComboBox.SetItems(value); }
        public string DistanceInfo { set => DisLbl.Text = value; }

        public void Init(AlternateRowPresenter presenter)
        {
            this.presenter = presenter;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            using (var frm = new FindAltnForm())
            {
                frm.AlternateSet += (_s, _e) => IcaoTxtBox.Text = frm.SelectedIcao;

                var altnPresenter = presenter.FindAltnPresenter(frm);
                frm.Init(presenter.DestIcao, altnPresenter);
                frm.ShowDialog();
            }
        }
    }
}
