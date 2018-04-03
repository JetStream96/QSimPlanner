using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using QSP.MathTools.Interpolation;
using QSP.Utilities;
using System;

namespace QSP.MathTools.Tables
{
    public class Table2D
    {
        public double[] x { get; }
        public double[] y { get; }
        public double[][] f { get; }

        /// <exception cref="ArgumentException"></exception>
        public Table2D(double[] x, double[] y, double[][] f)
        {
            this.x = x;
            this.y = y;
            this.f = f;

            Validate();
        }

        public double ValueAt(double x, double y)
        {
            return Interpolate2D.Interpolate(this.x, this.y, f, x, y);
        }

        public bool Equals(Table2D item, double delta)
        {
            return item != null &&
                DoubleArrayCompare.Equals(x, item.x, delta) &&
                DoubleArrayCompare.Equals(y, item.y, delta) &&
                DoubleArrayCompare.Equals(f, item.f, delta);
        }

        /// <exception cref="ArgumentException"></exception>
        public void Validate()
        {
            ExceptionHelpers.Ensure<ArgumentException>(
                LengthChecker.HasLength<double>(f, x.Length, y.Length));
        }
    }
}
