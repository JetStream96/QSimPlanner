using System.Linq;

namespace QSP.MathTools.Tables
{
    public static class TableUtil
    {
        public static Table1D TruncateRepeatedXValues(this Table1D table)
        {
            var count = NonRepeatedCount(table.x);
            var x = table.x.Take(count).ToArray();
            var f = table.f.Take(count).ToArray();
            return new Table1D(x, f);
        }

        private static int NonRepeatedCount(double[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] == array[i + 1]) return i + 1;
            }

            return array.Length;
        }
    }
}