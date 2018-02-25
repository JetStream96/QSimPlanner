using QSP.MathTools.Tables;
using System.Collections.Generic;

namespace QSP.TOPerfCalculation.Airbus.PerfData
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
        public IList<TableDataNode> Tables { get; set; }
        public Table1D WetCorrectionTable { get; set; }
        public double EngineAICorrection { get; set; }
        public double AllAICorrection { get; set; }
        public double PacksOnCorrection { get; set; }
    }
}
