using QSP.RouteFinding.Routes;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Presenters.FuelPlan.Routes;
using QSP.UI.UserControls;
using QSP.UI.Util;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan.Routes
{
    // This is a general control used in both (A) main fuel calculation page and 
    // (B) advanced route finder. The differences includes:
    // 
    // 1. Among the 4 options, only airport to airport is supported in A.
    // 2. For B, the route export is enabled only when the selection option is 
    //    airport to airport.
    // 3. The fuel required to alternate is not considered in B. This affects the planned
    //    landing weight at destination and A and B can generate different routes  
    //    when wind-optimized route is enabled.

    public partial class RouteFinderControl : UserControl, IRouteFinderView
    {
        public event EventHandler OrigIcaoChanged;
        public event EventHandler DestIcaoChanged;

        private RouteFinderPresenter presenter;
        private RouteFinderModel model;
        private ActionContextMenu actionMenu;
        private RouteOptionContextMenu optionMenu;

        private bool userEditingRoute = true;

        public RouteFinderControl()
        {
            InitializeComponent();
        }

        public string OrigIcao => origRow.Icao;
        public string DestIcao => destRow.Icao;
        public string OrigRwy => origRow.Rwy;
        public string DestRwy => destRow.Rwy;
        public RouteGroup RouteGroup => presenter.Route;

        // For alternate calculations.
        public DestinationSidSelection DestSidProvider { get; private set; }

        /// <summary>
        /// This is only used is displaying maps.
        /// </summary>
        public Form MainForm { get; private set; }

        public void Init(RouteFinderModel model, Form MainForm, ExportMenu exportMenu)
        {
            this.model = model;
            this.MainForm = MainForm;
            this.presenter = new RouteFinderPresenter(this, model, exportMenu);

            InitOrigDestControls();
            SetRouteActionMenu();
            SetRouteOptionMenu();
            routeRichTxtBox.UpperCaseOnly();
            routeSummaryLbl.Text = "";
        }

        /// <summary>
        /// This value represents whether the user is able to select waypoint
        /// as origin or destination.
        /// </summary>
        public bool WaypointOptionEnabled
        {
            get => origRow.WaypointOptionEnabled;

            set
            {
                origRow.WaypointOptionEnabled = value;
                destRow.WaypointOptionEnabled = value;

                SuspendLayout();
                tableLayoutPanel1.Controls.Remove(destRow);

                if (value)
                {
                    tableLayoutPanel1.Controls.Add(destRow, 0, 1);
                }
                else
                {
                    tableLayoutPanel1.Controls.Add(destRow, 1, 0);
                }

                ResumeLayout();
            }
        }

        public string DistanceInfo { set => routeSummaryLbl.Text = value; }

        public string Route
        {
            get => routeRichTxtBox.Text;
            set
            {
                userEditingRoute = false;
                routeRichTxtBox.Text = value;
                userEditingRoute = true;
            }
        }

        public IRouteFinderRowView OrigRow => origRow;
        public IRouteFinderRowView DestRow => destRow;

        private void SetRouteOptionMenu()
        {
            var m = model.FuelPlanningModel;

            optionMenu = new RouteOptionContextMenu(m.CheckedCountryCodes,
                m.CountryCodeManager);

            optionMenu.Subscribe();
            routeOptionBtn.Click += (s, e) =>
                optionMenu.Show(routeOptionBtn, new Point(0, routeOptionBtn.Height));
        }

        private void SetRouteActionMenu()
        {
            actionMenu = new ActionContextMenu();
            InitActionMenu();

            showRouteActionsBtn.Click += (s, e) =>
            {
                // Some buttons should be unavilable if origin or destination is an airport.
                var showBtns = this.IsAirportToAirport();
                actionMenu.AnalyzeToolStripMenuItem.Visible = showBtns;
                actionMenu.ExportToolStripMenuItem.Visible = showBtns;

                actionMenu.Show(showRouteActionsBtn, new Point(0, showRouteActionsBtn.Height));
            };
        }

        private void InitOrigDestControls()
        {
            var m = model.FuelPlanningModel;
            origRow.Init(m.ToIFinderOptionModel(true));
            destRow.Init(m.ToIFinderOptionModel(false));

            DestSidProvider = new DestinationSidSelection(
                m.AirwayNetwork, m.AppOption, destRow.OptionView);

            origRow.IcaoChanged += (s, e) => OrigIcaoChanged?.Invoke(s, e);
            destRow.IcaoChanged += (s, e) => DestIcaoChanged?.Invoke(s, e);
        }

        private void InitActionMenu()
        {
            var a = actionMenu;
            a.FindToolStripMenuItem.Click += (s, e) => presenter.FindRoute();
            a.AnalyzeToolStripMenuItem.Click += (s, e) => presenter.AnalyzeRoute();
            a.MapToolStripMenuItem.Click += (s, e) => presenter.ShowMap();
            a.MapInBrowserToolStripMenuItem.Click += (s, e) => presenter.ShowMapBrowser();
            a.ExportToolStripMenuItem.Click += (s, e) => presenter.ExportRouteFiles();
        }

        public void ShowMap(Route route) =>
           ShowMapHelper.ShowMap(route, MainForm.Size, MainForm);

        public void ShowMapBrowser(Route route) =>
            ShowMapHelper.ShowMap(route, MainForm.Size, MainForm, true, true);

        public void ShowMessage(string s, MessageLevel lvl) => ParentForm.ShowMessage(s, lvl);

        public void OnNavDataChange() => presenter.OnNavDataChange();

        public bool RouteIsValid
        {
            set
            {
                // This flag is required here because editing the ForeColor of 
                // RichTextBox triggers the TextChanged event as well.
                userEditingRoute = false;

                if (value)
                {                    
                    routeRichTxtBox.ForeColor = Color.DarkGreen;
                    routeStatusLbl.ForeColor = Color.DarkGreen;
                    routeStatusLbl.Text = "✓ This route is valid.";
                }
                else
                {
                    DistanceInfo = "";
                    routeRichTxtBox.ForeColor = Color.Gray;
                    routeStatusLbl.ForeColor = Color.Gray;
                    routeStatusLbl.Text = "This route has not been analyzed.";
                }

                userEditingRoute = true;
            }
        }

        private void routeRichTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (userEditingRoute) RouteIsValid = false;
        }
    }
}
