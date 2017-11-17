using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateView
    {
        IAlternateRowView AddRow();
        void RemoveLastRow();
        IReadOnlyList<IAlternateRowView> Views { get; }
    }
}