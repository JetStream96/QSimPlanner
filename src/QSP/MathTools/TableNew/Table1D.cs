using QSP.LibraryExtension;
using System;
using System.Collections.Generic;

namespace QSP.MathTools.TableNew
{
    public class Table1D : ITable
    {
        private Interval[] intervals;
        private double[] x, f;

        public int Dimension => 1;
        public object F => f;

        public Table1D(double[] x, double[] f)
        {
            var len = x.Length;
            if (len > f.Length) throw new ArgumentException();

            var increasing = Util.IsIncreasing(x);
            this.x = x.ArrayCopy();
            this.f = f.ArrayCopy();

            if (!increasing)
            {
                Array.Reverse(x);
                Array.Reverse(f);
            }

            this.intervals = new Interval[len - 1];
            for (var i = 0; i < len - 1; i++)
            {
                var lower = i == 0 ? double.NegativeInfinity : x[i];
                var upper = i == len - 2 ? double.PositiveInfinity : x[i + 1];
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

        public IReadOnlyList<double> X(int dimension)
        {
            if (dimension != 0) throw new ArgumentException();
            return x;
        }

        public double ValueAt(IReadOnlyList<double> x)
        {
            return ValueAt(x[0]);
        }
    }
}