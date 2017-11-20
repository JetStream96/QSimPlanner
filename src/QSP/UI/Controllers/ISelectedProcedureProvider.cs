using QSP.UI.Presenters.FuelPlan.Route;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Controllers
{
    /// <summary>
    /// Provides the airport information selected by the user.
    /// </summary>
    public interface ISelectedProcedureProvider
    {
        string Icao { get; }
        string Rwy { get; }
        List<string> GetSelectedProcedures();//TODO:Use IEnumerable instead?
    }

    public static class ISelectedProcedureProviderHelper
    {
        public static IEnumerable<string> GetSelectedProcedures(this ComboBox c)
        {
            if (c.Text == FinderOptionPresenter.AutoProcedureTxt)
            {
                return c.Items.Cast<string>()
                    .Where(s => s != FinderOptionPresenter.AutoProcedureTxt);
            }

            if (c.Text != FinderOptionPresenter.NoProcedureTxt)
            {
                return new[] { c.Text };
            }

            return new string[0];
        }
    }
}
