using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Airbus.PerfData
{
    /// <summary>
    /// Unit and numerical values of the properties are exactly the same as described
    /// in file a320_200_CFM56_TO.xml.
    /// </summary>
    public class AirbusPerfTable : PerfTableItem
    {
        public Table2D WindCorrectionTable { get; set; }
        public Table2D SlopeCorrectionTable { get; set; }
        public IList<TableDataNode> Tables { get; set; }
        public Table1D WetCorrectionTable { get; set; }
        public double EngineAICorrection { get; set; }
        public double AllAICorrection { get; set; }
        public double PacksOnCorrection { get; set; }

        /// <summary>
        /// Returns null if failed to 
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public static (bool success, AirbusPerfTable val) Load(XElement rootNode)
        {
            try
            {
                if (rootNode.Element("Parameters").Element("Format").Value != "1")
                {
                    return (false, null);
                }


                var wet = rootNode.Element("WetCorrection").Value;
                var wetTable =

                var bleeds = rootNode.Element("Bleeds");
                var engineAI = double.Parse(bleeds.Attribute("engine_ai").Value);
                var allAI = double.Parse(bleeds.Attribute("all_ai").Value);
                var packsOn = double.Parse(bleeds.Attribute("packs_on").Value);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
