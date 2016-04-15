using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using QSP.MathTools.Interpolation;
using QSP.Utilities;
using System;

namespace QSP.MathTools.Tables
{
    public class Table2D
    {
        public double[] x { get; set; }
        public double[] y { get; set; }
        public double[][] f { get; set; }

        public Table2D(double[] x, double[] y, double[][] f)
        {
            this.x = x;
            this.y = y;
            this.f = f;
        }

        public double ValueAt(double x, double y)
        {
            return Interpolate2D.Interpolate(this.x, this.y, x, y, f);
        }
        
        public bool Equals(Table2D item, double delta)
        {
            return DoubleArrayCompare.Equals(x, item.x, delta) &&
                   DoubleArrayCompare.Equals(y, item.y, delta) &&
                   DoubleArrayCompare.Equals(f, item.f, delta);
        }

        private void validate()
        {
            ConditionChecker.Ensure<ArgumentException>(
                LengthChecker.HasLength<double>(f, x.Length, y.Length) &&
                x.IsValidAxis() &&
                y.IsValidAxis());
        }
    }
}
