using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using QSP.MathTools.TablesNew;

namespace QSP.MathTools.Tables.Readers
{
    public static class TableReader2D
    {
        private static char[] spaces = new char[] { ' ', '\t' };

        // See unit test for examples.
        public static Table Read(string source)
        {
            var lines = source.Lines();

            int lineNum = 0;
            var y = GetNextLine(lines, ref lineNum).ToDoubles();

            var x = new List<double>();
            var f = new List<double[]>();

            while (true)
            {
                var currentLine = GetNextLine(lines, ref lineNum);

                if (currentLine == null)
                {
                    return TableBuilder.Build2D(x, y, f);
                }
                else if (currentLine.Length >= 2)
                {
                    x.Add(double.Parse(currentLine[0]));
                    f.Add(currentLine.SubArray(1).ToDoubles());
                }
            }
        }

        private static string[] GetNextLine(string[] lines, ref int lineNum)
        {
            while (lineNum < lines.Length)
            {
                var words = lines[lineNum].Split(spaces, StringSplitOptions.RemoveEmptyEntries);
                lineNum++;
                if (words.Length > 0) return words;
            }

            return null;
        }
    }
}
