using QSP.MathTools.Tables.Readers;
using QSP.MathTools.Tables;

namespace QSP.FuelCalculation.FuelData
{
    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public class FuelTable : IFuelTable
    {
        private readonly Table2D table;

        public FuelTable(string text)
        {
            table = TableReader2D.Read(text);
        }

        public double FuelRequired(double airDistance, double landingWt)
        {
            return table.ValueAt(airDistance, landingWt);
        }
    }
}
