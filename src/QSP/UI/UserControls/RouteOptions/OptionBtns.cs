using QSP.Common.Options;
using QSP.RouteFinding.Routes;
using QSP.UI.Controllers;
using QSP.UI.Controls;
using QSP.WindAloft;
using System;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;
using QSP.LibraryExtension;
using QSP.RouteFinding;

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
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
            Func<AvgWindCalculator> windCalcGetter,
            Label routeDisLbl,
            DistanceDisplayStyle displayStyle,
            Func<string> routeTxtGetter,
            Action<string> routeTxtSetter)
        {
            controller = new RouteOptionController(
                appOptionsLocator, 
                airwayNetwork,
                origController,
                destController,
                windCalcGetter,
                routeDisLbl,
                displayStyle,
                routeTxtGetter,
                routeTxtSetter,
                findRouteBtn,
                analyzeRouteBtn,
                exportBtn,
                showMapBtn);
        }

        public void Subscribe()
        {
            controller.Subscribe();
        }

        public class ClickableButton : Button, IClickable { }
    }
}
