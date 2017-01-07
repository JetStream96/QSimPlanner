using QSP.LibraryExtension;
using QSP.Utilities;
using System;
using System.Linq;

namespace QSP.MathTools.Tables
{
    public static class TableUtil
    {
        public static Table1D TruncateInvalidXValues(this Table1D table)
        {
            var oldX = table.x;
            ExceptionHelpers.Ensure<ArgumentException>(oldX.Length > 0);
            var increasing = oldX.Length == 1 || (oldX[1] > oldX[0]);
            var count = increasing ? oldX.StrictlyIncreasingCount() : oldX.StrictlyDecreasingCount();
            var x = oldX.Take(count).ToArray();
            var f = table.f.Take(count).ToArray();
            return new Table1D(x, f);
        }
    }
}