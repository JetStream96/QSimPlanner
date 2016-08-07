using System;

namespace QSP.MathTools
{
    public class Matrix2by2
    {
        public double A11;
        public double A12;
        public double A21;
        public double A22;

        //
        //    (a11  a12)
        // A= (        )
        //    (a21  a22)

        public Matrix2by2(
            double A11, double A12, double A21, double A22)
        {
            this.A11 = A11;
            this.A12 = A12;
            this.A21 = A21;
            this.A22 = A22;
        }

        public double Determinant { get { return A11 * A22 - A12 * A21; } }

        public Vector2D Multiply(Vector2D v)
        {
            return new Vector2D(A11 * v.X + A12 * v.Y, A21 * v.X + A22 * v.Y);
        }

        public void Multiply(double c)
        {
            A11 *= c;
            A12 *= c;
            A21 *= c;
            A22 *= c;
        }

        public Matrix2by2 Inverse()
        {
            var det = Determinant;
            if (det == 0.0) throw new ArgumentException();
            var B = new Matrix2by2(A22, -A12, -A21, A11);
            B.Multiply(1.0 / det);
            return B;
        }
    }

}
