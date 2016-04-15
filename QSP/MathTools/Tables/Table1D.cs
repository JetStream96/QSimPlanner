using QSP.MathTools.Interpolation;
using QSP.LibraryExtension;
using QSP.Utilities;
using System;

namespace QSP.MathTools.Tables
{
    public class Table1D
    {
        public double[] x { get; set; }
        public double[] f { get; set; }

        /// <exception cref="ArgumentException"></exception>
        public Table1D(double[] x, double[] f)
        {
            this.x = x;
            this.f = f;

            Validate();
        }

        public double ValueAt(double x)
        {
            return Interpolate1D.Interpolate(this.x, f, x);
        }

        public bool Equals(Table1D item, double delta)
        {
            return DoubleArrayCompare.Equals(x, item.x, delta) &&
                   DoubleArrayCompare.Equals(f, item.f, delta);
        }

        /// <exception cref="ArgumentException"></exception>
        public void Validate()
        {
            ConditionChecker.Ensure<ArgumentException>(
                f.Length >= x.Length &&
                x.IsValidAxis());
        }
    }
}
