using System.Collections.Generic;

namespace QSP.MathTools.TableNew
{
    // Given a n-dimensional table, with size m_1 * m_2 * ... * m_n.
    // 
    // For example, this table
    // 
    //    | 20  30
    // ___|_________
    //  8 |  7   9
    // 10 | 11  13
    //
    // The dimension is 2. 
    // X(0) = [8, 10], X(1) = [20, 30].
    // F =[[7, 9], [11, 13]]
    // e.g. ValueAt([8, 30]) = 9, values are computed using interpolation.

    public interface ITable
    {
        int Dimension { get; }
        IReadOnlyList<double> X(int dimension);
        object F { get; }
        double ValueAt(IReadOnlyList<double> x);
    }
}