using System;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using QSP.WindAloft;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public interface IActionContextMenuModel
    {
        ISelectedProcedureProvider Orig { get; }
        ISelectedProcedureProvider Dest { get; }
        Locator<AppOptions> AppOptions { get; }
        AirwayNetwork AirwayNetwork { get; }
        Locator<CountryCodeCollection> CheckedCodes { get; }
        Func<AvgWindCalculator> WindCalc { get; }
    }

    public class ActionContextMenuModel
    {
        public ISelectedProcedureProvider Orig { get; }
        public ISelectedProcedureProvider Dest { get; }
        public Locator<AppOptions> AppOptions { get; }
        public AirwayNetwork AirwayNetwork { get; }
        public Locator<CountryCodeCollection> CheckedCodes { get; }
        public Func<AvgWindCalculator> WindCalc { get; }

        public ActionContextMenuModel(
            ISelectedProcedureProvider Orig,
            ISelectedProcedureProvider Dest,
            Locator<AppOptions> AppOptions,
            AirwayNetwork AirwayNetwork,
            Locator<CountryCodeCollection> CheckedCodes,
            Func<AvgWindCalculator> WindCalc)
        {
            this.Orig = Orig;
            this.Dest = Dest;
            this.AppOptions = AppOptions;
            this.AirwayNetwork = AirwayNetwork;
            this.CheckedCodes = CheckedCodes;
            this.WindCalc = WindCalc;
        }
    }
}