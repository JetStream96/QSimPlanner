using CommonLibrary.Attributes;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Util;
using QSP.UI.Views.Route;
using QSP.UI.Views.Route.Actions;
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
        private Form parentForm;

        public event EventHandler IcaoChanged;
        public string Icao { get => TrimIcao(IcaoTxtBox.Text); }
        public void SetIcao(string icao) => IcaoTxtBox.Text = icao;

        public IEnumerable<string> RunwayList { set => RwyComboBox.SetItems(value); }
        public string DistanceInfo { set => DisLbl.Text = value; }

        public string Route
        {
            get => RouteTxtBox.Text;
            set => RouteTxtBox.Text = value;
        }

        public string Rwy => RwyComboBox.Text;
        public void SetRwy(string rwy) => RwyComboBox.Text = rwy;

        public void ShowMessage(string s, MessageLevel lvl) => parentForm.ShowMessage(s, lvl);

        // TODO: Why not use default ParentForm?
        public void ShowMap(Routes.Route route)
        {
            ShowMapHelper.ShowMap(route, parentForm.Size, parentForm);
        }

        public void ShowMapBrowser(Routes.Route route)
        {
            ShowMapHelper.ShowMap(route, parentForm.Size, parentForm, true, true);
        }

        public AlternateRowControl()
        {
            InitializeComponent();
        }

        public void Init(AlternateRowPresenter presenter, Form parentForm)
        {
            ActionContextMenuView = new ActionContextMenu();

            this.Presenter = presenter;
            this.parentForm = parentForm;
        }

        private void SubscribeActionMenuEvents()
        {
            var a = ActionContextMenuView;
            a.FindToolStripMenuItem.Click += (s, e) => Presenter.FindRoute();
            a.AnalyzeToolStripMenuItem.Click += (s, e) => Presenter.AnalyzeRoute();
            a.ExportToolStripMenuItem.Click += (s, e) => Presenter.ExportRouteFiles();
            a.MapToolStripMenuItem.Click += (s, e) => Presenter.ShowMap();
            a.MapInBrowserToolStripMenuItem.Click += (s, e) => Presenter.ShowMapBrowser();

            ActionBtn.Click += (s, e) => a.Show(ActionBtn, new Point(-100, 30));
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
        public List<string> GetSelectedProcedures() => Presenter.GetAllProcedures();

        private void IcaoTxtBox_TextChanged(object sender, EventArgs e)
        {
            Presenter.UpdateRunways();
            IcaoChanged.Invoke(sender, e);
        }
    }
}
