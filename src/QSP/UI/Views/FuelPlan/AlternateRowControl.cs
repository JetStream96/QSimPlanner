using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Util;
using QSP.UI.Views.FuelPlan.Routes;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static CommonLibrary.AviationTools.Icao;

namespace QSP.UI.Views.FuelPlan
{
    public partial class AlternateRowControl : UserControl, IAlternateRowView
    {
        private bool userEditingRoute = true;

        public AlternateRowPresenter Presenter { get; private set; }
        public ActionContextMenu ActionContextMenuView { get; private set; }

        public event EventHandler IcaoChanged;
        public string Icao { get => TrimIcao(IcaoTxtBox.Text); set => IcaoTxtBox.Text = value; }

        public IEnumerable<string> RunwayList { set => RwyComboBox.SetItems(value); }
        public string DistanceInfo { set => DisLbl.Text = value; }

        public string Route
        {
            get => RouteTxtBox.Text;
            set
            {
                userEditingRoute = false;
                RouteTxtBox.Text = value;
                userEditingRoute = true;
            }
        }

        public string Rwy { get => RwyComboBox.Text; set => RwyComboBox.Text = value; }

        public bool RouteIsValid
        {
            set
            {
                if (value)
                {
                    RouteTxtBox.ForeColor = Color.DarkGreen;
                }
                else
                {
                    DistanceInfo = "";
                    RouteTxtBox.ForeColor = Color.Gray;
                }
            }
        }

        public void ShowMessage(string s, MessageLevel lvl) => ParentForm.ShowMessage(s, lvl);

        public void ShowMap(RouteFinding.Routes.Route route)
        {
            ShowMapHelper.ShowMap(route, ParentForm.Size, ParentForm);
        }

        public void ShowMapBrowser(RouteFinding.Routes.Route route)
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

            var a = ActionContextMenuView;
            a.FindToolStripMenuItem.Click += (s, e) => presenter.FindRoute();
            a.AnalyzeToolStripMenuItem.Click += (s, e) => presenter.AnalyzeRoute();
            a.MapToolStripMenuItem.Click += (s, e) => presenter.ShowMap();
            a.MapInBrowserToolStripMenuItem.Click += (s, e) => presenter.ShowMapBrowser();
            a.ExportToolStripMenuItem.Click += (s, e) => presenter.ExportRouteFiles();

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

        /// <exception cref="Exception"></exception>
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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                ActionContextMenuView.Dispose();
            }
            base.Dispose(disposing);
        }

        private void RouteTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (userEditingRoute) RouteIsValid = false;
        }
    }
}
