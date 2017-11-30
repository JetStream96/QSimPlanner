using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.UI.Controllers;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Views.FuelPlan.Route;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.UI.Presenters.FuelPlan.Route
{
    // Manages the selected SIDs/STARs by the user. 
    // This class is responsible for the SID/STAR ComboBoxes, the filter button,
    // and the instantiation of the filter form.
    //
    public class FinderOptionPresenter : ISelectedProcedureProvider, IRefreshForOptionChange
    {
        public event EventHandler IcaoChanged;

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
        public string SelectedProcedureText
        {
            get => view.SelectedProcedureText; set => view.SelectedProcedureText = value;
        }

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

            view.IsOrigin = IsDepartureAirport;
        }

        public string Icao { get => view.Icao; set => view.Icao = value; }
        public string Rwy { get => view.SelectedRwy; set => view.SelectedRwy = value; }
        public List<string> GetSelectedProcedures() => view.SelectedProcedures.Strings.ToList();

        public void UpdateRunways()
        {
            var rwyList = AirportList.RwyIdents(Icao)?.ToArray();

            if (rwyList != null && rwyList.Length > 0)
            {
                view.Runways = rwyList;
                view.SelectedRwy = rwyList[0];
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

        public void UpdateProcedures()
        {
            try
            {
                view.Procedures = AvailableProcedures.Where(ShouldShow);
            }
            catch (Exception ex)
            {
                view.ShowMessage(ex.Message, Views.MessageLevel.Error);
            }
        }

        private bool ShouldShow(string proc)
        {
            if (!ProcFilter.Exists(Icao, Rwy)) return true;
            var info = ProcFilter[Icao, Rwy];
            return info.Procedures.Contains(proc) ^ info.IsBlackList;
        }
        
        public SidStarFilterPresenter GetFilterPresenter(ISidStarFilterView v)
        {
            return new SidStarFilterPresenter(
                v,
                Icao,
                Rwy,
                AvailableProcedures,
                IsDepartureAirport,
                ProcFilter);
        }

        public void OnIcaoChanged(object s, EventArgs e) => IcaoChanged?.Invoke(s, e);

        public void RefreshForAirportListChange()
        {
            var rwy = view.SelectedRwy;
            var proc = view.SelectedProcedures;
            UpdateRunways();
            view.SelectedRwy = rwy;
            view.SelectedProcedures = proc;
        }

        public void RefreshForNavDataLocationChange()
        {
            RefreshForNavDataLocationChange();
        }
    }
}