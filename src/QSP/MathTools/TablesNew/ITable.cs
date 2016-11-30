using System.Collections.Generic;

namespace QSP.MathTools.TablesNew
{
    public interface ITable
    {
        int Dimension { get; }
        double ValueAt(IReadOnlyList<double> X);
        IReadOnlyList<double> XValues { get; }
        IReadOnlyList<ITable> FValues { get; }
    }
}