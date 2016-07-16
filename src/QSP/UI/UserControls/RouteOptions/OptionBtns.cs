using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.UI.Controllers;
using QSP.UI.Controls;
using System;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.UserControls.RouteOptions
{
    public partial class OptionBtns : UserControl
    {
        private RouteOptionController controller;

        public RouteGroup Route { get { return controller.Route; } }

        public OptionBtns()
        {
            InitializeComponent();
        }

        public void Init(
            AppOptions appSettings,
            WaypointList wptList,
            AirportManager airportList,
            TrackInUseCollection tracksInUse,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
            Label routeDisLbl,
            DistanceDisplayStyle displayStyle,
            Func<string> routeTxtGetter,
            Action<string> routeTxtSetter)
        {
            controller = new RouteOptionController(
                appSettings,
                wptList,
                airportList,
                tracksInUse,
                origController,
                destController,
                routeDisLbl,
                displayStyle,
                routeTxtGetter,
                routeTxtSetter,
                findRouteBtn,
                analyzeRouteBtn,
                exportBtn);
        }

        public void Subscribe()
        {
            controller.Subscribe();
        }

        public class ClickableButton : Button, IClickable { }
    }
}
