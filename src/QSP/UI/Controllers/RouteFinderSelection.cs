using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.UI.RoutePlanning;
using QSP.UI.Utilities;
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

        public string NavDataLocation { get; private set; }
        public WaypointList WptList { get; private set; }
        public AirportManager AirportList { get; private set; }
        public ProcedureFilter ProcFilter { get; private set; }
        public TextBox IcaoTxtBox { get; private set; }
        public ComboBox RwyCBox { get; private set; }
        public ComboBox TerminalProceduresCBox { get; private set; }
        public Button FilterBtn { get; private set; }
        public bool IsDepartureAirport { get; private set; }

        public RouteFinderSelection(
            TextBox IcaoTxtBox,
            bool IsDepartureAirport,
            ComboBox RwyCBox,
            ComboBox TerminalProceduresCBox,
            Button FilterBtn,
            string NavDataLocation,
            AirportManager AirportList,
            WaypointList WptList,
            ProcedureFilter ProcFilter)
        {
            this.IcaoTxtBox = IcaoTxtBox;
            this.IsDepartureAirport = IsDepartureAirport;
            this.RwyCBox = RwyCBox;
            this.TerminalProceduresCBox = TerminalProceduresCBox;
            this.FilterBtn = FilterBtn;
            this.NavDataLocation = NavDataLocation;
            this.AirportList = AirportList;
            this.WptList = WptList;
            this.ProcFilter = ProcFilter;
        }

        public string Icao
        {
            get
            {
                return IcaoTxtBox.Text.Trim().ToUpper();
            }
        }

        public string Rwy
        {
            get
            {
                return RwyCBox.Text;
            }
        }

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

        public void RefreshProcedureComboBox()
        {
            RwyChanged(this, EventArgs.Empty);
        }

        private void IcaoChanged(object sender, EventArgs e)
        {
            RwyCBox.Items.Clear();
            TerminalProceduresCBox.Items.Clear();
            FilterBtn.Enabled = false;

            var rwyList = AirportList.RwyIdentList(Icao);

            if (rwyList != null && rwyList.Count() > 0)
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
                         Icao, NavDataLocation, WptList,
                         WptList.GetEditor(), AirportList)
                         .GetSidList(Rwy);
                }
                else
                {
                    return StarHandlerFactory.GetHandler(
                        Icao, NavDataLocation, WptList,
                        WptList.GetEditor(), AirportList)
                        .GetStarList(Rwy);
                }
            }
        }

        private void RwyChanged(object sender, EventArgs e)
        {
            FilterBtn.Enabled = false;
            List<string> proc = null;

            try
            {
                proc = AvailableProcedures;
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowError(ex.Message);
            }

            SetProcedures(proc.Where(ShouldShow));
        }

        private bool ShouldShow(string proc)
        {
            if (ProcFilter.Exists(Icao, Rwy) == false)
            {
                return true;
            }

            var info = ProcFilter[Icao, Rwy];

            return info.Procedures.Contains(proc) ^ info.IsBlackList;
        }

        private void SetProcedures(IEnumerable<string> proc)
        {
            TerminalProceduresCBox.Items.Clear();

            if (proc.Count() == 0)
            {
                TerminalProceduresCBox.Items.Add(NoProcedureTxt);
            }
            else
            {
                TerminalProceduresCBox.Items.Add(AutoProcedureTxt);
                TerminalProceduresCBox.Items.AddRange(proc.ToArray());
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

                filter.FinishedSelection += (_sender, _e) =>
                {
                    frm.Close();
                    RefreshProcedureComboBox();
                };

                frm.ShowDialog();
            }
        }
    }
}
