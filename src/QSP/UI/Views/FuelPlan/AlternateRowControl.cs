using QSP.UI.Controllers;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Util;
using QSP.UI.Views.Route;
using QSP.UI.Views.Route.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static CommonLibrary.AviationTools.Icao;
using Routes = QSP.RouteFinding.Routes;

namespace QSP.UI.Views.FuelPlan
{
    // TODO: is there need to dispose context menu?
    public partial class AlternateRowControl : UserControl, IAlternateRowView
    {
        private AlternateRowPresenter presenter;
        public ActionContextMenu ActionContextMenuView { get; private set; }
        private Form parentForm;

        public event EventHandler IcaoChanged;
        public string Icao { get => TrimIcao(IcaoTxtBox.Text); set => IcaoTxtBox.Text = value; }
        public IEnumerable<string> RunwayList { set => RwyComboBox.SetItems(value); }
        public string DistanceInfo { set => DisLbl.Text = value; }

        public string Route
        {
            get => RouteTxtBox.Text;
            set => RouteTxtBox.Text = value;
        }

        public string Rwy => RwyComboBox.Text;

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

            this.presenter = presenter;
            this.parentForm = parentForm;
        }

        private void SubscribeActionMenuEvents()
        {
            var a = ActionContextMenuView;
            a.FindToolStripMenuItem.Click += (s, e) => presenter.FindRoute();
            a.AnalyzeToolStripMenuItem.Click += (s, e) => presenter.AnalyzeRoute();
            a.ExportToolStripMenuItem.Click += (s, e) => presenter.ExportRouteFiles();
            a.MapToolStripMenuItem.Click += (s, e) => presenter.ShowMap();
            a.MapInBrowserToolStripMenuItem.Click += (s, e) => presenter.ShowMapBrowser();

            ActionBtn.Click += (s, e) => a.Show(ActionBtn, new Point(-100, 30));
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

        public List<string> GetSelectedProcedures() => RwyComboBox.GetSelectedProcedures().ToList();

        private void IcaoTxtBox_TextChanged(object sender, EventArgs e)
        {
            IcaoChanged.Invoke(sender, e);
        }
    }
}
