using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.MathTools.Interpolation.Common;
using static QSP.MathTools.Interpolation.Interpolate1D;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.MathTools.TablesNew
{
    public class Table : ITable
    {
        public int Dimension { get; }
        public IReadOnlyList<double> XValues { get; }
        public IReadOnlyList<ITable> FValues { get; }

        /// <exception cref="ArgumentException"></exception>
        public Table(IReadOnlyList<double> XValues, IReadOnlyList<ITable> FValues)
        {
            this.XValues = XValues;
            this.FValues = FValues;
            Dimension = FValues[0].Dimension + 1;

            Validate();
        }
        
        private void Validate()
        {
            Ensure<ArgumentException>(
                FValues.Count == XValues.Count &&
                (XValues.IsStrictlyDecreasing() || XValues.IsStrictlyIncreasing()) &&
                FValues.All(v => v.Dimension == Dimension - 1));
        }

        public double ValueAt(params double[] X) => ValueAt((IReadOnlyList<double>)X);

        public double ValueAt(IReadOnlyList<double> X)
        {
            var slice = new Slice<double>(X, 1);
            var index = GetIndex(XValues, X[0]);
            var f0 = FValues[index].ValueAt(slice);
            var f1 = FValues[index + 1].ValueAt(slice);
            return Interpolate(XValues[index], XValues[index + 1], f0, f1, X[0]);
        }
    }
}