using QSP.LibraryExtension;
using QSP.MathTools.TablesNew;
using System;
using System.Collections.Generic;

namespace QSP.MathTools.Tables.Readers
{
    public static class TableReader1D
    {
        private static char[] spaces = new char[] { ' ', '\t' };

        public static Table Read(string text)
        {
            return Read(text, double.Parse, double.Parse);
        }

        // See unit test for examples.
        public static Table Read(
            string text, Func<string, double> xParser, Func<string, double> fParser)
        {
            var lines = text.Lines();
            var x = new List<double>();
            var f = new List<double>();

            foreach (var i in lines)
            {
                var words = i.Split(spaces, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length == 2)
                {
                    x.Add(xParser(words[0]));
                    f.Add(fParser(words[1]));
                }
            }

            return TableBuilder.Build1D(x, f);
        }
    }
}
