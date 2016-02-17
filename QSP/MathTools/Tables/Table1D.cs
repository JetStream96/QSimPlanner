using QSP.MathTools.Interpolation;

namespace QSP.MathTools.Tables
{
    public class Table1D
    {
        private double[] x;
        private double[] f;
        
        public Table1D(double[] x, double[] f)
        {
            this.x = x;
            this.f = f;
        }

        public double ValueAt(double x)
        {
            return Interpolate1D.Interpolate(this.x, f, x);
        }
    }
}
