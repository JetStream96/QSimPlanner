using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.UI.MsgBox;
using QSP.UI.RoutePlanning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.UI.Factories.FormFactory;

namespace QSP.UI.Controllers
{
    public class RouteFinderSelection : ISelectedProcedureProvider
    {
        public static readonly string NoProcedureTxt = "NONE";
        public static readonly string AutoProcedureTxt = "AUTO";

        private readonly Locator<AppOptions> appOptionsLocator;
        private readonly Func<WaypointList> wptListGetter;
        private readonly Func<AirportManager> airportListGetter;
        private readonly Control parentControl;

        public ProcedureFilter ProcFilter { get; private set; }
        public TextBox IcaoTxtBox { get; private set; }
        public ComboBox RwyCBox { get; private set; }
        public ComboBox TerminalProceduresCBox { get; private set; }
        public Button FilterBtn { get; private set; }
        public bool IsDepartureAirport { get; private set; }

        public WaypointList WptList => wptListGetter();
        public AirportManager AirportList => airportListGetter();
        public string NavDataLocation => appOptionsLocator.Instance.NavDataLocation;

        public RouteFinderSelection(
            TextBox IcaoTxtBox,
            bool IsDepartureAirport,
            ComboBox RwyCBox,
            ComboBox TerminalProceduresCBox,
            Button FilterBtn,
            Control parentControl,
            Locator<AppOptions> appOptionsLocator,
            Func<AirportManager> airportListGetter,
            Func<WaypointList> wptListGetter,
            ProcedureFilter ProcFilter)
        {
            this.IcaoTxtBox = IcaoTxtBox;
            this.IsDepartureAirport = IsDepartureAirport;
            this.RwyCBox = RwyCBox;
            this.TerminalProceduresCBox = TerminalProceduresCBox;
            this.FilterBtn = FilterBtn;
            this.parentControl = parentControl;
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
            FilterBtn.Click += filterSidStar;

            FilterBtn.Enabled = false;
        }

        public void UnSubsribe()
        {
            IcaoTxtBox.TextChanged -= IcaoChanged;
            RwyCBox.SelectedIndexChanged -= RwyChanged;
            FilterBtn.Click -= filterSidStar;
        }

        public List<string> GetSelectedProcedures()
        {
            var proc = new List<string>();

            if (TerminalProceduresCBox.Text == AutoProcedureTxt)
            {
                foreach (var i in TerminalProceduresCBox.Items)
                {
                    if ((string)i != AutoProcedureTxt)
                    {
                        proc.Add((string)i);
                    }
                }
            }
            else if (TerminalProceduresCBox.Text != NoProcedureTxt)
            {
                proc.Add(TerminalProceduresCBox.Text);
            }

            return proc;
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

            var rwyList = AirportList.RwyIdentList(Icao)?.ToArray();

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

        private void filterSidStar(object sender, EventArgs e)
        {
            var filter = new SidStarFilter();

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
