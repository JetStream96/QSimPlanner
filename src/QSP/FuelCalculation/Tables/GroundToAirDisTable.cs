using QSP.MathTools.Tables.Readers;
using QSP.MathTools.Tables;

namespace QSP.FuelCalculation.Tables
{
    public class GroundToAirDisTable
    {
        private Table2D table;

        public GroundToAirDisTable(string text)
        {
            table = TableReader2D.Read(text);
        }

        public double GetAirDistance(double groundDis, double tailWind)
        {
            return table.ValueAt(groundDis, tailWind);
        }
    }
}
