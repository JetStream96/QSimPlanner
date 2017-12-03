using CommonLibrary.Attributes;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Util;
using QSP.UI.Views.FuelPlan.Route;
using QSP.UI.Views.FuelPlan.Route.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static CommonLibrary.AviationTools.Icao;
using Routes = QSP.RouteFinding.Routes;

namespace QSP.UI.Views.FuelPlan
{
    // TODO: is there need to dispose context menu?
    public partial class AlternateRowControl : UserControl, IAlternateRowView
    {
        public AlternateRowPresenter Presenter { get; private set; }
        public ActionContextMenu ActionContextMenuView { get; private set; }

        public event EventHandler IcaoChanged;
        public string Icao { get => TrimIcao(IcaoTxtBox.Text); set => IcaoTxtBox.Text = value; }

        public IEnumerable<string> RunwayList { set => RwyComboBox.SetItems(value); }
        public string DistanceInfo { set => DisLbl.Text = value; }

        public string Route
        {
            get => RouteTxtBox.Text;
            set => RouteTxtBox.Text = value;
        }

        public string Rwy { get => RwyComboBox.Text; set => RwyComboBox.Text = value; }

        public void ShowMessage(string s, MessageLevel lvl) => ParentForm.ShowMessage(s, lvl);

        // TODO: Why not use default ParentForm?
        public void ShowMap(Routes.Route route)
        {
            ShowMapHelper.ShowMap(route, ParentForm.Size, ParentForm);
        }

        public void ShowMapBrowser(Routes.Route route)
        {
            ShowMapHelper.ShowMap(route, ParentForm.Size, ParentForm, true, true);
        }

        public AlternateRowControl()
        {
            InitializeComponent();
        }

        public void Init(AlternateRowPresenter presenter)
        {
            ActionContextMenuView = new ActionContextMenu();
            ActionContextMenuView.Init(presenter.ContextMenuPresenter);
            this.Presenter = presenter;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            using (var frm = new FindAltnForm())
            {
                frm.AlternateSet += (_s, _e) => IcaoTxtBox.Text = frm.SelectedIcao;

                var altnPresenter = Presenter.FindAltnPresenter(frm);
                frm.Init(Presenter.DestIcao, altnPresenter);
                frm.ShowDialog();
            }
        }

        [Throws]
        public IEnumerable<string> GetSelectedProcedures() => Presenter.GetAllProcedures();

        private void IcaoTxtBox_TextChanged(object sender, EventArgs e)
        {
            Presenter.UpdateRunways();
            IcaoChanged.Invoke(sender, e);
        }

        private void ActionBtn_Click(object sender, EventArgs e)
        {
            ActionContextMenuView.Show(ActionBtn, new Point(0, ActionBtn.Height));
        }
    }
}
