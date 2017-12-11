using CommonLibrary.LibraryExtension;
using QSP.AviationTools.Coordinates;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Finder;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Presenters.FuelPlan.Routes;
using QSP.UI.Util;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using QSP.WindAloft;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static QSP.UI.Views.Factories.ToolTipFactory;

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

        /*
        private ControlGroup fromGroup;
        private ControlGroup toGroup;
        private SimpleActionContextMenu routeActionMenu;
        private RouteOptionContextMenu routeOptionMenu;

        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private Locator<CountryCodeManager> countryCodeLocator;
        private Locator<CountryCodeCollection> checkedCodesLocator;
        private ProcedureFilter procFilter;
        private Func<AvgWindCalculator> windCalcGetter;
        private RouteGroup Route;

        private WaypointList wptList => airwayNetwork.WptList;routed
        private AirportManager airportList => airwayNetwork.AirportList;
        private TrackInUseCollection tracksInUse => airwayNetwork.TracksInUse;
        */

        private RouteFinderPresenter presenter;
        private IRouteFinderModel model;
        private ActionContextMenu actionMenu;
        private RouteOptionContextMenu optionMenu;

        public ActionContextMenuPresenter ActionMenuPresenter => actionMenu.Presenter;

        public RouteFinderControl()
        {
            InitializeComponent();
        }

        public string OrigIcao => origRow.Icao;
        public string DestIcao => destRow.Icao;
        public string OrigRwy => origRow.Rwy;
        public string DestRwy => destRow.Rwy;
        public RouteGroup RouteGroup => actionMenu.Presenter.Route;

        // For alternate calculations.
        public DestinationSidSelection DestSidProvider { get; private set; }

        public void Init(IRouteFinderModel model)
        {
            this.model = model;
            this.presenter = new RouteFinderPresenter(this, model);

            InitOrigDestControls();
            SetRouteActionMenu();
            SetRouteOptionMenu();
            routeRichTxtBox.UpperCaseOnly();
            routeSummaryLbl.Text = "";
        }

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
        public string Route { get => routeRichTxtBox.Text; set => routeRichTxtBox.Text = value; }

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
            var m = model.FuelPlanningModel;
            var p = new ActionContextMenuPresenter(
                this,
                m.AppOption,
                m.AirwayNetwork,
                origRow.OptionControl.Presenter,
                destRow.OptionControl.Presenter,
                m.CheckedCountryCodes,
                model.WindCalc);

            actionMenu = new ActionContextMenu();
            actionMenu.Init(p);

            showRouteActionsBtn.Click += (s, e) =>
               actionMenu.Show(showRouteActionsBtn, new Point(0, showRouteActionsBtn.Height));
        }

        private void InitOrigDestControls()
        {
            var m = model.FuelPlanningModel;
            origRow.Init(m.ToIFinderOptionModel(true), this);
            destRow.Init(m.ToIFinderOptionModel(false), this);

            DestSidProvider = new DestinationSidSelection(
                m.AirwayNetwork, m.AppOption, destRow.OptionView);

            origRow.IcaoChanged += (s, e) => OrigIcaoChanged?.Invoke(s, e);
            destRow.IcaoChanged += (s, e) => DestIcaoChanged?.Invoke(s, e);
        }

        public void ShowMap(Route route) =>
           ShowMapHelper.ShowMap(route, ParentForm.Size, ParentForm);

        public void ShowMapBrowser(Route route) =>
            ShowMapHelper.ShowMap(route, ParentForm.Size, ParentForm, true, true);

        public void ShowMessage(string s, MessageLevel lvl) => ParentForm.ShowMessage(s, lvl);

        public void OnNavDataChange()
        {
            origRow.OnNavDataChange();
            destRow.OnNavDataChange();
        }

        /*
        public void Init(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            Locator<CountryCodeManager> countryCodeLocator,
            Locator<CountryCodeCollection> checkedCodesLocator,
            ProcedureFilter procFilter,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.countryCodeLocator = countryCodeLocator;
            this.checkedCodesLocator = checkedCodesLocator;
            this.procFilter = procFilter;
            this.windCalcGetter = windCalcGetter;

            SetBtnDisabledStyle();
            SetControlGroups();
            attachEventHandlers();
            SetDefaultState();
            AddToolTip();
            SetRouteOptionControl();
            SetRouteActionControl();
        }

        private void SetRouteOptionControl()
        {
            routeOptionMenu = new RouteOptionContextMenu(
                checkedCodesLocator, countryCodeLocator);

            routeOptionMenu.Subscribe();
            routeOptionBtn.Click += (s, e) =>
            routeOptionMenu.Show(
                routeOptionBtn, new Point(0, routeOptionBtn.Height));
        }

        private void SetRouteActionControl()
        {
            routeActionMenu = new SimpleActionContextMenu();
            routeActionMenu.FindToolStripMenuItem.Click += FindRouteBtnClick;
            routeActionMenu.MapToolStripMenuItem.Click += (s, e) =>
                ShowMapHelper.ShowMap(
                    Route.Expanded,
                    ParentForm.Owner.Size,
                    this,
                    false);

            showRouteActionsBtn.Click += (s, e) =>
            routeActionMenu.Show(
                showRouteActionsBtn, new Point(0, showRouteActionsBtn.Height));
        }

        private void SetBtnDisabledStyle()
        {
            var gray = Color.FromArgb(224, 224, 224);

            var style = new ControlDisableStyleController.ColorStyle(
                Color.DarkSlateGray,
                gray,
                Color.White,
                gray);

            new ControlDisableStyleController(filterSidBtn, style).Activate();
            new ControlDisableStyleController(filterStarBtn, style).Activate();
        }

        private void SetControlGroups()
        {
            fromGroup = new ControlGroup(
                this,
                fromTypeComboBox,
                fromIdentLbl,
                fromIdentTxtBox,
                fromRwyLbl,
                fromRwyComboBox,
                sidLbl,
                sidComboBox,
                fromWptLbl,
                fromWptComboBox,
                true,
                procFilter,
                filterSidBtn);

            toGroup = new ControlGroup(
                this,
                toTypeComboBox,
                toIdentLbl,
                toIdentTxtBox,
                toRwyLbl,
                toRwyComboBox,
                starLbl,
                starComboBox,
                toWptLbl,
                toWptComboBox,
                false,
                procFilter,
                filterStarBtn);
        }

        public void OnWptListChanged()
        {
            fromGroup.RefreshDisplay();
            toGroup.RefreshDisplay();
        }

        private void attachEventHandlers()
        {
            fromGroup.Subsribe();
            toGroup.Subsribe();
        }

        private void SetDefaultState()
        {
            InitTypes(fromTypeComboBox);
            InitTypes(toTypeComboBox);
            routeSummaryLbl.Text = "";
        }

        private void InitTypes(ComboBox cbox)
        {
            var items = cbox.Items;
            items.Clear();
            items.AddRange(new[] { "Airport", "Waypoint" });
            cbox.SelectedIndex = 0;
        }

        private void AddToolTip()
        {
            var tp = GetToolTip();
            tp.SetToolTip(filterSidBtn, "SID filter");
            tp.SetToolTip(filterStarBtn, "STAR filter");
        }

        private void FindRouteBtnClick(object sender, EventArgs e)
        {
            try
            {
                if (fromTypeComboBox.SelectedIndex == 0)
                {
                    if (toTypeComboBox.SelectedIndex == 0)
                    {
                        GetRouteAirportToAirport();
                    }
                    else
                    {
                        GetRouteAirportToWaypoint();
                    }
                }
                else
                {
                    if (toTypeComboBox.SelectedIndex == 0)
                    {
                        GetRouteWaypointToAirport();
                    }
                    else
                    {
                        GetRouteWaypointToWaypoint();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowWarning(ex.Message);
            }
        }

        private void GetRouteWaypointToWaypoint()
        {
            Route = new RouteGroup(
                new RouteFinder(wptList, checkedCodesLocator.Instance)
                .FindRoute(GetWptIndexFrom(), GetWptIndexTo()),
                tracksInUse);

            ShowRoute();
        }

        private void GetRouteWaypointToAirport()
        {
            var stars = toGroup.controller.GetSelectedProcedures();

            Route = new RouteGroup(
                GetRouteFinder().FindRoute(
                    GetWptIndexFrom(),
                    destination,
                    toRwyComboBox.Text,
                    stars),
                   tracksInUse);

            ShowRoute();
        }

        private void GetRouteAirportToWaypoint()
        {
            var sids = fromGroup.controller.GetSelectedProcedures();

            Route = new RouteGroup(
                GetRouteFinder().FindRoute(
                    origin,
                    fromRwyComboBox.Text,
                    sids,
                    GetWptIndexTo()),
                tracksInUse);

            ShowRoute();
        }

        private void GetRouteAirportToAirport()
        {
            var sids = fromGroup.controller.GetSelectedProcedures();
            var stars = toGroup.controller.GetSelectedProcedures();

            Route = new RouteGroup(
                GetRouteFinder().FindRoute(
                    origin,
                    fromRwyComboBox.Text,
                    sids,
                    destination,
                    toRwyComboBox.Text,
                    stars),
                tracksInUse);

            ShowRoute();
        }

        private string origin
        {
            get
            {
                var orig = fromIdentTxtBox.Text;

                if (airportList[orig] == null)
                {
                    throw new ArgumentException(
                        "Cannot find origin airport in Nav Data.");
                }

                return orig;
            }
        }

        private string destination
        {
            get
            {
                var dest = toIdentTxtBox.Text;

                if (airportList[dest] == null)
                {
                    throw new ArgumentException(
                        "Cannot find destination airport in Nav Data.");
                }

                return dest;
            }
        }

        private void ShowRoute()
        {
            var option = appOptionsLocator.Instance;
            var showDct = !option.HideDctInRoute;
            var selected = option.ShowTrackIdOnly ?
                Route.Folded : Route.Expanded;

            routeRichTxtBox.Text = selected.ToString(showDct);
            routeSummaryLbl.Text = RouteDistanceDisplay.GetDisplay(Route.Expanded, Style.Long);
        }

        /// <exception cref="InvalidUserInputException"></exception>
        private RouteFinderFacade GetRouteFinder()
        {
            return new RouteFinderFacade(
                wptList,
                airportList,
                appOptionsLocator.Instance.NavDataLocation,
                checkedCodesLocator.Instance,
                windCalcGetter());
        }

        private int GetWptIndexFrom()
        {
            if (fromWptComboBox.Items.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot find \"from\" waypoint in Nav Data.");
            }

            var latLon = ExtractLatLon(fromWptComboBox.Text);
            return wptList.FindAllById(fromIdentTxtBox.Text)
                .MinBy(i => wptList[i].Distance(latLon));
        }

        private int GetWptIndexTo()
        {
            if (toWptComboBox.Items.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot find \"to\" waypoint in Nav Data.");
            }

            var latLon = ExtractLatLon(toWptComboBox.Text);
            return wptList.FindAllById(toIdentTxtBox.Text)
                .MinBy(i => wptList[i].Distance(latLon));
        }

        // Gets the lat and lon.
        // Inpute sample: "LAT/22.55201 LON/-121.3554"
        private static LatLon ExtractLatLon(string s)
        {
            var matchLat = Regex.Match(s, @"LAT/-?([\d.]+) ");
            double lat = double.Parse(matchLat.Groups[1].Value);

            var matchLon = Regex.Match(s, @"LON/(-?[\d.]+)");
            double lon = double.Parse(matchLon.Groups[1].Value);

            return new LatLon(lat, lon);
        }

        private class ControlGroup
        {
            public AdvancedTool owner;
            public ComboBox TypeSelection;
            public Label IdentLbl;
            public TextBox Ident;
            public Label RwyLbl;
            public ComboBox Rwy;
            public Label TerminalProcedureLbl;
            public ComboBox TerminalProcedure;
            public Label WptLbl;
            public ComboBox Waypoints;
            public bool IsDepartureAirport;
            public ProcedureFilter procFilter;
            public Button FilterBtn;

            public ISelectedProcedureProvider provider;

            public ControlGroup(
                AdvancedTool owner,
                ComboBox TypeSelection,
                Label IdentLbl,
                TextBox Ident,
                Label RwyLbl,
                ComboBox Rwy,
                Label TerminalProcedureLbl,
                ComboBox TerminalProcedure,
                Label WptLbl,
                ComboBox Waypoints,
                bool IsDepartureAirport,
                ProcedureFilter procFilter,
                Button FilterBtn)
            {
                this.owner = owner;
                this.TypeSelection = TypeSelection;
                this.IdentLbl = IdentLbl;
                this.Ident = Ident;
                this.RwyLbl = RwyLbl;
                this.Rwy = Rwy;
                this.TerminalProcedureLbl = TerminalProcedureLbl;
                this.TerminalProcedure = TerminalProcedure;
                this.WptLbl = WptLbl;
                this.Waypoints = Waypoints;
                this.IsDepartureAirport = IsDepartureAirport;
                this.procFilter = procFilter;
                this.FilterBtn = FilterBtn;
            }

            public void Subsribe()
            {
                provider 
                controller = new RouteFinderSelection(
                    Ident,
                    IsDepartureAirport,
                    Rwy,
                    TerminalProcedure,
                    FilterBtn,
                    owner,
                    owner.appOptionsLocator,
                    () => owner.airwayNetwork.AirportList,
                    () => owner.airwayNetwork.WptList,
                    procFilter);

                TypeSelection.SelectedIndexChanged += TypeChanged;
            }

            public void UnSubsribe()
            {
                TypeSelection.SelectedIndexChanged -= TypeChanged;
            }

            private void ShowWpts(object sender, EventArgs e)
            {
                Waypoints.Items.Clear();

                var indices = owner.wptList.FindAllById(Ident.Text);

                if (indices.Count == 0)
                {
                    return;
                }

                Waypoints.Items.AddRange(
                    indices.Select(i =>
                    {
                        var wpt = owner.wptList[i];
                        return "LAT/" + wpt.Lat.ToString("F4") +
                          "  LON/" + wpt.Lon.ToString("F4");
                    })
                    .ToArray());

                Waypoints.SelectedIndex = 0;
            }

            public void RefreshDisplay()
            {
                var txt = Ident.Text;
                Ident.Text = txt + " ";
                Ident.Text = txt;
            }

            private void TypeChanged(object sender, EventArgs e)
            {
                if (TypeSelection.SelectedIndex == 0)
                {
                    // Airport
                    IdentLbl.Text = "ICAO";
                    RwyLbl.Enabled = true;
                    Rwy.Enabled = true;
                    TerminalProcedureLbl.Enabled = true;
                    TerminalProcedure.Enabled = true;
                    WptLbl.Enabled = false;
                    Waypoints.Enabled = false;

                    controller.Subscribe();
                    Ident.TextChanged -= ShowWpts;
                    RefreshDisplay();
                    Waypoints.Items.Clear();
                }
                else
                {
                    IdentLbl.Text = "Ident";
                    RwyLbl.Enabled = false;
                    Rwy.Enabled = false;
                    TerminalProcedureLbl.Enabled = false;
                    TerminalProcedure.Enabled = false;
                    WptLbl.Enabled = true;
                    Waypoints.Enabled = true;

                    controller.UnSubsribe();
                    Ident.TextChanged += ShowWpts;
                    RefreshDisplay();
                    Rwy.Items.Clear();
                    TerminalProcedure.Items.Clear();
                }
            }
        }*/
    }
}
