using QSP.MathTools.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.TOPerfCalculation.Airbus.DataClasses
{
    /// <summary>
    /// Unit and numerical values of the properties are exactly the same as described
    /// in file a320_200_CFM56_TO.xml.
    /// </summary>
    public class AirbusPerfTable : PerfTableItem
    {
        public Table1D HeadwindCorrectionTable { get; set; }
        public Table1D TailwindCorrectionTable { get; set; }
        public Table1D UphillCorrectionTable { get; set; }
        public Table1D DownHillCorrectionTable { get; set; }

        /// <summary>
        /// Note: No two TableDataNodes can have the same Flaps and IsaOffset.
        /// </summary>
        public IList<TableDataNode> Tables { get; set; }

        public Table1D WetCorrectionTable { get; set; }
        public double EngineAICorrection { get; set; }
        public double AllAICorrection { get; set; }
        public double PacksOnCorrection { get; set; }

        public bool Equals(AirbusPerfTable other, double delta)
        {
            bool NodesEqual(IList<TableDataNode> a, IList<TableDataNode> b)
            {
                return a != null && b != null && a.Count == b.Count &&
                    a.All(x => b.Any(y => x.Equals(y, delta)));
            }

            return other != null &&
                HeadwindCorrectionTable.Equals(other.HeadwindCorrectionTable, delta) &&
                TailwindCorrectionTable.Equals(other.TailwindCorrectionTable, delta) &&
                UphillCorrectionTable.Equals(other.UphillCorrectionTable, delta) &&
                DownHillCorrectionTable.Equals(other.DownHillCorrectionTable, delta) &&
                NodesEqual(Tables, other.Tables) &&
                WetCorrectionTable.Equals(other.WetCorrectionTable, delta) &&
                Math.Abs(EngineAICorrection - other.EngineAICorrection) <= delta &&
                Math.Abs(AllAICorrection - other.AllAICorrection) <= delta &&
                Math.Abs(PacksOnCorrection - other.PacksOnCorrection) <= delta;
        }
    }
}
