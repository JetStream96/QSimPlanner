using QSP.MathTools.Interpolation;

namespace QSP.MathTools.Tables
{
    public class Table3D
    {
        private double[] x;
        private double[] y;
        private double[] z;
        private double[][][] f;
        
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
    }
}
