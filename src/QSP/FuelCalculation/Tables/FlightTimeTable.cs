using System;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
using QSP.LibraryExtension;

namespace QSP.FuelCalculation.Tables
{
    public class FlightTimeTable
    {
        private Table1D table;

        public FlightTimeTable(string text)
        {
            Func<string, double> timeParser =
                t => TimeFormat.HourColonMinToMin(t);

            table = TableReader1D.Read(text, double.Parse, timeParser);
        }

        public double GetTimeMin(double airDis)
        {
            return table.ValueAt(airDis);
        }
    }
}
