using QSP.MathTools.Interpolation;
using QSP.LibraryExtension;

namespace QSP.MathTools.Tables
{
    public class Table1D
    {
        public double[] x { get; set; }
        public double[] f { get; set; }

        public Table1D(double[] x, double[] f)
        {
            this.x = x;
            this.f = f;
        }

        public double ValueAt(double x)
        {
            return Interpolate1D.Interpolate(this.x, f, x);
        }

        public bool Equals(Table1D item,double delta)
        {
            return DoubleArrayCompare.Equals(x, item.x, delta) &&
                   DoubleArrayCompare.Equals(f, item.f, delta);
        }
    }
}
