using System.Collections.Generic;
using System.Linq;

namespace QSP.MathTools.TablesNew
{
    // Some convenient methods to build tables from double arrays/lists.
    //
    public static class TableBuilder
    {
        public static Table Build1D(IReadOnlyList<double> xValues, IReadOnlyList<double> fValues)
        {
            return new Table(xValues, fValues.WrapperList());
        }

        public static Table Build2D(IReadOnlyList<double> xValues, IReadOnlyList<double> yValues, 
            IReadOnlyList<IReadOnlyList<double>> fValues)
        {
            var tables = fValues.Select(v => Build1D(yValues, v));
            return new Table(xValues, tables.ToList());
        }
    }
}