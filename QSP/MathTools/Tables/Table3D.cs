using QSP.LibraryExtension;
using QSP.MathTools.Interpolation;

namespace QSP.MathTools.Tables
{
    public class Table3D
    {
        public double[] x { get; set; }
        public double[] y { get; set; }
        public double[] z { get; set; }
        public double[][][] f { get; set; }

        public Table3D(double[] x, double[] y, double[] z, double[][][] f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.f = f;
        }

        public double ValueAt(double x, double y, double z)
        {
            return Interpolate3D.Interpolate(this.x, this.y, this.z, x, y, z, f);
        }

        public bool Equals(Table3D item, double delta)
        {
            return DoubleArrayCompare.Equals(x, item.x, delta) &&
                   DoubleArrayCompare.Equals(y, item.y, delta) &&
                   DoubleArrayCompare.Equals(z, item.z, delta) &&
                   DoubleArrayCompare.Equals(f, item.f, delta);
        }
    }
}
