using System;
using System.Collections.Generic;
using System.Drawing;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System.Linq;
using System.Windows.Forms;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.TerminalProcedures;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternatePresenter
    {
        private IAlternateView view;
        private List<AlternateRowPresenter> rowPresenters=new List<AlternateRowPresenter>();

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

        public int RowCount => view.Views.Count;

        /// <summary>
        /// Uppercase Icao codes of the alternates.
        /// </summary>
        public IEnumerable<string> Alternates => view.Views.Select(v => v.ICAO);

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
        }

        public void AddRow()
        {
            var row = view.AddRow();

            //var view = new AlternateRowControl();
            var selection = new RouteFinderSelection(
                view.IcaoTxtBox,
                false,
                view.RwyComboBox,
                new ComboBox(),
                new Button(),
                view,
                appOptionsLocator,
                () => airwayNetwork.AirportList,
                () => airwayNetwork.WptList,
                new ProcedureFilter());

            var presenter = new AlternateRowPresenter(
                row,
                appOptionsLocator,
                airwayNetwork,
                destSidProvider,
                selection,
                new CountryCodeCollection().ToLocator(),
                windCalcGetter);

            row.Init(presenter, view layoutPanel.FindForm());
            row.IcaoTxtBox.TextChanged += (s, e) =>
                AlternatesChanged?.Invoke(this, EventArgs.Empty);

            selection.Subscribe();
            row.ActionBtn.Click +=
                (s, e) => view.ActionContextMenuView.Show(view.ActionBtn, new Point(-100, 30));

            rowPresenters.Add(presenter);
            AlternatesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}