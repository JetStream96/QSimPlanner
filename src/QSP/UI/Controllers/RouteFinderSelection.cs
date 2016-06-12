using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
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
            WaypointList wptList)
        {
            this.IcaoTxtBox = IcaoTxtBox;
            this.IsDepartureAirport = IsDepartureAirport;
            this.RwyCBox = RwyCBox;
            this.TerminalProceduresCBox = TerminalProceduresCBox;
            this.appSettings = appSettings;
            this.airportList = airportList;
            this.wptList = wptList;
        }

        private string Icao
        {
            get
            {
                return IcaoTxtBox.Text.Trim().ToUpper();
            }
        }

        private string Rwy
        {
            get
            {
                return RwyCBox.Text;
            }
        }

        public void Subscribe()
        {
            IcaoTxtBox.TextChanged += icaoChanged;
            RwyCBox.SelectedIndexChanged += rwyChanged;
        }

        public void UnSubsribe()
        {
            IcaoTxtBox.TextChanged -= icaoChanged;
            RwyCBox.SelectedIndexChanged -= rwyChanged;
        }

        private void icaoChanged(object sender, EventArgs e)
        {
            RwyCBox.Items.Clear();
            var rwyList = airportList.RwyIdentList(Icao);

            if (rwyList != null && rwyList.Count() > 0)
            {
                RwyCBox.Items.AddRange(rwyList);
                RwyCBox.SelectedIndex = 0;
            }
        }

        private void rwyChanged(object sender, EventArgs e)
        {
            TerminalProceduresCBox.Items.Clear();
            List<string> proc = null;

            try
            {
                if (IsDepartureAirport)
                {
                    proc = SidHandlerFactory.GetHandler(
                        Icao, appSettings.NavDataLocation, wptList,
                        wptList.GetEditor(), airportList)
                        .GetSidList(Rwy);
                }
                else
                {
                    proc = StarHandlerFactory.GetHandler(
                        Icao, appSettings.NavDataLocation, wptList,
                        wptList.GetEditor(), airportList)
                        .GetStarList(Rwy);
                }
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowError(ex.Message.ToString());
            }

            if (proc.Count == 0)
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
