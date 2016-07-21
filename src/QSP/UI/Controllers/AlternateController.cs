using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.UserControls;
using QSP.UI.UserControls.RouteOptions;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.Controllers
{
    public class AlternateController
    {
        private const int RowSeperation = 38;

        private GroupBox altnGroupBox;
        private List<AltnRow> rows;
        private AppOptions appSettings;
        private AirportManager airportList;
        private WaypointList wptList;
        private TrackInUseCollection tracksInUse;
        private TableLayoutPanel layoutPanel;
        private DestinationSidSelection destSidProvider;
        private Func<AvgWindCalculator> windCalcGetter;

        // Fires when the number of rows changes.
        public event EventHandler RowCountChanged;

        // Fires when the collection of alternates changes.
        public event EventHandler AlternatesChanged;

        public int RowCount
        {
            get
            {
                return rows.Count;
            }
        }

        public IEnumerable<string> Alternates
        {
            get
            {
                return rows.Select(r =>
                r.Items.IcaoTxtBox.Text.Trim().ToUpper());
            }
        }

        public AlternateController(
            GroupBox altnGroupBox,
            AppOptions appSettings,
            AirportManager airportList,
            WaypointList wptList,
            TrackInUseCollection tracksInUse,
            TableLayoutPanel layoutPanel,
            DestinationSidSelection destSidProvider,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.altnGroupBox = altnGroupBox;
            this.appSettings = appSettings;
            this.airportList = airportList;
            this.wptList = wptList;
            this.tracksInUse = tracksInUse;
            this.layoutPanel = layoutPanel;
            this.destSidProvider = destSidProvider;
            this.windCalcGetter = windCalcGetter;

            rows = new List<AltnRow>();
        }

        public void AddRow()
        {
            var row = new AlternateRowItems();
            row.Init(() => destSidProvider.Icao, airportList);
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

        public Route[] Routes
        {
            get
            {
                return rows.Select(r => r.Control.OptionMenu.Route.Expanded)
                    .ToArray();
            }
        }

        public IEnumerable<AlternateRowItems> Controls
        {
            get
            {
                return rows.Select(r => r.Items);
            }
        }

        public struct AltnRow
        {
            public AlternateRowItems Items; public AltnRowControl Control;
        }

        public class AltnRowControl : IDisposable
        {
            public AlternateRowItems Row;
            public RouteFinderSelection Controller;
            public OptionContextMenu OptionMenu;

            public AltnRowControl(
                AlternateController Parent,
                AlternateRowItems row)
            {
                this.Row = row;

                Controller = new RouteFinderSelection(
                    Row.IcaoTxtBox,
                    false,
                    Row.RwyComboBox,
                    new ComboBox(),
                    new Button(),
                    Parent.appSettings.NavDataLocation,
                    Parent.airportList,
                    Parent.wptList,
                    new ProcedureFilter());

                OptionMenu = new OptionContextMenu(
                    Parent.appSettings,
                    Parent.wptList,
                    Parent.airportList,
                    Parent.tracksInUse,
                    Parent.destSidProvider,
                    Controller,
                    Parent.windCalcGetter,
                    Row.DisLbl,
                    DistanceDisplayStyle.Short,
                    () => Row.RouteTxtBox.Text,
                    (s) => Row.RouteTxtBox.Text = s);
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
