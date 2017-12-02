using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateView
    {
        void AddRow();
        IEnumerable<IAlternateRowView> Subviews { get; }
    }
}