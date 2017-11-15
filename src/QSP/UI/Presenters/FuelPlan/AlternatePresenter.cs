using System;
using System.Collections.Generic;
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

        private Func<AvgWindCalculator> windCalcGetter;

        private AppOptions AppOptions => appOptionsLocator.Instance;

        // Fires when the number of rows changes.
        public event EventHandler RowCountChanged;

        // Fires when the collection of alternates changes.
        public event EventHandler AlternatesChanged;

        public int RowCount => alternates.Count;

        public IEnumerable<string> Alternates =>
            alternates.Select(r => r.View.IcaoTxtBox.Text.Trim().ToUpper());

        public AlternateController AltnControl { get; private set; }

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

            // TODO: move it outside of FuelPlanningControl.
            windCalcGetter =  () => FuelPlanningControl.GetWindCalculator(AppOptions,
                    windTableLocator, airwayNetwork.AirportList, fuelData(),
                    zfwTon(), orig(), dest());

            SetAltnController();
            AltnControl.RowCountChanged +=
                (s, e) => removeAltnBtn.Enabled = AltnControl.RowCount > 1;
        }

        private void SetAltnController()
        {
            AltnControl = new AlternateController(
                appOptionsLocator,
                airwayNetwork,
                altnLayoutPanel,
                destSidProvider,
               


            removeAltnBtn.Enabled = false;
            AltnControl.AddRow();
        }
    }
}