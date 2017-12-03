using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternatePresenter : IRefreshForNavDataChange
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

        public int RowCount => view.Subviews.Count();

        public IEnumerable<RouteGroup> Routes => rowPresenters.Select(p => p.Route);

        /// <summary>
        /// Uppercase Icao codes of the alternates.
        /// </summary>
        public IEnumerable<string> Alternates
        {
            get => view.Subviews.Select(v => v.Icao);
        }

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

        public void SubsribeRowEventHandlers(IAlternateRowView row)
        {
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
        
        public void OnNavDataChange()
        {
            rowPresenters.ForEach(r => r.OnNavDataChange());
        }

        public void SetAlternates(IList<(string icao, string rwy)> alternates)
        {
            if (alternates.Count == 0) return;

            // Set number of alternates
            while (RowCount < alternates.Count)
            {
                view.AddRow();
            }

            for (int i = 0; i < alternates.Count; i++)
            {
                rowPresenters[i].View.Icao = alternates[i].icao;
                rowPresenters[i].View.Rwy = alternates[i].rwy;
            }
        }

        public IEnumerable<(string icao, string rwy)> GetAlternates()
        {
            return rowPresenters.Select(p =>
            {
                var view = p.View;
                return (view.Icao, view.Rwy);
            });
        }
    }
}