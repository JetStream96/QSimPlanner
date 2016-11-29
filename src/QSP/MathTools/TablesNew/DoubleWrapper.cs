using System.Collections.Generic;

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
}