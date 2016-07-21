using QSP.MathTools.Tables.Readers;
using QSP.MathTools.Tables;

namespace QSP.FuelCalculation.Tables
{
    public class FuelTable
    {
        private Table2D table;

        public FuelTable(string text)
        {
            table = TableReader2D.Read(text);
        }

        public double GetFuelRequiredTon(double airDisNm, double landingWtTon)
        {
            return table.ValueAt(airDisNm, landingWtTon);
        }
    }
}
