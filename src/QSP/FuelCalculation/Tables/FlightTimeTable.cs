using System;
using System.Collections.Generic;
using QSP.MathTools.Tables;
using QSP.LibraryExtension;

namespace QSP.FuelCalculation.Tables
{
    public class FlightTimeTable
    {
        private Table1D table;

        public FlightTimeTable(string text)
        {
            table = Convert(text);
        }

        public double GetTimeMin(double airDis)
        {
            return table.ValueAt(airDis);
        }

        private Table1D Convert(string text)
        {
            var lines = text.Lines();
            var sep = new char[] { ' ', '\t' };
            var x = new List<double>();
            var f = new List<double>();

            foreach (var i in lines)
            {
                var words = i.Split(
                    sep, StringSplitOptions.RemoveEmptyEntries);
                double valX;
                int valF;

                if (words.Length == 2 &&
                    double.TryParse(words[0], out valX) &&
                    TimeFormat.HourColonMinToMin(words[1], out valF))
                {
                    x.Add(valX);
                    f.Add(valF);
                }
            }

            return new Table1D(x.ToArray(), f.ToArray());
        }
    }
}
