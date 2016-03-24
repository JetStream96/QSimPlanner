using QSP.MathTools.Interpolation;
using QSP.MathTools.Tables.ReadOnlyTables;
using System;
using System.Collections.ObjectModel;
using QSP.LibraryExtension.JaggedArray;

namespace QSP.MathTools.Tables.ReadOnlyTables
{
    public class ReadOnlyTable3D
    {
        private Table3D table;

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

        public ReadOnlyCollection<double> z
        {
            get
            {
                return Array.AsReadOnly(table.z);
            }
        }

        public ReadOnlyJaggedArray3D<double> f
        {
            get
            {
                return new ReadOnlyJaggedArray3D<double>(table.f);
            }
        }

        public ReadOnlyTable3D(Table3D table)
        {
            this.table = table;
        }

        public double ValueAt(double x, double y, double z)
        {
            return table.ValueAt(x, y, z);
        }
    }
}
