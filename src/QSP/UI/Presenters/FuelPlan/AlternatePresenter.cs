using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternatePresenter
    {
        private IAlternateView view;
        private List<AlternateRowPresenter> rowPresenters = new List<AlternateRowPresenter>();

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

        /// Fires when the collection of alternates changes.
        public event EventHandler AlternatesChanged;

        public int RowCount => view.Views.Count();

        /// <summary>
        /// Uppercase Icao codes of the alternates.
        /// </summary>
        public IEnumerable<string> Alternates => view.Views.Select(v => v.Icao);

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
            windCalcGetter = () => FuelPlanningControl.GetWindCalculator(AppOptions,
                   windTableLocator, airwayNetwork.AirportList, fuelData(),
                   zfwTon(), orig(), dest());
        }

        public AlternateRowPresenter GetRowPresenter(IAlternateRowView v)
        {
            return new AlternateRowPresenter(
                v,
                appOptionsLocator,
                airwayNetwork,
                destSidProvider,
                new CountryCodeCollection().ToLocator(),
                windCalcGetter);
        }

        public void AddRow()
        {
            var row = view.AddRow();
            row.IcaoChanged += (s, e) => AlternatesChanged?.Invoke(s, e);
            rowPresenters.Add(row.Presenter);
            AlternatesChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveLastRow()
        {
            if (RowCount == 0) throw new InvalidOperationException();
            rowPresenters.RemoveAt(RowCount - 1);
            AlternatesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RefreshForAirportListChange()
        {
            alternates.ForEach(r => r.Selection.RefreshRwyComboBox());
        }

        public void RefreshForNavDataLocationChange()
        {
            alternates.ForEach(r => r.Selection.RefreshProcedureComboBox());
        }
    }
}