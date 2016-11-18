using System;
using System.Collections.Generic;

namespace QSP.MathTools.Table
{
    public class Interval
    {
        public double Lower { get; }
        public double Upper { get; }

        public Interval(double Lower, double Upper)
        {
            if (Lower > Upper) throw new ArgumentException();
            this.Lower = Lower;
            this.Upper = Upper;
        }

        public bool Intersects(Interval other)
        {
            return Lower <= other.Upper && other.Lower <= Upper;
        }

        public struct Comparer : IComparer<Interval>
        {
            public int Compare(Interval x, Interval y)
            {
                if (x.Intersects(y)) return 0;
                if (x.Lower > y.Upper) return 1;
                return -1;
            }
        }
    }
}