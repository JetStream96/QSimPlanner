using QSP.MathTools.Interpolation;
using System;
using System.Collections.ObjectModel;
using QSP.LibraryExtension.JaggedArray;

namespace QSP.MathTools.Tables.ReadOnlyTables
{
    public class ReadOnlyTable2D
    {
        private Table2D table;

        public ReadOnlyCollection<double> x
        {
            get
            {
                return Array.AsReadOnly(table.x);
            }
        }

        public ReadOnlyCollection<double> y
        {
            get
            {
                return Array.AsReadOnly(table.y);
            }
        }

        public ReadOnlyJaggedArray2D<double> f
        {
            get
            {
                return new ReadOnlyJaggedArray2D<double>(table.f);
            }
        }

        public ReadOnlyTable2D(Table2D table)
        {
            this.table = table;
        }

        public double ValueAt(double x, double y)
        {
            return table.ValueAt(x, y);
        }
    }
}
