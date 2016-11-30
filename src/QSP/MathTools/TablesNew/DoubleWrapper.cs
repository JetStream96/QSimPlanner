using System.Collections.Generic;
using System.Linq;

namespace QSP.MathTools.TablesNew
{
    public class DoubleWrapper : ITable
    {
        private double value;

        public int Dimension => 0;
        public double ValueAt(IReadOnlyList<double> X) => value;

        public DoubleWrapper(double value)
        {
            this.value = value;
        }
    }

    public static class DoubleWrapperExtension
    {
        public static DoubleWrapper Wrap(this double x) => new DoubleWrapper(x);

        public static List<DoubleWrapper> WrapperList(this IEnumerable<double> x)
        {
            return x.Select(i => i.Wrap()).ToList();
        }
    }
}