using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Controllers
{
    public class RouteFinderSelection
    {
        private AppOptions appSettings;
        private WaypointList wptList;
        private AirportManager airportList;
        private ProcedureFilter procFilter;

        public static readonly string NoProcedureTxt = "NONE";
        public static readonly string AutoProcedureTxt = "AUTO";

        public TextBox IcaoTxtBox { get; private set; }
        public ComboBox RwyCBox { get; private set; }
        public ComboBox TerminalProceduresCBox { get; private set; }
        public bool IsDepartureAirport { get; private set; }
        
        public RouteFinderSelection(
            TextBox IcaoTxtBox,
            bool IsDepartureAirport,
            ComboBox RwyCBox,
            ComboBox TerminalProceduresCBox,
            AppOptions appSettings,
            AirportManager airportList,
            WaypointList wptList,
            ProcedureFilter procFilter)
        {
            this.IcaoTxtBox = IcaoTxtBox;
            this.IsDepartureAirport = IsDepartureAirport;
            this.RwyCBox = RwyCBox;
            this.TerminalProceduresCBox = TerminalProceduresCBox;
            this.appSettings = appSettings;
            this.airportList = airportList;
            this.wptList = wptList;
            this.procFilter = procFilter;
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
        }

        public void UnSubsribe()
        {
            IcaoTxtBox.TextChanged -= IcaoChanged;
            RwyCBox.SelectedIndexChanged -= RwyChanged;
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
            var rwyList = airportList.RwyIdentList(Icao);

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
                         Icao, appSettings.NavDataLocation, wptList,
                         wptList.GetEditor(), airportList)
                         .GetSidList(Rwy);
                }
                else
                {
                    return StarHandlerFactory.GetHandler(
                        Icao, appSettings.NavDataLocation, wptList,
                        wptList.GetEditor(), airportList)
                        .GetStarList(Rwy);
                }
            }
        }

        private void RwyChanged(object sender, EventArgs e)
        {
            List<string> proc = null;

            try
            {
                proc = AvailableProcedures;
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowError(ex.Message.ToString());
            }

            SetProcedures(proc.Where(ShouldShow));
        }

        private bool ShouldShow(string proc)
        {
            if (procFilter.Exists(Icao, Rwy) == false)
            {
                return true;
            }

            var info = procFilter[Icao, Rwy];

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
        }
    }
}
