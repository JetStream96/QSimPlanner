using System;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternatePresenter
    {
        private IAlternateView view;

        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private Locator<IWindTableCollection> windTableLocator;
        private DestinationSidSelection destSidProvider;
        private Func<FuelDataItem> fuelData;
        private Func<double> zfwTon;
        private Func<string> orig;
        private Func<string> dest;

        private AppOptions AppOptions => appOptionsLocator.Instance;

        public AlternatePresenter(
            IAlternateView view,
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            Locator<IWindTableCollection> windTableLocator,
            DestinationSidSelection destSidProvider,
            Func<FuelDataItem> fuelData,
            Func<double> zfwTon,
            Func<string> orig,
            Func<string> dest)
        {
            this.view = view;
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.windTableLocator = windTableLocator;
            this.destSidProvider = destSidProvider;
            this.fuelData = fuelData;
            this.zfwTon = zfwTon;
            this.orig = orig;
            this.dest = dest;

            SetAltnController();
            AltnControl.RowCountChanged +=
                (s, e) => removeAltnBtn.Enabled = AltnControl.RowCount > 1;
        }

    }
}