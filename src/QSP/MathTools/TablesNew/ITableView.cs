using System.Collections.Generic;

namespace QSP.MathTools.TablesNew
{
    public interface ITableView : ITable
    {
        IReadOnlyList<double> XValues { get; }
        IReadOnlyList<ITable> FValues { get; }
    }
}