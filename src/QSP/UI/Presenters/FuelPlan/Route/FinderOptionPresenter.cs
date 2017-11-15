using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.UI.Util;
using QSP.UI.Views.Route;

namespace QSP.UI.Presenters.FuelPlan.Route
{
    // Manages the selected SIDs/STARs by the user. 
    // This class is responsible for the SID/STAR ComboBoxes, the filter button,
    // and the instantiation of the filter form.
    //
    public class FinderOptionPresenter
    {
        public static readonly string NoProcedureTxt = "NONE";
        public static readonly string AutoProcedureTxt = "AUTO";

        private IFinderOptionView view;
        private readonly Locator<AppOptions> appOptionsLocator;
        private readonly Func<WaypointList> wptListGetter;
        private readonly Func<AirportManager> airportListGetter;

        public ProcedureFilter ProcFilter { get; private set; }
        public bool IsDepartureAirport { get; private set; }

        public WaypointList WptList => wptListGetter();
        public AirportManager AirportList => airportListGetter();
        public string NavDataLocation => appOptionsLocator.Instance.NavDataLocation;

        public FinderOptionPresenter(
            IFinderOptionView view,
            bool IsDepartureAirport,
            Locator<AppOptions> appOptionsLocator,
            Func<AirportManager> airportListGetter,
            Func<WaypointList> wptListGetter,
            ProcedureFilter ProcFilter)
        {
            this.view = view;
            this.IsDepartureAirport = IsDepartureAirport;
            this.appOptionsLocator = appOptionsLocator;
            this.airportListGetter = airportListGetter;
            this.wptListGetter = wptListGetter;
            this.ProcFilter = ProcFilter;
        }

        public string Icao => IcaoTxtBox.Text.Trim().ToUpper();

        public string Rwy => RwyCBox.Text;

        public void Subscribe()
        {
            IcaoTxtBox.TextChanged += IcaoChanged;
            RwyCBox.SelectedIndexChanged += RwyChanged;
            FilterBtn.Click += FilterSidStar;

            FilterBtn.Enabled = false;
        }

        public void UnSubsribe()
        {
            IcaoTxtBox.TextChanged -= IcaoChanged;
            RwyCBox.SelectedIndexChanged -= RwyChanged;
            FilterBtn.Click -= FilterSidStar;
        }

        public List<string> GetSelectedProcedures()
        {
            if (TerminalProceduresCBox.Text == AutoProcedureTxt)
            {
                return TerminalProceduresCBox.Items.Cast<string>()
                    .Where(s => s != AutoProcedureTxt)
                    .ToList();
            }

            if (TerminalProceduresCBox.Text != NoProcedureTxt)
            {
                return new List<string>() { TerminalProceduresCBox.Text };
            }

            return new List<string>();
        }

        public void RefreshRwyComboBox()
        {
            var selected = Rwy;
            IcaoChanged(this, EventArgs.Empty);
            RwyCBox.Text = selected;
        }

        public void RefreshProcedureComboBox()
        {
            if (RwyCBox.Items.Count > 0)
            {
                var selected = TerminalProceduresCBox.Text;
                RwyChanged(this, EventArgs.Empty);
                TerminalProceduresCBox.Text = selected;
            }
        }

        private void IcaoChanged(object sender, EventArgs e)
        {
            RwyCBox.Items.Clear();
            TerminalProceduresCBox.Items.Clear();
            FilterBtn.Enabled = false;

            var rwyList = AirportList.RwyIdents(Icao)?.ToArray();

            if (rwyList != null && rwyList.Length > 0)
            {
                RwyCBox.Items.AddRange(rwyList);
                RwyCBox.SelectedIndex = 0;
            }
        }

        /// <exception cref="LoadSidFileException"></exception>
        /// <exception cref="LoadStarFileException"></exception>
        public List<string> AvailableProcedures
        {
            get
            {
                if (IsDepartureAirport)
                {
                    return SidHandlerFactory.GetHandler(
                         Icao, NavDataLocation, WptList, WptList.GetEditor(), AirportList)
                         .GetSidList(Rwy);
                }
                else
                {
                    return StarHandlerFactory.GetHandler(
                        Icao, NavDataLocation, WptList, WptList.GetEditor(), AirportList)
                        .GetStarList(Rwy);
                }
            }
        }

        private void RwyChanged(object sender, EventArgs e)
        {
            FilterBtn.Enabled = false;

            try
            {
                SetProcedures(AvailableProcedures.Where(ShouldShow).ToArray());
            }
            catch (Exception ex)
            {
                parentControl.ShowError(ex.Message);
            }
        }

        private bool ShouldShow(string proc)
        {
            if (!ProcFilter.Exists(Icao, Rwy)) return true;
            var info = ProcFilter[Icao, Rwy];
            return info.Procedures.Contains(proc) ^ info.IsBlackList;
        }

        private void SetProcedures(string[] proc)
        {
            TerminalProceduresCBox.Items.Clear();

            if (proc.Length == 0)
            {
                TerminalProceduresCBox.Items.Add(NoProcedureTxt);
            }
            else
            {
                TerminalProceduresCBox.Items.Add(AutoProcedureTxt);
                TerminalProceduresCBox.Items.AddRange(proc);
            }

            TerminalProceduresCBox.SelectedIndex = 0;
            FilterBtn.Enabled = true;
        }

        private void FilterSidStar(object sender, EventArgs e)
        {
            var filter = new SidStarFilterControl();

            filter.Init(
                Icao,
                Rwy,
                AvailableProcedures,
                IsDepartureAirport,
                ProcFilter);

            filter.Location = new Point(0, 0);

            using (var frm = GetForm(filter.Size))
            {
                frm.Controls.Add(filter);

                filter.FinishedSelection += (_s, _e) =>
                {
                    frm.Close();
                    RefreshProcedureComboBox();
                };

                frm.ShowDialog();
            }
        }
    }
}