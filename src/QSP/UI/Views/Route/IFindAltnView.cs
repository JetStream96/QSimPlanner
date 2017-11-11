using QSP.RouteFinding.Airports;
using QSP.Utilities.Units;
using System.Collections.Generic;

// TODO: This namespace should be moved into FuelPlan.
namespace QSP.UI.Views.Route
{
    public interface IFindAltnView
    {
        string ICAO { get; }
        string Length { get; }
        LengthUnit LengthUnit { get; }
        string SelectedIcao { get; }
        void ShowWarning(string msg);
        IReadOnlyList<AlternateFinder.AlternateInfo> Alternates { set; }
    }
}
