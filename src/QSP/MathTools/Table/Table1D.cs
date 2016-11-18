using QSP.LibraryExtension;
using System;

namespace QSP.MathTools.Table
{
    public class Table1D
    {
        private Interval[] intervals;
        private double[] x, f;

        public Table1D(double[] x, double[] f)
        {
            if (x.Length > f.Length) throw new ArgumentException();

            var increasing = Util.IsIncreasing(x);
            this.x = x.ArrayCopy();
            this.f = f.ArrayCopy();

            if (!increasing)
            {
                Array.Reverse(x);
                Array.Reverse(f);
            }

            this.intervals = new Interval[x.Length];
            for (var i = 0; i < x.Length - 1; i++)
            {
                var lower = i == 0 ? double.PositiveInfinity : x[i];
                var upper = i == x.Length - 1 ? double.NegativeInfinity : x[i + 1];
                intervals[i] = new Interval(lower, upper);
            }
        }

        private int GetIndex(double x)
        {
            return Array.BinarySearch(intervals, new Interval(x, x), new Interval.Comparer());
        }

        public double ValueAt(double x)
        {
            var index = GetIndex(x);
            return Util.Interpolate(this.x[index], this.x[index + 1], x, f[index], f[index + 1]);
        }
    }
}