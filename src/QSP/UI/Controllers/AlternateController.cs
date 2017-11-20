using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Controllers
{
    // TODO: Delete this.
    public class AlternateController
    {
        private List<Entry> alternates = new List<Entry>();
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private TableLayoutPanel layoutPanel;
        private DestinationSidSelection destSidProvider;
        private Func<AvgWindCalculator> windCalcGetter;

        // Fires when the number of rows changes.
        public event EventHandler RowCountChanged;

        // Fires when the collection of alternates changes.
        public event EventHandler AlternatesChanged;

        public int RowCount => alternates.Count;

        public IEnumerable<string> Alternates =>
            alternates.Select(r => r.View.IcaoTxtBox.Text.Trim().ToUpper());

        public AlternateController(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            TableLayoutPanel layoutPanel,
            DestinationSidSelection destSidProvider,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.layoutPanel = layoutPanel;
            this.destSidProvider = destSidProvider;
            this.windCalcGetter = windCalcGetter;
        }

        public void AddRow()
        {
            var view = new AlternateRowControl();
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
                view,
                appOptionsLocator,
                airwayNetwork,
                destSidProvider,
                selection,
                new CountryCodeCollection().ToLocator(),
                windCalcGetter);

            view.Init(presenter, layoutPanel.FindForm());
            view.AddToLayoutPanel(layoutPanel);
            view.IcaoTxtBox.TextChanged += (s, e) =>
                AlternatesChanged?.Invoke(this, EventArgs.Empty);

            selection.Subscribe();
            view.ActionBtn.Click +=
                (s, e) => view.ActionContextMenuView.Show(view.ActionBtn, new Point(-100, 30));

            alternates.Add(new Entry { View = view, Presenter = presenter, Selection = selection });
            RowCountChanged?.Invoke(this, EventArgs.Empty);
            AlternatesChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveLastRow()
        {
            if (alternates.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var rowToRemove = alternates[alternates.Count - 1];

            rowToRemove.View.Dispose();
            alternates.RemoveAt(alternates.Count - 1);
            RowCountChanged?.Invoke(this, EventArgs.Empty);
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

        public Route[] Routes => alternates.Select(r => r.Presenter.Route?.Expanded).ToArray();

        public IEnumerable<AlternateRowControl> Controls => alternates.Select(r => r.View);

        public struct Entry
        {
            public AlternateRowControl View;
            public AlternateRowPresenter Presenter;
            public RouteFinderSelection Selection;
        }
    }
}
