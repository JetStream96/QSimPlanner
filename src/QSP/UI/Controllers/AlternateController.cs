using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.UserControls;
using QSP.UI.UserControls.RouteActions;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QSP.UI.Views.FuelPlan;
using QSP.UI.Views.Route.Actions;
using static QSP.UI.Util.RouteDistanceDisplay;

namespace QSP.UI.Controllers
{
    public class AlternateController
    {
        private List<AltnRow> rows;
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private TableLayoutPanel layoutPanel;
        private DestinationSidSelection destSidProvider;
        private Func<AvgWindCalculator> windCalcGetter;

        // Fires when the number of rows changes.
        public event EventHandler RowCountChanged;

        // Fires when the collection of alternates changes.
        public event EventHandler AlternatesChanged;

        public int RowCount => rows.Count;

        public IEnumerable<string> Alternates
        {
            get
            {
                return rows.Select(r => r.Items.IcaoTxtBox.Text.Trim().ToUpper());
            }
        }

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

            rows = new List<AltnRow>();
        }

        public void AddRow()
        {
            var row = new AlternateRowControl();
            row.Init(() => destSidProvider.Icao, () => airwayNetwork.AirportList);
            row.AddToLayoutPanel(layoutPanel);
            row.IcaoTxtBox.TextChanged += (s, e) =>
                AlternatesChanged?.Invoke(this, EventArgs.Empty);

            var controller = new AltnRowControl(this, row);
            controller.Subsribe();

            rows.Add(new AltnRow() { Items = row, Control = controller });
            RowCountChanged?.Invoke(this, EventArgs.Empty);
            AlternatesChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveLastRow()
        {
            if (rows.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var rowToRemove = rows[rows.Count - 1];

            rowToRemove.Items.Dispose();
            rowToRemove.Control.Dispose();
            rows.RemoveAt(rows.Count - 1);
            RowCountChanged?.Invoke(this, EventArgs.Empty);
            AlternatesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RefreshForAirportListChange()
        {
            rows.ForEach(r => r.Control.Controller.RefreshRwyComboBox());
        }

        public void RefreshForNavDataLocationChange()
        {
            rows.ForEach(r => r.Control.Controller.RefreshProcedureComboBox());
        }

        public Route[] Routes
        {
            get
            {
                return rows.Select(r => r.Control.OptionMenu.Route?.Expanded)
                    .ToArray();
            }
        }

        public IEnumerable<AlternateRowControl> Controls => rows.Select(r => r.Items);

        public struct AltnRow
        {
            public AlternateRowControl Items; public AltnRowControl Control;
        }

        public sealed class AltnRowControl : IDisposable
        {
            public AlternateRowControl Row;
            public RouteFinderSelection Controller;
            public ActionContextMenu OptionMenu;
            
            public AltnRowControl(AlternateController Parent, AlternateRowControl row)
            {
                this.Row = row;

                Controller = new RouteFinderSelection(
                    Row.IcaoTxtBox,
                    false,
                    Row.RwyComboBox,
                    new ComboBox(),
                    new Button(),
                    row,
                    Parent.appOptionsLocator,
                    () => Parent.airwayNetwork.AirportList,
                    () => Parent.airwayNetwork.WptList,
                    new ProcedureFilter());
                
                OptionMenu = new ActionContextMenu(
                    Parent.appOptionsLocator,
                    Parent.airwayNetwork,
                    Parent.destSidProvider,
                    Controller,
                    new CountryCodeCollection().ToLocator(),
                    Parent.windCalcGetter,
                    Row.DisLbl,
                    DistanceDisplayStyle.Short,
                    () => Row.RouteTxtBox.Text,
                    (s) => Row.RouteTxtBox.Text = s,
                    Parent.layoutPanel.FindForm());
            }

            public void Subsribe()
            {
                Controller.Subscribe();
                Row.ShowMoreBtn.Click += ShowBtns;
                OptionMenu.Subscribe();
            }

            private void ShowBtns(object sender, EventArgs e)
            {
                OptionMenu.Show(Row.ShowMoreBtn, new Point(-100, 30));
            }

            public void Dispose()
            {
                OptionMenu.Dispose();
            }
        }
    }
}
