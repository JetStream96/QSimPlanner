using static QSP.MathTools.Interpolation;

namespace QSP.MathTools
{
    public class Table1D
    {
        public double[] Array;
        public ArrayOrder Order;
        public double[] f;

        public Table1D(double[] array, double[] table) : this(array, array.GetOrder(), table) { }

        public Table1D(double[] array, ArrayOrder arrayOrder, double[] table)
        {
            this.Array = array;
            this.Order = arrayOrder;
            this.f = table;
        }

        public double ValueAt(double x)
        {
            return Interpolate(Array, x, f, Order);
        }
    }

    public class Table2D
    {
        public double[] XArray;
        public double[] YArray;
        public ArrayOrder XOrder;
        public ArrayOrder YOrder;

        public double[,] f;

        public Table2D(double[] xArray, double[] yArray, double[,] table)
            : this(xArray, yArray, xArray.GetOrder(), yArray.GetOrder(), table)
        { }

        public Table2D(double[] xArray, double[] yArray, ArrayOrder xOrder, ArrayOrder yOrder, double[,] table)
        {
            this.XArray = xArray;
            this.YArray = yArray;
            this.XOrder = xOrder;
            this.YOrder = yOrder;
            this.f = table;
        }

        public double ValueAt(double x, double y)
        {
            return Interpolate(XArray, YArray, x, y, f, XOrder, YOrder);
        }

    }
    
    public class Table3D
    {
        public double[] XArray;
        public double[] YArray;
        public double[] ZArray;
        public ArrayOrder XOrder;
        public ArrayOrder YOrder;
        public ArrayOrder ZOrder;

        public double[,,] f;

        public Table3D(double[] xArray, double[] yArray, double[] zArray, double[,,] table)
        : this(xArray, yArray, zArray, xArray.GetOrder(), yArray.GetOrder(), zArray.GetOrder(), table)
        { }

        public Table3D(double[] xArray, double[] yArray, double[] zArray, ArrayOrder xOrder, ArrayOrder yOrder, ArrayOrder zOrder, double[,,] table)
        {
            this.XArray = xArray;
            this.YArray = yArray;
            this.ZArray = zArray;
            this.XOrder = xOrder;
            this.YOrder = yOrder;
            this.ZOrder = zOrder;
            f = table;
        }

        public double ValueAt(double x, double y, double z)
        {
            return Interpolate(XArray, YArray, ZArray, x, y, z, f, XOrder, YOrder, ZOrder);
        }

    }

}
