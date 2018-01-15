using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Views.FuelPlan.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.LibraryExtension;
using QSP.AviationTools;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    // Manages the selected SIDs/STARs by the user. 
    // This class is responsible for the SID/STAR ComboBoxes, the filter button,
    // and the instantiation of the filter form.
    //
    public class FinderOptionPresenter : ISelectedProcedureProvider, IRefreshForNavDataChange
    {
        public event EventHandler IcaoChanged;

        public static readonly string NoProcedureTxt = "NONE";
        public static readonly string AutoProcedureTxt = "AUTO";

        private readonly IFinderOptionView view;
        private readonly IFinderOptionModel model;

        public WaypointList WptList => model.WptList();
        public AirportManager AirportList => model.AirportList();
        public string NavDataLocation => model.AppOptions.Instance.NavDataLocation;
        public string SelectedProcedureText
        {
            get => view.SelectedProcedureText; set => view.SelectedProcedureText = value;
        }

        public FinderOptionPresenter(IFinderOptionView view, IFinderOptionModel model)
        {
            this.view = view;
            this.model = model;

            view.IsOrigin = model.IsDepartureAirport;
        }

        public string Icao { get => view.Icao; set => view.Icao = value; }
        public string Rwy { get => view.SelectedRwy; set => view.SelectedRwy = value; }
        public IEnumerable<string> GetSelectedProcedures() => view.SelectedProcedures;

        /// <summary>
        /// Update the runway list and automatically select one based on wind. 
        /// </summary>
        public async void UpdateRunways()
        {
            var icaoBeforeAwait = Icao;
            var rwyList = AirportList.RwyIdents(Icao)?.ToArray();

            if (rwyList != null && rwyList.Length > 0)
            {
                view.Runways = rwyList;
                var runways = AirportList[icaoBeforeAwait].Rwys;
                var (success, rwy) = await SelectRwyBasedOnWeather(runways);

                // Only choose runway based on weather if the ICAO does not change
                // during the await.
                view.SelectedRwy = (success && icaoBeforeAwait == Icao) ? rwy : rwyList[0];
            }
            else
            {
                view.Runways = new string[0];
            }
        }

        private async Task<(bool success, string rwy)> SelectRwyBasedOnWeather(
            IEnumerable<IRwyData> rwys)
        {
            int timeoutMs = 1000;
            var task = Task.Factory.StartNew(() => MetarDownloader.DownloadMetar(view.Icao));

            if (await Task.WhenAny(task, Task.Delay(timeoutMs)) == task)
            {
                // Task completed within timeout
                var (downloaded, metar) = await task;
                if (!downloaded) return (false, null);
                var wind = ParaExtractor.GetWind(metar);
                if (!wind.HasValue) return (false, null);
                return (true, SelectRwy(rwys, wind.Value.Direction));
            }
            else
            {
                // Timeout
                return (false, null);
            }
        }

        // TODO: Move this.
        private static string SelectRwy(IEnumerable<IRwyData> rwys, double windHeading)
        {
            return rwys
                .Where(r => r.Heading.IsDouble())
                .MaxBy(r => ConversionTools.HeadwindComponent(
                    double.Parse(r.Heading), windHeading, 1.0))
                .RwyIdent;
        }

        /// <summary>
        /// A list of SID or STAR, depending on whether this is origin 
        /// or destination airport.
        /// </summary>
        /// <exception cref="LoadSidFileException"></exception>
        /// <exception cref="LoadStarFileException"></exception>
        public List<string> AvailableProcedures
        {
            get
            {
                if (model.IsDepartureAirport)
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
            catch { }
        }

        private bool ShouldShow(string proc)
        {
            var filter = model.ProcFilter;
            var isDep = model.IsDepartureAirport;

            if (!filter.Exists(Icao, Rwy, isDep)) return true;
            var info = filter[Icao, Rwy, isDep];
            return info.Procedures.Contains(proc) ^ info.IsBlackList;
        }

        public SidStarFilterPresenter GetFilterPresenter(ISidStarFilterView v)
        {
            return new SidStarFilterPresenter(
                v,
                Icao,
                Rwy,
                AvailableProcedures,
                model.IsDepartureAirport,
                model.ProcFilter);
        }

        public void OnIcaoChanged(object s, EventArgs e) => IcaoChanged?.Invoke(s, e);

        public void OnNavDataChange()
        {
            var rwy = view.SelectedRwy;
            var proc = view.SelectedProcedures;
            UpdateRunways();
            view.SelectedRwy = rwy;
            view.SelectedProcedures = proc;
        }
    }
}