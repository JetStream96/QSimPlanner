using QSP.AviationTools;
using QSP.MathTools.Interpolation;
using System.Linq;

namespace QSP.WindAloft
{

    public class WxFileLoader
    {
        private WindTable[] windTables = new WindTable[Utilities.FullWindDataSet.Count()];

        public void ImportAllTables()
        {
            // For 100mb, u_table = wx1.csv, v_table = wx2.csv
            // For 200mb, u_table = wx3.csv, v_table = wx4.csv
            // ...
            
            for (int i = 0; i < Utilities.FullWindDataSet.Length; i++)
            {
                var table = new WindTable();

                string u = Utilities.WxFileDirectory + @"\wx" + (i * 2 + 1).ToString() + ".csv";
                string v = Utilities.WxFileDirectory + @"\wx" + (i * 2 + 2).ToString() + ".csv";

                table.LoadFromFile(u, v);
                windTables[i] = table;
            }
        }

        public WindUV GetWindUv(double lat, double lon, double FL)
        {
            double press = CoversionTools.AltToPressureMb(FL * 100);
            int index = GetIndicesForInterpolation(press);

            double uWind = Interpolate1D.Interpolate(
                Utilities.FullWindDataSet[index], Utilities.FullWindDataSet[index + 1],
                windTables[index].GetUWind(lat, lon), windTables[index + 1].GetUWind(lat, lon),
                press);

            double vWind = Interpolate1D.Interpolate(
                Utilities.FullWindDataSet[index], Utilities.FullWindDataSet[index + 1],
                windTables[index].GetVWind(lat, lon), windTables[index + 1].GetVWind(lat, lon),
                press);

            return new WindUV(uWind, vWind);
        }

        private int GetIndicesForInterpolation(double press)
        {
            //let the return value be x, use indices x and x+1 for interpolation
            //works for extrapolation as well

            int len = Utilities.FullWindDataSet.Length;

            if (press < Utilities.FullWindDataSet[0])
            {
                return 0;
            }

            for (int i = 0; i < len - 1; i++)
            {
                if (press >= Utilities.FullWindDataSet[i] && 
                    press <= Utilities.FullWindDataSet[i + 1])
                {
                    return i;
                }
            }

            return len - 2;

        }

    }
}

