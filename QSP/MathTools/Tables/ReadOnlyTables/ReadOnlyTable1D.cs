using QSP.MathTools.Interpolation;
using System.Collections.ObjectModel;
using System;

namespace QSP.MathTools.Tables.ReadOnlyTables
{
    public class ReadOnlyTable1D
    {
        private Table1D table;

        public ReadOnlyCollection<double> x
        {
            get
            {
                return Array.AsReadOnly(table.x);
            }
        }

        public ReadOnlyCollection<double> f
        {
            get
            {
                return Array.AsReadOnly(table.f);
            }
        }

        public ReadOnlyTable1D(Table1D table)
        {
            this.table = table;
        }

        public double ValueAt(double x)
        {
            return table.ValueAt(x);
        }
    }
}
