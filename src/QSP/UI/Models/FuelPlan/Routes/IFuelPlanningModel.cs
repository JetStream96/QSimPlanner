using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.Metar;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System.Collections.Generic;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public interface IFuelPlanningModel
    {
        AirwayNetwork AirwayNetwork { get; }
        Locator<AppOptions> AppOption { get; }
        Locator<CountryCodeManager> CountryCodeManager { get; }
        Locator<CountryCodeCollection> CheckedCountryCodes { get; }
        ProcedureFilter ProcFilter { get; }
        Locator<IWindTableCollection> WindTables { get; }
        AcConfigManager Aircrafts { get; }
        IEnumerable<FuelData> FuelData { get; }
        MetarCache MetarCache { get; }
    }

    public class FuelPlanningModel : IFuelPlanningModel
    {
        public AirwayNetwork AirwayNetwork { get; }
        public Locator<AppOptions> AppOption { get; }
        public Locator<CountryCodeManager> CountryCodeManager { get; }
        public Locator<CountryCodeCollection> CheckedCountryCodes { get; }
        public ProcedureFilter ProcFilter { get; }
        public Locator<IWindTableCollection> WindTables { get; }
        public AcConfigManager Aircrafts { get; }
        public IEnumerable<FuelData> FuelData { get; }
        public MetarCache MetarCache { get; }

        public FuelPlanningModel(
            AirwayNetwork AirwayNetwork,
            Locator<AppOptions> AppOption,
            Locator<CountryCodeManager> CountryCodeManager,
            Locator<CountryCodeCollection> CheckedCountryCodes,
            ProcedureFilter ProcFilter,
            Locator<IWindTableCollection> WindTables,
            AcConfigManager Aircrafts,
            IEnumerable<FuelData> FuelData,
            MetarCache MetarCache)
        {
            this.AirwayNetwork = AirwayNetwork;
            this.AppOption = AppOption;
            this.CountryCodeManager = CountryCodeManager;
            this.CheckedCountryCodes = CheckedCountryCodes;
            this.ProcFilter = ProcFilter;
            this.WindTables = WindTables;
            this.Aircrafts = Aircrafts;
            this.FuelData = FuelData;
            this.MetarCache = MetarCache;
        }
    }

    public static class IFuelPlanningModelExtension
    {
        public static IFinderOptionModel ToIFinderOptionModel(this IFuelPlanningModel m, 
            bool isDeparture)
        {
            return new FinderOptionModel(
                isDeparture,
                m.AppOption,
                () => m.AirwayNetwork.AirportList,
                () => m.AirwayNetwork.WptList,
                m.ProcFilter);
        }

        /// <exception cref="InvalidUserInputException"></exception>
        public static AvgWindCalculator GetWindCalculator(this IFuelPlanningModel m,
            IFuelPlanningView v)
        {
            return AvgWindCalculatorExtension.GetWindCalculator(
                m.AppOption.Instance,
                m.WindTables,
                m.AirwayNetwork.AirportList,
                v.GetFuelData(),
                v.GetZfwTon(),
                v.OrigIcao,
                v.DestIcao);
        }
    }
}
