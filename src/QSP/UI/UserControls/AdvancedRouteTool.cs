using QSP.AviationTools.Coordinates;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using QSP.UI.UserControls.RouteActions;
using QSP.UI.Utilities;
using QSP.WindAloft;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static QSP.UI.Factories.ToolTipFactory;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.UserControls
{
    public partial class AdvancedRouteTool : UserControl
    {
        private ControlGroup fromGroup;
        private ControlGroup toGroup;
        private SimpleActionContextMenu routeActionMenu;
        private RouteOptionContextMenu routeOptionMenu;

        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private Locator<CountryCodeManager> countryCodeLocator;
        private Locator<CountryCodeCollection> checkedCodesLocator;
        private ProcedureFilter procFilter;
        private CountryCodeManager countryCodes;
        private Func<AvgWindCalculator> windCalcGetter;
        private RouteGroup Route;

        private WaypointList wptList
        {
            get { return airwayNetwork.WptList; }
        }

        private AirportManager airportList
        {
            get { return airwayNetwork.AirportList; }
        }

        private TrackInUseCollection tracksInUse
        {
            get { return airwayNetwork.TracksInUse; }
        }

        public AdvancedRouteTool()
        {
            InitializeComponent();
        }

        public void Init(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            Locator<CountryCodeManager> countryCodeLocator,
            Locator<CountryCodeCollection> checkedCodesLocator,
            ProcedureFilter procFilter,
            CountryCodeManager countryCodes,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.countryCodeLocator = countryCodeLocator;
            this.checkedCodesLocator = checkedCodesLocator;
            this.procFilter = procFilter;
            this.countryCodes = countryCodes;
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
            routeActionMenu.MapToolStripMenuItem.Click +=
                (s, e) => ShowMapHelper.ShowMap(Route);

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
            items.AddRange(new string[] { "Airport", "Waypoint" });
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
                MsgBoxHelper.ShowWarning(ex.Message);
            }
        }

        private void GetRouteWaypointToWaypoint()
        {
            Route = new RouteGroup(
                new RouteFinder(wptList, checkedCodesLocator.Instance)
                    .FindRoute(GetWptIndexFrom(), GetWptIndexTo()),
                    tracksInUse);

            ShowRoute(true, true);
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

            ShowRoute(true, false);
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

            ShowRoute(false, true);
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
                    toIdentTxtBox.Text,
                    destination,
                    stars),
                 tracksInUse);

            ShowRoute(false, false);
        }

        private string origin
        {
            get
            {
                var orig = fromIdentTxtBox.Text;

                if (airportList.Find(orig) == null)
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

                if (airportList.Find(dest) == null)
                {
                    throw new ArgumentException(
                        "Cannot find destination airport in Nav Data.");
                }

                return dest;
            }
        }

        private void ShowRoute(bool ShowFirstWaypoint, bool ShowLastWaypoint)
        {
            var option = appOptionsLocator.Instance;
            var routeToShow = option.ShowTrackIdOnly ?
                Route.Folded : Route.Expanded;

            var showDct = !option.HideDctInRoute;
            routeRichTxtBox.Text = routeToShow.ToString(
                ShowFirstWaypoint, ShowLastWaypoint, showDct);

            UpdateRouteDistanceLbl(
                routeSummaryLbl, Route.Expanded, DistanceDisplayStyle.Long);
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
            var latLon = ExtractLatLon(fromWptComboBox.Text);
            return wptList.FindByWaypoint(
                fromIdentTxtBox.Text, latLon.Lat, latLon.Lon);
        }

        private int GetWptIndexTo()
        {
            var latLon = ExtractLatLon(toWptComboBox.Text);
            return wptList.FindByWaypoint(
                toIdentTxtBox.Text, latLon.Lat, latLon.Lon);
        }

        // Gets the lat and lon.
        // Inpute sample: "LAT/22.55201 LON/121.3554"
        private static LatLon ExtractLatLon(string s)
        {
            var matchLat = Regex.Match(s, @"LAT/([\d.]+) ");
            double lat = double.Parse(matchLat.Groups[1].Value);

            var matchLon = Regex.Match(s, @"LON/([\d.]+)");
            double lon = double.Parse(matchLon.Groups[1].Value);

            return new LatLon(lat, lon);
        }

        private class ControlGroup
        {
            public AdvancedRouteTool owner;
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

            public RouteFinderSelection controller;

            public ControlGroup(
                AdvancedRouteTool owner,
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
                controller = new RouteFinderSelection(
                    Ident,
                    IsDepartureAirport,
                    Rwy,
                    TerminalProcedure,
                    FilterBtn,
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
                        return "LAT/" + wpt.Lat + "  LON/" + wpt.Lon;
                    }).ToArray());

                Waypoints.SelectedIndex = 0;
            }

            private void ForceRefresh()
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
                    ForceRefresh();
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
                    ForceRefresh();
                    Rwy.Items.Clear();
                    TerminalProcedure.Items.Clear();
                }
            }
        }
    }
}
