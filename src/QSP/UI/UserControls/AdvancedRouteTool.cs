using QSP.AviationTools.Coordinates;
using QSP.Common.Options;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using QSP.UI.RoutePlanning;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static QSP.MainForm;
using static QSP.UI.Factories.FormFactory;
using static QSP.UI.Factories.ToolTipFactory;

namespace QSP.UI.UserControls
{
    public partial class AdvancedRouteTool : UserControl
    {
        public CountryCodeCollection CheckedCodes { get; private set; }

        private ControlGroup fromGroup;
        private ControlGroup toGroup;

        private AppOptions appSettings;
        private WaypointList wptList;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private ProcedureFilter procFilter;
        private CountryCodeManager countryCodes;

        public AdvancedRouteTool()
        {
            InitializeComponent();
        }

        public void Init(
            AppOptions appSettings,
            WaypointList wptList,
            AirportManager airportList,
            TrackInUseCollection tracksInUse,
            ProcedureFilter procFilter,
            CountryCodeManager countryCodes)
        {
            this.appSettings = appSettings;
            this.wptList = wptList;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
            this.procFilter = procFilter;
            this.countryCodes = countryCodes;
            CheckedCodes = new CountryCodeCollection();

            SetControlGroups();
            attachEventHandlers();
            SetDefaultState();
            AddToolTip();
            SetImages();
        }

        private void SetImages()
        {
            var width = avoidCountryBtn.Width - 4;
            var height = avoidCountryBtn.Height - 4;
            var image = new Bitmap(width, height);

            var graph = Graphics.FromImage(image);
            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.DrawImage(Properties.Resources.prohibitIcon,
                new Rectangle(0, 0, width, height));

            avoidCountryBtn.BackgroundImage = image;
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
            tp.SetToolTip(avoidCountryBtn, "Avoid counties");
        }

        private void FindRouteBtnClick(object sender, EventArgs e)
        {
            if (fromTypeComboBox.SelectedIndex == 0)
            {
                if (toTypeComboBox.SelectedIndex == 0)
                {
                    // Airport to airport
                    var sids = fromGroup.controller.GetSelectedProcedures();
                    var stars = toGroup.controller.GetSelectedProcedures();

                    try
                    {
                        var myRoute = new RouteGroup(
                            new RouteFinderFacade(
                                wptList,
                                airportList,
                                appSettings.NavDataLocation,
                                CheckedCodes)
                                .FindRoute(
                                    fromIdentTxtBox.Text,
                                    fromRwyComboBox.Text,
                                    sids,
                                    toIdentTxtBox.Text,
                                    toRwyComboBox.Text,
                                    stars),
                                tracksInUse);

                        var route = myRoute.Expanded;

                        routeRichTxtBox.Text = route.ToString();
                        UpdateRouteDistanceLbl(routeSummaryLbl, route);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    // Airport to waypoint
                    try
                    {
                        var sids = fromGroup.controller.GetSelectedProcedures();
                        var latLon = ExtractLatLon(toWptComboBox.Text);
                        var wpt = wptList.FindByWaypoint(
                            toIdentTxtBox.Text, latLon.Lat, latLon.Lon);

                        var myRoute = new RouteGroup(
                            new RouteFinderFacade(
                                wptList,
                                airportList,
                                appSettings.NavDataLocation,
                                CheckedCodes)
                                .FindRoute(
                                    fromIdentTxtBox.Text,
                                    fromRwyComboBox.Text,
                                    sids,
                                    wpt),
                                tracksInUse);

                        var route = myRoute.Expanded;

                        routeRichTxtBox.Text = route.ToString(false, true);
                        UpdateRouteDistanceLbl(routeSummaryLbl, route);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            else
            {
                if (toTypeComboBox.SelectedIndex == 0)
                {
                    // Waypoint to airport
                    var latLon = ExtractLatLon(fromWptComboBox.Text);
                    var wpt = wptList.FindByWaypoint(
                        fromIdentTxtBox.Text, latLon.Lat, latLon.Lon);
                    var stars = toGroup.controller.GetSelectedProcedures();

                    try
                    {
                        var myRoute = new RouteGroup(
                            new RouteFinderFacade(
                                wptList,
                                airportList,
                                appSettings.NavDataLocation,
                                CheckedCodes)
                                .FindRoute(
                                    wpt,
                                    toIdentTxtBox.Text,
                                    toRwyComboBox.Text,
                                    stars),
                                tracksInUse);

                        var route = myRoute.Expanded;

                        routeRichTxtBox.Text = route.ToString(true, false);
                        UpdateRouteDistanceLbl(routeSummaryLbl, route);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    // Waypoint to waypoint
                    var latLonFrom = ExtractLatLon(fromWptComboBox.Text);
                    var wptFrom = wptList.FindByWaypoint(
                        fromIdentTxtBox.Text, latLonFrom.Lat, latLonFrom.Lon);

                    var latLonTo = ExtractLatLon(toWptComboBox.Text);
                    var wptTo = wptList.FindByWaypoint(
                        toIdentTxtBox.Text, latLonTo.Lat, latLonTo.Lon);

                    try
                    {
                        var myRoute = new RouteGroup(
                            new RouteFinder(wptList, CheckedCodes)
                                .FindRoute(wptFrom, wptTo),
                                tracksInUse);

                        var route = myRoute.Expanded;

                        routeRichTxtBox.Text = route.ToString(true, true);
                        UpdateRouteDistanceLbl(routeSummaryLbl, route);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
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
                    owner.appSettings,
                    owner.airportList,
                    owner.wptList,
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

                List<int> indices = owner.wptList.FindAllById(Ident.Text);

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

        private void avoidCountryBtnClick(object sender, EventArgs e)
        {
            var countrySelection = new AvoidCountrySelection();
            countrySelection.Init(countryCodes);
            countrySelection.Location = new Point(0, 0);
            countrySelection.CheckedCodes = CheckedCodes;

            var frm = GetForm(countrySelection.Size);
            frm.Controls.Add(countrySelection);

            countrySelection.CancelBtn.Click += (_sender, _e) =>
            {
                frm.Close();
            };

            countrySelection.OkBtn.Click += (_sender, _e) =>
            {
                CheckedCodes = countrySelection.CheckedCodes;
                frm.Close();
            };

            frm.ShowDialog();
        }
    }
}
