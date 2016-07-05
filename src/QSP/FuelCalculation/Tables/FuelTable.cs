using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                
        // airDis: NM, landingWt: Ton, Fuel: Ton
        public double GetFuelRequired(double airDis, double landingWt)
        {
            return table.ValueAt(airDis, landingWt);
        }
    }
}
