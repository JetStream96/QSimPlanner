using QSP.LibraryExtension.JaggedArrays;
using QSP.MathTools.Interpolation;
using QSP.Utilities;
using System;

namespace QSP.MathTools.Tables
{
    public class Table4D
    {
        private double[] x;
        private double[] y;
        private double[] z;
        private double[] t;
        private double[][][][] f;

        public Table4D(double[] x, double[] y, double[] z, double[] t,
                       double[][][][] f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.t = t;
            this.f = f;
        }

        public double ValueAt(double x, double y, double z, double t)
        {
            return Interpolate4D.Interpolate(
                this.x, this.y, this.z, this.t, x, y, z, t, f);
        }

        private void validate()
        {
            bool hasLen = LengthChecker.HasLength<double>(
                    f, x.Length, y.Length, z.Length, t.Length);

            ConditionChecker.Ensure<ArgumentException>(
                hasLen &&
                x.IsValidAxis() &&
                y.IsValidAxis() &&
                z.IsValidAxis() &&
                t.IsValidAxis());
        }
    }
}
