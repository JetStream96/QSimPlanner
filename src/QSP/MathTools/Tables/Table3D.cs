using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using QSP.MathTools.Interpolation;
using QSP.Utilities;
using System;

namespace QSP.MathTools.Tables
{
    public class Table3D
    {
        public double[] x { get; set; }
        public double[] y { get; set; }
        public double[] z { get; set; }
        public double[][][] f { get; set; }

        /// <exception cref="ArgumentException"></exception>
        public Table3D(double[] x, double[] y, double[] z, double[][][] f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.f = f;

            Validate();
        }

        public double ValueAt(double x, double y, double z)
        {
            return Interpolate3D.Interpolate(
                this.x, this.y, this.z, x, y, z, f);
        }

        public bool Equals(Table3D item, double delta)
        {
            return DoubleArrayCompare.Equals(x, item.x, delta) &&
                   DoubleArrayCompare.Equals(y, item.y, delta) &&
                   DoubleArrayCompare.Equals(z, item.z, delta) &&
                   DoubleArrayCompare.Equals(f, item.f, delta);
        }

        /// <exception cref="ArgumentException"></exception>
        public void Validate()
        {
            bool hasLen = LengthChecker.HasLength<double>(
                    f, x.Length, y.Length, z.Length);

            ConditionChecker.Ensure<ArgumentException>(
                hasLen &&
                x.IsValidAxis() &&
                y.IsValidAxis() &&
                z.IsValidAxis());
        }
    }
}
