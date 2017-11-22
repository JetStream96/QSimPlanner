using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateView
    {
        IAlternateRowView AddRow();
        IEnumerable<IAlternateRowView> Views { get; }
    }
}