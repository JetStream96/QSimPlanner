using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System.Collections.Generic;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public class FuelPlanningModel
    {
        public AirwayNetwork AirwayNetwork { get; set; }
        public Locator<AppOptions> AppOption { get; set; }
        public Locator<CountryCodeManager> CountryCodeManager { get; set; }
        public Locator<CountryCodeCollection> CheckedCountryCodes { get; set; }
        public ProcedureFilter ProcFilter { get; set; }
        public Locator<IWxTableCollection> WindTables { get; set; }
        public AcConfigManager Aircrafts { get; set; }
        public IEnumerable<FuelData> FuelData { get; set; }
    }

    public static class FuelPlanningModelExtension
    {
        public static FinderOptionModel ToIFinderOptionModel(this FuelPlanningModel m,
            bool isDeparture)
        {
            return new FinderOptionModel()
            {
                IsDepartureAirport = isDeparture,
                AppOptions = m.AppOption,
                AirportList = () => m.AirwayNetwork.AirportList,
                WptList = () => m.AirwayNetwork.WptList,
                ProcFilter = m.ProcFilter
            };
        }

        /// <exception cref="InvalidUserInputException"></exception>
        public static AvgWindCalculator GetWindCalculator(this FuelPlanningModel m,
            FuelPlanningControl c)
        {
            return AvgWindCalculatorExtension.GetWindCalculator(
                m.AppOption.Instance,
                m.WindTables,
                m.AirwayNetwork.AirportList,
                c.GetFuelData(),
                c.GetZfwTon(),
                c.OrigIcao,
                c.DestIcao);
        }
    }
}
